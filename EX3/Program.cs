using System;
using MapGIS.GeoMap;
using MapGIS.GeoObjects;
using MapGIS.GeoObjects.Geometry;
using System.Threading;

namespace EX3
{
    class Program
    {
        static void Main(string[] args)
        {
            Document doc = new Document();
            String fileURL = "D:/Desktop/HighPerform/data/新校区.mapx";
            doc.Open(fileURL);
            Maps maps = doc.GetMaps();
            if (maps.Count > 0)
            {
                //获取当前第一个地图
                Map map = maps.GetMap(0);

                var threadOne = new Thread(() => ToSlices(map, 0, 3, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX3/thread1/"));
                threadOne.Name = "ThreadOne";
                var threadTwo = new Thread(() => ToSlices(map, 4, 4, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX3/thread2/"));
                threadTwo.Name = "ThreadTwo";

                threadOne.Start();
                threadTwo.Start();
            }
        }
        

        static void ToSlices(Map map, int start_level, int end_level, string URL)
        {
            int i, j, k = 0;//作为循环变量使用
            int T_Row, T_Col = 0;//总行数，总列数
            double T_edge, edge = 0;//总边长,每个级别的子范围的边长
            string name = "";//要存储的图片的名字
            Rect rect = new Rect();//在地图上每个切片的矩形范围

            //将原先图层的范围纠正为正方形
            if ((map.Range.XMax - map.Range.XMin) >= (map.Range.YMax - map.Range.YMin))
            {
                T_edge = map.Range.XMax - map.Range.XMin;//正方形的边长
            }
            else
            {
                T_edge = map.Range.YMax - map.Range.YMin;//正方形的边长
            }

            for (i = start_level; i <= end_level; i++)
            {
                Console.WriteLine("正在生成第" + i.ToString() + "级的切片！");
                edge = T_edge / Math.Pow(2, i);//每个格子的边长
                T_Row = Convert.ToInt32(Math.Pow(2, i));//格网的总行数
                T_Col = Convert.ToInt32(Math.Pow(2, i));//格网的总列数
                for (j = 0; j < T_Row; j++)
                {
                    for (k = 0; k < T_Col; k++)
                    {
                        name = i.ToString() + "_" + j.ToString() + "_" + k.ToString();
                        rect = new Rect();

                        rect.XMin = map.Range.XMin + k * edge;
                        rect.YMin = map.Range.YMin + j * edge;
                        rect.XMax = map.Range.XMin + (k + 1) * edge;
                        rect.YMax = map.Range.YMin + (j + 1) * edge;

                        Console.WriteLine("正在保存名字为" + name + "的图片！");
                        string picURL = URL + name + ".PNG";
                        map.SetViewRange(rect);
                        bool rtn = map.OutputToImageFile(picURL, 500, 500, ImgType.PNG, true, 0, false);
                        
                    }
                }
                Console.WriteLine("第" + i.ToString() + "级的切片保存结束！");
            }
        }
    }
}
