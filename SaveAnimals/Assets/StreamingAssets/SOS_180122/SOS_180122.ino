
// Libraries for I2C and the HMC6343 sensor
#include <Wire.h>
#include "SFE_HMC6343.h"

int tonoNum = 0;

SFE_HMC6343 compass; // Declare the sensor object

void setup()
{
  // Start serial communication at 115200 baud
  Serial.begin(115200);
  
  // Give the HMC6343 a half second to wake up
  delay(500); 
  
  // Initialize the HMC6343 and verify its physical presence
  if (!compass.init())
  {
    Serial.println("Sensor Initialization Failed\n\r"); // Report failure, is the sensor wiring correct?
  }
}

void loop()
{
  tonoNum = random(40,60);
  // Read, calculate, and print the heading, pitch, and roll from the sensor
  compass.readHeading();
  printHeadingData();
  delay(250); // Minimum delay of 200ms (HMC6343 has 5Hz sensor reads/calculations)
}
String tmp;
void printHeadingData()
{
  Serial.print("Tonometer:");
  Serial.print(tonoNum);
  Serial.print("+Compass:");
  Serial.print(compass.heading/10);
  Serial.print("+OffsetX:");
  Serial.print(compass.pitch/10);
  Serial.print("+OffsetZ:");
  Serial.println(compass.roll/10);
}

