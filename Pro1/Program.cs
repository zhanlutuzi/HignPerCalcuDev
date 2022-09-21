using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MapGIS.GeoDataBase;
using MapGIS.GeoDataBase.GeoRaster;

namespace Pro1
{
    class Program
    {
        static void Main(string[] args)
        {
            Program P = new Program();
            P.getPixel();
            Console.WriteLine("close getPixel");
        }

        public void getPixel()
        {
            Console.WriteLine("getPixel");
            RasterDataSet cRasDataSet = null;//栅格数据集
            RasterBand band = null;//波段
            DataBase database = null;//数据库
            Server srv = new Server();//服务
            bool rtn = false;//判断栅格数据集是否打开
            int height = 0;//行数
            int width = 0;//列数
            double val = 0;//像元值
            string str = "";//字符串

            srv.Connect("MapGISLocalPlus", "", "");//连接数据源
            database = srv.OpenGDB("sample");//打开数据库

            cRasDataSet = new RasterDataSet(database);//获取栅格数据集
            rtn = cRasDataSet.Open("地大新校区卫星图", RasAccessType.RasAccessType_Update);
            Console.WriteLine(rtn);
            //文件流
            FileStream fs = new FileStream(@"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\Pro1\txt1-0.txt", System.IO.FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            //string str =  "行号" + "," + "列号" + "," + "像元值";
            //sw.WriteLine(str);
            if (rtn)
            {
                height = cRasDataSet.Height;//获取栅格数据集的行数
                width = cRasDataSet.Width;//获取栅格数据集的列数
                band = cRasDataSet.GetRasterBand(1);//获取栅格数据集的
                bool flg = cRasDataSet.OpenPyramidLayer(1);//打开栅格数据的金字塔

                for (int i = 290; i < 291; i++)
                {
                    for (int j = 50; j < 70; j++)
                    {
                        val = band.GetPixel(j, i);//获取像元值
                        Console.WriteLine(i + "，" + val);//控制台输出
                        str = i + "," + j + "," + val;
                        sw.WriteLine(str);//将行号、列号和对应的像元值按行写入本地文本文件
                    }
                }

            }
            else
            {
                Console.WriteLine("栅格数据集获取失败");
            }

            sw.Close();//
            cRasDataSet.Close();//
            database.Close();//
            srv.DisConnect();//
        }

    }
}
