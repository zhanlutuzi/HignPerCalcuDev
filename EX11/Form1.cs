using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EX11
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CutImg cut1 = new CutImg();
            CutImg cut2 = new CutImg();

            cut1.cut("1.tif","range1");
            cut2.cut("2.tif","range2");

            string url1 = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX11\result\1.tif";//进程1裁剪的结果
            string url2 = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX11\result\2.tif";//进程2裁剪的结果
            this.pictureBox1.Load(url1);
            this.pictureBox2.Load(url2);
        }
    }
}
