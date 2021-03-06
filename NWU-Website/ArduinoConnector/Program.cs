﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ArduinoConnector
{
    class Program
    {
        //Arduino ip/port
        private static IPAddress arduinoIp = IPAddress.Any;
        private static int arduinoPort = 8090;

        private static byte[] workingBuffer;

        private static ConnectionManager cm;
        private static DatabaseManager dbMan;

        static void Main(string[] args)
        {
            //Test the system by sending random data to the database
            if (args.Contains("testdb".ToLower()))
            {
                Console.WriteLine("Køre test...");
                Console.WriteLine("Luk for at afslutte");
                Utilities.DatabaseTest();
                Environment.Exit(1);//prevent the rest of the program fom running if testmode has been enabled
            }

            cm = new ConnectionManager(arduinoIp, arduinoPort);
            dbMan = new DatabaseManager();

            //Main loop
            while (true)
            {
                Console.WriteLine("Lytter...");

                //start listening for arduinos
                cm.StartListening();

                Console.WriteLine("Fik forbinelse");

                //read what the arduino is sending
                workingBuffer = cm.ReadIncommingBuffer();

                if (cm.AmountReceived == 0)
                {
                    //we got nothing return to listion state
                    Console.WriteLine("Ingen forbinelse");
                    cm.CloseStream();
                }
                else
                {
                    #region DEBUG
                    Console.Write("Got: ");
                    for (int i = 0; i < cm.AmountReceived; i++)
                    {
                        Console.Write(cm.Buffer[i]);
                    }
                    Console.WriteLine();
                    #endregion

                    //check agenst DB and return a value we can send to the arduino
                    byte testResault = dbMan.CheckCard(workingBuffer);

                    #region DEBUG
                    //debug
                    switch (testResault)
                    {
                        case 1:
                            Console.WriteLine("Check in");
                            break;
                        case 2:
                            Console.WriteLine("Check out");
                            break;
                        case 3:
                            Console.WriteLine("Too many");
                            break;
                        case 0:
                            Console.WriteLine("Worng card");
                            break;
                        default:
                            break;
                    }
                    #endregion

                    workingBuffer = new byte[] { testResault };

                    Console.WriteLine("Færdig");
                    Console.WriteLine("Sender svar");
                    Console.WriteLine("Sendte: " + workingBuffer[0]);

                    //send the resault to the arduino
                    cm.SendBuffer(workingBuffer, 0, workingBuffer.Length);
                    cm.CloseStream();

                    Console.WriteLine("Sendt.");
                }
            }
        }
    }
}
