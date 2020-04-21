using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketIBLL.前台SuperMarketCashier;
using SuperMarketIDAL.前台SuperMarketCashier;
using SuperMarketDAL.前台SuperMarketCashier;
using SuperMarketModel;

namespace SuperMarketBLL.前台SuperMarketCashier
{
    public class SuperMarketProductManager : ISuperMarketProductManager
    {
        ISuperMarketProductServer server = new SuperMarketProductServer();
        /// <summary>
        /// 通过商品Id查找整个商品信息，返回值是ProdutsModel里面的所有数据
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProdutsModel GetProductWithId(string productId)
        {
            return server.GetProductWithId(productId);
        }

        /// <summary>
        /// SaveSalerInfo：
        /// 【1】AddSalesList销售主表：以发票的形式记录消费的客户的小票数据，放在SalesList表里面记录客户的买的东西
        /// 【2】AddSalesListDetail商品销售统计表：就是顾客买的东西记录一下，放在AddSalesListDetail表里面记录收银员的给客户卖东西是的账本
        /// 【3】RefreshMemberPoint修改会员积分：如果有会员则修改会员积分
        /// </summary>
        /// <param name="sales"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public bool SaveSalerInfo(SalesListModel1 sales, SMMembersModel10 members)
        {
            return server.SaveSalerInfo(sales, members);
        }
    }
}
