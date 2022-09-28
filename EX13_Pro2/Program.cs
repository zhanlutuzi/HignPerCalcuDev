using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapGIS.GeoDataBase.GeoRaster;
using MapGIS.GeoDataBase;
using MapGIS.RasAnalysis;

namespace EX13_Pro2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.rasterIn();
            p.rasterOut();
        }
        public void rasterIn()
        {
            string url1 = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX13\data\13-2.tif";
            string url2 = "gdbp://MapGisLocal/sample/ras/13-2";
            string frm = "GTiff";
            RasTrans ras = new RasTrans();
            int flg = ras.RsImgTrans(url1, url2, frm);
            if (flg != 0)
            {
                Console.WriteLine("数据导入成功");
            }
            else
            {
                Console.WriteLine("数据导入失败");
            }
        }
        public void rasterOut()
        {
            string url3 = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX13\result\13-2.tif";
            string url4 = "gdbp://MapGisLocal/sample/ras/13-2";
            string frm1 = "GTiff";
            RasTrans ras1 = new RasTrans();
            int flg = ras1.RsImgTrans(url4, url3, frm1);
            if (flg != 0)
            {
                Console.WriteLine("数据导出成功");
            }
            else
            {
                Console.WriteLine("数据导出失败");
            }
        }

    }
}
