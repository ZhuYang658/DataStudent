using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;//获取实体类中的

namespace SuperMarketIDAL.前台SuperMarketCashier
{
    public interface ISelesPersonServer
    {
        //收银员登录信息
        SelesPersonSModel SelesLogin(SelesPersonSModel seles);

        //记录到日志信息
        int WriteSelesLog(LoginLogsModel loginLogs);

        //退出日志：受影响行数
        //通过Id获取收银员退出超市管理系统（这个结果通过日志进入时拿到当前的返回Id）
        int WriteSelesExitLog(int loginID);

        //获取数据库的系统时间
        DateTime GetSysTime();

        /*//通过商品Id查找整个商品信息，返回值是ProdutsModel里面的所有数据
        ProdutsModel GetProductWithId(string productId);*/
    }
}
