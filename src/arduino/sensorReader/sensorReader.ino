#define USE_TIMER_1 true
#define USE_TIMER_2 true

#define TIMER0_FREQ_HZ 1.0
#define TIMER1_FREQ_HZ 2000.0

#include "TimerInterrupt.h"

const unsigned int REEDSW1 = 1;
const unsigned int REEDSW2 = 2;

// IR avoidance sensor signal wire
const int PIN_FLYWHEEL = 9;
// Built in reed switches on the rower.  Male connector is SW ???
const int PIN_REEDSW_1 = 10;
const int PIN_REEDSW_2 = 11;

// Buffers for debouncing switch input: https://my.eng.utah.edu/~cs5780/debouncing.pdf
// See listing 2: An Alternative something or other I closed the PDF 
volatile unsigned int FlywheelBuffer = 0;
volatile unsigned int ReedSw1Buffer = 0;
volatile unsigned int ReedSw2Buffer = 0;

// Milliseconds the last flywheel sensor changed to high
volatile unsigned long lastFlywheelTicks;

// USe to determine radians/sec rotation rate ()
volatile unsigned long ellapsedTicksFlywheel;

// Current state of the switches outside of the readSensor method.
volatile bool flywheel = false;
volatile bool reed1 = false;
volatile bool reed2 = false;

// Stroke parameters
volatile unsigned long strokeBeginTicks = 0;

// Stroke parameters
volatile unsigned long recoveryBeginTicks = 0;

// Maybe useful?
//volatile int strokeLengthCounter = 0;
//volatile int recoveryLengthCounter = 0;
//volatile float radiansPerSecond = 0.0;

// Number of strokes
volatile unsigned int strokeCount = 0;

//volatile unsigned long flywheelCount;
//volatile float strokeLength = 0;
//volatile float recoveryLength = 0;
//volatile unsigned int strokeTicks = 0;
//volatile unsigned int recoveryTicks = 0;

volatile unsigned int trailingSwitch = 0;

void setup() {
  
  Serial.begin(115200);
  
  // TimerInterrupt setup.
  ITimer1.init();
  ITimer2.init();
  
  ITimer1.attachInterrupt(TIMER0_FREQ_HZ, writeData);
  ITimer2.attachInterrupt(TIMER1_FREQ_HZ, readSensors);
}
inline void writeData(){}
void loop() {
  // put your main code here, to run repeatedly:
}

void readSensors() 
{
  bool switchChanged = false;
  
  unsigned long ticks = millis();
  unsigned int nextTrailingSwitch = trailingSwitch;
  
  // debounce sensor input
  bool nextFlywheel = debounce(PIN_FLYWHEEL, &FlywheelBuffer);
  bool nextReed1 = debounce(PIN_REEDSW_1, &ReedSw1Buffer);
  bool nextReed2 = debounce(PIN_REEDSW_2, &ReedSw2Buffer);

  // Flywheel LOW -> HIGH transition 
  // Collect the ellapsed time to the last LH transition and save for radians/sec calc
  if (nextFlywheel && !flywheel) 
  {
    if (lastFlywheelTicks > 0) 
    {
        ellapsedTicksFlywheel = ticks - lastFlywheelTicks;
    }
    
    lastFlywheelTicks = ticks;
    writeFlywheel(0.0, ellapsedTicksFlywheel);
  }

  // REED1 HIGH to LOW transition
  if (reed1 && !nextReed1) 
  {
     switchChanged = true;
     nextTrailingSwitch = REEDSW1;
  }

  // REED2 HIGH to LOW transition
  if (reed2 && !nextReed2) 
  {
     switchChanged = true;
    nextTrailingSwitch = REEDSW2;
  }

  // Check to see if a reed switch is deactivated back to back signialling a transation between stroke and recovery
  if (switchChanged && nextTrailingSwitch == trailingSwitch )
  {
    if (trailingSwitch == REEDSW1) 
    {
       endRecovery(ticks);
        beginStroke(ticks);
    } 
    else 
    {
         endStroke(ticks);
         beginRecovery(ticks);
    }
  }

  // transition variables to next state.
  trailingSwitch = nextTrailingSwitch;

  flywheel = nextFlywheel;

  reed1 = nextReed1; 
  reed2 = nextReed2; 
}

inline void beginStroke(unsigned long millis) 
{
  strokeCount++;
  strokeBeginTicks = millis;
  writeBeginStroke(strokeCount);
}
inline void endStroke(unsigned long millis) 
{
   writeEndStroke(0, strokeBeginTicks - millis);
}

inline void beginRecovery(unsigned long millis) 
{
  recoveryBeginTicks = millis;
  writeBeginRecovery();
}

inline void endRecovery(unsigned long millis) 
{
//  writeEndRecovery(0, recoveryBeginTicks - millis);
}


inline bool debounce(int inputPin, volatile unsigned int* buffer) 
{
  bool currentStateIsLow = digitalRead(inputPin) == LOW;
  *buffer = (*buffer << 1) | currentStateIsLow | 0xe000;
  
  return *buffer == 0xf000;
}  

//beginStroke:{\"count\":1}
void writeBeginStroke(unsigned long count) 
{
  Serial.print("beginStroke:{\"count\":");
  Serial.print(count);
  Serial.println("}");
}

//  endStroke:{\"length\":1,\"duration\":1000}
void writeEndStroke(int strokeLength, unsigned int durationMs) 
{
  Serial.print("endStroke:{\"length\":");
  Serial.print(strokeLength);
  Serial.print(",\"duration\":");
  Serial.print(durationMs);
  Serial.println("}");
}

//  beginRecovery:{}
void writeBeginRecovery()
{
  Serial.println("beginRecovery:{}");
}

//  endRecovery:{\"length\":1,\"duration\":2000}
void writeEndRecovery(int strokeLength, unsigned int durationMs) 
{  
  Serial.print("endRecovery:{\"length\":");
  Serial.print(strokeLength);
  Serial.print(",\"duration\":");
  Serial.print(durationMs);
  Serial.println("}");
}

//  flywheel:{\"rps\":1.1073647484,\"ellapsed\"}
void writeFlywheel(float rps, unsigned int ellapsed)
{
  Serial.print("flywheel:{\"rps\":");
  Serial.print(rps);
  Serial.print(",\"ellapsed\":");
  Serial.print(ellapsed);
  Serial.println("}");

}

void writeIdle() 
{
  Serial.println("idle: {}");
}
