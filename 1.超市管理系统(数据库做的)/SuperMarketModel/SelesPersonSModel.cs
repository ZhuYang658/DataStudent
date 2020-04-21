using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketModel
{
    /// <summary>
    /// 登录销售员
    /// </summary>
    [Serializable]//表示类可以被序列化，此类不能被继承
    public class SelesPersonSModel
    {
        public int SalesPersonId { get; set; }
        public string SPName { get; set; }
        public string LoginPwd { get; set; }
        //当日志离线时使用返回记录当前的Id值
        public int LogId { get; set; }
    }
}
