using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace EX9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = @"C:\Application\MapGIS\MapGIS 10\Program\netcoreapp3.1\Pro1.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //启动程序
            Process p1 = new Process();
            //设置要启动的应用程序
            p1.StartInfo.FileName = @"C:\Application\MapGIS\MapGIS 10\Program\netcoreapp3.1\Pro2.exe";
            //是否使用操作系统shell启动
            p1.StartInfo.UseShellExecute = false;
            // 接受来自调用程序的输入信息
            p1.StartInfo.RedirectStandardInput = true;
            //输出信息
            p1.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p1.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p1.StartInfo.CreateNoWindow = true;

            //启动程序
            p1.Start();
            p.Start();
            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            p1.WaitForExit();
            p1.Close();

            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            dt.Columns.Add("行号", typeof(int));
            dt.Columns.Add("列号", typeof(int));
            dt.Columns.Add("像元值", typeof(double));
            dt1.Columns.Add("行号", typeof(int));
            dt1.Columns.Add("列号", typeof(int));
            dt1.Columns.Add("像元值", typeof(double));
            dt2.Columns.Add("行号", typeof(int));
            dt2.Columns.Add("列号", typeof(int));
            dt2.Columns.Add("像元值", typeof(double));

            string[] rows = File.ReadAllLines(@"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\Pro1\txt1-0.txt");
            foreach (string row in rows)
            {
                dt.Rows.Add(row.Split(','));
                dt2.Rows.Add(row.Split(','));
            }
            //
            string[] rows1 = File.ReadAllLines(@"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\Pro1\txt1-1.txt");
            foreach (string row1 in rows1)
            {
                dt1.Rows.Add(row1.Split(','));
                dt2.Rows.Add(row1.Split(','));
            }

            dataGridView1.DataSource = dt;
            dataGridView2.DataSource = dt1;
            dataGridView3.DataSource = dt2;
        }
    }
}
