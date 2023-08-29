#include <Arduino.h>
#include "WiFi.h"
#include "esp_now.h"
#include "USB.h"
#include "USBHIDKeyboard.h"
USBHIDKeyboard Keyboard;

uint8_t broadcastAddress[] = {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF}; //broadcast all

typedef struct hosttoclientspacket {
    int playerNumber;
} hosttoclientspacket;

// Create a struct_message called myData
hosttoclientspacket myData;

// callback function that will be executed when data is received
void OnDataRecv(const uint8_t * mac, const uint8_t *incomingData, int len) {
  memcpy(&myData, incomingData, sizeof(myData));
  Serial.println(myData.playerNumber);
  switch(myData.playerNumber)
  {
    case 0:
      Keyboard.write(KEY_F1);
      break;
    case 1:
      Keyboard.write(KEY_F2);
      break;
    case 2:
      Keyboard.write(KEY_F3);
      break;
    case 3:
      Keyboard.write(KEY_F4);
      break;
    case 4:
      Keyboard.write(KEY_F5);
      break;
    case 5:
      Keyboard.write(KEY_F6);
      break;
    case 6:
      Keyboard.write(KEY_F7);
      break;
    case 7:
      Keyboard.write(KEY_F8);
      break;
    case 8:
      Keyboard.write(KEY_F9);
      break;
    case 9:
      Keyboard.write(KEY_F10);
      break;                                                      
  }
  
  
}

void setup() {
  Serial.begin(9600);
  // Set device as a Wi-Fi Station
  WiFi.mode(WIFI_STA);

  // Init ESP-NOW
  if (esp_now_init() != ESP_OK) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }

  // Once ESPNow is successfully Init, we will register for recv CB to
  // get recv packer info
  esp_now_register_recv_cb(OnDataRecv);

  esp_now_peer_info_t peerInfo;
  // Register peer
  memcpy(peerInfo.peer_addr, broadcastAddress, 6);
  peerInfo.channel = 0;  
  peerInfo.encrypt = false;
  
  // Add peer        
  if (esp_now_add_peer(&peerInfo) != ESP_OK){
    //Serial.println("Failed to add peer");
    return;
  }

  Keyboard.begin();
  USB.begin();
}

void loop() {
  // put your main code here, to run repeatedly:

}
