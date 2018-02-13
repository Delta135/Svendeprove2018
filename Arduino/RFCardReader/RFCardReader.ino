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
/*Genral*/
#define MYID 1 //the card readers area id
#define SERVERPORT 1000

/*LCD vars*/
//RS digital 9
#define LCDRS 9

//Enable digital 8
#define LCDE 8

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
#define LEDGREED 48

//pin 7 red led
#define LEDRED 49

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

//Reserved Etherner pins
#define EhternetSPI1 10
#define EhternetSPI2 11
#define EhternetSPI3 12
#define EhternetSPI4 13
#pragma endregion

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
byte MAC[] = {0x90, 0xA2, 0xDA, 0x10, 0x5F, 0x81};
IPAddress ArduinoIP = { 172, 16, 1, 50};
IPAddress ServerIP = {172, 16, 1, 4};
EthernetClient client;

//Test
//const byte cardUID[4] = {172, 12, 63, 213};
const byte UIDlength = 4;
IPAddress PCIP = { 172, 16, 1, 1 };

void setup() 
{
	Serial.begin(9600);
	Serial.println(F("Starting..."));

	//led
	Serial.println(F("Setting Up LED..."));
	initLED();

	//LCD
	//LCD columns and rows, small is 16 columns on ech row (2 in total)
	Serial.println(F("Setting Up LCD..."));
	initLCD();

	//RFID
	Serial.println(F("Setting Up RFID..."));
	initRFID();

	//Ethernet
	Serial.println(F("Setting Up Ehternet"));
	initEhternet();
}

void loop()
{

	//wait for new card
	if (!rfid.PICC_IsNewCardPresent())
		return;

	//Verify if the NUID has been readed
	if (!rfid.PICC_ReadCardSerial())
		return;

	// Halt PICC
	rfid.PICC_HaltA();

	// Stop encryption on PCD
	rfid.PCD_StopCrypto1();

	//a card was used display wait
	printLCDAndSerial(wait);
	digitalWrite(LEDGREED, LOW);
	digitalWrite(LEDRED, HIGH);
	
	//simulate network
	//delay(1000);

	//real network
	byte bufferWithID[5];

	for (size_t i = 0; i < 4; i++)
	{
		bufferWithID[i] = rfid.uid.uidByte[i];
	}
	bufferWithID[4] = MYID;

	for (size_t i = 0; i < 5; i++)
	{
		Serial.print(bufferWithID[i]), Serial.print(",");
	}

	client.write(bufferWithID, 5);
	Serial.println(F("Sendt data."));

	Serial.println(F("Got:"));
	uint8_t *rbuffer;
	size_t buffersize = 1;
	/*rbuffer =*/ client.readBytes(rbuffer, buffersize);
	Serial.println(*rbuffer);

	switch (*rbuffer)
	{
	case 0:
		printLCDAndSerial(denied);
		digitalWrite(LEDGREED, LOW);
		digitalWrite(LEDRED, LOW);
		break;
	case 1:
		printLCDAndSerial(ok);
		digitalWrite(LEDGREED, HIGH);
		digitalWrite(LEDRED, LOW);
		break;
	case 2:
		printLCDAndSerial(ok);
		digitalWrite(LEDGREED, HIGH);
		digitalWrite(LEDRED, LOW);
		break;
	case 3:
		printLCDAndSerial(tooManyRow1);
		lcd.setCursor(0, 2);
		lcd.print(tooManyRow2);
		digitalWrite(LEDGREED, HIGH);
		digitalWrite(LEDRED, LOW);
		break;
	default:
		printLCDAndSerial(error);
		digitalWrite(LEDGREED, LOW);
		digitalWrite(LEDRED, LOW);
		break;
	}

	//show the message for 2 sec
	delay(2000);

	printLCDAndSerial(readCard);
	digitalWrite(LEDGREED, HIGH);
	digitalWrite(LEDRED, LOW);
}

//for debug
void printLCDAndSerial(const String &msg)
{
	lcd.clear();
	lcd.print(msg);
	Serial.println(msg);
}

void initLED()
{
	pinMode(LEDGREED, OUTPUT);
	pinMode(LEDRED, OUTPUT);

	digitalWrite(LEDGREED, HIGH);
	digitalWrite(LEDRED, LOW);
}

void initLCD()
{
	lcd.begin(LCDCOLUMNS, LCDROWS);
	lcd.print(readCard);
}

void initRFID()
{
	SPI.begin();
	rfid.PCD_Init();
}

void initEhternet()
{
	//Use to connect to server
	if (Ethernet.begin(MAC) == 1)
	{
		Serial.println(F("Ehternet config done."));
	}
	else
	{
		Serial.println(F("Ehternet config failed."));

		//do nothing forever
		for (;;) {}
	}

	//Ethernet.begin(MAC, ArduinoIP); //use to connect to pc

	//a delay here seems to give more stabilaty when connecting
	delay(1000);

	if (client.connect(ServerIP, SERVERPORT))
	//if (client.connect(PCIP, SERVERPORT))
	{
		Serial.println(F("Connected!"));

		Serial.println(F("IP:"));
		Serial.println(Ethernet.localIP());

		//C#
		/*client.write(cardUID, 4);
		Serial.println(F("Sendt data."));

		//delay
		//delay(1000);

		//recive data
		Serial.println(F("Got:"));
		String recive = client.readString();
		Serial.println(recive);

		for (;;) {}*/
	}
	else
	{
		Serial.println(F("Connection failed"));

		//stop the arduino form keeping connecting
		for (;;) {}
	}
}

//TODO
void LCDPrintNew(const String &msg)
{

}

//print a messege on the first row and the 2ed row
void LCDPrintNew(const String &msg, const String &msg2)
{

}

//TODO
void toggleLEDs(const byte &LED1, const byte &LED2)
{

}

//TODO
//NOTE: Can't return arrays in C, use pointers to manipulate them
//might have to remove const... see string link in the top
void scanCard(const byte *UID)
{

}

//TODO
bool connectToServer()
{
	return NULL;
}

//TODO
void sendUID()
{

}

//TODO
bool getResponse()
{
	return NULL;
}

//debug
bool checkUID(const byte *UID1, const byte *UID2, const byte &UIDSize)
{
	for (byte i = 0; i < UIDSize; i++)
	{
		if (UID1[i] != UID2[i])
		{
			return false;
		}
	}

	return true;
}