using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketClient
{
    internal class TcpReceive
    {
        public static void Run()
        {
            using (var tcpClient = new TcpClient("localhost", 2112))
            using (var ns = tcpClient.GetStream())
            {
                for (int i = 0; i < 1000000000; i++)
                {
                    using (var fs = File.Open("abc.txt", FileMode.Open))
                    {
                        int data = fs.ReadByte();
                        while (data != -1)
                        {
                            ns.WriteByte((byte)data);
                            data = fs.ReadByte();
                        }
                    }
                }
            }
        }
    }
}
