using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApp6
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] key;
        byte[] iv;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("New Command triggered by " + e.Source.ToString());
        }

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text.Length < 5)
            {
                MessageBox.Show("密码长度不能低于5位");
                return;
            }
            AesHelp.GenKeyIV(tbPassword.Text, out key, out iv);
            //加密
            byte[] encryptData = AesHelp.EncryptString(tbOriginal.Text, key, iv);
            this.tbEncrypt.Text = Convert.ToBase64String(encryptData);
        }

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            if (tbPassword.Text.Length < 5)
            {
                MessageBox.Show("密码长度不能低于5位");
                return;
            }
            AesHelp.GenKeyIV(tbPassword.Text, out key, out iv);
            //解密
            byte[] data = Convert.FromBase64String(this.tbEncrypt.Text);
            this.tbDecrypt.Text = AesHelp.DecryptString(data, key, iv);
        }
        private void element_MouseEnter(object sender,MouseEventArgs e)
        {
            if(sender is TextBlock textBlock)
            {
                textBlock.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
            }
        }
        private void element_MouseLeave(object sender, MouseEventArgs e)
        {

            if (sender is TextBlock textBlock)
            {
                textBlock.Background = null;
            }
        }
    }
}
