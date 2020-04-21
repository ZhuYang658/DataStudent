using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketIDAL;
using SuperMarketModel;
using System.Data.SqlClient;
using System.Data;

namespace SuperMarketDAL.前台SuperMarketCashier
{
    public class SelesPersonServer : SuperMarketIDAL.前台SuperMarketCashier.ISelesPersonServer
    {
        //登录收银员的方法
        public SelesPersonSModel SelesLogin(SelesPersonSModel seles)
        {
            string str = "SelesLogeIn";
            SqlParameter[] sp = 
            {
                new SqlParameter("@LoginId", SqlDbType.Int),
                new SqlParameter("@LoginPwd", SqlDbType.NVarChar,50)
            };
            sp[0].Value = seles.SalesPersonId;
            sp[1].Value = seles.LoginPwd;
            SqlDataReader reader = SQLHelper.GetDataReader(str, sp);
            SelesPersonSModel model = null;
            while (reader.Read())//reader拿到数据库里面的值
            {
                model = new SelesPersonSModel()
                {
                    SalesPersonId = Convert.ToInt32(reader["SalesPersonId"]),//reader里面对应的值是数据库里面的值
                    LoginPwd = reader["LoginPwd"].ToString(),
                    SPName = reader["SPName"].ToString()
                };
            }
            return model;
        }

        /// <summary>
        /// 日志记录成功返回返录当前的Id值
        /// 用@@IDENTITY获取到当前的Id
        /// </summary>
        /// <param name="loginLogs"></param>
        /// <returns></returns>
        public int WriteSelesLog(LoginLogsModel loginLogs)
        {
            string str = "WriteLog";
            SqlParameter[] sp =
           {
                new SqlParameter("@LoginId", SqlDbType.Int),
                new SqlParameter("@SPName", SqlDbType.NVarChar,50),
                new SqlParameter("@ServerName",SqlDbType.NVarChar,100)
            };
            sp[0].Value = loginLogs.LoginId;
            sp[1].Value = loginLogs.SPName;
            sp[2].Value = loginLogs.ServerName;
            //int a=SQLHelper.ExecuteNonQuery(str,sp);
            object a = SQLHelper.ExecuteScalar(str, sp);//拿单一结果Id值（当前结果）
            if (a==null)
            {
                return -1;
            }
            return int.Parse(a.ToString());
        }

        //通过Id获取收银员退出超市管理系统（这个结果通过日志进入时拿到当前的返回Id）
        public int WriteSelesExitLog(int loginID)
        {
            string str = "ExitSysWriteLog";//通过Id修改ExitSysWriteLog存储过程里面的内容
            SqlParameter[] sp =
           {
                new SqlParameter("@LogId", SqlDbType.Int)
            };
            sp[0].Value = loginID;
            int a=SQLHelper.ExecuteNonQuery(str,sp);
            return a;
        }
        //获取数据库的系统时间
        public DateTime GetSysTime()
        {
            string str = "GetSysTime";
            //只查询，不需要参数，所以这里参数设为null
            return Convert.ToDateTime(SQLHelper.ExecuteScalar(str, null));
        }
        /*/// <summary>
        ///  通过商品Id查找整个商品信息，返回值是ProdutsModel里面的所有数据
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProdutsModel GetProductWithId(string productId)
        {
            string procName = "GetProductWithId";
           //在ADO.NET里面创建参数，并给参数赋值，来给该存储过程名里传递参数
            SqlParameter[] sp =
            {
                new SqlParameter("@productId", SqlDbType.NVarChar,50)
            };
            // 获取通过主窗口传输进来的商品ID
            sp[0].Value = productId;
            //获取多个数据用GetDataReader方法
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            ProdutsModel produt = null;
            while (reader.Read())
            {
                //把数据库中获取到的所有数据并给ProdutsModel里面赋值
                produt = new ProdutsModel()
                {
                    ProductId = reader["ProductId"].ToString(),
                    ProductName = reader["ProductName"].ToString(),
                    UnitPrice = Convert.ToDouble(reader["UnitPrice"]),
                    Unit = reader["Unit"].ToString(),
                    Discount = Convert.ToInt32(reader["Discount"]),
                    CategoryId = Convert.ToInt32(reader["Categoryld"]),
                    CategoryName = reader["CategoryName"].ToString()
                };
            }
            reader.Close();
            return produt;
        }*/
    }
}
