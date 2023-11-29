using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;

int imagecounter = 0;
DirectoryInfo di = Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/ScreenShots");




    

    byte[] CaptureAndSaveScreenshot()
    {

        
            int width = 1920;
            int height = 1080;

            using (Bitmap bmp = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(0,0, 0, 0, new System.Drawing.Size(width, height));
                }

                if (File.Exists("last index.txt")) imagecounter = Convert.ToInt32(File.ReadAllText("last index.txt"));
                string fileName = $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/{di.Name}/Screenshot_{++imagecounter}.png";
                


                using (MemoryStream stream = new MemoryStream())
                {
                    bmp.Save(fileName, ImageFormat.Png);
                    bmp.Save(stream, ImageFormat.Png);

                    File.WriteAllText("last index.txt", imagecounter.ToString());

                    return stream.ToArray();
                }




            }

        
        
        
    }



UdpClient udpClient = new UdpClient(27001);
var remoteEP = new IPEndPoint(IPAddress.Any, 0);

while (true)
{
    var result = await udpClient.ReceiveAsync();

    new Task(async () =>
    {
        remoteEP = result.RemoteEndPoint;
        
        byte[] image = CaptureAndSaveScreenshot();
        var chunks = image.Chunk(ushort.MaxValue - 29);
        foreach (var item in chunks)
        {
            await udpClient.SendAsync(item, item.Length, remoteEP);
        }
        
    }).Start();
}






