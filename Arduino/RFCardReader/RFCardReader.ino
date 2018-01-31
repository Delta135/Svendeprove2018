//include librarys
#include <MFRC522Extended.h>
#include <MFRC522.h>
#include <util.h>
#include <EthernetUdp2.h>
#include <EthernetServer.h>
#include <EthernetClient.h>
#include <Ethernet2.h>
#include <LiquidCrystal.h>
#include <SPI.h>

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

//change these if using a LCD with a non-standart size
#define LCDCOLUMNS 16
#define LCDROWS 2

/*LED vars*/
//pin 6 green led
#define LEDGREED 30

//pin 7 red led
#define LEDRED 31

/*RFID*/
//RST pin 6
#define RFIDRST 6

//MISO pin 50
#define RFIDMISO 50

//MOSI pin 51
#define RFIDMOSI 51

//SCK pin 52
#define RFIDSCK 52

//SDA/SS pin 53
#define RFIDSDA 53
#pragma endregion

//Setup LCD Pins, Save in PROGMEM (Flash storge) as we don't need to change it after compile
//https://www.arduino.cc/reference/en/language/variables/utilities/progmem/

//LCD strings
const PROGMEM String readCard = "Swipe Armbaand";
const PROGMEM String wait = "Vent...";
const PROGMEM String ok = "OK!";
const PROGMEM String denied = "Kort afvist";
const PROGMEM String tooManyRow1 = "For mange";
const PROGMEM String tooManyRow2 = "i omraade";
const PROGMEM String error = "Fejl";
const PROGMEM String outOfService = "Ikke i service";

//Setup LCD
LiquidCrystal lcd(LCDRS, LCDE, LCDD4, LCDD5, LCDD6, LCDD7);

//Setup RFID
MFRC522 rfid(RFIDSDA, RFIDRST);
MFRC522::MIFARE_Key rfKey;

//Setup ethernet
const PROGMEM byte MAC[] = {0x90, 0xA2, 0xDA, 0x10, 0x5F, 0x81};
EthernetClient client;

//Test
const byte cardID[4] = {172, 12, 63, 213};
byte inputID[4];

void setup() 
{
	Serial.begin(9600);
	Serial.println(F("Starting..."));

	//led
	Serial.println(F("Setting Up LED..."));
	pinMode(LEDGREED, OUTPUT);
	pinMode(LEDRED, OUTPUT);

	digitalWrite(LEDGREED, HIGH);
	digitalWrite(LEDRED, LOW);

	//LCD
	//LCD columns and rows, small is 16 columns on ech row (2 in total)
	Serial.println(F("Setting Up LCD..."));
	lcd.begin(LCDCOLUMNS, LCDROWS);
	lcd.print(readCard);

	//RFID
	Serial.println(F("Setting Up RFID..."));
	SPI.begin();
	rfid.PCD_Init();

	//Ethernet
	
}

void loop()
{
	//wait for new card
	if (!rfid.PICC_IsNewCardPresent())
		return;

	//Verify if the NUID has been readed
	if (!rfid.PICC_ReadCardSerial())
		return;

	Serial.print(F("RF UID:  "));

	// Halt PICC
	rfid.PICC_HaltA();

	// Stop encryption on PCD
	rfid.PCD_StopCrypto1();

	//printDec(rfid.uid.uidByte, rfid.uid.size);
	//Serial.println();

	for (byte i = 0; i < rfid.uid.size; i++)
	{
		Serial.print(rfid.uid.uidByte[i]);
	}

	Serial.println();

	Serial.print("cardID:  ");
	for (byte i = 0; i < 4; i++)
	{
		Serial.print(cardID[i]);
	}

	getUID(rfid.uid.uidByte, 4, inputID);

	Serial.println();

	Serial.print("inputID: ");
	for (byte i = 0; i < 4; i++)
	{
		Serial.print(inputID[i]);
	}

	
}

//https://github.com/miguelbalboa/rfid/blob/master/examples/ReadNUID/ReadNUID.ino
//bytes to DEC
void printDec(byte *buffer, byte bufferSize) 
{
	for (byte i = 0; i < bufferSize; i++) 
	{
		//Serial.print(buffer[i] < 0x10 ? " 0" : " ");

		if (buffer[i] < 0x10)
		{
			Serial.print(" 0");
		}
		else
		{
			Serial.print(" ");
		}

		Serial.print(buffer[i], DEC);
	}
}

//copy UID to temp var
void getUID(byte *buffer, byte bufferSize, byte *intput)
{
	for (byte i = 0; i < bufferSize; i++)
	{
		intput[i] = buffer[i];
	}
}

void printLCDAndSerial(String msg)
{
	lcd.clear();
	lcd.print(msg);
	Serial.println(msg);
}