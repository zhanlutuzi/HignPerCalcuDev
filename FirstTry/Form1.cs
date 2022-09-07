using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapGIS.GeoMap;
using MapGIS.GISControl;
using MapGIS.UI.Controls;
namespace FirstTry
{
    public partial class Form1 : Form
    {
        MapControl mapCtrl = new MapControl();
        MapWorkSpaceTree mTree = new MapWorkSpaceTree();
        public Form1()
        {
            InitializeComponent();
            init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void init()
        {
            this.splitContainer1.Panel2.Controls.Add(mapCtrl);
            mapCtrl.Width = this.splitContainer1.Panel2.Width;
            mapCtrl.Height = this.splitContainer1.Panel2.Height;

            mTree.Dock = DockStyle.Fill;
            this.splitContainer1.Panel1.Controls.Add(mTree);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            Document doc = mTree.Document;
            if (doc.Close(false))
            {
                OpenFileDialog mapxDialog = new OpenFileDialog();
                mapxDialog.Filter = ".mapx(地图文档)|*.mapx|.map(地图文档)|*.map|.mbag(地图包)|*.mbag";
                if (mapxDialog.ShowDialog() != DialogResult.OK)
                {
                    Console.WriteLine("Error");
                }
                string MapURL = mapxDialog.FileName;
                doc.Open(MapURL);
            }

            Maps maps = doc.GetMaps();
            if (maps.Count > 0)
            {
                Map map = maps.GetMap(0);
                map.get_Layer(0).State = LayerState.Active;
                this.mapCtrl.ActiveMap = map;
                this.mapCtrl.Restore();
            }
            return;
        }
    }
}
