using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    internal class SocketServer
    {
        static void Run()
        {
            Console.WriteLine("Starting: Creating Socket object");
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, 2112));
            listener.Listen(10);

            while (true)
            {
                Console.WriteLine("Waitting for connection on port 2112");
                Socket socket = listener.Accept();
                string receiveValue = string.Empty;
                while (true)
                {
                    byte[] receivedBytes = new byte[1024];
                    int numBytes = socket.Receive(receivedBytes);
                    Console.WriteLine("Receiving...");
                    receiveValue += Encoding.ASCII.GetString(receivedBytes, 0, numBytes);
                    if (numBytes < receivedBytes.Length)
                    {
                        break;
                    }
                }
                Console.WriteLine($"Received value:{receiveValue}");

                string replyValue = "Message successfully received.";
                byte[] replyMessage = Encoding.ASCII.GetBytes(replyValue);
                socket.Send(replyMessage);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            listener.Close();
        }
    }
}
