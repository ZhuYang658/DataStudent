using System;
using System.Collections.Generic;
using System.Data.SqlClient;//SqlParameter
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketIDAL.后台SuperMarketManager;
using SuperMarketModel;

namespace SuperMarketDAL.后台SuperMarketManager
{
    public class SuperMarketAdminServer : ISuperMarketAdminServer
    {
        /// <summary>
        /// 获取管理者登录的信息
        /// </summary>
        /// <param name="admin">通过SysAdminsModel在窗口中拿到Id和pwd的数据：的可以查找数据库中 存储过程（SysAdminLogin存储过程名）中 管理者的具体数据</param>
        /// <returns></returns>
        public SysAdminsModel AdminLogin(SysAdminsModel admin)
        {
            string procName = "SysAdminLogin";
            //实例化参数SqlParameter，并给以参数赋获取的值
            SqlParameter[] sp = new SqlParameter[] {
                    new SqlParameter("@logId",admin.LoginId),
                    new SqlParameter("@LogPwd",admin.LoginPwd)
            };
            //向数据库中的方法获取值，这样又拿到数据库中的数据（把通过Id和pwd获取的正行数据）
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            //创建SysAdminsModel管理者的存储数据的类
            SysAdminsModel admins = null;
            //reader把所有管理者的数据拿到了
            while (reader.Read())
            {
                //把所有的数据库获取的数据放在SysAdminsModel类里面存储
                admins = new SysAdminsModel()
                {
                    AdminName = reader["AdminName"].ToString(),
                    LoginId = Convert.ToInt32(reader["LoginId"]),
                    LoginPwd = reader["LoginPwd"].ToString(),
                    Roleld = Convert.ToInt32(reader["Roleld"]),
                    AdminStatus = Convert.ToInt32(reader["AdminStatus"])
                };
            }
            reader.Close();
            return admins;
        }
        /// <summary>
        /// 更改管理者密码
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public int AdminUpdatePwd(SysAdminsModel admin)
        {
            //通过UpdateSysPwd存储过程名，把通过Id的管理者修改对应的密码
            string procName = "UpdateSysPwd";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@logId",admin.LoginId),
                new SqlParameter("@logPwd",admin.LoginPwd)
            };
            return SQLHelper.ExecuteNonQuery(procName, sp);
        }

        //【4Day15剩下的】

        /// <summary>
        /// 获取数据库中管理者的所有内容
        /// GetAllTables可以获取所有表的储存过程
        /// </summary>
        /// <returns></returns>
        public List<SysAdminsModel> GetAdmins()
        {
            //GetAllTables可以获取所有表的储存过程
            //获取这种表的存储过程
            string procName = "GetAllTables";
            SqlParameter[] sp =
            {
                //获取数据库中管理者的所有内容
                new SqlParameter("@tableName","SysAdmins")
            };
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            List<SysAdminsModel> list = new List<SysAdminsModel>();
            while (reader.Read())
            {
                SysAdminsModel admins = new SysAdminsModel();
                admins.AdminName = reader["AdminName"].ToString();
                admins.LoginId = Convert.ToInt32(reader["LoginId"]);
                admins.LoginPwd = reader["LoginPwd"].ToString();
                admins.Roleld = Convert.ToInt32(reader["Roleld"]);
                admins.AdminStatus = Convert.ToInt32(reader["AdminStatus"]);
                admins.StatusName = admins.AdminStatus == 1 ? "启用" : "禁用";
                admins.RoleName = admins.Roleld == 1 ? "超级管理员" : "一般管理员";
                list.Add(admins);
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 营业员表的显示（GetAllTables可以获取所有表的储存过程）
        /// </summary>
        /// <returns></returns>
        public List<SelesPersonSModel> GetSales()
        {
            //获取这种表的存储过程
            string procName = "GetAllTables";
            SqlParameter[] sp =
            {
                new SqlParameter("@tableName","SelesPerson")
            };
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            List<SelesPersonSModel> list = new List<SelesPersonSModel>();
            while (reader.Read())
            {
                SelesPersonSModel admins = new SelesPersonSModel();
                admins.SalesPersonId = Convert.ToInt32(reader["SalesPersonId"]);
                admins.LoginPwd = reader["LoginPwd"].ToString();
                admins.SPName = reader["SPName"].ToString();
                list.Add(admins);
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 添加管理者在数据库中显示
        /// </summary>
        /// <param name="admi"></param>
        public SysAdminsModel InsertAdmin(SysAdminsModel admi)
        {
            //添加管理者存储过程
            string procName = "InsertAdmin";
            SqlParameter[] sp =
            {
                new SqlParameter("@adminName",admi.AdminName),
                new SqlParameter("@loginPwd",admi.LoginPwd),
                new SqlParameter("@roleId",admi.Roleld)
            };
            object res = SQLHelper.ExecuteScalar(procName, sp);
            if (res != null)
            {
                admi.LoginId = Convert.ToInt32(res);
            }
            else
            {
                admi = null;
            }
            return admi;
        }
        /// <summary>
        /// 修改管理者
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public int UpdateAdmin(SysAdminsModel admin)
        {
            //修改管理者存储过程
            string procName = "UpdateAdmin";
            SqlParameter[] sp =
            {
                new SqlParameter("@adminName",admin.AdminName),
                new SqlParameter("@loginPwd",admin.LoginPwd),
                new SqlParameter("@roleId",admin.Roleld),
                new SqlParameter("@loginId",admin.LoginId)
            };
            return SQLHelper.ExecuteNonQuery(procName, sp);
        }
        /// <summary>
        /// 管理者的修改状态1启0禁
        /// 禁用启用
        /// 当前状态1启0禁的修改过程
        /// 改禁用系统用户管理
        /// 改启动系统用户管理
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public int SetSysStatus(SysAdminsModel admin)
        {
            string procName = "SetSysAdmStatus";
            SqlParameter[] sp =
            {
                new SqlParameter("@role",admin.AdminStatus),
                new SqlParameter("@id",admin.LoginId)
            };
            return SQLHelper.ExecuteNonQuery(procName, sp);
        }
        /// <summary>
        /// 添加营业员
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public SelesPersonSModel InsertSales(SelesPersonSModel person)
        {
            string procName = "InsertSales";
            SqlParameter[] sp ={
                new SqlParameter("@spName",person.SPName),
                new SqlParameter("@loginPwd",person.LoginPwd)
            };
            object res = SQLHelper.ExecuteScalar(procName, sp);
            if (res != null)
            {
                person.SalesPersonId = Convert.ToInt32(res);
            }
            else
            {
                person = null;
            }
            return person;
        }

        /// <summary>
        /// 修改营业员
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public int UpdateSales(SelesPersonSModel person)
        {
            string procName = "UpdateSaleInfor";
            SqlParameter[] sp =
            {
                new SqlParameter("@saleName",person.SPName),
                new SqlParameter("@loginPwd",person.LoginPwd),
                new SqlParameter("@loginId",person.SalesPersonId)
            };
            return SQLHelper.ExecuteNonQuery(procName, sp);
        }
    }
}
