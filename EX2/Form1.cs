using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using MapGIS.GeoMap;
using MapGIS.GeoObjects;
using MapGIS.GeoObjects.Geometry;

namespace EX2
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Document doc = new Document();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            String fileURL = openFileDialog.FileName;
            doc.Open(fileURL);

            Maps maps = doc.GetMaps();
            Map map = maps.GetMap(0);

            Rect rect1 = new Rect();
            Rect rect2 = new Rect();

            rect1.XMax = map.Range.XMin + (map.Range.XMax - map.Range.XMin) / 2;
            rect1.YMax = map.Range.YMax;
            rect1.YMin = map.Range.YMin;
            rect1.XMin = map.Range.XMin;

            rect2.XMax = map.Range.XMax;
            rect2.YMax = map.Range.YMax;
            rect2.YMin = map.Range.YMin;
            rect2.XMin = map.Range.XMin + (map.Range.XMax - map.Range.XMin) / 2;


            var ThreadOne = new Thread(() => ToPicture(map, rect1, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX2/temp/ThreadOne.png", pictureBox1));
            ThreadOne.Name = "ThreadOne";
            var ThreadTwo = new Thread(() => ToPicture(map, rect2, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX2/temp/ThreadTwo.png", pictureBox2));
            ThreadTwo.Name = "ThreadTwo";

            ThreadOne.Start();
            ThreadTwo.Start();
        }

        public void ToPicture(Map map,Rect rect,string picURL,PictureBox picBox)
        {
            map.SetViewRange(rect);
            double width = rect.XMax - rect.XMin;
            double height = rect.YMax - rect.YMin;
            map.OutputToImageFile(picURL, width, height, ImgType.PNG, true, 0, false);
            picBox.ImageLocation = picURL;
        }
    }
}
