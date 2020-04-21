using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;
using SuperMarketIDAL.后台SuperMarketManager;
using SuperMarketDAL.后台SuperMarketManager;
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketIBLL.前台SuperMarketCashier;
using SuperMarketBLL.前台SuperMarketCashier;
using SuperMarketIDAL.前台SuperMarketCashier;
using SuperMarketDAL.前台SuperMarketCashier;
using System.Net;//Dns

namespace SuperMarketBLL.后台SuperMarketManager
{
    public class SuperMarketAdminManager : ISuperMarketAdminManager
    {
        //获取IDAL里面的方法
        ISuperMarketAdminServer GetServer = new SuperMarketAdminServer();
        //获取数据库日志的类里面
        ISelesPersonManager manager = new SelesPersonManager();
        /// <summary>
        /// 获取管理者登录的所有信息
        /// </summary>
        /// <param name="admin">通过SysAdminsModel在窗口中拿到Id和pwd的数据：的可以查找数据库中 存储过程（SysAdminLogin存储过程名）中 管理者的具体数据</param>
        /// <returns></returns>
        public SysAdminsModel AdminLogin(SysAdminsModel admin)
        {
            //【1】根据用户账号和密码调用查询用户登录
            SysAdminsModel sys =  GetServer.AdminLogin(admin);
            //管理员登录然后状态是启用，可以登录
            if (sys != null && sys.AdminStatus == 1)
            {
                //【2】写入登录日志
                //管理者的登录退出也要写入到数据库的日志里面
                LoginLogsModel log = new LoginLogsModel()
                {
                    LoginId = sys.LoginId,
                    SPName = sys.AdminName,
                    //获取本地计算机名字
                    ServerName = Dns.GetHostName()
                };
                //保存当前管理员登录日志的ID
                //通过Id获取收银员退出超市管理系统（这个结果通过日志进入时拿到当前的返回Id）
                sys.LoginLogId = manager.BllWriteSelesLog(log);
            }
            else
            {
                sys = null;
            }
            return sys;
        }
        /// <summary>
        /// 更改管理者密码
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool AdminUpdatePwd(SysAdminsModel admin)
        {
            int res = GetServer.AdminUpdatePwd(admin);
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        //【4Day15】
        /// <summary>
        /// 获取数据库中管理者的所有内容
        /// </summary>
        /// <returns></returns>
        public List<SysAdminsModel> GetAdmins()
        {
            //获取数据库中管理者的所有内容
            return GetServer.GetAdmins();
        }
        /// <summary>
        /// 营业员表的显示（GetAllTables可以获取所有表的储存过程）
        /// </summary>
        /// <returns></returns>
        public List<SelesPersonSModel> GetSales()
        {
            return GetServer.GetSales();
        }
        /// <summary>
        /// 添加管理者内容
        /// </summary>
        /// <param name="admi"></param>
        /// <returns></returns>
        public SysAdminsModel InsertAdmin(SysAdminsModel admi)
        {
            return GetServer.InsertAdmin(admi);
        }
        /// <summary>
        /// 修改管理者
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public SysAdminsModel UpdateAdmin(SysAdminsModel admin)
        {
            if (GetServer.UpdateAdmin(admin) > 0)
            {
                return admin;
            }
            else
            {
                return null;
            }
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
        public bool SetSysStatus(SysAdminsModel admin)
        {
            if (GetServer.SetSysStatus(admin) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 添加营业员
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public SelesPersonSModel InsertSales(SelesPersonSModel person)
        {
            return GetServer.InsertSales(person);
        }
        /// <summary>
        /// 修改营业员
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public SelesPersonSModel UpdateSales(SelesPersonSModel person)
        {
            if(GetServer.UpdateSales(person) > 0)
            {
                return person;
            }
            else
            {
                return null;
            }
        }
    }
}
