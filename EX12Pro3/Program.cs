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
            double width18 = (x2 - x1) / Math.Pow(2, 17);
            double L18min = Math.Floor((xmin - x1) / width18);
            double H18min = Math.Floor((y2 - ymax) / width18);
            double L18max = Math.Floor((xmax - x1) / width18);
            double H18max = Math.Floor((y2 - ymin) / width18);
            Console.WriteLine("18级" + "," + H18min + "," + H18max + "," + L18min + "," + L18max);
            for (int i = Convert.ToInt32(H18min); i < Convert.ToInt32(H18max) + 1; i++)
            {
                for (int j = Convert.ToInt32(L18min); j < Convert.ToInt32(L18max) + 1; j++)
                {
                    string filename = 18 + "-" + num + "." + "png";
                    num = num + 1;
                    string dir = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX12\export\" + filename;
                    string url = "http://localhost:6163/igs/rest/mrms/tile/TILE/" + (18 - 1) + "/" + i + "/" + j;
                    client.DownloadFile(url, dir);
                }
            }
            client.Dispose();
        }
    }
}
