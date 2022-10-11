using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MapGIS.GISControl;
using MapGIS.UI;
using MapGIS.GeoDataBase;
using MapGIS.UI.Controls;
using System.Threading;
using MapGIS.GeoMap;
using MapGIS.Analysis.SpatialAnalysis;

namespace EX8
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
            initialize();
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

            SFeatureCls sfcls = null;
            SFeatureCls overlay_sfcls1 = null;
            SFeatureCls overlay_sfcls2 = null;
            SFeatureCls overlay_sfcls3 = null;
            SFeatureCls overlay_sfcls4 = null;

            sfcls = new SFeatureCls(GDB);
            overlay_sfcls1 = new SFeatureCls(GDB);
            overlay_sfcls2 = new SFeatureCls(GDB);
            overlay_sfcls3 = new SFeatureCls(GDB);
            overlay_sfcls4 = new SFeatureCls(GDB);

            sfcls.Open("道路", 1);
            overlay_sfcls1.Open("矩形框1", 1);
            overlay_sfcls2.Open("矩形框2", 1);
            overlay_sfcls3.Open("矩形框3", 1);
            overlay_sfcls4.Open("矩形框4", 1);

            //创建结果简单要素类
            SFeatureCls ResultSFeatureCls1 = new SFeatureCls(GDB);
            int id = ResultSFeatureCls1.Create("thread1_overlay", sfcls.GeomType, 0, 0, null);
            //创建结果简单要素类
            SFeatureCls ResultSFeatureCls2 = new SFeatureCls(GDB);
            id = ResultSFeatureCls2.Create("thread2_overlay", sfcls.GeomType, 0, 0, null);
            //创建结果简单要素类
            SFeatureCls ResultSFeatureCls3 = new SFeatureCls(GDB);
            id = ResultSFeatureCls3.Create("thread3_overlay", sfcls.GeomType, 0, 0, null);
            //创建结果简单要素类
            SFeatureCls ResultSFeatureCls4 = new SFeatureCls(GDB);
            id = ResultSFeatureCls4.Create("thread4_overlay", sfcls.GeomType, 0, 0, null);

            Control.CheckForIllegalCrossThreadCalls = false;
            var threadOne = new Thread(() => Overlay(sfcls, overlay_sfcls1, ResultSFeatureCls1));
            threadOne.Name = "ThreadOne";
            var threadTwo = new Thread(() => Overlay(sfcls, overlay_sfcls2, ResultSFeatureCls2));
            threadTwo.Name = "ThreadTwo";
            var threadThr = new Thread(() => Overlay(sfcls, overlay_sfcls3, ResultSFeatureCls3));
            threadThr.Name = "ThreadThr";
            var threadFour = new Thread(() => Overlay(sfcls, overlay_sfcls4, ResultSFeatureCls4));
            threadFour.Name = "ThreadFour";

            threadOne.Start();
            threadTwo.Start();
            threadThr.Start();
            threadFour.Start();
        }

        private void initialize()
        { 
            //MapControl控件在Panel2里
            this.splitContainer1.Panel2.Controls.Add(mapCtrl);
            mapCtrl.Width = this.splitContainer1.Panel2.Width;
            mapCtrl.Height = this.splitContainer1.Panel2.Height;
            //工作空间树控件加载到Panel1上
            _Tree.Dock = DockStyle.Fill;
            this.splitContainer1.Panel1.Controls.Add(_Tree);
        }

        public void Overlay(SFeatureCls SFeatureCls, SFeatureCls OverLaySFeature, SFeatureCls ResultSFeatureCls)
        {
            //定义变量
            SpatialAnalysis SpaAnalysis = null;
            //设置叠加参数
            SPOverlayOption OverOption = null;

            //变量初始化
            SpaAnalysis = new SpatialAnalysis();
            OverOption = new SPOverlayOption();

            //设置叠加参数          
            OverOption.OverlayType = OverlayType.Ovly_Inter;
            //设置容差半径
            SpaAnalysis.Tolerance = 0.0001;

            //叠加分析
            bool rtn = SpaAnalysis.SP_OverLay(SFeatureCls, OverLaySFeature, ResultSFeatureCls, OverOption);


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
