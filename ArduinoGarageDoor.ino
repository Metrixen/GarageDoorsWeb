#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

const char* ssid = "A1_1P6BF";  // Your WiFi SSID
const char* password = "kakeparolata";  // Your WiFi Password
const char* url = "https://192.168.0.100:7136/status";  // Server URL

int relayPin = 2;  // Relay connected to GPIO pin
String lastPayload = "";  // Variable to store the last payload for comparison
unsigned long lastChangeTime = 0;  // Time of the last payload change
bool isTimerActive = false;  // Is the timer active?

void setup() {
  pinMode(relayPin, OUTPUT);  // Set relay pin as an output
  digitalWrite(relayPin, HIGH);  // Initially turn off the relay (Normally High)

  Serial.begin(115200);
  WiFi.begin(ssid, password);
  Serial.println("Connecting to WiFi...");

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("WiFi Connected.");
}

void loop() {
  HTTPClient http;
  WiFiClientSecure wifiClient;
  wifiClient.setInsecure();  // Use this only for testing with self-signed certificates

  http.begin(wifiClient, url);
  Serial.println("Sending HTTP GET request...");

  int httpCode = http.GET();
  if (httpCode > 0) {
    String payload = http.getString();
    Serial.println("Received payload:");
    Serial.println(payload);
    
    if (payload != lastPayload) {
      if (isTimerActive && (millis() - lastChangeTime) < 71000) {
        // If the timer is active and it's been less than 71 seconds
        Serial.println("Double Trigger because of payload change within 71 seconds.");
        triggerRelayTwice();
      } else {
        // Start the timer
        lastChangeTime = millis();
        isTimerActive = true;
        Serial.println("Payload changed, timer started.");
        triggerRelay();
      }
      lastPayload = payload;
    }
  } else {
    Serial.print("HTTP GET failed, error code: ");
    Serial.println(httpCode);
    digitalWrite(relayPin, HIGH);  // Ensure relay is off if communication fails
  }
  http.end();

  delay(2000);  // Wait for 2 seconds before sending the next request
}

void triggerRelay() {
  digitalWrite(relayPin, LOW);  // Trigger the relay
  delay(1000);  // Relay is activated for 1 second
  digitalWrite(relayPin, HIGH);  // Turn off the relay
}

void triggerRelayTwice() {
  for (int i = 0; i < 2; i++) {
    digitalWrite(relayPin, LOW);
    delay(1000);  // Relay on for 1 second
    digitalWrite(relayPin, HIGH);
    if (i == 0) delay(500);  // Pause between double triggers
  }
}
