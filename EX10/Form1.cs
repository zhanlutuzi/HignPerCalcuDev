using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EX10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calculate cal1 = new calculate();
            calculate cal2 = new calculate();
            calculate cal3 = new calculate();

            cal1.calculatePixel("1.txt", 1);
            cal2.calculatePixel("2.txt", 2);
            cal3.calculatePixel("3.txt", 3);

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            dt.Columns.Add("行号", typeof(int));
            dt.Columns.Add("列号", typeof(int));
            dt.Columns.Add("计算前像元值", typeof(double));
            dt.Columns.Add("计算后像元值", typeof(double));
            dt1.Columns.Add("行号", typeof(int));
            dt1.Columns.Add("列号", typeof(int));
            dt1.Columns.Add("计算前像元值", typeof(double));
            dt1.Columns.Add("计算后像元值", typeof(double));
            dt2.Columns.Add("行号", typeof(int));
            dt2.Columns.Add("列号", typeof(int));
            dt2.Columns.Add("计算前像元值", typeof(double));
            dt2.Columns.Add("计算后像元值", typeof(double));
            dt3.Columns.Add("行号", typeof(int));
            dt3.Columns.Add("列号", typeof(int));
            dt3.Columns.Add("计算前像元值", typeof(double));
            dt3.Columns.Add("计算后像元值", typeof(double));

            string[] rows = File.ReadAllLines(@"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX10\txt\1.txt");
            //读取返回的计算结果的所有行
            foreach (string row in rows)
            {
                dt.Rows.Add(row.Split(','));//将每行根据“，”分隔，并加载表格dt
                dt3.Rows.Add(row.Split(','));//将每行根据“，”分隔，并加载表格dt2
            }
            string[] rows1 = File.ReadAllLines(@"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX10\txt\2.txt");
            //读取返回的计算结果的所有行
            foreach (string row1 in rows1)
            {
                dt1.Rows.Add(row1.Split(','));//将每行根据“，”分隔，并加载表格dt1
                dt3.Rows.Add(row1.Split(','));//将每行根据“，”分隔，并加载表格dt2
            }
            string[] rows2 = File.ReadAllLines(@"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX10\txt\3.txt");
            //读取返回的计算结果的所有行
            foreach (string row2 in rows2)
            {
                dt2.Rows.Add(row2.Split(','));//将每行根据“，”分隔，并加载表格dt1
                dt3.Rows.Add(row2.Split(','));//将每行根据“，”分隔，并加载表格dt2
            }


            dataGridView1.DataSource = dt;
            dataGridView2.DataSource = dt1;
            dataGridView3.DataSource = dt2;
            dataGridView4.DataSource = dt3;
        }
    }
}
