"""
Симулятор кібератак на IoT-інфраструктуру (attack_simulation.py).
Демонструє практичну стійкість розробленого комплексу криптозахисту
проти класичних векторів атак на Інтернет речей:
1. Базовий сценарій (Baseline): успішна передача легітимного криптопакету.
2. Man-in-the-Middle (MitM): підміна шифротексту/даних сенсора у каналі зв'язку.
3. Replay Attack (Атака повторного відтворення):
   - 3.1: Повтор перехопленого пакету в межах часового вікна (блокування через дублікат nonce);
   - 3.2: Відправка перехопленого пакету із затримкою (блокування через застарілий timestamp).
4. Unauthorized Access (Несанкціонований доступ):
   - 4.1: Спроба підключення невідомого пристрою з фіктивним ID;
   - 4.2: Спроба передачі з недійсним токеном автентифікації.
5. Bit-Flipping / Cryptographic Forgery: порушення цілісності блоку AES-GCM.
"""

import sys
import time
import json
import copy

if sys.stdout.encoding and sys.stdout.encoding.lower() != "utf-8":
    try:
        sys.stdout.reconfigure(encoding="utf-8")
    except Exception:
        pass

from crypto_utils import generate_aes_key, generate_hmac_key
from secure_iot_node import SecureIoTNode
from security_gateway import SecurityGateway


