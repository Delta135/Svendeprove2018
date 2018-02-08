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

                int amount = stream.Read(buffer, 0, buffer.Length);

                if (amount == 0)
                {
                    Console.WriteLine("Got nothing...");
                    for (;;) { }
                }
                else
                {
                    for (int i = 0; i < amount; i++)
                    {
                        Console.Write(buffer[i]);
                    }

                    Console.WriteLine();

                    Console.WriteLine(Encoding.ASCII.GetString(buffer, 0, buffer.Length));
                    Console.WriteLine("Done");
                    Console.WriteLine("Will now talk back...");

                    buffer = Encoding.ASCII.GetBytes("Thanks.");
                    stream.Write(buffer, 0, buffer.Length);

                    Console.WriteLine("Sendt.");

                    for (;;) { }
                }


            }
        }
    }
}
