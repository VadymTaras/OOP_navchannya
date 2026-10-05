"""
Центральний контролер автоматизації розумного будинку (SMART Home Hub)
Дисципліна: «Технології IoT та SMART-технології»
Практична робота 4: «SMART-технології у побуті: системи автоматизації будинку та інтеграція розумних побутових приладів»
Студент: ТАРАС Вадим, група аІк43

Опис:
SmartHomeHub реалізує централізовану координацію гетерогенних підсистем розумного будинку:
1. Клімат-контролер (Climate Controller) з гістерезисом та зональним регулюванням.
2. Енергетичний моніторинг та захист розеток (Smart Energy Plug Controller).
3. Підсистема безпеки та захисту (Security & Safety Subsystem).
4. Механізм бізнес-правил (Rule Engine) для автоматичних сценаріїв («Нічний режим»,
   «Енергозбереження при відсутності», «Аварія при протіканні води»).
5. Структуроване логування подій та телеметрії у JSON Lines форматі.
"""

import os
import sys
import json
import time
from typing import Dict, Any, Optional

# Налаштування кодування виводу для сумісності з Windows
if hasattr(sys.stdout, 'reconfigure'):
    sys.stdout.reconfigure(encoding='utf-8')

try:
    import paho.mqtt.client as mqtt
    MQTT_AVAILABLE = True
except ImportError:
    MQTT_AVAILABLE = False

from devices_simulator import SmartDeviceSimulator


class ClimateController:
    """
    Підсистема клімат-контролю.
    Забезпечує терморегуляцію за принципом двопозиційного регулятора з гістерезисом
    для уникнення частого перемикання реле обігрівача (relay chattering).
    """

    def __init__(self, zones_config: Dict[str, Any]):
        self.zones = zones_config
        # Поточний стан зон: { "living_room": {"temp": 21.0, "heater": False} }
        self.state: Dict[str, Dict[str, Any]] = {}
        for zone_id, conf in self.zones.items():
            self.state[zone_id] = {
                "name": conf["name"],
                "temperature": 20.0,
                "humidity": 50.0,
                "heater": False,
                "mode": conf.get("current_mode", "comfort")
            }

    def update_telemetry(self, room: str, temp: float, humidity: float):
        """Оновлення поточних вимірювань датчика температури."""
        if room in self.state:
            self.state[room]["temperature"] = temp
            self.state[room]["humidity"] = humidity

    def set_zone_mode(self, room: str, mode: str):
        """Встановлення режиму роботи зони ('comfort' або 'eco')."""
        if room in self.state and mode in ["comfort", "eco"]:
            self.state[room]["mode"] = mode
            print(f"[Клімат] Зона '{self.state[room]['name']}' переведена у режим: {mode.upper()}")

    def evaluate_heating(self, room: str) -> Optional[str]:
        """
        Розрахунок керуючої дії за алгоритмом гістерезису:
        - Якщо T < T_target - Hysteresis => ON
        - Якщо T >= T_target => OFF
        Повертає нову команду ("ON" / "OFF") або None, якщо змін немає.
        """
        if room not in self.state:
            return None

        zone_conf = self.zones[room]
        current_temp = self.state[room]["temperature"]
        mode = self.state[room]["mode"]
        target = zone_conf["target_temp_comfort"] if mode == "comfort" else zone_conf["target_temp_eco"]
        hysteresis = zone_conf.get("hysteresis", 0.5)
        current_heater = self.state[room]["heater"]

        # Логіка термостата з гістерезисом
        if not current_heater and current_temp < (target - hysteresis):
            self.state[room]["heater"] = True
            return "ON"
        elif current_heater and current_temp >= target:
            self.state[room]["heater"] = False
            return "OFF"

        return None


