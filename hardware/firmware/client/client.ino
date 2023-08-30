#include "WiFi.h"
#include "esp_now.h"

int currentPlayer = 2;

bool prevState = false;
int timeOut = 200;
unsigned long lastPress;

uint8_t broadcastAddress[] = {0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF}; //host mac
typedef struct hosttoclientspacket {
    int playerNumber;
} hosttoclientspacket;

hosttoclientspacket myData;

esp_now_peer_info_t peerInfo;
void OnDataSent(const uint8_t *mac_addr, esp_now_send_status_t status) 
{
}

void setup() {
  Serial.begin(9600);
  pinMode(14,INPUT);
  pinMode(LED_BUILTIN, OUTPUT);
  
  lastPress = millis();
  WiFi.mode(WIFI_STA);
  // Init ESP-NOW
    if (esp_now_init() != ESP_OK) {
      //Serial.println("Error initializing ESP-NOW");
      return;
    }
  
    // Once ESPNow is successfully Init, we will register for Send CB to
    // get the status of Trasnmitted packet
    esp_now_register_send_cb(OnDataSent);
    
   
    // Register peer
    memcpy(peerInfo.peer_addr, broadcastAddress, 6);
    peerInfo.channel = 0;  
    peerInfo.encrypt = false;
    
    // Add peer        
    if (esp_now_add_peer(&peerInfo) != ESP_OK){
      //Serial.println("Failed to add peer");
      return;
    }

}

void loop() {
  if(digitalRead(14) == LOW)
  {
    if(prevState == false)
    {
      if((lastPress + timeOut) < millis())
      {
        Serial.println("PRESSED");
        lastPress = millis();    
        //send ESP now data
        myData.playerNumber = currentPlayer;
        esp_err_t result = esp_now_send(broadcastAddress, (uint8_t *) &myData, sizeof(myData));
        digitalWrite(LED_BUILTIN, HIGH);
      }else{
        digitalWrite(LED_BUILTIN, LOW);
      }
    }
    prevState = true;    
  }else{
   digitalWrite(LED_BUILTIN, LOW);
   prevState = false;
  }
}
