using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketIBLL.前台SuperMarketCashier;
using SuperMarketModel;
using SuperMarketIDAL.前台SuperMarketCashier;
using SuperMarketDAL.前台SuperMarketCashier;

namespace SuperMarketBLL.前台SuperMarketCashier
{
    public class SuperMarketMemberManager3 : ISuperMarketMemberManager3
    {
        ISuperMarketMemberServer3 server3 = new SuperMarketMemberServer3();
        //通过ID获取会员；并接收会员的所有信息
        public SMMembersModel10 GetMembersById(string id)
        {
            return server3.GetMembersById(id);
        }
    }
}
