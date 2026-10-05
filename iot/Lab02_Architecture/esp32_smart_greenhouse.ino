#include <WiFi.h>
#include <PubSubClient.h>
#include <DHTesp.h>
#include <ArduinoJson.h>

// Конфігурація апаратних пінів
const int PIN_DHT   = 15;
const int PIN_RELAY = 4;
const int PIN_LED   = 2;

// Параметри бездротової мережі та MQTT-брокера
const char* ssid        = "College_IoT_Network";
const char* password    = "SmartCollege2026";
const char* mqtt_server = "broker.emqx.io";
const int   mqtt_port   = 1883;
const char* client_id   = "ESP32_Greenhouse_Taras_aIk43";

// MQTT топіки відповідно до ієрархічної структури
const char* topic_telemetry = "greenhouse/node1/telemetry";
const char* topic_relay_cmd = "greenhouse/node1/relay/cmd";

WiFiClient espClient;
PubSubClient client(espClient);
DHTesp dhtSensor;

unsigned long lastMsgTime = 0;
const long interval = 5000; // Період публікації телеметрії — 5 секунд

// Обробник вхідних MQTT-повідомлень (Callback)
void mqttCallback(char* topic, byte* payload, unsigned int length) {
  String message = "";
  for (unsigned int i = 0; i < length; i++) {
    message += (char)payload[i];
  }
  
  Serial.print("[MQTT-IN] Вхідна команда в топік: ");
  Serial.print(topic);
  Serial.print(" | Дані: ");
  Serial.println(message);

  if (String(topic) == topic_relay_cmd) {
    if (message == "ON" || message.indexOf("\"state\":\"ON\"") >= 0) {
      digitalWrite(PIN_RELAY, HIGH);
      digitalWrite(PIN_LED, HIGH);
      Serial.println("[ACTION] Силове реле АКТИВОВАНО (вентиляція увімкнена).");
    } else if (message == "OFF" || message.indexOf("\"state\":\"OFF\"") >= 0) {
      digitalWrite(PIN_RELAY, LOW);
      digitalWrite(PIN_LED, LOW);
      Serial.println("[ACTION] Силове реле ДЕАКТИВОВАНО (вентиляція вимкнена).");
    }
  }
}

void setupWifi() {
  delay(10);
  Serial.print("\n[WIFI] Підключення до мережі: ");
  Serial.println(ssid);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\n[WIFI] Підключено успішно!");
  Serial.print("[WIFI] Призначена IP-адреса: ");
  Serial.println(WiFi.localIP());
}

void reconnectMqtt() {
  while (!client.connected()) {
    Serial.print("[MQTT] Спроба підключення до брокера...");
    if (client.connect(client_id)) {
      Serial.println(" Успішно підключено!");
      // Підписка на топік керування реле з якістю обслуговування QoS 1
      client.subscribe(topic_relay_cmd, 1);
      Serial.print("[MQTT] Підписано на топік команд: ");
      Serial.println(topic_relay_cmd);
    } else {
      Serial.print(" Помилка, rc=");
      Serial.print(client.state());
      Serial.println(" Повторна спроба через 3 секунди...");
      delay(3000);
    }
  }
}

void setup() {
  Serial.begin(115200);
  
  pinMode(PIN_RELAY, OUTPUT);
  pinMode(PIN_LED, OUTPUT);
  digitalWrite(PIN_RELAY, LOW);
  digitalWrite(PIN_LED, LOW);

  dhtSensor.setup(PIN_DHT, DHTesp::DHT22);
  Serial.println("[INIT] Сенсор DHT22 та актуатори ініціалізовано.");

  setupWifi();
  client.setServer(mqtt_server, mqtt_port);
  client.setCallback(mqttCallback);
}

void loop() {
  if (!client.connected()) {
    reconnectMqtt();
  }
  client.loop();

  unsigned long now = millis();
  if (now - lastMsgTime >= interval) {
    lastMsgTime = now;

    TempAndHumidity data = dhtSensor.getTempAndHumidity();

    if (isnan(data.temperature) || isnan(data.humidity)) {
      Serial.println("[ERROR] Помилка зчитування даних з сенсора DHT22!");
      return;
    }

    // Симуляція зчитування аналогового датчика вологості ґрунту (64%)
    int soilMoisture = 64;
    int relayState = digitalRead(PIN_RELAY);

    // Формування JSON-пакета для публікації телеметрії
    StaticJsonDocument<256> doc;
    doc["device_id"] = client_id;
    doc["temp"]      = serialized(String(data.temperature, 1));
    doc["hum"]       = serialized(String(data.humidity, 1));
    doc["soil"]      = soilMoisture;
    doc["relay"]     = relayState;
    doc["uptime_s"]  = now / 1000;

    char jsonBuffer[256];
    serializeJson(doc, jsonBuffer);

    Serial.print("[MQTT-PUB] Відправка в '");
    Serial.print(topic_telemetry);
    Serial.print("': ");
    Serial.println(jsonBuffer);

    client.publish(topic_telemetry, jsonBuffer);
  }
}
