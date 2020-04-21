using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIDAL.前台SuperMarketCashier
{
    public interface ISuperMarketMemberServer3
    {
        //通过ID获取会员；并接收会员的所有信息
        SMMembersModel10 GetMembersById(string id);
    }
}
