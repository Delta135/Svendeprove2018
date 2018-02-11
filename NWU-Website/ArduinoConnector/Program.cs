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
        static IPAddress arduinoIp = IPAddress.Any;
        static int arduinoPort = 80;

        static byte[] workingBuffer;

        static ConnectionManager cm;
        static DatabaseManager dbMan;

        static void Main(string[] args)
        {
            cm = new ConnectionManager(arduinoIp, arduinoPort);
            dbMan = new DatabaseManager();

            //Main loop
            while (true)
            {
                Console.WriteLine("Listening...");

                cm.StartListening();

                Console.WriteLine("Conn get!");

                workingBuffer = cm.ReadIncommingBuffer();

                if (cm.AmountReceived == 0)
                {
                    Console.WriteLine("Got nothing...");
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

                    //check agenst DB
                    byte[] fakeDB = new byte[] { 172, 12, 63, 213 };
                    byte testResault = dbMan.CheckCard(fakeDB, workingBuffer);

                    #region DEBUG
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

                    Console.WriteLine("Done");
                    Console.WriteLine("Will now talk back...");
                    Console.WriteLine("Send result: " + workingBuffer[0]);

                    cm.SendBuffer(workingBuffer, 0, workingBuffer.Length);
                    cm.CloseStream();

                    Console.WriteLine("Sendt.");
                }
            }
        }
    }
}
