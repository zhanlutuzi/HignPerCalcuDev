using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapGIS.GeoDataBase;
using MapGIS.GeoObjects;
using MapGIS.GeoObjects.Att;
using System.Threading;


namespace EX4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Server Svr = null;
            DataBase GDB = null;
            SFeatureCls SFCLs = null;

            Svr = new Server();
            Svr.Connect("MapGisLocal", "", "");

            GDB = new DataBase();
            GDB = Svr.OpenGDB("空间信息高性能计算实验");

            SFCLs = new SFeatureCls(GDB);
            SFCLs.Open("道路", 1);

            Control.CheckForIllegalCrossThreadCalls = false;
            var threadOne = new Thread(() => GetSFCIs(SFCLs, listView1, 1));
            threadOne.Name = "ThreadOne";
            var threadTwo = new Thread(() => GetSFCIs(SFCLs, listView2, 2));
            threadTwo.Name = "ThreadTwo";
            var threadThr = new Thread(() => GetSFCIs(SFCLs, listView3, 3));
            threadThr.Name = "ThreadThr";
            var threadFour = new Thread(() => GetSFCIs(SFCLs, listView4, 4));
            threadFour.Name = "ThreadFour";

            threadOne.Start();
            threadTwo.Start();
            threadThr.Start();
            threadFour.Start();
        }

        public void GetSFCIs(SFeatureCls SFCls,ListView listView,int pre_or_next)
        {
            Fields Flds = null;
            Field Fld = null;
            Record Rcd = null;
            Rcd = new Record();
            Flds = new Fields();

            Flds = SFCls.Fields;
            int num = Flds.Count;

            if (num > 0)
                FieldName("OID", listView);
            for(int i = 0;i<num;i++)
            {
                Fld = Flds.GetItem(i);
                string name = Fld.FieldName;
                FieldName(name, listView);
            }

            int objCount = SFCls.Count;
            int n = 0;
            long id = 0;
            double task_count = objCount / 4;

            while(n <objCount)
            {
                Rcd = SFCls.GetAtt(id);
                if (Rcd == null)
                {
                    id++;
                    continue;
                }
                else
                    n++;
                Flds = Rcd.Fields;
                if (pre_or_next == 1)
                {
                    if (n <= task_count)
                    {
                        ListViewItem items = null;
                        items = listView.Items.Add(id.ToString());
                        for (int i = 0; i < num; i++)
                        {
                            Fld = Flds.GetItem(i);
                            string name = Fld.FieldName;
                            object val = Rcd.get_FldVal(name);
                            ObjectVal(items, val);
                        }
                    }
                }
                if (pre_or_next == 2)
                {
                    if (n > task_count && n<=(2*task_count))
                    {
                        ListViewItem items = null;
                        items = listView.Items.Add(id.ToString());
                        for (int i = 0; i < num; i++)
                        {
                            Fld = Flds.GetItem(i);
                            string name = Fld.FieldName;
                            object val = Rcd.get_FldVal(name);
                            ObjectVal(items, val);
                        }
                    }
                }
                if (pre_or_next == 3)
                {
                    if (n > (2*task_count) && n <= (3 * task_count))
                    {
                        ListViewItem items = null;
                        items = listView.Items.Add(id.ToString());
                        for (int i = 0; i < num; i++)
                        {
                            Fld = Flds.GetItem(i);
                            string name = Fld.FieldName;
                            object val = Rcd.get_FldVal(name);
                            ObjectVal(items, val);
                        }
                    }
                }
                else
                {
                    if (n > 3*task_count)
                    {
                        ListViewItem items = null;
                        items = listView.Items.Add(id.ToString());
                        for (int i = 0; i < num; i++)
                        {
                            Fld = Flds.GetItem(i);
                            string name = Fld.FieldName;
                            object val = Rcd.get_FldVal(name);
                            ObjectVal(items, val);
                        }
                    }
                }
                id++;
            }

        }

        public void ObjectVal(ListViewItem items,object val)
        {
            if (val == null)
                items.SubItems.Add("");
            else
                items.SubItems.Add(val.ToString());
        }
        public void FieldName(string name,ListView listView)
        {
            listView.Columns.Add(name, 120, HorizontalAlignment.Center);
        }
    }
}