class EnergyPlugManager:
    """
    Підсистема моніторингу енергоспоживання та захисту силових ліній.
    Відстежує миттєву потужність (Вт), накопичену енергію (кВт·год)
    та відключає навантаження при перевантаженні (Overload Protection).
    """

    def __init__(self, energy_config: Dict[str, Any]):
        self.config = energy_config
        self.plugs: Dict[str, Dict[str, Any]] = {}
        for plug_id, conf in self.config["plugs"].items():
            self.plugs[plug_id] = {
                "name": conf["name"],
                "zone": conf["zone"],
                "state": "ON",
                "power_w": 0.0,
                "total_kwh": 0.0,
                "overload_threshold_w": conf.get("overload_threshold_w", 2200.0)
            }
        self.tariff = self.config.get("tariff_per_kwh", 4.32)

    def process_telemetry(self, plug_id: str, data: Dict[str, Any]) -> Optional[str]:
        """
        Аналіз телеметрії розетки.
        Повертає команду 'EMERGENCY_CUTOFF', якщо виявлено перевантаження по потужності.
        """
        if plug_id not in self.plugs:
            return None

        self.plugs[plug_id]["power_w"] = data.get("active_power_w", 0.0)
        self.plugs[plug_id]["total_kwh"] = data.get("total_energy_kwh", 0.0)
        self.plugs[plug_id]["state"] = data.get("relay_state", "ON")

        threshold = self.plugs[plug_id]["overload_threshold_w"]
        current_p = self.plugs[plug_id]["power_w"]

        # Захисне спрацьовування при перевищенні порогу безпеки
        if self.plugs[plug_id]["state"] == "ON" and current_p > threshold:
            print(f"[Енергія УВАГА] Перевантаження розетки '{self.plugs[plug_id]['name']}': "
                  f"{current_p} Вт > ліміту {threshold} Вт! Автоматичне захисне знеструмлення.")
            self.plugs[plug_id]["state"] = "OFF"
            return "OFF"

        return None

    def get_total_cost(self) -> float:
        """Розрахунок вартості спожитої електроенергії за встановленим тарифом."""
        total_kwh = sum(p["total_kwh"] for p in self.plugs.values())
        return round(total_kwh * self.tariff, 2)


class SafetySecuritySubsystem:
    """
    Підсистема захисту від побутових аварій та виявлення присутності.
    Контролює сенсори протікання води (Flood Sensor) та датчики руху (PIR).
    """

    def __init__(self, config: Dict[str, Any]):
        self.config = config
        self.leak_state = False
        self.valve_state = "OPEN"
        self.last_motion_timestamp = time.time()
        self.occupancy_status = "OCCUPIED"  # OCCUPIED або AWAY
        self.timeout = self.config.get("occupancy_timeout_seconds", 15)

    def process_leak_sensor(self, detected: bool) -> bool:
        """
        Обробка даних датчика затоплення.
        Повертає True, якщо зафіксовано нову аварію.
        """
        if detected and not self.leak_state:
            self.leak_state = True
            print("[Безпека КРИТИЧНО] Зафіксовано затоплення в зоні санвузла!")
            return True
        elif not detected and self.leak_state:
            self.leak_state = False
            print("[Безпека] Затоплення ліквідовано. Датчики в нормі.")
        return False

    def process_motion(self, has_motion: bool):
        """Оновлення таймера присутності мешканців."""
        if has_motion:
            self.last_motion_timestamp = time.time()
            if self.occupancy_status != "OCCUPIED":
                self.occupancy_status = "OCCUPIED"
                print("[Безпека] Виявлено активність: статус 'ВДОМА' (OCCUPIED).")

    def check_occupancy_timeout(self) -> bool:
        """
        Перевірка таймауту відсутності руху.
        Повертає True, якщо статус перемикається на 'AWAY'.
        """
        if self.occupancy_status == "OCCUPIED":
            if (time.time() - self.last_motion_timestamp) > self.timeout:
                self.occupancy_status = "AWAY"
                print(f"[Безпека] Рух відсутній понад {self.timeout} сек: перехід у статус 'ВІДСУТНІСТЬ' (AWAY).")
                return True
        return False


