using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIDAL.后台SuperMarketManager
{
    public interface ISuperMarketLoginLogServer4
    {
        /// <summary>
        /// 数据库中记录收银人员和管理人员登录的【日志】的所有内容查询
        /// </summary>
        /// <returns></returns>
        List<LoginLogsModel> GetLoginLog();
        /// <summary>
        /// 数据库中日志LoginLogs的模糊查询
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="dateTime"></param>
        /// <param name="wheres"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        List<LoginLogsModel> GetLoginLogBy(DateTime starttime, DateTime dateTime, string wheres, int check);
    }
}
