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

        public ConnectionManager(IPAddress clientIP, int clientPort)
        {
            this.clientIP = clientIP;
            this.clientPort = clientPort;
            buffer = new byte[512];

            listener = new TcpListener(clientIP, clientPort);
        }

        public void StartListening()
        {
            client = listener.AcceptTcpClient();
        }
    }
}
