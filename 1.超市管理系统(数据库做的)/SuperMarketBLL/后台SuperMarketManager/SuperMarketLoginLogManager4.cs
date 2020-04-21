using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketDAL.后台SuperMarketManager;
using SuperMarketIDAL.后台SuperMarketManager;

namespace SuperMarketBLL.后台SuperMarketManager
{
    public class SuperMarketLoginLogManager4 : ISuperMarketLoginLogManager4
    {
        ISuperMarketLoginLogServer4 server4 = new SuperMarketLoginLogServer4();
        /// <summary>
        /// 数据库中记录收银人员和管理人员登录的【日志】的所有内容查询
        /// </summary>
        /// <returns></returns>
        public List<LoginLogsModel> GetLoginLog()
        {
            return server4.GetLoginLog();
        }
        /// <summary>
        /// 数据库中日志LoginLogs的模糊查询
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="dateTime"></param>
        /// <param name="wheres"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public List<LoginLogsModel> GetLoginLogBy(DateTime starttime, DateTime dateTime, string wheres, int check)
        {
            return server4.GetLoginLogBy(starttime, dateTime, wheres, check);
        }
    }
}
