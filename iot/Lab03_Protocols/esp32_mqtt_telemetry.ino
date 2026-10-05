/*
 * ==============================================================================
 * Практична робота № 3: Протоколи зв'язку в IoT: MQTT, CoAP, HTTP
 * Дисципліна: Технології IoT та SMART-технології
 * Студент: ТАРАС Вадим, група аІк43
 * Викладач: КЛИМЕНКО О.А.
 * 
 * Опис програми:
 * Програмний модуль для мікроконтролера ESP32 DevKit v1, що реалізує надійну
 * передачу телеметричних даних через брокер MQTT (Publish/Subscribe) з використанням
 * структурованих JSON-повідомлень, механізму Last Will and Testament (LWT),
 * автоматичного перепідключення до Wi-Fi та MQTT, а також двонаправленого
 * асинхронного керування виконавчими механізмами (реле та світлодіодом).
 * ==============================================================================
 */

#include <WiFi.h>
#include <PubSubClient.h>
#include <ArduinoJson.h>

// --- Параметри підключення до бездротової мережі Wi-Fi ---
const char* WIFI_SSID = "Wokwi-GUEST";       // Назва точки доступу (для Wokwi або локального роутера)
const char* WIFI_PASS = "";                  // Пароль точки доступу

// --- Параметри MQTT-брокера ---
// Для тестування можна використовувати публічний брокер broker.emqx.io або локальний Eclipse Mosquitto
const char* MQTT_SERVER = "broker.emqx.io";
const int   MQTT_PORT   = 1883;
const char* MQTT_USER   = "";                // Ім'я користувача (якщо увімкнено автентифікацію)
const char* MQTT_PASS   = "";                // Пароль користувача
const char* CLIENT_ID   = "ESP32_Taras_SensorNode_01";

// --- Дерево топіків MQTT ---
const char* TOPIC_TELEMETRY = "devices/esp32_sensor_01/telemetry"; // Публікація сенсорних даних
const char* TOPIC_STATUS    = "devices/esp32_sensor_01/status";    // LWT та стан доступності вузла
const char* TOPIC_CONTROL   = "devices/esp32_sensor_01/control";   // Підписка на команди керування
const char* TOPIC_STATE     = "devices/esp32_sensor_01/state";     // Публікація підтвердження зміни стану

// --- Апаратна конфігурація пінів введення-виведення (GPIO) ---
const int PIN_RELAY = 4;   // Модуль реле 5V (керування навантаженням)
const int PIN_LED   = 2;   // Вбудований індикаторний світлодіод
const int PIN_LDR   = 34;  // Аналоговий вхід фоторезистора (ADC1_CH6)

// --- Глобальні мережеві об'єкти ---
WiFiClient espClient;
PubSubClient mqttClient(espClient);

// --- Глобальні таймери та стан системи ---
unsigned long lastTelemetryTime = 0;
const unsigned long TELEMETRY_INTERVAL_MS = 5000; // Інтервал публікації телеметрії (5 секунд)

bool relayState = false;
bool ledState   = false;
unsigned long messageCounter = 0;

// --- Прототипи допоміжних функцій ---
void setupWiFi();
void reconnectMQTT();
void mqttCallback(char* topic, byte* payload, unsigned int length);
void publishTelemetry();
void publishStateFeedback(const char* triggerReason);

void setup() {
  // Ініціалізація апаратного послідовного порту для логування (115200 бод)
  Serial.begin(115200);
  delay(500);

  Serial.println("\n========================================================");
  Serial.println(" ESP32 IoT MQTT Telemetry Client (Лабораторна робота 3)");
  Serial.println(" Виконавець: студент гр. аІк43 Тарас Вадим");
  Serial.println("========================================================");

  // Конфігурація пінів периферії
  pinMode(PIN_RELAY, OUTPUT);
  pinMode(PIN_LED, OUTPUT);
  pinMode(PIN_LDR, INPUT);

  // Початковий безпечний стан актуаторів (вимкнено)
  digitalWrite(PIN_RELAY, LOW);
  digitalWrite(PIN_LED, LOW);

  // Налаштування зв'язку з Wi-Fi
  setupWiFi();

  // Налаштування параметрів MQTT-клієнта
  mqttClient.setServer(MQTT_SERVER, MQTT_PORT);
  mqttClient.setCallback(mqttCallback);
  mqttClient.setBufferSize(512); // Збільшуємо розмір буфера для повноцінних JSON-пакетів
}

void loop() {
  // Перевірка активності з'єднання з Wi-Fi
  if (WiFi.status() != WL_CONNECTED) {
    setupWiFi();
  }

  // Перевірка активності сесії з MQTT-брокером
  if (!mqttClient.connected()) {
    reconnectMQTT();
  }

  // Обробка вхідних мережевих пакетів та утримання Keep-Alive пінгу
  mqttClient.loop();

  // Періодична неблокуюча відправка телеметричних даних за таймером millis()
  unsigned long currentMillis = millis();
  if (currentMillis - lastTelemetryTime >= TELEMETRY_INTERVAL_MS) {
    lastTelemetryTime = currentMillis;
    publishTelemetry();
  }
}

