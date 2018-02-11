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

        private int amount;

        public int Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        public ConnectionManager(IPAddress clientIP, int clientPort)
        {
            this.clientIP = clientIP;
            this.clientPort = clientPort;
            buffer = new byte[512];

            listener = new TcpListener(clientIP, clientPort);
        }

        public bool StartListening()
        {
            //keep listening until we get a connection
            listener.Start();
            client = listener.AcceptTcpClient();
            return true;
        }

        public byte[] ReadIncommingBuffer()
        {
            client.ReceiveBufferSize = buffer.Length;
            client.ReceiveTimeout = int.MaxValue;

            connectionStream = client.GetStream();
            connectionStream.ReadTimeout = int.MaxValue;
            Amount = connectionStream.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public void SendBuffer(byte[] buffer, int offset, int bufferLength)
        {
            connectionStream.Write(buffer, offset, bufferLength);
            connectionStream.Close();
        }

        public void next()
        {
            

            
        }
    }
}
