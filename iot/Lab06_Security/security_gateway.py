"""
Безпечний шлюз/брокер IoT (security_gateway.py).
Виконує повний цикл багатошарової верифікації вхідних пакетів телеметрії:
1. Ідентифікація та автентифікація пристрою за базою реєстрації та токеном;
2. Контроль цілісності пакету через перевірку підпису HMAC-SHA256;
3. Захист від атак повторного відтворення (Replay Attack) через Timestamp та Nonce;
4. Автентифіковане дешифрування корисного навантаження через AES-256-GCM;
5. Реєстрація всіх подій у структурованому журналі аудиту безпеки (Security Audit Log).
"""

import sys
import time
import json
from typing import Dict, Any, Tuple, Optional, List
from cryptography.exceptions import InvalidTag

if sys.stdout.encoding and sys.stdout.encoding.lower() != "utf-8":
    try:
        sys.stdout.reconfigure(encoding="utf-8")
    except Exception:
        pass
from crypto_utils import (
    decrypt_aes_gcm,
    verify_hmac_sha256,
    ReplayAttackValidator,
    canonical_json_bytes
)


class SecurityGateway:
    """Центральний вузол безпеки для збору та валідації IoT телеметрії."""

    def __init__(self, replay_window_seconds: float = 30.0):
        """
        :param replay_window_seconds: Допустиме вікно затримки пакетів для захисту від повторів
        """
        # Реєстр зареєстрованих та довірених пристроїв:
        # device_id -> {aes_key, hmac_key, auth_token, device_name, is_active}
        self.device_registry: Dict[str, Dict[str, Any]] = {}
        
        # Модуль анти-повтору
        self.replay_validator = ReplayAttackValidator(time_window_seconds=replay_window_seconds)
        
        # Журнал аудиту подій безпеки (Security Audit Log)
        self.audit_log: List[Dict[str, Any]] = []

    def register_device(
        self,
        device_id: str,
        auth_token: str,
        aes_key: bytes,
        hmac_key: bytes,
        device_name: str = "Sensor Node"
    ) -> None:
        """Реєстрація довіреного IoT-пристрою у внутрішній базі шлюзу."""
        self.device_registry[device_id] = {
            "auth_token": auth_token,
            "aes_key": aes_key,
            "hmac_key": hmac_key,
            "device_name": device_name,
            "is_active": True
        }
        self._log_event(
            event_type="DEVICE_REGISTERED",
            device_id=device_id,
            status="SUCCESS",
            details=f"Пристрій '{device_name}' успішно додано до довіреного реєстру."
        )

    def process_incoming_packet(self, packet: Dict[str, Any]) -> Tuple[bool, Optional[Dict[str, Any]], str]:
        """
        Обробка вхідного пакету через конвеєр перевірок безпеки (Zero Trust Pipeline).
        
        :param packet: Отриманий пакет із полями {header, encrypted_payload, signature}
        :return: (is_success: bool, decrypted_telemetry: Optional[dict], message: str)
        """
        # 1. Структурна валідація формату пакету
        if not isinstance(packet, dict) or not all(k in packet for k in ("header", "encrypted_payload", "signature")):
            err_msg = "Пакет відхилено: порушено базову структуру JSON або відсутні обов'язкові поля."
            self._log_event("MALFORMED_PACKET", "UNKNOWN", "REJECTED", err_msg)
            return False, None, err_msg

        header = packet["header"]
        encrypted_payload = packet["encrypted_payload"]
        signature = packet["signature"]
        device_id = header.get("device_id", "UNKNOWN")
        received_token = header.get("auth_token", "")
        packet_timestamp = header.get("timestamp", 0.0)
        packet_nonce = header.get("nonce", "")

        # 2. Перевірка автентичності та прав доступу пристрою (Authentication check)
        if device_id not in self.device_registry:
            err_msg = f"Відмовлено в доступі: пристрій '{device_id}' не зареєстрований у системі."
            self._log_event("UNAUTHORIZED_DEVICE", device_id, "BLOCKED", err_msg)
            return False, None, err_msg

        device_info = self.device_registry[device_id]
        if not device_info["is_active"]:
            err_msg = f"Доступ заблоковано: пристрій '{device_id}' позначено як деактивований."
            self._log_event("DEVICE_SUSPENDED", device_id, "BLOCKED", err_msg)
            return False, None, err_msg

        if received_token != device_info["auth_token"]:
            err_msg = f"Помилка автентифікації: надано недійсний токен доступу для '{device_id}'."
            self._log_event("AUTH_FAILED", device_id, "BLOCKED", err_msg)
            return False, None, err_msg

        # 3. Перевірка підпису цілісності HMAC-SHA256 (Integrity check)
        signed_body = {
            "header": header,
            "encrypted_payload": encrypted_payload
        }
        signed_body_bytes = canonical_json_bytes(signed_body)
        hmac_key = device_info["hmac_key"]

        if not verify_hmac_sha256(hmac_key, signed_body_bytes, signature):
            err_msg = f"Порушення цілісності: підпис HMAC-SHA256 недійсний! Спроба підміни даних (MitM)."
            self._log_event("INTEGRITY_VIOLATION", device_id, "ALERT", err_msg)
            return False, None, err_msg

        # 4. Захист від Replay-атак: перевірка часової мітки та унікальності nonce
        is_fresh, replay_reason = self.replay_validator.validate_and_record(packet_timestamp, packet_nonce)
        if not is_fresh:
            err_msg = f"Блокування Replay-атаки: {replay_reason}"
            self._log_event("REPLAY_ATTACK_DETECTED", device_id, "BLOCKED", err_msg)
            return False, None, err_msg

        # 5. Автентифіковане дешифрування корисного навантаження AES-256-GCM (Confidentiality)
        aes_key = device_info["aes_key"]
        try:
            plaintext_bytes = decrypt_aes_gcm(encrypted_payload, aes_key)
            telemetry_data = json.loads(plaintext_bytes.decode("utf-8"))
        except InvalidTag:
            err_msg = "Критична помилка дешифрування: тег автентифікації AES-GCM пошкоджено!"
            self._log_event("GCM_TAG_INVALID", device_id, "ALERT", err_msg)
            return False, None, err_msg
        except Exception as ex:
            err_msg = f"Помилка дешифрування телеметрії: {str(ex)}"
            self._log_event("DECRYPTION_ERROR", device_id, "ERROR", err_msg)
            return False, None, err_msg

        # Успішна обробка валідного пакету
        success_msg = f"Пакет від '{device_id}' успішно верифіковано та розшифровано."
        self._log_event("TELEMETRY_ACCEPTED", device_id, "SUCCESS", success_msg)
        return True, telemetry_data, success_msg

    def _log_event(self, event_type: str, device_id: str, status: str, details: str) -> None:
        """Запис інциденту або транзакції у Security Audit Log."""
        record = {
            "timestamp": time.time(),
            "formatted_time": time.strftime("%Y-%m-%d %H:%M:%S", time.localtime()),
            "event_type": event_type,
            "device_id": device_id,
            "status": status,
            "details": details
        }
        self.audit_log.append(record)

    def print_audit_summary(self) -> None:
        """Вивід таблиці аудиту безпеки у консоль."""
        print("\n" + "=" * 90)
        print("          ЖУРНАЛ АУДИТУ БЕЗПЕКИ ШЛЮЗУ IOT (SECURITY AUDIT LOG)")
        print("=" * 90)
        print(f"{'Час':<20} | {'Тип події':<25} | {'Пристрій':<15} | {'Статус':<10} | {'Деталі'}")
        print("-" * 90)
        for entry in self.audit_log:
            print(
                f"{entry['formatted_time']:<20} | "
                f"{entry['event_type']:<25} | "
                f"{entry['device_id']:<15} | "
                f"{entry['status']:<10} | "
                f"{entry['details'][:50]}"
            )
        print("=" * 90 + "\n")
