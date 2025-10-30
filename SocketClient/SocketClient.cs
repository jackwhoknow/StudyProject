using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    internal class SocketClient
    {
        static void Run()
        {

            IPHostEntry ipHost = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = ipHost.AddressList[0];

            IPEndPoint iPEndPoint = new IPEndPoint(ipAddress, 2112);
            Console.WriteLine("Starting: Create socket object");

            Socket sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(iPEndPoint);

            Console.WriteLine($"Successfully connected to {sender.RemoteEndPoint}");

            StringBuilder sb = new StringBuilder();
            int count = 5000;
            for (int i = 1; i <= count; i++)
            {
                sb.Append(i);
                if (i < count)
                {
                    sb.Append(',');
                }
                else
                {
                    sb.Append('.');
                }
            }
            byte[] forwardMessage = Encoding.ASCII.GetBytes(sb.ToString());
            sender.Send(forwardMessage);

            byte[] receivedBytes = new byte[1024];
            int totalBytesReceived = sender.Receive(receivedBytes);
            Console.WriteLine($"Message provided from server:{Encoding.ASCII.GetString(receivedBytes, 0, totalBytesReceived)}");
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            Console.ReadLine();
        }
    }
}
