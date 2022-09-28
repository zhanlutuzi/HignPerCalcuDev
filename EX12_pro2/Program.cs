using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace EX12_pro2
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
            double width17 = (y2 - y1) / Math.Pow(2, 16);
            double L17min = Math.Floor((xmin - x1) / width17);
            double H17min = Math.Floor((y2 - ymax) / width17);
            double L17max = Math.Floor((xmax - x1) / width17);
            double H17max = Math.Floor((y2 - ymin) / width17);
            Console.WriteLine("17级" + "," + H17min + "," + H17max + "," + L17min + "," + L17max);
            for (int i = Convert.ToInt32(H17min); i < Convert.ToInt32(H17max) + 1; i++)
            {
                for (int j = Convert.ToInt32(L17min); j < Convert.ToInt32(L17max) + 1; j++)
                {
                    string filename = 17 + "-" + num + "." + "png";
                    num = num + 1;
                    string dir = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX12\export\" + filename;
                    string url = "http://localhost:6163/igs/rest/mrms/tile/瓦片地图/" + 16 + "/" + i + "/" + j;
                    client.DownloadFile(url, dir);
                }
            }
            client.Dispose();
        }
    }
}
