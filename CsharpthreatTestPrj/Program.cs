using System;
using System.Threading;

namespace CsharpthreatTestPrj
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.testThread();

        }

        public void testThread()
        {

            Thread th = new Thread(new ThreadStart(ThreadPro));
            Thread th2 = new Thread(new ThreadStart(ThreadPro));
            th.Name = "threadOne";
            th2.Name = "threadTwo";
            Console.WriteLine("threadOne start!\n");
            th.Start();
            Console.WriteLine("threadTwo start!\n");
            th2.Start();

        }
        public void ThreadPro()
        {

            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine("CurrentThread = " + Thread.CurrentThread.Name + " i = " + i);
                // Console.WriteLine("[0]:[1]", Thread.CurrentThread.Name, i);
            }
        }
    }
}
