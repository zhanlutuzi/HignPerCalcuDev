using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapGIS.GeoDataBase;
using System.Threading;
using MapGIS.GeoObjects.Att;
using MapGIS.GeoObjects;
using MapGIS.GeoObjects.Geometry;

namespace EX5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //定义数据源、数据库变量
            Server Svr = null;
            DataBase GDB = null;
            SFeatureCls SFCls = null;

            Svr = new Server();
            //连接数据源
            Svr.Connect("MapGisLocal", "", "");

            GDB = new DataBase();
            GDB = Svr.OpenGDB("空间信息高性能计算实验");
            SFCls = new SFeatureCls(GDB);
            //打开图层
            SFCls.Open("道路", 1);

            Control.CheckForIllegalCrossThreadCalls = false;
            var threadOne = new Thread(() => statistics(SFCls, listView1, 1));
            threadOne.Name = "ThreadOne";
            var threadTwo = new Thread(() => statistics(SFCls, listView2, 2));
            threadTwo.Name = "ThreadTwo";
            var threadThr = new Thread(() => statistics(SFCls, listView3, 3));
            threadThr.Name = "ThreadThr";
            var threadFour = new Thread(() => statistics(SFCls, listView4, 4));
            threadFour.Name = "ThreadFour";

            threadOne.Start();
            threadTwo.Start();
            threadThr.Start();
            threadFour.Start();
        }

        private void statistics(SFeatureCls resource_sfcls, ListView listView, int regionID)
        {
            RecordSet _RecordSet = null;
            IGeometry _Geometry = null;
            QueryDef _QueryDef = new QueryDef();
            Rect rect = new Rect();
            SpaQueryMode mode = new SpaQueryMode();
            GeoVarLine _GeoVarLine = new GeoVarLine();
            GeoLines _GeoLines = new GeoLines();
            long oid = 0;
            double length = 0;

            double recXMin = resource_sfcls.Range.XMin;
            double recXMax = resource_sfcls.Range.XMax;
            double recYMin = resource_sfcls.Range.YMin;
            double recYMax = resource_sfcls.Range.YMax;
            double halfx = (resource_sfcls.Range.XMax - resource_sfcls.Range.XMin) / 2;
            double halfy = (resource_sfcls.Range.YMax - resource_sfcls.Range.YMin) / 2;

            if (regionID == 1)
            {
                // 左上角的矩形
                // 相交查询
                rect.XMax = recXMin + halfx;
                rect.YMax = recYMax;
                rect.YMin = recYMin + halfy;
                rect.XMin = recXMin;
                mode = SpaQueryMode.Intersect;

            }
            if (regionID == 2)
            {
                // 右上角的矩形
                // 包含查询
                rect.XMax = recXMax;
                rect.YMax = recYMax;
                rect.YMin = recYMin + halfy;
                rect.XMin = recXMin + halfx;
                mode = SpaQueryMode.Contain;
            }
            if(regionID == 3)
            {
                // 左下角的矩形
                // 区域外查询
                rect.XMax = recXMin + halfx;
                rect.YMax = recYMin + halfy;
                rect.YMin = recYMin;
                rect.XMin = recXMin;
                mode = SpaQueryMode.Intersect;
            }
            if(regionID == 4)
            {
                // 右下角的矩形
                // 最小外包矩形查询
                rect.XMax = recXMax;
                rect.YMax = recYMin + halfy;
                rect.YMin = recYMin;
                rect.XMin = recXMin + halfx;
                mode = SpaQueryMode.MBRIntersect;
            }

            //在ListView1控件第一列增加“OID”字段
            FieldName("OID", listView);
            FieldName("要素的长度", listView);

            //将符合查询条件的要素存入RecordSet，然后进行遍历可以得到每一个要素的信息
            _QueryDef = new QueryDef();
            _QueryDef.SetRect(rect, mode);
            _RecordSet = resource_sfcls.Select(_QueryDef);

            bool rtn;
            rtn = _RecordSet.MoveFirst();

            while (!_RecordSet.IsEOF)
            {
                _Geometry = _RecordSet.Geometry;//获取当前要素的空间信息
                oid = _RecordSet.CurrentID;
                GeometryType type = _Geometry.Type;//获取当前要素的几何约束类型

                if (_Geometry != null)
                {
                    switch (type)
                    {
                        case GeometryType.VarLine:
                            {
                                _GeoVarLine = new GeoVarLine();
                                _GeoVarLine = _Geometry as GeoVarLine;
                                length = _GeoVarLine.CalLength();
                                break;
                            }
                        case GeometryType.Lines:
                            {
                                _GeoLines = new GeoLines();
                                _GeoLines = _Geometry as GeoLines;
                                length = _GeoLines.CalLength();
                                break;
                            }
                    }
                }
                ListViewItem items = null;
                items = listView.Items.Add(oid.ToString());
                items.SubItems.Add(length.ToString());
                rtn = _RecordSet.MoveNext();
            }
        }

        public void FieldName(string name, ListView listView)
        {
            listView.Columns.Add(name, 120, HorizontalAlignment.Center);
        }
    }
}
