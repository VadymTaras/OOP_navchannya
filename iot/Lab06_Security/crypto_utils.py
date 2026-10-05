"""
Криптографічний модуль комплексу безпеки IoT (crypto_utils.py).
Реалізує:
- Симетричне автентифіковане шифрування AES-256-GCM для забезпечення конфіденційності телеметрії;
- Обчислення та верифікацію HMAC-SHA256 підпису для контролю цілісності та автентичності повідомлень;
- Захист від атак повторного відтворення (Replay Attack Protection) на основі часових міток (timestamp) та унікальних одноразових кодів (nonce).
"""

import os
import time
import hmac
import hashlib
import json
import base64
from typing import Dict, Any, Tuple, Optional
from cryptography.hazmat.primitives.ciphers.aead import AESGCM


def generate_aes_key() -> bytes:
    """Генерація криптографічно стійкого 256-бітного симетричного ключа (32 байти)."""
    return AESGCM.generate_key(bit_length=256)


def generate_hmac_key() -> bytes:
    """Генерація 256-бітного секретного ключа для HMAC-SHA256 (32 байти)."""
    return os.urandom(32)


def encrypt_aes_gcm(plaintext: bytes, key: bytes, associated_data: Optional[bytes] = None) -> Dict[str, str]:
    """
    Шифрування даних за стандартом AES-256-GCM (Galois/Counter Mode).
    Забезпечує конфіденційність та автентифіковане шифрування з контролем цілісності.
    
    :param plaintext: Відкриті байти повідомлення
    :param key: 256-бітний симетричний ключ
    :param associated_data: Додаткові відкриті автентифіковані дані (AAD)
    :return: Словник з nonce, ciphertext та тегом автентифікації у Base64
    """
    if len(key) != 32:
        raise ValueError("Довжина ключа AES-256 повинна становити рівно 32 байти (256 біт).")

    # Рекомендована стандартом NIST довжина nonce для GCM становить 12 байтів (96 біт)
    nonce = os.urandom(12)
    aesgcm = AESGCM(key)
    
    # AESGCM в cryptography повертає ciphertext + 16-байтний authentication tag у кінці
    encrypted_blob = aesgcm.encrypt(nonce, plaintext, associated_data)
    
    ciphertext = encrypted_blob[:-16]
    auth_tag = encrypted_blob[-16:]
    
    return {
        "nonce": base64.b64encode(nonce).decode("utf-8"),
        "ciphertext": base64.b64encode(ciphertext).decode("utf-8"),
        "tag": base64.b64encode(auth_tag).decode("utf-8")
    }


def decrypt_aes_gcm(encrypted_dict: Dict[str, str], key: bytes, associated_data: Optional[bytes] = None) -> bytes:
    """
    Дешифрування даних AES-256-GCM з перевіркою тегу автентифікації.
    
    :param encrypted_dict: Словник з nonce, ciphertext та tag у Base64
    :param key: 256-бітний симетричний ключ
    :param associated_data: Додаткові відкриті автентифіковані дані (AAD)
    :return: Розшифровані байти відкритого тексту
    :raises cryptography.exceptions.InvalidTag: У разі підробки шифротексту або тегу
    """
    if len(key) != 32:
        raise ValueError("Довжина ключа AES-256 повинна становити рівно 32 байти.")

    nonce = base64.b64decode(encrypted_dict["nonce"])
    ciphertext = base64.b64decode(encrypted_dict["ciphertext"])
    auth_tag = base64.b64decode(encrypted_dict["tag"])
    
    encrypted_blob = ciphertext + auth_tag
    aesgcm = AESGCM(key)
    
    return aesgcm.decrypt(nonce, encrypted_blob, associated_data)


def compute_hmac_sha256(key: bytes, message: bytes) -> str:
    """
    Обчислення контрольного підпису цілісності HMAC-SHA256.
    
    :param key: Секретний ключ підпису
    :param message: Дані повідомлення для хешування
    :return: Шістнадцятковий рядок (Hex string) підпису
    """
    h = hmac.new(key, message, hashlib.sha256)
    return h.hexdigest()


def verify_hmac_sha256(key: bytes, message: bytes, expected_signature_hex: str) -> bool:
    """
    Константно-часова перевірка підпису HMAC-SHA256 (захист від timing attacks).
    
    :param key: Секретний ключ підпису
    :param message: Отримані байти повідомлення
    :param expected_signature_hex: Очікуваний шістнадцятковий рядок підпису
    :return: True, якщо підпис валідний; False, якщо дані змінено або сфальсифіковано
    """
    actual_signature_hex = compute_hmac_sha256(key, message)
    return hmac.compare_digest(actual_signature_hex, expected_signature_hex)


class ReplayAttackValidator:
    """
    Клас захисту від атак повторного відтворення (Replay Attack Protection).
    Поєднує перевірку часового вікна валідності (timestamp window) та
    кеш використаних унікальних одноразових номерів (nonce cache).
    """

    def __init__(self, time_window_seconds: float = 30.0):
        """
        :param time_window_seconds: Допустимий часовий інтервал затримки пакету (у секундах)
        """
        self.time_window_seconds = time_window_seconds
        # Словник: nonce -> timestamp отримання (для запобігання повторної обробки)
        self.seen_nonces: Dict[str, float] = {}

    def validate_and_record(self, message_timestamp: float, nonce: str) -> Tuple[bool, str]:
        """
        Перевірка повідомлення на актуальність за часом та унікальність nonce.
        
        :param message_timestamp: Часова мітка генерації повідомлення (UTC epoch seconds)
        :param nonce: Унікальний ідентифікатор повідомлення/одноразове число
        :return: (is_valid: bool, reason: str)
        """
        current_time = time.time()
        self._purge_expired_nonces(current_time)

        # 1. Перевірка часового вікна (Timestamp freshness check)
        time_skew = abs(current_time - message_timestamp)
        if time_skew > self.time_window_seconds:
            return False, (
                f"Застаріла часова мітка: відхилення {time_skew:.2f} с "
                f"перевищує допустимий ліміт {self.time_window_seconds:.2f} с"
            )

        # 2. Перевірка на повторне використання nonce (Nonce uniqueness check)
        if nonce in self.seen_nonces:
            return False, f"Виявлено дублікат nonce '{nonce}'! Загроза Replay Attack."

        # Реєстрація нового nonce
        self.seen_nonces[nonce] = current_time
        return True, "Пакет свіжий та унікальний"

    def _purge_expired_nonces(self, current_time: float) -> None:
        """Очищення кешу від застарілих nonce для оптимізації пам'яті MCU/шлюзу."""
        expiry_threshold = current_time - (self.time_window_seconds * 2)
        expired_keys = [n for n, ts in self.seen_nonces.items() if ts < expiry_threshold]
        for k in expired_keys:
            del self.seen_nonces[k]


def canonical_json_bytes(data: Dict[str, Any]) -> bytes:
    """
    Формування детермінованого бінарного представлення JSON (канонічна серіалізація).
    Гарантує однаковий порядок ключів для розрахунку HMAC без колізій форматування.
    """
    return json.dumps(data, sort_keys=True, separators=(",", ":")).encode("utf-8")