/**
 * Ініціалізація та встановлення підключення до бездротової мережі Wi-Fi.
 */
void setupWiFi() {
  delay(10);
  Serial.print("[Wi-Fi] Підключення до SSID: ");
  Serial.println(WIFI_SSID);

  WiFi.mode(WIFI_STA);
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  int attempts = 0;
  while (WiFi.status() != WL_CONNECTED && attempts < 30) {
    delay(500);
    Serial.print(".");
    attempts++;
  }

  if (WiFi.status() == WL_CONNECTED) {
    Serial.println("\n[Wi-Fi] З'єднання успішно встановлено!");
    Serial.print("[Wi-Fi] Отримана IP-адреса: ");
    Serial.println(WiFi.localIP());
    Serial.print("[Wi-Fi] Рівень сигналу (RSSI): ");
    Serial.print(WiFi.RSSI());
    Serial.println(" dBm");
  } else {
    Serial.println("\n[Wi-Fi] Помилка підключення! Повторна спроба у наступному циклі.");
  }
}

/**
 * Відновлення з'єднання з MQTT-брокером та налаштування механізму LWT.
 */
void reconnectMQTT() {
  while (!mqttClient.connected()) {
    Serial.print("[MQTT] Підключення до брокера: ");
    Serial.print(MQTT_SERVER);
    Serial.print(":");
    Serial.println(MQTT_PORT);

    // Створення LWT-повідомлення ("Заповіту").
    // Якщо вузол втратить зв'язок (аварійний обрив TCP), брокер автоматично
    // опублікує це повідомлення з прапорцем retain=true та рівнем QoS=1.
    const char* willTopic = TOPIC_STATUS;
    const int willQoS = 1;
    const bool willRetain = true;
    const char* willMessage = "{\"status\":\"offline\",\"reason\":\"unexpected_disconnect\"}";

    // Спроба підключення з реєстрацією LWT
    bool isConnected = false;
    if (strlen(MQTT_USER) > 0) {
      isConnected = mqttClient.connect(CLIENT_ID, MQTT_USER, MQTT_PASS, willTopic, willQoS, willRetain, willMessage);
    } else {
      isConnected = mqttClient.connect(CLIENT_ID, willTopic, willQoS, willRetain, willMessage);
    }

    if (isConnected) {
      Serial.println("[MQTT] Успішно підключено до брокера!");

      // 1. Оновлення статусу доступності вузла (online) з прапорцем retain=true
      StaticJsonDocument<128> docOnline;
      docOnline["status"] = "online";
      docOnline["ip"] = WiFi.localIP().toString();
      docOnline["firmware"] = "v1.3.0";
      docOnline["free_heap"] = ESP.getFreeHeap();

      char onlinePayload[128];
      serializeJson(docOnline, onlinePayload);
      mqttClient.publish(TOPIC_STATUS, onlinePayload, true); // retain = true
      Serial.println("[MQTT] Статус 'online' опубліковано у топік статусу (retain=true).");

      // 2. Підписка на топік команд керування з гарантією доставки QoS 1
      mqttClient.subscribe(TOPIC_CONTROL, 1);
      Serial.print("[MQTT] Оформлено підписку на топік керування: ");
      Serial.println(TOPIC_CONTROL);

      // 3. Відправка поточного стану актуаторів
      publishStateFeedback("connect");
    } else {
      Serial.print("[MQTT] Помилка підключення, rc=");
      Serial.print(mqttClient.state());
      Serial.println(" -> очікування 4 секунди перед повтором...");
      delay(4000);
    }
  }
}

/**
 * Колбек-функція для обробки вхідних повідомлень за підпискою.
 */
