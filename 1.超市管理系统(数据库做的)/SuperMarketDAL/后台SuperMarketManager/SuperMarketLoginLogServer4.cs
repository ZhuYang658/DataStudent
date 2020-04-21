using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;
using SuperMarketIDAL.后台SuperMarketManager;
using System.Data.SqlClient;

namespace SuperMarketDAL.后台SuperMarketManager
{
    public class SuperMarketLoginLogServer4 : ISuperMarketLoginLogServer4
    {
        /// <summary>
        /// 数据库中记录收银人员和管理人员登录的【日志】的所有内容查询
        /// </summary>
        /// <returns></returns>
        public List<LoginLogsModel> GetLoginLog()
        {
            //获取数据库中的人员的登录日志的存储过程
            string procName = "GetLoginLogs";
            SqlDataReader reader = SQLHelper.GetDataReader(procName, null);
            List<LoginLogsModel> list = new List<LoginLogsModel>();
            while (reader.Read())
            {
                LoginLogsModel logs = new LoginLogsModel();
                logs.LogId = Convert.ToInt32(reader["LogId"]);
                if (string.IsNullOrEmpty(reader["ExitTime"].ToString()))
                {
                    //加问号就不会报错
                    logs.ExitTime = null;
                }
                else
                {
                    logs.ExitTime = Convert.ToDateTime(reader["ExitTime"]);
                }
                logs.LoginId = Convert.ToInt32(reader["LoginId"]);
                logs.SPName = reader["SPName"].ToString();
                logs.ServerName = reader["ServerName"].ToString();
                logs.LoginTime = Convert.ToDateTime(reader["LoginTime"]);
                list.Add(logs);
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 数据库中日志LoginLogs的模糊查询
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="dateTime"></param>
        /// <param name="wheres"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public List<LoginLogsModel> GetLoginLogBy(DateTime starttime, DateTime endTime, string wheres, int check)
        {
            string procName = "GetLoginLogBy";
            SqlParameter[] sp =
            {
                new SqlParameter("@startTime",starttime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@where",wheres),
                new SqlParameter("@check",check)
            };
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            List<LoginLogsModel> list = new List<LoginLogsModel>();
            while (reader.Read())
            {
                LoginLogsModel logs = new LoginLogsModel();
                logs.LogId = Convert.ToInt32(reader["LogId"]);
                if (string.IsNullOrEmpty(reader["ExitTime"].ToString()))
                {
                    //加问号就不会报错
                    logs.ExitTime = null;
                }
                else
                {
                    logs.ExitTime = Convert.ToDateTime(reader["ExitTime"]);
                }
                logs.LoginId = Convert.ToInt32(reader["LoginId"]);
                logs.SPName = reader["SPName"].ToString();
                logs.ServerName = reader["ServerName"].ToString();
                logs.LoginTime = Convert.ToDateTime(reader["LoginTime"]);
                list.Add(logs);
            }
            reader.Close();
            return list;
        }
    }
}
