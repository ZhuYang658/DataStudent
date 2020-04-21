using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIBLL.前台SuperMarketCashier
{
    public interface ISuperMarketProductManager
    {
        //通过商品Id查找整个商品信息，返回值是ProdutsModel里面的所有数据
        ProdutsModel GetProductWithId(string productId);

        /// <summary>
        /// 【1】AddSalesList销售主表：以发票的形式记录消费的客户的小票数据，放在SalesList表里面记录客户的买的东西
        /// 【2】AddSalesListDetail商品销售统计表：就是顾客买的东西记录一下，放在AddSalesListDetail表里面记录收银员的给客户卖东西是的账本
        /// 【3】RefreshMemberPoint修改会员积分：如果有会员则修改会员积分
        /// </summary>
        /// <param name="sales"></param>
        /// <param name="members"></param>
        /// <returns></returns>

        //启动事务处理购买商品对象更新会员积分
        bool SaveSalerInfo(SalesListModel1 sales, SMMembersModel10 members);
    }
}
