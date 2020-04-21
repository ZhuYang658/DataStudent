using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIBLL.前台SuperMarketCashier
{
    public interface ISelesPersonManager
    {
        //登录人员
        SelesPersonSModel SelesLogin(SelesPersonSModel seles);

        //记录到日志信息
        int BllWriteSelesLog(LoginLogsModel loginLogs);
        //受影响行数
        bool BllWriteSelesExitLog(int id);
        //获取数据库系统时间
        DateTime GetSysTime();
        /*//通过商品Id查找整个商品信息，返回值是ProdutsModel里面的所有数据
        ProdutsModel GetProductWithId(string productId);*/
    }
}
