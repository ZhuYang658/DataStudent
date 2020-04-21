using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;
using SuperMarketIBLL;
using SuperMarketIDAL;
using SuperMarketDAL;


namespace SuperMarketBLL.前台SuperMarketCashier
{
    public class SelesPersonManager : SuperMarketIBLL.前台SuperMarketCashier.ISelesPersonManager
    {
        SuperMarketIDAL.前台SuperMarketCashier.ISelesPersonServer server = new SuperMarketDAL.前台SuperMarketCashier.SelesPersonServer();//SuperMarketDAL这个的方法DAL

        /// <summary>
        /// 收银员登录信息
        /// </summary>
        /// <param name="seles"></param>
        /// <returns></returns>
        public SelesPersonSModel SelesLogin(SelesPersonSModel seles)
        {
            return server.SelesLogin(seles);
        }

        //日志：记录到日志信息
        public int BllWriteSelesLog(LoginLogsModel loginLogs)
        {
            return server.WriteSelesLog(loginLogs);
        }
        /// <summary>
        /// 退出日志
        /// 通过Id获取收银员退出超市管理系统（这个结果通过日志进入时拿到当前的返回Id）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool BllWriteSelesExitLog(int id)
        {
            if (server.WriteSelesExitLog(id) >0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //获取数据库系统时间
        public DateTime GetSysTime()
        {
            return server.GetSysTime();
        }
/*
        public ProdutsModel GetProductWithId(string productId)
        {
            return server.GetProductWithId(productId);
        }*/
    }
}
