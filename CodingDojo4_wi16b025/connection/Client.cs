using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CodingDojo4_wi16b025.connection
{
    class Client
    {
        byte[] buffer = new byte[1024];
        Socket clientSocket;
        Action<string> MessageInformer;
        Action AbortInformer; 
        
        public Client (string ip, int port, Action<string> messageInformer, Action abortInformer) 
        {
            try
            {
                this.AbortInformer = abortInformer;
                this.MessageInformer = messageInformer;
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(ip), port);
                clientSocket = client.Client;
                StartReceiving();
            }
            catch (Exception)
            {
                messageInformer("No Connection to Server");
                AbortInformer();
            }
        }

        public void StartReceiving()
        {
            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            string message = ""; 
            while(!message.Contains("@quit"))
            {
                int length = clientSocket.Receive(buffer);
                message = Encoding.UTF8.GetString(buffer, 0, length);

                MessageInformer(message);
            }
            Close();
        }

        public void Close()
        {
            clientSocket.Close();
            AbortInformer();
        }

        public void Send(string message)
        {
            if(clientSocket != null)
            {
                clientSocket.Send(Encoding.UTF8.GetBytes(message));
            }
        }
    }
}
