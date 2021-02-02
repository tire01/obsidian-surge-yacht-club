#define USE_TIMER_1 true
#define USE_TIMER_2 true

#define TIMER0_FREQ_HZ 2000.0

#define BAUD 115200 

#include "TimerInterrupt.h"

void setup() {
  // put your setup code here, to run once:
  Serial.begin(BAUD);

  ITimer2.attachInterrupt(TIMER0_FREQ_HZ, writeTestData);
}

String messages[] =  
  {
     "beginStroke:{\"count\":1}",
     "update:{\"rps\":1.1073647484}",
     "endStroke:{\"length\":1,\"duration\":1000}",
     "beginRecovery:{}",
     "update:{\"rps\":1.1073647484}",
     "endRecovery:{\"length\":1,\"duration\":2000}",
     "beginStroke:{\"count\":1}",
     "update:{\"rps\":1.1073647484}",
     "endStroke:{\"length\":1,\"duration\":1000}",
     "beginRecovery:{}",
     "endRecovery:{\"length\":1,\"duration\":2000}",
     "update:{\"rps\":1.1073647484}",

     "idle:{}"
  };

void loop() {
  // put your main code here, to run repeatedly:

}

volatile int messageIndex = 0;

void writeTestData() 
{
  Serial.println(messages[messageIndex]);
  
  messageIndex++;
  if (messageIndex == 13) 
  {
    messageIndex = 0;
  }
}
