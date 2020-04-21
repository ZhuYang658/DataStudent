using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketModel
{
    /// <summary>
    /// 商品表
    /// </summary>
    [Serializable]
    public class ProdutsModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }//单价
        public double Discount { get; set; }//打折
        public int CategoryId { get; set; }

        //商品维护页面时新加的属性
        public int TotalCount { get; set; }

        //扩展属性（商品表的扩展内容属性）
        public int ProductNo { get; set; }
        public int Quantity { get; set; }//商品数量
        public decimal SubTotal { get; set; }//总钱数
        public string CategoryName { get; set; }
    }
}
