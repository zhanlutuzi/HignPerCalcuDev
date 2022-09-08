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
            Rect rect3 = new Rect();
            Rect rect4 = new Rect();

            double width = map.Range.XMax - map.Range.XMin;
            double height = map.Range.YMax - map.Range.YMin;

            rect1.XMax = map.Range.XMin + width / 2;
            rect1.YMax = map.Range.YMax;
            rect1.YMin = map.Range.YMin + height / 2;
            rect1.XMin = map.Range.XMin;

            rect2.XMax = map.Range.XMax;
            rect2.YMax = map.Range.YMax;
            rect2.YMin = map.Range.YMin + height / 2;
            rect2.XMin = map.Range.XMin + width / 2;

            rect3.XMax = map.Range.XMax + width / 2;
            rect3.YMax = map.Range.YMax + height / 2;
            rect3.YMin = map.Range.YMin;
            rect3.XMin = map.Range.XMin;

            rect4.XMax = map.Range.XMax;
            rect4.YMax = map.Range.YMin + height / 2;
            rect4.YMin = map.Range.YMin;
            rect4.XMin = map.Range.XMin + width / 2;


            var ThreadOne = new Thread(() => ToPicture(map, rect1, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX2/temp/ThreadOne.png", pictureBox1));
            ThreadOne.Name = "ThreadOne";
            var ThreadTwo = new Thread(() => ToPicture(map, rect2, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX2/temp/ThreadTwo.png", pictureBox2));
            ThreadTwo.Name = "ThreadTwo";
            var ThreadThree = new Thread(() => ToPicture(map, rect3, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX2/temp/ThreadThree.png", pictureBox3));
            ThreadThree.Name = "ThreadThree";
            var ThreadFour = new Thread(() => ToPicture(map, rect4, "D:/Desktop/code/Learning/HignPerCalcuDev/HignPerCalcuDev/EX2/temp/ThreadFour.png", pictureBox4));
            ThreadFour.Name = "ThreadFour";


            ThreadOne.Start();
            ThreadTwo.Start();
            ThreadThree.Start();
            ThreadFour.Start();
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
