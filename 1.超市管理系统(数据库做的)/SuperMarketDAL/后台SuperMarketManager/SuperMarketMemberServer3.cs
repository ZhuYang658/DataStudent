using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;
using SuperMarketIDAL.后台SuperMarketManager;
using System.Data.SqlClient;

namespace SuperMarketDAL.后台SuperMarketManager
{
    public class SuperMarketMemberServer3 : ISuperMarketMemberServer3
    {
        /// <summary>
        /// 添加会员（AddMember存储过程名字），并返回一个会员编号
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SMMembersModel10 AddMember(SMMembersModel10 member)
        {
            //添加会员的存储过程
            string procName = "AddMember";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@memberName",member.MemberName ),
                new SqlParameter("@phoneNumber ",member.PhoneNumber ),
                new SqlParameter("@memberAddress ",member.MemberAddress )
            };

            object obj = SQLHelper.ExecuteScalar(procName, sp);
            if (obj != null)
            {
                member.MemberId = obj.ToString();
            }
            else
            {
                member = null;
            }
            return member;
        }
        /// <summary>
        /// 通过会员的手机号或Id编号查询全部信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SMMembersModel10 GETMemberByIdOrPhone(string id)
        {
            //通过之前在前台写的手机号获取或编号获取的方法在这里调用
            前台SuperMarketCashier.SuperMarketMemberServer3 server = new 前台SuperMarketCashier.SuperMarketMemberServer3();
            return server.GetMembersById(id);
        }
    }
}
