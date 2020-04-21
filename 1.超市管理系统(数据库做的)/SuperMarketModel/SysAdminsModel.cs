using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketModel
{
    /// <summary>
    /// 管理者的登录
    /// </summary>
    public class SysAdminsModel
    {
        public int LoginId { get; set; }
        public string LoginPwd { get; set; }
        public string AdminName { get; set; }
        public int AdminStatus { get; set; }//当前状态1启0禁
        public int Roleld { get; set; }

        //扩展属性
        public string StatusName { get; set; }//运行的状态内容是什么
        public string RoleName { get; set; }//管理者的身份

        //管理员登录和退出时间也需要记录
        public int LoginLogId { get; set; }
    }
}
