using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIBLL.前台SuperMarketCashier
{
    public interface ISuperMarketMemberManager3
    {
        //通过ID获取会员；并接收会员的所有信息
        SMMembersModel10 GetMembersById(string id);
    }
}
