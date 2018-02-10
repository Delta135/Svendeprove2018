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
        static void Main(string[] args)
        {
            //Arduino ip/port
            IPAddress arduinoIp = IPAddress.Any;
            int arduinoPort = 80;

            TcpListener listener = new TcpListener(arduinoIp, arduinoPort);

            byte[] buffer = new byte[512]; 

            while (true)
            {
                Console.WriteLine("Listening...");
                listener.Start();

                TcpClient client = listener.AcceptTcpClient();

                if (!client.Connected)
                {
                    Console.WriteLine("No conn!");
                    for (;;) { }
                }

                Console.WriteLine("Conn get!");

                client.ReceiveBufferSize = buffer.Length;
                client.ReceiveTimeout = 5000;

                NetworkStream stream = client.GetStream();
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

                    for (int i = 0; i < buffer.Length; i++)
                    {
                        if (buffer[i] != fakeDB[i])
                        {
                            Console.WriteLine("Worng");
                            buffer = new byte[] { 0 };
                            break;
                        }

                        Console.WriteLine("Same");
                        buffer = new byte[] { 1 };
                    }
                    
                    Console.WriteLine("Done");
                    Console.WriteLine("Will now talk back...");
                    Console.WriteLine("Buffer: " + buffer[0]);

                    stream.Write(buffer, 0, buffer.Length);

                    Console.WriteLine("Sendt.");

                    for (;;) { }
                }


            }
        }
    }
}
