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
        private System.Net.Sockets.UdpClient client = new System.Net.Sockets.UdpClient(27002);
        public MainWindow()
        {
            InitializeComponent();


            

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
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 27001);
            var buffer = new byte[ushort.MaxValue - 29];
            await client.SendAsync(buffer, buffer.Length, remoteEP);
            var list = new List<byte>();
            var maxlen = buffer.Length;
            var len = 0;
            
                while (true)
                {
                    var result = await client.ReceiveAsync();
                    list.AddRange(result.Buffer);
                    if (result.Buffer.Length != maxlen) break;
                }

                ClientImage.Source = ByteToImage(list.ToArray());
                list.Clear();


            //var client = new Socket(AddressFamily.InterNetwork,
            //              SocketType.Dgram,
            //              ProtocolType.Udp);


            //var Ip = IPAddress.Parse("127.0.0.1");
            //var Port = 27001;

            //var remoteEP = new IPEndPoint(Ip, Port);

            //var len = 0;
            //var buffer = new byte[ushort.MaxValue - 29];


            //buffer = Encoding.Default.GetBytes(txtbx.Text);
            //client.SendTo(buffer, remoteEP);

            //await client.SendToAsync(buffer, SocketFlags.None, remoteEP);
            //var list = new List<byte>();
            //var maxlen = buffer.Length;
            //while (true)
            //{
            //    while (true)
            //    {
            //        var result = await client.ReceiveFromAsync();
            //        list.AddRange(result.Buffer);
            //        if (result.Buffer.Length != maxlen) break;
            //    }

            //    ClientImage.Source = ByteToImage(list.ToArray());
            //    list.Clear();
            //}
        }
    }
}
