using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketIDAL.前台SuperMarketCashier;
using SuperMarketModel;

namespace SuperMarketDAL.前台SuperMarketCashier
{
    public class SuperMarketMemberServer3 : ISuperMarketMemberServer3
    {
        /// <summary>
        /// 通过ID获取会员；并接收会员的所有信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SMMembersModel10 GetMembersById(string id)
        {
            string procName = "GetMemberById";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@MemberId", System.Data.SqlDbType.Int),
                new SqlParameter("@PhoneNumber",System.Data.SqlDbType.NVarChar,50)
            };
            //获取手机Id（如果满足11位数字则进入获取手机号）
            if (id.Length == 11)
            {
                sp[0].Value = -1;
                sp[1].Value = id;
            }
            //获取会员编号（否则只获取会员编号）
            else
            {
                sp[0].Value = id;
                sp[1].Value = "";
            }
            //sp[0].Value = id;
            SMMembersModel10 members = null;
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            while (reader.Read())
            {
                members = new SMMembersModel10()
                {
                    MemberId = reader["MemberId"].ToString(),
                    MemberAddress = reader["MemberAddress"].ToString(),
                    MemberName = reader["MemberName"].ToString(),
                    MemberStatus = Convert.ToInt32(reader["MemberStatus"]),
                    OpenTime = Convert.ToDateTime(reader["OpenTime"]),
                    PhoneNumber = reader["PhoneNumber"].ToString(),
                    Points = Convert.ToInt32(reader["Points"])
                };
            }
            reader.Close();
            return members;
        }
    }
}
