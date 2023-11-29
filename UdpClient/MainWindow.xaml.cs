using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UdpClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ReceiveScreenshot();

            

        }

        private async void ReceiveScreenshot()
        {

            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            var Ip = IPAddress.Parse("192.168.100.8");
            var Port = 65002;

            var remoteEP = new IPEndPoint(Ip, Port);
            listener.Bind(remoteEP);

            var msg = "";
            var len = 0;
            var buffer = Array.Empty<byte>();

            EndPoint remoteEPServer = new IPEndPoint(IPAddress.Any, 0);

            await Task.Run(() => {
                while (true)
                {
                    listener.ReceiveFrom(buffer, ref remoteEPServer);
                    MessageBox.Show(buffer.Length.ToString());
                    
                    ClientImage.Source= ByteToImage(buffer);
                }
            });
        }

        BitmapImage ByteToImage(byte[]byteArray)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Freeze the BitmapImage to make it read-only and thread-safe
            }
            return bitmapImage;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new Socket(AddressFamily.InterNetwork,
                          SocketType.Dgram,
                          ProtocolType.Udp);


            var Ip = IPAddress.Parse("192.168.100.8");
            var Port = 65001;

            var remoteEP = new IPEndPoint(Ip, Port);

            var len = 0;
            var buffer = Array.Empty<byte>();

      
            buffer = Encoding.Default.GetBytes(txtbx.Text);
            client.SendTo(buffer, remoteEP);

            
        }
    }
}
