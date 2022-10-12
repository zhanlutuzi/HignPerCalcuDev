using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace EX12
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.startProcess();
        }
        public void startProcess()
        {
            //实例化p进程
            Process p = new Process();
            //设置要启动的应用程序
            p.StartInfo.FileName = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX12\bin\netcoreapp3.1\EX12_Pro1.exe";
            //是否使用操作系统shell启动
            p.StartInfo.UseShellExecute = false;
            // 接收来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;
            //输出信息
            p.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p.StartInfo.CreateNoWindow = true;
            //实例化p1进程
            Process p1 = new Process();
            //是否使用操作系统shell启动
            p1.StartInfo.FileName = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX12\bin\netcoreapp3.1\EX12_pro2.exe";
            //是否使用操作系统shell启动
            p1.StartInfo.UseShellExecute = false;

            // 接收来自调用程序的输入信息
            p1.StartInfo.RedirectStandardInput = true;
            //输出信息
            p1.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p1.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p1.StartInfo.CreateNoWindow = true;

            //实例化p进程
            Process p2 = new Process();
            //设置要启动的应用程序
            p2.StartInfo.FileName = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX12\bin\netcoreapp3.1\EX12Pro3.exe";
            //是否使用操作系统shell启动
            p2.StartInfo.UseShellExecute = false;
            // 接收来自调用程序的输入信息
            p2.StartInfo.RedirectStandardInput = true;
            //输出信息
            p2.StartInfo.RedirectStandardOutput = true;
            // 输出错误
            p2.StartInfo.RedirectStandardError = true;
            //不显示程序窗口
            p2.StartInfo.CreateNoWindow = true;


            p1.Start();//启动进程2
            p.Start();//启动进程1
            p2.Start();//启动进程3
            p.WaitForExit();//等待程序执行完退出进程
            int code = p.ExitCode;//进程退出码，正确退出返回0
            if (code == 0)
            {
                Console.WriteLine("16级的瓦片地图下载成功");
            }
            p.Close();//关闭进程
            p1.WaitForExit();//等待程序执行完退出进程
            int code1 = p1.ExitCode;//进程退出码，正确退出返回0
            if (code1 == 0)
            {
                Console.WriteLine("17级的瓦片地图下载成功");
            }
            p1.Close();//进程退出码，正确退出返回0
            p2.WaitForExit();//等待程序执行完退出进程
            int code2 = p2.ExitCode;//进程退出码，正确退出返回0
            if (code2 == 0)
            {
                Console.WriteLine("18级的瓦片地图下载成功");
            }
            p2.Close();//进程退出码，正确退出返回0
        }
    }
}
