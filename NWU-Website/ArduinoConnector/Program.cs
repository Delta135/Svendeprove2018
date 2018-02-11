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
        static TcpListener listener;
        static TcpClient client;
        static NetworkStream stream;

        //Arduino ip/port
        static IPAddress arduinoIp = IPAddress.Any;
        static int arduinoPort = 80;

        static byte[] buffer;

        static void Main(string[] args)
        {
            listener = new TcpListener(arduinoIp, arduinoPort); 

            while (true)
            {
                buffer = new byte[512];
                Console.WriteLine("Listening...");
                listener.Start();

                client = listener.AcceptTcpClient();

                if (!client.Connected)
                {
                    Console.WriteLine("No conn!");
                    for (;;) { }
                }

                Console.WriteLine("Conn get!");

                client.ReceiveBufferSize = buffer.Length;
                client.ReceiveTimeout = 5000;

                stream = client.GetStream();
                stream.ReadTimeout = int.MaxValue;

                int amount = stream.Read(buffer, 0, buffer.Length);

                if (amount == 0)
                {
                    Console.WriteLine("Got nothing...");
                    for (;;) { }
                }
                else
                {
                    Console.Write("Got: ");
                    for (int i = 0; i < amount; i++)
                    {
                        Console.Write(buffer[i]);
                    }

                    Console.WriteLine();
                    byte[] fakeDB = new byte[] { 172, 12, 63, 213 };
                    bool checkInd = false, same = false;

                    for (int i = 0; i < fakeDB.Length; i++)
                    {
                        if (buffer[i] != fakeDB[i])
                        {
                            same = false;
                            break;
                        }
                        else
                        {
                            same = true;
                        }
                    }

                    if (same)
                    {
                        Console.WriteLine("Same");
                        if (checkInd)
                        {
                            buffer = new byte[] { 2 }; //checkud
                        }
                        else
                        {
                            buffer = new byte[] { 1 };
                        }
                    }
                    else
                    {
                        Console.WriteLine("Worng");
                        buffer = new byte[] { 0 };
                    }

                    //for (int i = 0; i < buffer.Length; i++)
                    //{
                    //    if (buffer[i] != fakeDB[i])
                    //    {
                    //        Console.WriteLine("Worng");
                    //        buffer = new byte[] { 0 };
                    //        break;
                    //    }

                    //    Console.WriteLine("Same");
                    //    buffer = new byte[] { 1 };
                    //}
                    
                    Console.WriteLine("Done");
                    Console.WriteLine("Will now talk back...");
                    Console.WriteLine("Buffer: " + buffer[0]);

                    stream.Write(buffer, 0, buffer.Length);

                    Console.WriteLine("Sendt.");

                    //for (;;) { }
                }

                stream.Close();


            }
        }
    }
}