def run_full_security_simulation():
    print("\n" + "=" * 80)
    print("      КОМПЛЕКСНИЙ СИМУЛЯТОР КІБЕРАТАК ТА КРИПТОЗАХИСТУ СИСТЕМ IOT")
    print("      Дисципліна: Технології IoT та SMART-технології | Практична робота 6")
    print("=" * 80 + "\n")

    # -------------------------------------------------------------
    # 0. Ініціалізація інфраструктури (Шлюз та Довірені вузли)
    # -------------------------------------------------------------
    print("[1/6] ІНІЦІАЛІЗАЦІЯ БЕЗПЕЧНОГО СЕРЕДОВИЩА...")
    gateway = SecurityGateway(replay_window_seconds=15.0)

    # Генерація криптографічних ключів для легітимного пристрою
    node1_aes_key = generate_aes_key()
    node1_hmac_key = generate_hmac_key()
    node1_token = "auth_tok_iot_lab06_secure_98471"

    # Реєстрація пристрою на шлюзі
    gateway.register_device(
        device_id="esp32_sensor_01",
        auth_token=node1_token,
        aes_key=node1_aes_key,
        hmac_key=node1_hmac_key,
        device_name="ESP32-SmartOffice-Node"
    )

    legitimate_node = SecureIoTNode(
        device_id="esp32_sensor_01",
        auth_token=node1_token,
        aes_encryption_key=node1_aes_key,
        hmac_integrity_key=node1_hmac_key,
        location="Lab-Room-404"
    )
    print(" -> Шлюз безпеки та довірений сенсор успішно налаштовані та синхронізовані.\n")

    test_results = []

    # -------------------------------------------------------------
    # Сценарій 0: Легітимна передача телеметрії (Baseline Test)
    # -------------------------------------------------------------
    print("--- СЦЕНАРІЙ 0: Легітимна передача захищеного пакету (Baseline) ---")
    telemetry_raw = legitimate_node.generate_telemetry()
    print(f"[*] Згенеровано показники сенсора: T={telemetry_raw['temperature_celsius']}°C, "
          f"H={telemetry_raw['humidity_percent']}%, P={telemetry_raw['pressure_hpa']} hPa")
    
    valid_packet = legitimate_node.build_secure_packet(telemetry_raw)
    print(f"[*] Пакет зашифровано (AES-256-GCM) та підписано (HMAC-SHA256).")
    print(f"    Nonce: {valid_packet['header']['nonce'][:16]}...")
    print(f"    HMAC:  {valid_packet['signature'][:24]}...")

    success, decrypted, msg = gateway.process_incoming_packet(valid_packet)
    if success and decrypted:
        print(f"[+] РЕЗУЛЬТАТ: УСПІШНО! Шлюз прийняв пакет: {msg}")
        print(f"    Розшифровані дані: {decrypted}")
        test_results.append(("Сценарій 0: Легітимна передача даних", "ПРОЙДЕНО", "Дані успішно розшифровано"))
    else:
        print(f"[-] РЕЗУЛЬТАТ: ПОМИЛКА! Не вдалося обробити легітимний пакет: {msg}")
        test_results.append(("Сценарій 0: Легітимна передача даних", "ПРОВАЛЕНО", msg))
    print()

    # -------------------------------------------------------------
    # Сценарій 1: Атака «Людина посередині» (Man-in-the-Middle, MitM)
    # -------------------------------------------------------------
    print("--- СЦЕНАРІЙ 1: Атака 'Людина посередині' (Man-in-the-Middle / Data Tampering) ---")
    print("[*] Зловмисник перехоплює транзитний пакет у бездротовому каналі (Wi-Fi/Zigbee)")
    print("[*] Зловмисник змінює байти шифротексту, намагаючись сфальсифікувати телеметрію...")
    
    # Справжня атака MitM: перехоплення сформованого пакету та підміна байтів у каналі
    captured_packet = legitimate_node.build_secure_packet()
    tampered_packet = copy.deepcopy(captured_packet)
    orig_ct = tampered_packet["encrypted_payload"]["ciphertext"]
    tampered_packet["encrypted_payload"]["ciphertext"] = orig_ct[:-4] + "AAAA"
    
    success, decrypted, msg = gateway.process_incoming_packet(tampered_packet)
    if not success and "Порушення цілісності" in msg:
        print(f"[+] ВІДБИТО АТАКУ! Шлюз виявив невідповідність HMAC-SHA256: {msg}")
        test_results.append(("Сценарій 1: Атака MitM (Підміна шифротексту)", "ВІДБИТО", "HMAC верифікація заблокувала модифікацію"))
    else:
        print(f"[-] ВРАЗЛИВІСТЬ! Модифікований пакет не було відхилено належним чином: {msg}")
        test_results.append(("Сценарій 1: Атака MitM", "ПРОВАЛЕНО", msg))
    print()

    # -------------------------------------------------------------
    # Сценарій 2: Атака повторного відтворення (Replay Attack)
    # -------------------------------------------------------------
    print("--- СЦЕНАРІЙ 2.1: Атака Replay Attack (Повторна відправка перехопленого пакету) ---")
    print("[*] Зловмисник перехопив раніше переданий валідний пакет і надсилає його вдруге без змін.")
    print("[*] Мета: змусити шлюз повторно зареєструвати показання або виконати повторну команду.")

    # Повторна відправка раніше валідного пакету
    success, decrypted, msg = gateway.process_incoming_packet(valid_packet)
    if not success and "Replay Attack" in msg:
        print(f"[+] ВІДБИТО АТАКУ! Шлюз виявив дублювання nonce: {msg}")
        test_results.append(("Сценарій 2.1: Replay Attack (Дублікат Nonce)", "ВІДБИТО", "Шлюз заблокував повторний пакет через кеш nonce"))
    else:
        print(f"[-] ВРАЗЛИВІСТЬ! Повторний пакет пройшов перевірку: {msg}")
        test_results.append(("Сценарій 2.1: Replay Attack", "ПРОВАЛЕНО", msg))
    print()

    print("--- СЦЕНАРІЙ 2.2: Атака Replay Attack (Застаріла часова мітка Timestamp) ---")
    print("[*] Зловмисник надсилає повідомлення з новим унікальним nonce, але з застарілим часом (затримка 120 с).")
    
    expired_packet = legitimate_node.build_secure_packet(forced_timestamp=time.time() - 120.0)
    success, decrypted, msg = gateway.process_incoming_packet(expired_packet)
    if not success and "Застаріла часова мітка" in msg:
        print(f"[+] ВІДБИТО АТАКУ! Шлюз відхилив пакет через перевищення часового вікна: {msg}")
        test_results.append(("Сценарій 2.2: Replay Attack (Застарілий Timestamp)", "ВІДБИТО", "Шлюз заблокував пакет поза часовим вікном 15 с"))
    else:
        print(f"[-] ВРАЗЛИВІСТЬ! Застарілий пакет пройшов перевірку: {msg}")
        test_results.append(("Сценарій 2.2: Replay Attack Timestamp", "ПРОВАЛЕНО", msg))
    print()

    # -------------------------------------------------------------
    # Сценарій 3: Несанкціонований доступ (Unauthorized Access)
    # -------------------------------------------------------------
    print("--- СЦЕНАРІЙ 3.1: Неавторизований пристрій (Rogue / Unknown Device) ---")
    print("[*] Зловмисник підключає підроблений сенсор 'fake_malicious_mcu_99' без реєстрації у системі.")
    
    rogue_node = SecureIoTNode(
        device_id="fake_malicious_mcu_99",
        auth_token="stolen_or_guessed_token_123",
        aes_encryption_key=generate_aes_key(),
        hmac_integrity_key=generate_hmac_key()
    )
    rogue_packet = rogue_node.build_secure_packet()
    success, decrypted, msg = gateway.process_incoming_packet(rogue_packet)
    if not success and "не зареєстрований" in msg:
        print(f"[+] ВІДБИТО АТАКУ! Шлюз заблокував невідомий пристрій: {msg}")
        test_results.append(("Сценарій 3.1: Незареєстрований пристрій (Rogue Device)", "ВІДБИТО", "Шлюз відмовив у доступі пристрою поза білим списком"))
    else:
        print(f"[-] ВРАЗЛИВІСТЬ! Невідомий пристрій отримав доступ: {msg}")
        test_results.append(("Сценарій 3.1: Rogue Device", "ПРОВАЛЕНО", msg))
    print()

    print("--- СЦЕНАРІЙ 3.2: Невалідний токен автентифікації (Bad Auth Token) ---")
    print("[*] Зареєстрований пристрій передає пакет з недійсним або підробленим токеном.")
    
    bad_token_packet = legitimate_node.build_secure_packet(forced_token="invalid_token_xyz_666")
    success, decrypted, msg = gateway.process_incoming_packet(bad_token_packet)
    if not success and "Помилка автентифікації" in msg:
        print(f"[+] ВІДБИТО АТАКУ! Шлюз заблокував пакет з недійсним токеном: {msg}")
        test_results.append(("Сценарій 3.2: Недійсний токен доступу", "ВІДБИТО", "Шлюз перевірив токен перед виконанням криптографічних операцій"))
    else:
        print(f"[-] ВРАЗЛИВІСТЬ! Пакет з підробленим токеном було прийнято: {msg}")
        test_results.append(("Сценарій 3.2: Bad Token", "ПРОВАЛЕНО", msg))
    print()

    # -------------------------------------------------------------
    # Сценарій 4: Спроба фальсифікації підпису HMAC (Signature Forgery)
    # -------------------------------------------------------------
    print("--- СЦЕНАРІЙ 4: Фальсифікація криптографічного підпису (Signature Forgery) ---")
    print("[*] Зловмисник генерує пакет із підробленим хешем підпису.")
    forged_sig_packet = legitimate_node.build_secure_packet(tamper_mode="bad_signature")
    success, decrypted, msg = gateway.process_incoming_packet(forged_sig_packet)
    if not success and "Порушення цілісності" in msg:
        print(f"[+] ВІДБИТО АТАКУ! Шлюз заблокував пакет через підроблений HMAC: {msg}")
        test_results.append(("Сценарій 4: Фальсифікація підпису HMAC", "ВІДБИТО", "Шлюз відхилив фальшивий підпис"))
    else:
        print(f"[-] ВРАЗЛИВІСТЬ! Фальшивий підпис пройшов верифікацію: {msg}")
        test_results.append(("Сценарій 4: Signature Forgery", "ПРОВАЛЕНО", msg))
    print()

    # -------------------------------------------------------------
    # Підсумковий звіт та вивід журналу аудиту
    # -------------------------------------------------------------
    gateway.print_audit_summary()

    print("=" * 80)
    print("                  ПІДСУМКОВА ТАБЛИЦЯ ТЕСТУВАННЯ СИСТЕМИ БЕЗПЕКИ")
    print("=" * 80)
    print(f"{'Тестовий сценарій / Вектор атаки':<50} | {'Статус':<10} | {'Опис результату'}")
    print("-" * 80)
    for test_name, status, desc in test_results:
        print(f"{test_name:<50} | {status:<10} | {desc}")
    print("=" * 80 + "\n")


if __name__ == "__main__":
    run_full_security_simulation()
