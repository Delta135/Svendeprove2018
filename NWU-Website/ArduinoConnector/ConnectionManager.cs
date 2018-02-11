using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoConnector
{
    class ConnectionManager
    {
        //what to connect to
        private IPAddress clientIP;
        private int clientPort;

        private TcpListener listener;
        private TcpClient client;
        private NetworkStream connectionStream;
        private byte[] buffer;

        private int amountReceived;

        public int AmountReceived
        {
            get
            {
                return amountReceived;
            }

            set
            {
                amountReceived = value;
            }
        }

        public byte[] Buffer
        {
            get
            {
                return buffer;
            }

            set
            {
                buffer = value;
            }
        }

        public ConnectionManager(IPAddress clientIP, int clientPort)
        {
            this.clientIP = clientIP;
            this.clientPort = clientPort;
            Buffer = new byte[4];

            listener = new TcpListener(clientIP, clientPort);
        }

        public void StartListening()
        {
            //keep listening until we get a connection
            listener.Start();
            client = listener.AcceptTcpClient();
        }

        //read data from the ardunio
        public byte[] ReadIncommingBuffer()
        {
            client.ReceiveBufferSize = Buffer.Length;
            client.ReceiveTimeout = int.MaxValue;

            connectionStream = client.GetStream();
            connectionStream.ReadTimeout = int.MaxValue;
            AmountReceived = connectionStream.Read(Buffer, 0, Buffer.Length);

            #region debug
            Console.WriteLine();
            Console.WriteLine("FROM CM:");
            Console.WriteLine("Buffer length: " + Buffer.Length);
            Console.WriteLine("Buffer contants: ");
            for (int i = 0; i < Buffer.Length; i++)
            {
                Console.Write(Buffer[i]);
            }
            Console.WriteLine();
            Console.WriteLine("END CM:");
            Console.WriteLine();
            #endregion

            return Buffer;
        }

        //send data to the arduino
        public void SendBuffer(byte[] buffer, int offset, int bufferLength)
        {
            connectionStream.Write(buffer, offset, bufferLength);
            CloseStream();
        }

        public void CloseStream()
        {
            connectionStream.Close();
        }
    }
}
