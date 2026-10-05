"""
Симулятор розумних пристроїв та сенсорних вузлів для розумного будинку (SMART Home Simulator)
Дисципліна: «Технології IoT та SMART-технології»
Практична робота 4: «SMART-технології у побуті: системи автоматизації будинку та інтеграція розумних побутових приладів»
Студент: ТАРАС Вадим, група аІк43

Опис:
Клас DeviceSimulator емулює поведінку фізичних IoT-сенсорів та виконавчих механізмів (актуаторів):
1. Сенсори температури та вологості (Living Room, Bedroom).
2. Датчики присутності/руху (PIR Sensors).
3. Датчик аварійного затоплення (Water Leakage Sensor).
4. Розумні силові розетки (Smart Energy Plugs) з вимірюванням напруги, струму та активної потужності.
5. Сервоприводний кульовий кран перекриття води (Motorized Shutoff Valve).
6. Електричні обігрівачі (Actuators).
"""

import json
import random
import time
import threading
from typing import Dict, Any, Callable, Optional

# Перевірка наявності бібліотеки paho-mqtt
try:
    import paho.mqtt.client as mqtt
    MQTT_AVAILABLE = True
except ImportError:
    MQTT_AVAILABLE = False


class SmartDeviceSimulator:
    """
    Симулятор фізичних IoT-пристроїв побутової автоматизації.
    Підтримує публікацію телеметрії та обробку команд керування.
    """

    def __init__(self, config_path: str = "config.json", on_telemetry_callback: Optional[Callable[[str, Dict[str, Any]], None]] = None):
        """Ініціалізація симулятора пристроїв з конфігураційного файлу."""
        with open(config_path, "r", encoding="utf-8") as f:
            self.config = json.load(f)

        self.topic_base = self.config["system"]["topic_base"]
        self.on_telemetry_callback = on_telemetry_callback
        self.running = False

        # Стан виконавчих механізмів (актуаторів)
        self.actuators_state = {
            "heaters": {
                "living_room": False,
                "bedroom": False
            },
            "valves": {
                "main_water_valve": "OPEN"
            },
            "plugs": {
                "kitchen_boiler": {"state": "ON", "power_w": 1800.0, "total_kwh": 12.45},
                "washing_machine": {"state": "ON", "power_w": 650.0, "total_kwh": 4.12}
            }
        }

        # Базові кліматичні показники (для реалістичної динаміки)
        self.room_temperatures = {
            "living_room": 21.0,
            "bedroom": 19.5
        }
        self.room_humidity = {
            "living_room": 45.0,
            "bedroom": 50.0
        }

        # Аварійні прапорці для симуляції подій
        self.simulate_leakage = False
        self.simulate_overload = False
        self.motion_detected = {"living_room": True, "corridor": False}

        # MQTT клієнт
        self.mqtt_client = None
        if MQTT_AVAILABLE:
            self._setup_mqtt()

    def _setup_mqtt(self):
        """Налаштування клієнта MQTT протоколу."""
        client_id = f"Simulator_{random.randint(1000, 9999)}"
        try:
            # Сумісність з різними версіями paho-mqtt (v1 та v2)
            if hasattr(mqtt, "CallbackAPIVersion"):
                self.mqtt_client = mqtt.Client(mqtt.CallbackAPIVersion.VERSION1, client_id)
            else:
                self.mqtt_client = mqtt.Client(client_id)

            self.mqtt_client.on_connect = self._on_connect
            self.mqtt_client.on_message = self._on_message
        except Exception as e:
            print(f"[Симулятор] Попередження ініціалізації MQTT: {e}")
            self.mqtt_client = None

    def _on_connect(self, client, userdata, flags, rc):
        """Обробник підключення до MQTT брокера."""
        if rc == 0:
            print(f"[Симулятор] Успішно підключено до MQTT-брокера ({self.config['system']['mqtt_broker']}).")
            # Підписка на теми команд від розумного хаба
            cmd_topic = f"{self.topic_base}/commands/#"
            client.subscribe(cmd_topic)
            print(f"[Симулятор] Оформлено підписку на команди хаба: {cmd_topic}")
        else:
            print(f"[Симулятор] Помилка з'єднання з брокером. Код: {rc}")

    def _on_message(self, client, userdata, msg):
        """Обробник вхідних команд керування від Smart Hub."""
        try:
            topic = msg.topic
            payload = json.loads(msg.payload.decode("utf-8"))
            self.handle_command(topic, payload)
        except Exception as ex:
            print(f"[Симулятор] Помилка обробки команди: {ex}")

    def handle_command(self, topic: str, command: Dict[str, Any]):
        """
        Обробка вхідної керуючої директиви для актуатора.
        Формат теми: smarthome/commands/{category}/{device_id}
        """
        parts = topic.split("/")
        if len(parts) >= 4:
            category = parts[2]
            device_id = parts[3]
        else:
            category = command.get("category", "")
            device_id = command.get("device_id", "")

        action = command.get("action", "")

        # 1. Керування краном подачі води
        if category == "valve" and device_id in self.actuators_state["valves"]:
            self.actuators_state["valves"][device_id] = action
            print(f"[Симулятор АКТУАТОР] Кран '{device_id}' переведено у стан: {action}")

        # 2. Керування обігрівачами (клімат-контроль)
        elif category == "heater" and device_id in self.actuators_state["heaters"]:
            new_state = (action == "ON")
            self.actuators_state["heaters"][device_id] = new_state
            state_str = "УВІМКНЕНО" if new_state else "ВИМКНЕНО"
            print(f"[Симулятор АКТУАТОР] Обігрівач '{device_id}' переведено у стан: {state_str}")

        # 3. Керування розумними розетками
        elif category == "plug" and device_id in self.actuators_state["plugs"]:
            self.actuators_state["plugs"][device_id]["state"] = action
            if action == "OFF":
                self.actuators_state["plugs"][device_id]["power_w"] = 0.0
            print(f"[Симулятор АКТУАТОР] Розумна розетка '{device_id}' переведена у стан: {action}")

    def emit_telemetry(self, subtopic: str, data: Dict[str, Any]):
        """Надсилання телеметричних даних через MQTT та/або внутрішній колбек."""
        full_topic = f"{self.topic_base}/telemetry/{subtopic}"
        data["timestamp"] = time.time()
        payload_str = json.dumps(data, ensure_ascii=False)

        # Публікація через MQTT брокер
        if self.mqtt_client and self.mqtt_client.is_connected():
            self.mqtt_client.publish(full_topic, payload_str)

        # Передача через локальний колбек (для автономного режиму або тестів)
        if self.on_telemetry_callback:
            self.on_telemetry_callback(full_topic, data)

    def generate_sensor_cycle(self):
        """Один такт генерації показників з урахуванням термодинаміки та подій."""
        # 1. Оновлення температури на основі роботи нагрівачів
        for room, is_heating in self.actuators_state["heaters"].items():
            if is_heating:
                self.room_temperatures[room] += random.uniform(0.1, 0.25)
            else:
                self.room_temperatures[room] -= random.uniform(0.05, 0.15)
            
            # Телеметрія клімату
            climate_data = {
                "room": room,
                "temperature": round(self.room_temperatures[room], 2),
                "humidity": round(self.room_humidity[room] + random.uniform(-0.5, 0.5), 1),
                "heater_active": self.actuators_state["heaters"][room]
            }
            self.emit_telemetry(f"climate/{room}", climate_data)

        # 2. Сенсори присутності / руху
        for room, has_motion in self.motion_detected.items():
            motion_data = {
                "sensor_id": f"{room}_motion_01",
                "room": room,
                "motion": has_motion
            }
            self.emit_telemetry(f"motion/{room}", motion_data)

        # 3. Сенсор протікання води
        leak_data = {
            "sensor_id": "bathroom_leak_01",
            "zone": "bathroom",
            "leak_detected": self.simulate_leakage
        }
        self.emit_telemetry("safety/water_leak", leak_data)

        # 4. Розумні розетки (Smart Energy Plugs)
        for plug_id, plug_info in self.actuators_state["plugs"].items():
            voltage = round(random.uniform(227.0, 233.0), 1)
            if plug_info["state"] == "ON":
                # Якщо активовано режим симуляції перевантаження
                if self.simulate_overload and plug_id == "kitchen_boiler":
                    current_power = 2750.0 + random.uniform(50, 150)
                else:
                    current_power = plug_info["power_w"] + random.uniform(-20, 20)
                
                current_amps = round(current_power / voltage, 2)
                # Приріст споживання кВт·год
                plug_info["total_kwh"] += (current_power / 1000.0) * (2.0 / 3600.0)
            else:
                current_power = 0.0
                current_amps = 0.0

            plug_data = {
                "plug_id": plug_id,
                "relay_state": plug_info["state"],
                "voltage_v": voltage,
                "current_a": current_amps,
                "active_power_w": round(current_power, 1),
                "total_energy_kwh": round(plug_info["total_kwh"], 4)
            }
            self.emit_telemetry(f"energy/{plug_id}", plug_data)

        # 5. Стан кульового крана
        valve_data = {
            "valve_id": "main_water_valve",
            "state": self.actuators_state["valves"]["main_water_valve"]
        }
        self.emit_telemetry("safety/valve_status", valve_data)

    def trigger_water_leak(self, active: bool = True):
        """Імітація аварійного затоплення (сигнал датчика вологи)."""
        self.simulate_leakage = active
        status = "ВИЯВЛЕНО ВОДУ (АВАРІЯ)!" if active else "В нормі (сухо)."
        print(f"\n[СИМУЛЯТОР ПОДІЙ] Зміна стану датчика протікання: {status}")

    def trigger_energy_overload(self, active: bool = True):
        """Імітація стрибка навантаження на розетці бойлера."""
        self.simulate_overload = active
        status = "ПЕРЕВАНТАЖЕННЯ ПОТУЖНОСТІ (>2700 Вт)!" if active else "Штатне навантаження."
        print(f"\n[СИМУЛЯТОР ПОДІЙ] Зміна навантаження бойлера: {status}")

    def set_motion(self, room: str, detected: bool):
        """Зміна стану датчика присутності в кімнаті."""
        self.motion_detected[room] = detected
        status = "Рух виявлено" if detected else "Спокій (порожньо)"
        print(f"[СИМУЛЯТОР ПОДІЙ] Датчик руху {room}: {status}")

    def start_background_loop(self, interval: float = 2.0):
        """Запуск фонового циклу генерації телеметрії."""
        self.running = True

        def _loop():
            while self.running:
                self.generate_sensor_cycle()
                time.sleep(interval)

        thread = threading.Thread(target=_loop, daemon=True)
        thread.start()

    def stop(self):
        """Зупинка симулятора."""
        self.running = False
        if self.mqtt_client and self.mqtt_client.is_connected():
            self.mqtt_client.disconnect()


if __name__ == "__main__":
    print("=== ЗАПУСК АВТОНОМНОГО СИМУЛЯТОРА РОЗУМНОГО БУДИНКУ ===")
    sim = SmartDeviceSimulator()
    print("Симулятор ініціалізовано. Початок демонстраційного циклу телеметрії...")
    try:
        for i in range(3):
            sim.generate_sensor_cycle()
            time.sleep(1.0)
        print("Тестову генерацію завершено успішно.")
    except KeyboardInterrupt:
        sim.stop()
