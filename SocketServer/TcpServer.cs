using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    internal class TcpServer
    {
        public static void Run()
        {
            Thread thread = new Thread(new ThreadStart(Listen));
            thread.Start();
        }
        private static void Listen()
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            int port = 2112;
            TcpListener tcpListener = new TcpListener(localAddr, port);
            tcpListener.Start();
            while(true)
            {
                using (var tcpClient = tcpListener.AcceptTcpClient())
                {
                    NetworkStream ns = tcpClient.GetStream();
                    StreamReader streamReader = new StreamReader(ns);
                    string result = streamReader.ReadToEnd();
                    Console.WriteLine(result);
                    tcpClient.Close();
                    if (result.Equals("error", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }                    
            }            
            tcpListener.Stop();
        }
    }
}
