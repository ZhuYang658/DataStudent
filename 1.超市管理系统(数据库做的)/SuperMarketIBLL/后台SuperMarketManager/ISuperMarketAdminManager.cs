using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIBLL.后台SuperMarketManager
{
    //interface接口单词
    public interface ISuperMarketAdminManager
    {
        /// <summary>
        /// 获取管理者登录的信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        SysAdminsModel AdminLogin(SysAdminsModel admin);
        /// <summary>
        /// 更改管理者密码
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        bool AdminUpdatePwd(SysAdminsModel admin);


        //【4Day15剩下的】

        /// <summary>
        /// 获取数据库中管理者的所有内容
        /// </summary>
        /// <returns></returns>
        List<SysAdminsModel> GetAdmins();
        /// <summary>
        /// 营业员表的显示（GetAllTables可以获取所有表的储存过程）
        /// </summary>
        /// <returns></returns>
        List<SelesPersonSModel> GetSales();

        /// <summary>
        /// 添加管理者内容
        /// </summary>
        /// <param name="admi"></param>
        /// <returns></returns>
        SysAdminsModel InsertAdmin(SysAdminsModel admi);
        /// <summary>
        /// 修改管理者
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        SysAdminsModel UpdateAdmin(SysAdminsModel admin);
        /// <summary>
        /// 管理者的修改状态1启0禁
        /// 禁用启用
        /// 当前状态1启0禁的修改过程
        /// 改禁用系统用户管理
        /// 改启动系统用户管理
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        bool SetSysStatus(SysAdminsModel admin);
        /// <summary>
        /// 添加营业员
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        SelesPersonSModel InsertSales(SelesPersonSModel person);
        /// <summary>
        /// 修改营业员
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        SelesPersonSModel UpdateSales(SelesPersonSModel person);
    }
}
