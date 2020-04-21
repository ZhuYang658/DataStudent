using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketIDAL.后台SuperMarketManager;
using SuperMarketDAL.后台SuperMarketManager;

namespace SuperMarketBLL.后台SuperMarketManager
{
    public class SuperMarketMemberManager3 : ISuperMarketMemberManager3
    {
        ISuperMarketMemberServer3 GetManager3 = new SuperMarketMemberServer3();
        /// <summary>
        /// 添加会员（AddMember存储过程名字），并返回一个会员编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SMMembersModel10 AddMember(SMMembersModel10 member)
        {
            return GetManager3.AddMember(member);
        }
        /// <summary>
        /// 通过会员的手机号或Id编号查询全部信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SMMembersModel10 GETMemberByIdOrPhone(string id)
        {
            return GetManager3.GETMemberByIdOrPhone(id);
        }
    }
}
