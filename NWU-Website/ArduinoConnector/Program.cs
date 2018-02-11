using System;
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

                //start listening for arduinos
                cm.StartListening();

                Console.WriteLine("Conn get!");

                //read what the arduino is sending
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

                    Console.WriteLine("Done");
                    Console.WriteLine("Will now talk back...");
                    Console.WriteLine("Send result: " + workingBuffer[0]);

                    //send the resault to the arduino
                    cm.SendBuffer(workingBuffer, 0, workingBuffer.Length);
                    cm.CloseStream();

                    Console.WriteLine("Sendt.");
                }
            }
        }
    }
}