void mqttCallback(char* topic, byte* payload, unsigned int length) {
  Serial.print("\n[MQTT Rx] Отримано повідомлення у топіку: ");
  Serial.println(topic);

  // Копіювання payload у буфер рядка
  char messageBuffer[256];
  if (length >= sizeof(messageBuffer)) {
    length = sizeof(messageBuffer) - 1;
  }
  memcpy(messageBuffer, payload, length);
  messageBuffer[length] = '\0';

  Serial.print("[MQTT Rx] Корисне навантаження (Payload): ");
  Serial.println(messageBuffer);

  // Спроба розбору як структурованого JSON
  StaticJsonDocument<256> docCmd;
  DeserializationError error = deserializeJson(docCmd, messageBuffer);

  if (!error) {
    // Обробка JSON-команд: наприклад {"relay": true, "led": false}
    if (docCmd.containsKey("relay")) {
      relayState = docCmd["relay"].as<bool>();
      digitalWrite(PIN_RELAY, relayState ? HIGH : LOW);
      Serial.printf("[АКТУАТОР] Стан реле оновлено: %s\n", relayState ? "УВІМКНЕНО" : "ВИМКНЕНО");
    }
    if (docCmd.containsKey("led")) {
      ledState = docCmd["led"].as<bool>();
      digitalWrite(PIN_LED, ledState ? HIGH : LOW);
      Serial.printf("[АКТУАТОР] Стан LED оновлено: %s\n", ledState ? "УВІМКНЕНО" : "ВИМКНЕНО");
    }
  } else {
    // Обробка текстових команд простого формату
    String cmdStr = String(messageBuffer);
    cmdStr.trim();

    if (cmdStr.equalsIgnoreCase("RELAY_ON") || cmdStr.equalsIgnoreCase("ON")) {
      relayState = true;
      digitalWrite(PIN_RELAY, HIGH);
    } else if (cmdStr.equalsIgnoreCase("RELAY_OFF") || cmdStr.equalsIgnoreCase("OFF")) {
      relayState = false;
      digitalWrite(PIN_RELAY, LOW);
    } else if (cmdStr.equalsIgnoreCase("LED_ON")) {
      ledState = true;
      digitalWrite(PIN_LED, HIGH);
    } else if (cmdStr.equalsIgnoreCase("LED_OFF")) {
      ledState = false;
      digitalWrite(PIN_LED, LOW);
    } else {
      Serial.println("[MQTT Rx] Невідомий формат команди!");
    }
  }

  // Публікація зворотного підтвердження про новий актуальний стан
  publishStateFeedback("command_processed");
}

/**
 * Публікація періодичних сенсорних даних телеметрії.
 */
void publishTelemetry() {
  messageCounter++;

  // Симуляція та зчитування фізичних величин
  // Аналогове зчитування освітленості з фоторезистора
  int rawLdr = analogRead(PIN_LDR);
  float lightPercent = (rawLdr / 4095.0f) * 100.0f;

  // Симуляція даних мікроклімату (температура та відносна вологість)
  // Базується на циклічній математичній моделі з шумом для реалістичності
  float baseTemp = 22.5f;
  float tempVariation = 2.0f * sin((millis() / 1000.0f) * 0.05f);
  float temperature = baseTemp + tempVariation + ((random(-10, 10)) / 50.0f);

  float baseHum = 58.0f;
  float humVariation = 5.0f * cos((millis() / 1000.0f) * 0.05f);
  float humidity = baseHum + humVariation;

  // Формування JSON-пакета
  StaticJsonDocument<384> doc;
  doc["seq"]          = messageCounter;
  doc["device_id"]    = CLIENT_ID;
  doc["temp_c"]       = round(temperature * 10.0f) / 10.0f;
  doc["humidity_pct"] = round(humidity * 10.0f) / 10.0f;
  doc["light_pct"]    = round(lightPercent * 10.0f) / 10.0f;
  doc["relay"]        = relayState;
  doc["led"]          = ledState;
  doc["rssi_dbm"]     = WiFi.RSSI();
  doc["uptime_sec"]   = millis() / 1000;
  doc["free_heap"]    = ESP.getFreeHeap();

  char buffer[384];
  size_t bytesWritten = serializeJson(doc, buffer);

  // Публікація телеметрії з рівнем QoS 0 (періодичні потокові дані)
  bool success = mqttClient.publish(TOPIC_TELEMETRY, buffer, false);

  if (success) {
    Serial.printf("[MQTT Tx] #%lu Телеметрію опубліковано (%d байт):\n", messageCounter, bytesWritten);
    Serial.printf("          T=%.1f°C | H=%.1f%% | Lux=%.1f%% | Relay=%d | Heap=%u B\n",
                  temperature, humidity, lightPercent, relayState, ESP.getFreeHeap());
  } else {
    Serial.println("[MQTT Tx] Помилка публікації телеметрії!");
  }
}

/**
 * Публікація підтвердження поточного апаратного стану вузла.
 */
void publishStateFeedback(const char* triggerReason) {
  StaticJsonDocument<256> doc;
  doc["device_id"] = CLIENT_ID;
  doc["trigger"]   = triggerReason;
  doc["relay"]     = relayState;
  doc["led"]       = ledState;
  doc["timestamp"] = millis();

  char buffer[256];
  serializeJson(doc, buffer);

  // Публікація стану з retain=true (щоб нові підписники відразу дізнавалися стан)
  mqttClient.publish(TOPIC_STATE, buffer, true);
  Serial.printf("[MQTT Tx] Стан системи зафіксовано (тригер: %s, retain=true)\n", triggerReason);
}