class SmartHomeHub:
    """
    Головний контролер розумного будинку (Центральний координатор).
    Об'єднує підсистеми, взаємодіє з MQTT-брокером та виконує правила Rule Engine.
    """

    def __init__(self, config_path: str = "config.json"):
        with open(config_path, "r", encoding="utf-8") as f:
            self.config = json.load(f)

        self.hub_id = self.config["system"]["hub_id"]
        self.topic_base = self.config["system"]["topic_base"]
        self.log_file = self.config["system"]["log_file"]

        # Ініціалізація підсистем
        self.climate = ClimateController(self.config["climate"]["zones"])
        self.energy = EnergyPlugManager(self.config["energy"])
        self.safety = SafetySecuritySubsystem(self.config["safety_security"])

        # Загальний режим функціонування будинку: "NORMAL", "NIGHT", "AWAY", "EMERGENCY"
        self.system_mode = "NORMAL"

        # MQTT клієнт
        self.mqtt_client = None
        if MQTT_AVAILABLE:
            self._setup_mqtt()

    def _setup_mqtt(self):
        """Ініціалізація MQTT клієнта хаба."""
        try:
            client_id = f"HubController_{self.hub_id}"
            if hasattr(mqtt, "CallbackAPIVersion"):
                self.mqtt_client = mqtt.Client(mqtt.CallbackAPIVersion.VERSION1, client_id)
            else:
                self.mqtt_client = mqtt.Client(client_id)

            self.mqtt_client.on_connect = self._on_connect
            self.mqtt_client.on_message = self._on_message
        except Exception as e:
            print(f"[Хаб] Попередження MQTT: {e}")
            self.mqtt_client = None

    def _on_connect(self, client, userdata, flags, rc):
        if rc == 0:
            print(f"[Хаб {self.hub_id}] Підключено до MQTT-брокера. Запуск підписок.")
            client.subscribe(f"{self.topic_base}/telemetry/#")
        else:
            print(f"[Хаб] Помилка з'єднання: {rc}")

    def _on_message(self, client, userdata, msg):
        try:
            topic = msg.topic
            data = json.loads(msg.payload.decode("utf-8"))
            self.process_incoming_telemetry(topic, data)
        except Exception as e:
            print(f"[Хаб] Помилка обробки MQTT телеметрії: {e}")

    def send_command(self, category: str, device_id: str, action: str):
        """Відправка керівної директиви актуатору."""
        cmd_topic = f"{self.topic_base}/commands/{category}/{device_id}"
        payload = {
            "source_hub": self.hub_id,
            "category": category,
            "device_id": device_id,
            "action": action,
            "timestamp": time.time()
        }
        payload_str = json.dumps(payload, ensure_ascii=False)

        # Публікація в MQTT брокер
        if self.mqtt_client and self.mqtt_client.is_connected():
            self.mqtt_client.publish(cmd_topic, payload_str)

        self.log_event("COMMAND_SENT", {
            "target": f"{category}/{device_id}",
            "action": action,
            "topic": cmd_topic
        })
        print(f"[Хаб КОМАНДА] -> Надіслано директиву на '{cmd_topic}': action='{action}'")

    def log_event(self, event_type: str, details: Dict[str, Any]):
        """Запис події або телеметрії у JSON Lines лог-файл."""
        entry = {
            "timestamp": time.strftime("%Y-%m-%d %H:%M:%S"),
            "event_type": event_type,
            "system_mode": self.system_mode,
            "details": details
        }
        try:
            with open(self.log_file, "a", encoding="utf-8") as f:
                f.write(json.dumps(entry, ensure_ascii=False) + "\n")
        except Exception as e:
            print(f"[Хаб] Помилка логування: {e}")

    def process_incoming_telemetry(self, topic: str, data: Dict[str, Any]):
        """
        Диспетчер телеметрії та запуск Rule Engine для прийняття рішень.
        """
        # 1. Телеметрія клімату
        if "/climate/" in topic:
            room = data.get("room")
            temp = data.get("temperature", 20.0)
            hum = data.get("humidity", 50.0)
            self.climate.update_telemetry(room, temp, hum)

            # Перевірка гістерезису термостата
            action = self.climate.evaluate_heating(room)
            if action:
                self.send_command("heater", room, action)
                self.log_event("CLIMATE_HEATER_TRIGGER", {"room": room, "action": action, "temperature": temp})

        # 2. Сенсор протікання води (Критичний захист)
        elif "/safety/water_leak" in topic:
            leak_detected = data.get("leak_detected", False)
            if self.safety.process_leak_sensor(leak_detected):
                self._execute_emergency_leak_scenario()

        # 3. Сенсори руху / присутності
        elif "/motion/" in topic:
            has_motion = data.get("motion", False)
            self.safety.process_motion(has_motion)
            if self.system_mode == "AWAY" and self.safety.occupancy_status == "OCCUPIED":
                self._switch_to_normal_mode()

        # 4. Розумні розетки (Smart Plugs)
        elif "/energy/" in topic:
            plug_id = data.get("plug_id")
            cutoff_action = self.energy.process_telemetry(plug_id, data)
            if cutoff_action:
                self.send_command("plug", plug_id, cutoff_action)
                self.log_event("OVERLOAD_SAFETY_TRIP", {
                    "plug_id": plug_id,
                    "power_w": data.get("active_power_w"),
                    "action": cutoff_action
                })

    # ==================== СЦЕНАРІЇ RULE ENGINE ====================

    def _execute_emergency_leak_scenario(self):
        """
        Сценарій: «Аварійне знеструмлення при протіканні води».
        1. Негайне перекриття основного моторизованого крана.
        2. Знеструмлення побутових приладів у зоні санвузла/кухні для запобігання КЗ.
        """
        self.system_mode = "EMERGENCY"
        print("\n" + "="*70)
        print("[RULE ENGINE / АВАРІЯ] АКТИВАЦІЯ СЦЕНАРІЮ: АВАРІЙНЕ ЗНЕСТРУМЛЕННЯ ТА ПЕРЕКРИТТЯ ВОДИ")
        print("="*70)

        # Перекриваємо кран
        self.send_command("valve", "main_water_valve", "CLOSE")
        self.safety.valve_state = "CLOSED"

        # Знеструмлюємо розетки в зоні небезпеки
        self.send_command("plug", "washing_machine", "OFF")

        self.log_event("EMERGENCY_LEAK_EXECUTED", {
            "valve": "main_water_valve",
            "valve_action": "CLOSE",
            "plugs_shutoff": ["washing_machine"]
        })

    def execute_away_mode_scenario(self):
        """
        Сценарій: «Енергозбереження при відсутності мешканців» (Away Mode).
        1. Зниження цільової температури всіх кліматичних зон до рівня ECO (17–18.5°C).
        2. Вимкнення енергоємних непримусових приладів.
        """
        self.system_mode = "AWAY"
        print("\n" + "="*70)
        print("[RULE ENGINE / ЕКО] АКТИВАЦІЯ СЦЕНАРІЮ: ЕНЕРГОЗБЕРЕЖЕННЯ ПРИ ВІДСУТНОСТІ МЕШКАНЦІВ")
        print("="*70)

        # Переведення всіх зон у режим ECO
        for room in self.climate.zones:
            self.climate.set_zone_mode(room, "eco")

        # Вимкнення бойлера для збереження енергії
        self.send_command("plug", "kitchen_boiler", "OFF")

        self.log_event("AWAY_MODE_ACTIVATED", {"climate_mode": "eco", "disabled_plugs": ["kitchen_boiler"]})

    def execute_night_mode_scenario(self):
        """
        Сценарій: «Нічний режим» (Night Mode).
        Зниження температури в спальні для комфортного сну та підтримання енергозбереження.
        """
        self.system_mode = "NIGHT"
        print("\n" + "="*70)
        print("[RULE ENGINE / НІЧ] АКТИВАЦІЯ СЦЕНАРІЮ: НІЧНИЙ РЕЖИМ")
        print("="*70)

        self.climate.set_zone_mode("bedroom", "eco")
        self.log_event("NIGHT_MODE_ACTIVATED", {"bedroom_mode": "eco"})

    def _switch_to_normal_mode(self):
        """Повернення до штатного режиму."""
        self.system_mode = "NORMAL"
        print("\n[RULE ENGINE] Повернення до штатного режиму COMFORT.")
        for room in self.climate.zones:
            self.climate.set_zone_mode(room, "comfort")
        self.send_command("plug", "kitchen_boiler", "ON")
        self.log_event("NORMAL_MODE_RESTORED", {})

    def run_demonstration_cycle(self):
        """
        Комплексний демонстраційний сценарій для лабораторної перевірки:
        1. Клімат-контроль (гістерезис).
        2. Захист від перевантаження (Smart Energy Plug).
        3. Таймаут руху та перехід у режим відсутності (Away Mode).
        4. Аварія затоплення (перекриття крана + аварійне відключення).
        """
        print("\n==================================================================")
        print(" РОЗПОЧАТО КОМПЛЕКСНУ СИМУЛЯЦІЮ РОЗУМНОГО БУДИНКУ ТА ПРАВИЛ ХАБА ")
        print("==================================================================")

        # Створюємо екземпляр симулятора
        sim = SmartDeviceSimulator(on_telemetry_callback=self.process_incoming_telemetry)

        # Крок 1: Базовий кліматичний моніторинг
        print("\n--- ЕТАП 1: Моніторинг клімату та термостат з гістерезисом ---")
        sim.room_temperatures["living_room"] = 21.2  # Нижче порогу комфорту (22.0 - 0.5 = 21.5)
        sim.generate_sensor_cycle()
        time.sleep(1.0)

        # Крок 2: Нічний сценарій
        print("\n--- ЕТАП 2: Активація сценарію «Нічний режим» ---")
        self.execute_night_mode_scenario()
        sim.generate_sensor_cycle()
        time.sleep(1.0)

        # Крок 3: Симуляція перевантаження струму на розетці
        print("\n--- ЕТАП 3: Захист Smart Plug при критичному стрибку потужності ---")
        sim.trigger_energy_overload(True)
        sim.generate_sensor_cycle()
        sim.trigger_energy_overload(False)
        time.sleep(1.0)

        # Крок 4: Сценарій тривалої відсутності мешканців (Away Mode)
        print("\n--- ЕТАП 4: Сценарій «Енергозбереження при відсутності мешканців» ---")
        sim.set_motion("living_room", False)
        # Штучно зміщуємо час для емуляції таймауту відсутності
        self.safety.last_motion_timestamp = time.time() - (self.safety.timeout + 5)
        if self.safety.check_occupancy_timeout():
            self.execute_away_mode_scenario()
        sim.generate_sensor_cycle()
        time.sleep(1.0)

        # Крок 5: Аварійна подія - Протікання води
        print("\n--- ЕТАП 5: Аварія протікання води та локалізація загрози ---")
        sim.trigger_water_leak(True)
        sim.generate_sensor_cycle()
        sim.trigger_water_leak(False)
        time.sleep(1.0)

        # Підсумок розрахунку споживання енергії
        cost = self.energy.get_total_cost()
        print("\n" + "="*70)
        print(" ПІДСУМОК ЕНЕРГОСПОЖИВАННЯ РОЗУМНОГО БУДИНКУ:")
        for pid, pdata in self.energy.plugs.items():
            print(f"  • Прилад '{pdata['name']}': {pdata['total_kwh']} кВт·год | Стан: {pdata['state']}")
        print(f"  • Розрахункова вартість спожитої енергії (за тарифом 4.32 грн): {cost} грн")
        print(f"  • Логи телеметрії збережено у файл: '{self.log_file}'")
        print("==================================================================\n")


if __name__ == "__main__":
    hub = SmartHomeHub()
    hub.run_demonstration_cycle()
