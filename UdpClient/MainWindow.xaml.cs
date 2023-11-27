using System;
using System.Collections.Generic;
using System.IO;
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

namespace UdpClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();



            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var client = new Socket(AddressFamily.InterNetwork,
                          SocketType.Dgram,
                          ProtocolType.Udp);


            var Ip = IPAddress.Parse("192.168.100.8");
            var Port = 54709;

            var remoteEP = new IPEndPoint(Ip, Port);

            var len = 0;
            var buffer = Array.Empty<byte>();

      
            buffer = Encoding.Default.GetBytes(txtbx.Text);
            client.SendTo(buffer, remoteEP);

            
        }
    }
}
