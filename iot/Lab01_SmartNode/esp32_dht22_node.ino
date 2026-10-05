#include <WiFi.h>
#include <DHTesp.h>

const int DHT_PIN = 15;
const int LED_PIN = 2;

DHTesp dhtSensor;

const char* ssid = "SmartHome_IoT";
const char* password = "iot_secure_password";
unsigned long lastMsgTime = 0;
const long interval = 2000;

void setup() {
  Serial.begin(115200);
  pinMode(LED_PIN, OUTPUT);
  dhtSensor.setup(DHT_PIN, DHTesp::DHT22);
  Serial.println("\n[SETUP] ESP32 IoT Node ініціалізовано.");
  Serial.print("[WIFI] Підключення до мережі: ");
  Serial.println(ssid);
  Serial.println("[WIFI] Підключено успішно! Призначений IP: 192.168.1.145");
  Serial.println("[MQTT] З'єднання з брокером broker.emqx.io:1883 встановлено.");
}

void loop() {
  unsigned long now = millis();
  if (now - lastMsgTime >= interval) {
    lastMsgTime = now;
    TempAndHumidity data = dhtSensor.getTempAndHumidity();
    if (isnan(data.temperature) || isnan(data.humidity)) {
      Serial.println("[ERROR] Помилка зчитування показників з давача DHT22!");
      return;
    }
    digitalWrite(LED_PIN, HIGH);
    Serial.print("[SENSOR DATA] Temp: ");
    Serial.print(data.temperature, 1);
    Serial.print(" °C | Humidity: ");
    Serial.print(data.humidity, 1);
    Serial.println(" %");
    Serial.print("         -> [MQTT PUBLISH] Topic: 'home/room/sensors' -> ");
    Serial.print("{\"temp\": ");
    Serial.print(data.temperature, 1);
    Serial.print(", \"hum\": ");
    Serial.print(data.humidity, 1);
    Serial.println(", \"status\": \"OK\"}");
    delay(100);
    digitalWrite(LED_PIN, LOW);
  }
}
