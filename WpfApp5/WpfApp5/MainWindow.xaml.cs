using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp5
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSynSocketDev_Click(object sender, RoutedEventArgs e)
        {
            //IPAddress myIP = IPAddress.Parse("127.0.0.1");
            //IPEndPoint myserver = new IPEndPoint(myIP, 80);

            IPHostEntry myAddress = Dns.GetHostEntry(this.tbIP.Text);
            IPAddress[] myIPAddress = myAddress.AddressList;
            foreach(IPAddress address in myIPAddress)
            {
                list1.Items.Add(address.ToString());
            }

            //Dns.GetHostName(); 获取本机名
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            IPAddress myIP = IPAddress.Parse(this.tbAddress.Text);
            IPEndPoint myServer = new IPEndPoint(myIP, 2020);
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(myServer);
            socket.Listen(50);
            Socket bbb = socket.Accept();
            IPHostEntry myHost = Dns.GetHostEntry(myIP);
        }
    }
}
