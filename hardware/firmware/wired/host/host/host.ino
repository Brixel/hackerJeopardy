#include <Arduino.h>
#include "USB.h"
#include "USBHIDKeyboard.h"
USBHIDKeyboard Keyboard;

int ports[6] = {40,39,38,37,36,35};
bool portStatus[6] = {false,false,false,false,false,false};
char keys[6] = {KEY_F1,KEY_F2,KEY_F3,KEY_F4,KEY_F5,KEY_F6};
unsigned long lastpress[6] {0,0,0,0,0,0}; 
int keys_wait = 2000;


void setup() {
  // put your setup code here, to run once:
  //set last press to now
  for(int i = 0; i<6;i++)
  {
    lastpress[i] = millis();
    pinMode(i,INPUT);
    portStatus[i] = digitalRead(ports[i]);
  }
  Keyboard.begin();
  USB.begin();
  Serial.begin(9600);
}

void loop() {
  for(int i = 0; i<6;i++)
  {
    //see if a buttin is pressed
    if(digitalRead(ports[i]) == HIGH && portStatus[i] == false)
    {
      //it is, is it time yet?
      if((lastpress[i] + keys_wait) < millis())
      {
        //yes
        Serial.print("Button ");
        Serial.print(i);
        Serial.println(" pressed");
        
        portStatus[6] = true;
        lastpress[i] = millis();

        //TODO: do keypress
        Keyboard.write(keys[i]);  
      }
    }else{
      //set port to low?
      if(digitalRead(ports[i]) == LOW)
      {
        portStatus[i] = false;
      }
    }
  }
}
