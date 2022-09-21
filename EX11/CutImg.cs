using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapGIS.GeoDataBase;
using MapGIS.GeoDataBase.GeoRaster;
using MapGIS.RasAnalysis;

namespace EX11
{
    class CutImg
    {
        public void cut(string filename,string cutFeaturename)
        {
            RasterDataSet cRasDataSet = null;//栅格数据集
            RasterBand band = null;//波段
            int[] bandlist = { 1, 2, 3 };//波段列表
            DataBase database = null;//数据库
            Server srv = new Server();//服务
            RasImgSubset imgsubset = new RasImgSubset(); //影像子集类
            bool rtn = false;//判断栅格数据集是否打开

            srv.Connect("MapGISLocalPlus", "", "");//连接数据源
            database = srv.OpenGDB("sample");//打开数据库
            SFeatureCls VectorCls = new SFeatureCls(database);//获取简单要素类
            bool flg = VectorCls.Open(cutFeaturename, 0);//打开简单要素类
            cRasDataSet = new RasterDataSet(database);//获取栅格数据集

            rtn = cRasDataSet.Open("地大新校区卫星图", RasAccessType.RasAccessType_Update);//打开栅格数据集

            if (rtn)
            {
                string url = @"D:\Desktop\code\Learning\HignPerCalcuDev\HignPerCalcuDev\EX11\result\"+filename;//裁剪影像输出路径
                imgsubset.SetData(cRasDataSet, bandlist);//设置裁剪数据源和波段列表
                imgsubset.SetClipType(0);//设置裁剪类型
                imgsubset.SetDstNoDataValue(255);//设置输出数据无效值
                int flg1 = imgsubset.RsClipImageBySFCls(VectorCls, 0, 1, url);//进行裁剪
            }



            VectorCls.Close();//关闭简单要素类
            cRasDataSet.Close();//关闭栅格数据集
            database.Close();//关闭数据库
            srv.DisConnect();//断开连接
        }
    }
}
