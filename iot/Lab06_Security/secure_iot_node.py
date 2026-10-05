"""
Модель захищеного IoT-вузла (secure_iot_node.py).
Емулює поведінку мікроконтролерного сенсорного вузла (ESP32 / STM32):
- Збір сенсорної телеметрії (температура, вологість, тиск, заряд батареї);
- Формування унікальних nonce та UTC timestamp для кожного пакету;
- Наскрізне симетричне шифрування корисного навантаження через AES-256-GCM;
- Розрахунок криптографічного підпису цілісності HMAC-SHA256;
- Автентифікація на шлюзі через попередньо узгоджений секретний токен.
"""

import time
import uuid
import random
from typing import Dict, Any, Optional
from crypto_utils import (
    encrypt_aes_gcm,
    compute_hmac_sha256,
    canonical_json_bytes
)


class SecureIoTNode:
    """Клас сенсорного IoT-пристрою із вбудованими механізмами кіберзахисту."""

    def __init__(
        self,
        device_id: str,
        auth_token: str,
        aes_encryption_key: bytes,
        hmac_integrity_key: bytes,
        location: str = "Smart-Lab-01"
    ):
        """
        :param device_id: Унікальний апаратний ідентифікатор пристрою
        :param auth_token: Секретний токен автентифікації на шлюзі
        :param aes_encryption_key: 256-бітний симетричний ключ шифрування даних
        :param hmac_integrity_key: 256-бітний ключ контролю цілісності HMAC
        :param location: Фізична локація сенсорного вузла
        """
        self.device_id = device_id
        self.auth_token = auth_token
        self.aes_key = aes_encryption_key
        self.hmac_key = hmac_integrity_key
        self.location = location
        self._message_counter = 0

    def generate_telemetry(self) -> Dict[str, Any]:
        """
        Емуляція збору показників з фізичних сенсорів мікроконтролера.
        """
        self._message_counter += 1
        return {
            "device_id": self.device_id,
            "sample_index": self._message_counter,
            "temperature_celsius": round(random.uniform(20.0, 26.5), 2),
            "humidity_percent": round(random.uniform(40.0, 60.0), 2),
            "pressure_hpa": round(random.uniform(1010.0, 1025.0), 1),
            "battery_level_pct": max(15, 100 - (self._message_counter // 10)),
            "system_status": "NORMAL"
        }

    def build_secure_packet(
        self,
        telemetry: Optional[Dict[str, Any]] = None,
        tamper_mode: Optional[str] = None,
        forced_timestamp: Optional[float] = None,
        forced_nonce: Optional[str] = None,
        forced_token: Optional[str] = None
    ) -> Dict[str, Any]:
        """
        Створення криптографічно захищеного пакету телеметрії.
        
        Підтримує опційні параметри для лабораторної симуляції кібератак:
        - tamper_mode: 'modify_payload' (підміна відкритого/шифрованого тексту),
                       'bad_signature' (пошкодження підпису)
        - forced_timestamp: примусове встановлення застарілої мітки часу
        - forced_nonce: примусове повторне використання nonce
        - forced_token: підміна токена автентифікації
        """
        if telemetry is None:
            telemetry = self.generate_telemetry()

        # 1. Формування відкритого корисного навантаження у бінарний вигляд
        payload_bytes = canonical_json_bytes(telemetry)

        # 2. Шифрування корисного навантаження AES-256-GCM
        encrypted_payload = encrypt_aes_gcm(payload_bytes, self.aes_key)

        # Симуляція несанкціонованої модифікації шифротексту (MitM)
        if tamper_mode == "modify_payload":
            # Зловмисник змінює байти у зашифрованому полі
            original_ct = encrypted_payload["ciphertext"]
            tampered_ct = original_ct[:-4] + "AAAA" if len(original_ct) > 4 else "AAAA"
            encrypted_payload["ciphertext"] = tampered_ct

        # 3. Формування незашифрованого, але автентифікованого заголовка
        current_timestamp = forced_timestamp if forced_timestamp is not None else time.time()
        packet_nonce = forced_nonce if forced_nonce is not None else str(uuid.uuid4())
        packet_token = forced_token if forced_token is not None else self.auth_token

        header = {
            "device_id": self.device_id,
            "location": self.location,
            "timestamp": current_timestamp,
            "nonce": packet_nonce,
            "auth_token": packet_token,
            "protocol_version": "1.0-SEC-GCM"
        }

        # 4. Формування канонічного вмісту для підпису цілісності (Заголовок + Зашифроване тіло)
        signed_body = {
            "header": header,
            "encrypted_payload": encrypted_payload
        }
        signed_body_bytes = canonical_json_bytes(signed_body)

        # 5. Розрахунок підпису HMAC-SHA256
        signature = compute_hmac_sha256(self.hmac_key, signed_body_bytes)

        if tamper_mode == "bad_signature":
            # Пошкодження підпису
            signature = "0000000000000000000000000000000000000000000000000000000000000000"

        return {
            "header": header,
            "encrypted_payload": encrypted_payload,
            "signature": signature
        }
