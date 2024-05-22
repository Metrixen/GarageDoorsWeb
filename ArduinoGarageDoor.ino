#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>
#include <ESP8266HTTPClient.h>

// WiFi credentials
const char* ssid = "SSID";
const char* password = "password";

// Server URLs
const char* loginUrl = "/login";  // Login URL endpoint
const char* statusUrl = "/";  // Status page URL

const char* host = "192.168.0.100";  // Host URL

// Login credentials
const char* username = "controller";
const char* userPassword = "123";

const int numDoors = 2;  // Number of doors (adjust as needed)
int relayPins[] = {2, 4};  // Relay pins for each door (e.g., GPIO 2 and GPIO 4)
String doorNames[] = {"Vrataata", "vtorata"};  // Names of the doors as they appear in the HTML
String lastStatus[numDoors];  // Last status for each door

WiFiClientSecure wifiClient;
String jwtToken;  // Variable to store the JWT token

void setup() {
  for (int i = 0; i < numDoors; i++) {
    pinMode(relayPins[i], OUTPUT);
    digitalWrite(relayPins[i], HIGH);  // Initially turn off the relay (Normally High)
    lastStatus[i] = "";  // Initialize last status
  }

  Serial.begin(115200);
  WiFi.begin(ssid, password);
  Serial.println("Connecting to WiFi...");

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\nWIFI CONNECTED.");

  // Perform login
  if (login()) {
    Serial.println("LOGIN SUCCESSFUL.");
  } else {
    Serial.println("LOGIN FAILED.");
  }
}

void loop() {
  if (jwtToken.length() > 0) {
    fetchPageContent(statusUrl);
  } else {
    Serial.println("No valid session. Attempting to re-login...");
    login();  // Re-attempt login if no valid session
  }

  delay(2000);  // Wait for 2 seconds before sending the next request
}

bool login() {
  wifiClient.setInsecure();  // Use this only for testing with self-signed certificates

  if (!wifiClient.connect(host, 7136)) {
    Serial.println("Connection failed");
    return false;
  }

  String postData = "username=" + String(username) + "&password=" + String(userPassword);
  String request = String("POST ") + loginUrl + " HTTP/1.1\r\n" +
                   "Host: " + host + "\r\n" +
                   "Content-Type: application/x-www-form-urlencoded\r\n" +
                   "Content-Length: " + postData.length() + "\r\n" +
                   "Connection: close\r\n\r\n" +
                   postData;

  wifiClient.print(request);

  // Wait for response and read headers
  bool headersEnded = false;
  while (wifiClient.connected()) {
    String line = wifiClient.readStringUntil('\n');
    if (line == "\r") {
      headersEnded = true;
      break;
    }
    if (line.startsWith("Set-Cookie:")) {
      int start = line.indexOf("jwt=");
      int end = line.indexOf(";", start);
      if (start != -1 && end != -1) {
        jwtToken = line.substring(start + 4, end);
        Serial.println("JWT TOKEN EXTRACTED.");
      }
    }
  }

  // Read response body
  wifiClient.readString();

  wifiClient.stop();

  if (jwtToken.length() > 0) {
    return true;
  } else {
    Serial.println("Failed to extract JWT token from cookie");
    return false;
  }
}

void fetchPageContent(const char* url) {
  wifiClient.setInsecure();  // Use this only for testing with self-signed certificates

  if (!wifiClient.connect(host, 7136)) {
    Serial.println("Connection failed");
    return;
  }

  String request = String("GET ") + url + " HTTP/1.1\r\n" +
                   "Host: " + host + "\r\n" +
                   "Authorization: Bearer " + jwtToken + "\r\n" +
                   "Connection: close\r\n\r\n";

  wifiClient.print(request);

  while (wifiClient.connected()) {
    String line = wifiClient.readStringUntil('\n');
    if (line == "\r") {
      break;
    }
  }

  // Read response
  String content = wifiClient.readString();
  parseAndProcessPayload(content);

  wifiClient.stop();
}

void parseAndProcessPayload(const String& payload) {
  for (int i = 0; i < numDoors; i++) {
    String doorName = doorNames[i];
    int index = payload.indexOf("<td>" + doorName + "</td>");
    if (index != -1) {
      int statusIndex = payload.indexOf("<td>", index + doorName.length() + 9);  // Length of "<td>" and "</td>"
      int statusEnd = payload.indexOf("</td>", statusIndex + 4);
      String status = payload.substring(statusIndex + 4, statusEnd);
      status.trim();  // Modify the status in place

      Serial.print("Status of ");
      Serial.print(doorName);
      Serial.print(": ");
      Serial.println(status);

      if (status != lastStatus[i]) {
        Serial.print("Status changed for ");
        Serial.println(doorName);
        triggerRelay(i);  // Trigger relay for the door
        lastStatus[i] = status;
      }
    }
  }
}

void triggerRelay(int doorIndex) {
  Serial.print("Triggering relay for door ");
  Serial.println(doorIndex);
  digitalWrite(relayPins[doorIndex], LOW);  // Trigger the relay
  delay(1000);  // Relay is activated for 1 second
  digitalWrite(relayPins[doorIndex], HIGH);  // Turn off the relay
}
