using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketIDAL.前台SuperMarketCashier;
using SuperMarketModel;

namespace SuperMarketDAL.前台SuperMarketCashier
{
    public class SuperMarketProductServer:ISuperMarketProductServer
    {

        /// <summary>
        ///  通过商品Id查找整个商品信息，返回值是ProdutsModel里面的所有数据
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProdutsModel ISuperMarketProductServer.GetProductWithId(string productId)
        {
            string procName = "GetProductWithId";
            //在ADO.NET里面创建参数，并给参数赋值，来给该存储过程名里传递参数
            SqlParameter[] sp =
            {
                new SqlParameter("@productId", SqlDbType.NVarChar,50)
            };
            // 获取通过主窗口传输进来的商品ID
            sp[0].Value = productId;
            //获取多个数据用GetDataReader方法
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            ProdutsModel produt = null;
            while (reader.Read())
            {
                //把数据库中获取到的所有数据并给ProdutsModel里面赋值
                produt = new ProdutsModel()
                {
                    ProductId = reader["ProductId"].ToString(),
                    ProductName = reader["ProductName"].ToString(),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                    Unit = reader["Unit"].ToString(),
                    Discount = Convert.ToInt32(reader["Discount"]),
                    CategoryId = Convert.ToInt32(reader["Categoryld"]),
                    CategoryName = reader["CategoryName"].ToString()
                };
            }
            reader.Close();
            return produt;
        }

        /// <summary>
        /// 【1】AddSalesList销售主表：以发票的形式记录消费的客户的小票数据，放在SalesList表里面记录客户的买的东西
        /// 【2】AddSalesListDetail商品销售统计表：就是顾客买的东西记录一下，放在AddSalesListDetail表里面记录收银员的给客户卖东西是的账本
        /// 【3】RefreshMemberPoint修改会员积分：如果有会员则修改会员积分
        /// </summary>
        /// <param name="sales">sales（包含了在主窗口中文本框中的所有数据，还获取了ListDetail里面添加的所有数据）获取所有的商品表并数据传递到数据库里</param>
        /// <param name="members"></param>
        /// <returns></returns>
        public bool SaveSalerInfo(SalesListModel1 sales, SMMembersModel10 members)
        {
            //【4月8日】
            List<string> procList = new List<string>();
            List<SqlParameter[]> psList = new List<SqlParameter[]>();
            //给消费主表中添加数据
            procList.Add("AddSalesList");
            //【1】
            SqlParameter[] saleSp = new SqlParameter[] {
                new SqlParameter("@SerialNum", SqlDbType.NVarChar,50),
                new SqlParameter("@TotalMoney", SqlDbType.Decimal),
                new SqlParameter("@RealReceive", SqlDbType.Decimal),
                new SqlParameter("@ReturnMoney", SqlDbType.Decimal),
                new SqlParameter("@SalesPersonId", SqlDbType.Int)
            };
            saleSp[0].Value = sales.SerialNum;
            saleSp[1].Value = sales.TotalMoney;
            saleSp[2].Value = sales.RealReceive;
            saleSp[3].Value = sales.ReturnMoney;
            saleSp[4].Value = sales.SalesPersonId;
            // 把参数对应的值放在psList里面
            psList.Add(saleSp);
            //【2】
            //通过SalesListModel1也可以知道SalesListDetailModel里面对应的值
            //给消费明细表中添加每次购物的详细数据
            foreach (SalesListDetailModel detail in sales.ListDetail)
            {
                procList.Add("AddSalesListDetail");
                SqlParameter[] detailList = new SqlParameter[] {
                  new SqlParameter("@SerialNum", SqlDbType.NVarChar,50),
                  new SqlParameter("@ProductId", SqlDbType.NVarChar,50),
                  new SqlParameter("@ProductName", SqlDbType.NVarChar,50),
                  new SqlParameter("@UnitPrice", SqlDbType.Decimal),
                  new SqlParameter("@Discount", SqlDbType.Float),
                  new SqlParameter("@Quantity", SqlDbType.Int),
                  new SqlParameter("@SubTotalMoney", SqlDbType.Decimal)
                };
                detailList[0].Value = detail.SerialNum;
                detailList[1].Value = detail.ProductId;
                detailList[2].Value = detail.ProductName;
                detailList[3].Value = detail.UnitPrice;
                detailList[4].Value = detail.Discount;
                detailList[5].Value = detail.Quantity;
                detailList[6].Value = detail.SubTotalMoney;
                psList.Add(detailList);

                //【4月9日】
                //更新库存数据:就是对商品数量的改变
                procList.Add("InventoryOut");
                SqlParameter[] inventorySp = new SqlParameter[]
                {
                    new SqlParameter("@ProductId", SqlDbType.NVarChar,50),
                    new SqlParameter("@TotalCount", SqlDbType.Int)
                };
                inventorySp[0].Value = detail.ProductId;
                inventorySp[1].Value = detail.Quantity;
                psList.Add(inventorySp);
            }

            //【4月8日】
            //【3】
            //更新会员的积分
            if (members != null)
            {
                procList.Add("RefreshMemberPoint");
                SqlParameter[] memberSp = new SqlParameter[]
                {
                new SqlParameter("@point", SqlDbType.Int),
                new SqlParameter("@memberId", SqlDbType.Int)
                };
                memberSp[0].Value = members.Points;
                memberSp[1].Value = members.MemberId;
                psList.Add(memberSp);
            }
            return SQLHelper.UpdateByTran(procList, psList);
        }
    }
}
