using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace udpServerr
{
    internal class Program
    {
        static int imagecounter = 0;
        static DirectoryInfo di = Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/ScreenShots");




        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }


        static Bitmap CaptureAndSaveScreenshot()
        {
            IntPtr handle = GetForegroundWindow();

            if (GetWindowRect(handle, out RECT rect))
            {
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;

                using (Bitmap bmp = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(rect.Left, rect.Top, 0, 0, new Size(width, height));
                    }

                    if (File.Exists("last index.txt")) imagecounter = Convert.ToInt32(File.ReadAllText("last index.txt"));
                    string fileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/{di.Name}/Screenshot_{++imagecounter}.png";

                    bmp.Save(fileName, ImageFormat.Png);
                    File.WriteAllText("last index.txt", imagecounter.ToString());

                    return bmp;


                }

            }
            else
            {
                return null;
            }
            
        }

       
        static void Main(string[] args)
        {
            var listener = new Socket(AddressFamily.InterNetwork,
                          SocketType.Dgram,
                          ProtocolType.Udp);


            var Ip = IPAddress.Parse("192.168.100.8");
            var Port = 54709;

            var listenerEP = new IPEndPoint(Ip, Port);

            listener.Bind(listenerEP);

            var msg = "";
            var len = 0;
            var buffer = new byte[ushort.MaxValue - 29];


            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);


            
            len = listener.ReceiveFrom(buffer, ref remoteEP);

            msg = Encoding.Default.GetString(buffer, 0, len);
            if(msg=="Salam")
            {
                Bitmap image =CaptureAndSaveScreenshot();
                using MemoryStream memoryStream = new MemoryStream();
                image.Save(memoryStream, ImageFormat.Png);
                byte[] imageinbytes= memoryStream.ToArray();
                
                var client = new Socket(AddressFamily.InterNetwork,
                           SocketType.Dgram,
                           ProtocolType.Udp);




                
                var serverEndPoint = new IPEndPoint(Ip, Port);

                client.SendTo(imageinbytes,serverEndPoint);

            }
            Console.WriteLine($"{remoteEP} : {msg}");
            
        }
    }
}