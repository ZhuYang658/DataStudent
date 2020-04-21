using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIDAL.后台SuperMarketManager
{
    public interface ISuperMarketMemberServer3
    {
        //添加会员（AddMember存储过程名字），并返回一个会员编号
        SMMembersModel10 AddMember(SMMembersModel10 member);

        //通过会员的手机号或Id编号查询全部信息
        SMMembersModel10 GETMemberByIdOrPhone(string id);

    }
}
