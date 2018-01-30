//include librarys
#include <LiquidCrystal.h>

#pragma region Defines
/*LCD vars*/
//RS digital 12
#define LCDRS 12

//Enable digital 11
#define LCDE 11

//D4 digital 5
#define LCDD4 5

//D5 digital 4
#define LCDD5 4

//D6 digital 3
#define LCDD6 3

//D7 digital 2
#define LCDD7 2

//RW to ground
#pragma endregion

//Setup LCD Pins, Save in PROGMEM (Flash storge) as we don't need to change it after compile
//https://www.arduino.cc/reference/en/language/variables/utilities/progmem/
//const PROGMEM byte RS = 12, Enable = 11, D4 = 5, D5 = 4, D6 = 3, D7 = 2;

//LCD strings
const PROGMEM String readCard = "Swipe Armbaand";
const PROGMEM String wait = "Vent...";
const PROGMEM String ok = "OK!";
const PROGMEM String denied = "Kort afvist";
const PROGMEM String tooManyRow1 = "For mange";
const PROGMEM String tooManyRow2 = "i omrode";
const PROGMEM String error = "Fejl";
const PROGMEM String outOfService = "Ikke i service";

//Setup LCD
//LiquidCrystal lcd(RS, Enable, D4, D5, D6, D7);
LiquidCrystal lcd(LCDRS, LCDE, LCDD4, LCDD5, LCDD6, LCDD7);

void setup() 
{
	//LCD
	//LCD columns and rows, small is 16 columns on ech row (2 in total)
	lcd.begin(16, 2);
	lcd.print(readCard);
}

void loop() 
{

}