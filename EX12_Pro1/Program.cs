using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace EX12_Pro1
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.downloadTile();
        }
        public void downloadTile()
        {
            double x1 = -20037508.34;
            double x2 = 20037508.34;
            double y1 = -20037508.34;
            double y2 = 20037508.34;
            double xmin = 12757777.1450514;
            double xmax = 12759241.3918745;
            double ymin = 3562378.75577342;
            double ymax = 3563243.44965588;
            int num = 1;
            WebClient client = new WebClient();
            double width16 = (x2 - x1) / Math.Pow(2, 15);
            double L16min = Math.Floor((xmin - x1) / width16);
            double H16min = Math.Floor((y2 - ymax) / width16);
            double L16max = Math.Floor((xmax - x1) / width16);
            double H16max = Math.Floor((y2 - ymin) / width16);
            Console.WriteLine("16级" + "," + H16min + "," + H16max + "," + L16min + "," + L16max);
            for (int i = Convert.ToInt32(H16min); i < Convert.ToInt32(H16max) + 1; i++)
            {
                for (int j = Convert.ToInt32(L16min); j < Convert.ToInt32(L16max) + 1; j++)
                {
                    string filename = 16 + "-" + num + "." + "png";
                    num = num + 1;
                    string dir = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX12\export\" + filename;
                    string url = "http://localhost:6163/igs/rest/mrms/tile/TILE/" + (16 - 1) + "/" + i + "/" + j;
                    client.DownloadFile(url, dir);
                }
            }
            client.Dispose();
        }
    }
}
