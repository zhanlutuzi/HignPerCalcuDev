using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapGIS.GeoDataBase;
using MapGIS.GeoMap;
using MapGIS.Analysis.SpatialAnalysis;
using MapGIS.GISControl;
using MapGIS.UI.Controls;
using System.Threading;

namespace EX7
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
            //新建一个文档树
            _Tree.WorkSpace.BeginUpdateTree();
            _Tree.Document.Title = "地图文档";
            _Tree.Document.New();

            //在地图文档下添加一个地图
            Map map = new Map();
            map.Name = "新地图";

            _Tree.Document.GetMaps().Append(map);
            //将该地图设置为MapConrol的激活地图
            this.mapCtrl.ActiveMap = map;
            this.mapCtrl.Restore();

            //展开所有的节点
            _Tree.ExpandAll();
            _Tree.WorkSpace.EndUpdateTree();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //打开数据源
            Svr = new Server();
            Svr.Connect("MapGisLocal", "", "");
            GDB = new DataBase();
            GDB = Svr.OpenGDB("空间信息高性能计算实验");

            SFeatureCls cliped_sfcls = null;
            SFeatureCls clip_sfcls1 = null;
            SFeatureCls clip_sfcls2 = null;
            cliped_sfcls = new SFeatureCls(GDB);
            clip_sfcls1 = new SFeatureCls(GDB);
            clip_sfcls2 = new SFeatureCls(GDB);

            cliped_sfcls.Open("道路", 1);
            clip_sfcls1.Open("矩形框1", 1);
            clip_sfcls2.Open("矩形框2", 1);

            //创建结果简单要素类
            SFeatureCls ResultSFeatureCls1 = new SFeatureCls(GDB);
            int id = ResultSFeatureCls1.Create("thread1_clip", cliped_sfcls.GeomType, 0, 0, null);
            //创建结果简单要素类
            SFeatureCls ResultSFeatureCls2 = new SFeatureCls(GDB);
            id = ResultSFeatureCls2.Create("thread2_clip", cliped_sfcls.GeomType, 0, 0, null);

            Control.CheckForIllegalCrossThreadCalls = false;
            var threadOne = new Thread(() => Clip(cliped_sfcls, clip_sfcls1, ResultSFeatureCls1));
            threadOne.Name = "ThreadOne";
            var threadTwo = new Thread(() => Clip(cliped_sfcls, clip_sfcls2, ResultSFeatureCls2));
            threadTwo.Name = "ThreadTwo";

            threadOne.Start();
            threadTwo.Start();
        }

        public void Clip(SFeatureCls cliped_sfcls, SFeatureCls clip_sfcls, SFeatureCls ResultSFeatureCls)
        {
            //空间分析类 
            SpatialAnalysis SpaAnalysis = null;
            SpaAnalysis = new SpatialAnalysis();

            bool rtn = false;
            //设置容差半径
            SpaAnalysis.Tolerance = 0.0001;

            SPOverlayOption Option = new SPOverlayOption();
            Option.OverlayType = OverlayType.Ovly_InClip;
            rtn = SpaAnalysis.SP_Clip(cliped_sfcls, clip_sfcls, ResultSFeatureCls, Option);

            //判断地图视图中是否有处于显示状态中的地图
            if (this.mapCtrl.ActiveMap == null)
            {
                MessageBox.Show("请先在地图视图中显示一幅地图！！！");
                return;
            }

            this._Tree.WorkSpace.BeginUpdateTree();
            //附加矢量图层
            VectorLayer vecLayer = new VectorLayer(VectorLayerType.SFclsLayer);
            vecLayer.AttachData(ResultSFeatureCls);
            //将图层添加到地图中
            vecLayer.Name = ResultSFeatureCls.ClsName;
            //获取激活地图
            Map activeMap = this.mapCtrl.ActiveMap;
            activeMap.Append(vecLayer);
            vecLayer.DetachData();
            //复位
            this.mapCtrl.ActiveMap = activeMap;
            this.mapCtrl.Restore();
            this._Tree.WorkSpace.EndUpdateTree();


        }
    }
}
