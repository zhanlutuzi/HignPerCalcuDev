using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapGIS.GISControl;
using MapGIS.UI.Controls;
using MapGIS.GeoDataBase;
using MapGIS.GeoMap;
using MapGIS.GeoObjects.Geometry;
using MapGIS.GeoObjects;
using MapGIS.GeoObjects.Info;
using System.Threading;

namespace EX6
{
    public partial class Form1 : Form
    {
        //在SplitContainer添加MapControl控件
        MapControl mapCtrl = new MapControl();
        //工作空间树
        MapWorkSpaceTree _Tree = new MapWorkSpaceTree();
        //定义数据源、数据库变量
        Server Svr = null;
        DataBase GDB = null;
        SFeatureCls sfcls = null;
        public Form1()
        {
            InitializeComponent();
            initControls();
        }

        public void initControls()
        {
            //MapControl控件在Panel2里
            this.splitContainer1.Panel2.Controls.Add(mapCtrl);
            mapCtrl.Width = this.splitContainer1.Panel2.Width;
            mapCtrl.Height = this.splitContainer1.Panel2.Height;
            //工作空间树控件加载到Panel1上
            _Tree.Dock = DockStyle.Fill;
            this.splitContainer1.Panel1.Controls.Add(_Tree);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //打开数据源
            Svr = new Server();
            Svr.Connect("MapGisLocal", "", "");
            GDB = new DataBase();
            GDB = Svr.OpenGDB("空间信息高性能计算实验");
            sfcls = new SFeatureCls(GDB);

            //打开图层
            sfcls.Open("道路", 1);

            //新建一个文档树
            _Tree.WorkSpace.BeginUpdateTree();
            _Tree.Document.Title = "地图文档";
            _Tree.Document.New();

            //在地图文档下添加一个地图
            Map map = new Map();
            map.Name = "新地图";
            //附加矢量图层
            VectorLayer vecLayer = new VectorLayer(VectorLayerType.SFclsLayer);
            vecLayer.AttachData(sfcls);
            //将图层添加到地图中
            vecLayer.Name = sfcls.ClsName;
            map.Append(vecLayer);

            _Tree.Document.GetMaps().Append(map);
            //将该地图设置为MapConrol的激活地图
            this.mapCtrl.ActiveMap = map;
            this.mapCtrl.Restore();

            //展开所有的节点
            _Tree.ExpandAll();
            _Tree.WorkSpace.EndUpdateTree();

            vecLayer.DetachData();//附加解除 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            var threadOne = new Thread(() => Buffer(sfcls, GDB, "threadOne_buffer", 1));
            threadOne.Name = "ThreadOne";
            var threadTwo = new Thread(() => Buffer(sfcls, GDB, "threadTwo_buffer", 2));
            threadTwo.Name = "ThreadTwo";

            threadOne.Start();
            threadTwo.Start();

        }

        public void Buffer(SFeatureCls resource_sfcls, DataBase GDB, string name, int L_or_R)
        {
            QueryDef _QueryDef = null;
            Rect rect = null;
            RecordSet _RecordSet = null;
            IGeometry bufferGeometry = null;
            IGeometry _Geometry = null;
            SpaQueryMode mode = new SpaQueryMode();
            GeoVarLine _GeoVarLine = null;
            GeoLines _GeoLines = null;
            RegInfo _RegInfo = null; //线的图形信息
            rect = new Rect();

            if (L_or_R == 1)
            {
                //左边的矩形框
                rect.XMax = resource_sfcls.Range.XMin + (resource_sfcls.Range.XMax - resource_sfcls.Range.XMin) / 2;
                rect.YMax = resource_sfcls.Range.YMax;
                rect.YMin = resource_sfcls.Range.YMin;
                rect.XMin = resource_sfcls.Range.XMin;
                mode = SpaQueryMode.Intersect;
                _RegInfo = new RegInfo();
                //设置面的图形信息
                _RegInfo.FillClr = 168;//黄色

            }
            else
            {
                rect.XMax = resource_sfcls.Range.XMax;
                rect.YMax = resource_sfcls.Range.YMax;
                rect.YMin = resource_sfcls.Range.YMin;
                rect.XMin = resource_sfcls.Range.XMin + (resource_sfcls.Range.XMax - resource_sfcls.Range.XMin) / 2;
                mode = SpaQueryMode.Contain;
                _RegInfo = new RegInfo();
                //设置面的图形信息
                _RegInfo.FillClr = 376;//绿色
            }

            //将符合查询条件的要素存入RecordSet，然后进行遍历可以得到每一个要素的信息
            _QueryDef = new QueryDef();
            _QueryDef.SetRect(rect, mode);
            _RecordSet = resource_sfcls.Select(_QueryDef);

            int id = 0;
            //创建面简单要素类
            SFeatureCls result_sfcls = null;
            result_sfcls = new SFeatureCls(GDB);
            id = result_sfcls.Create(name, GeomType.Reg, 0, 0, null);

            bool rtn;
            rtn = _RecordSet.MoveFirst();

            while (!_RecordSet.IsEOF)
            {
                _Geometry = _RecordSet.Geometry;//获取当前要素的空间信息
                GeometryType type = _Geometry.Type;//获取当前要素的几何约束类型

                if (_Geometry != null)
                {
                    switch (type)
                    {
                        case GeometryType.VarLine:
                            {
                                _GeoVarLine = new GeoVarLine();
                                _GeoVarLine = _Geometry as GeoVarLine;

                                bufferGeometry = null;
                                bufferGeometry = _GeoVarLine.Buffer(15, 15);
                                result_sfcls.Append(bufferGeometry, _RecordSet.Att, _RegInfo);

                                break;
                            }
                        case GeometryType.Lines:
                            {
                                _GeoLines = new GeoLines();
                                _GeoLines = _Geometry as GeoLines;
                                bufferGeometry = _GeoLines.Buffer(15, 15);
                                result_sfcls.Append(bufferGeometry, _RecordSet.Att, _RegInfo);

                                break;
                            }
                    }
                }
                rtn = _RecordSet.MoveNext();
            }

            //判断地图视图中是否有处于显示状态中的地图
            if (this.mapCtrl.ActiveMap == null)
            {
                MessageBox.Show("请先在地图视图中显示一幅地图！！！");
                return;
            }

            this._Tree.WorkSpace.BeginUpdateTree();
            //附加矢量图层
            VectorLayer vecLayer = new VectorLayer(VectorLayerType.SFclsLayer);
            //？？？？？？？？？？？？？
            //此处出现BUG System.AccessViolationException:“尝试读取或写入受保护的内存。这通常指示其他内存已损坏。”
            //？？？？？？？？？？？？？
            vecLayer.AttachData(result_sfcls);
            //将图层添加到地图中
            vecLayer.Name = result_sfcls.ClsName;
            //获取激活地图
            Map activeMap = new Map();
            activeMap = this.mapCtrl.ActiveMap;
            activeMap.Append(vecLayer);
            vecLayer.DetachData();
            //复位
            this.mapCtrl.ActiveMap = activeMap;
            this.mapCtrl.Restore();
            this._Tree.WorkSpace.EndUpdateTree();
        }
    }
}
