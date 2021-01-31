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
volatile unsigned long strokeEllapsedTicks = 0;

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
  
  ITimer1.attachInterrupt(TIMER0_FREQ_HZ, readSensors);
  ITimer2.attachInterrupt(TIMER1_FREQ_HZ, writeData);
}

void loop() {
  // put your main code here, to run repeatedly:
}

void readSensors() 
{
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
  }

  // REED1 HIGH to LOW transition
  if (!nextReed1 && reed1) 
  {
     nextTrailingSwitch = REEDSW1;
  }

  // REED2 HIGH to LOW transition
  if (!nextReed2 && reed2) 
  {
    nextTrailingSwitch = REEDSW2;
  }

  // Check to see if a reed switch is deactivated back to back signialling a transation between stroke and recovery
  if (nextTrailingSwitch == trailingSwitch)
  {
    if (trailingSwitch == REEDSW1) 
    {
      beginStroke(ticks);
      //endRecovery(ticks);
    } 
    else 
    {
       endStroke(ticks);
      //beginRecovery(ticks);
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
  strokeEllapsedTicks = strokeBeginTicks - millis;
}

inline void endStroke(unsigned long millis) 
{
  strokeBeginTicks = millis;
  strokeCount++;
}

inline bool debounce(int inputPin, volatile unsigned int* buffer) 
{
  bool currentStateIsLow = digitalRead(inputPin) == LOW;
  *buffer = (*buffer << 1) | currentStateIsLow | 0xe000;
  
  return *buffer == 0xf000;
}  

// write stuff to serial computer.
void writeData() 
{
  Serial.print("strokes:");
  Serial.println(strokeCount);
}

void clockReset() {}
