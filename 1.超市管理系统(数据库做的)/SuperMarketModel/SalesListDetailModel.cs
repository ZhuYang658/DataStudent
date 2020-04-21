using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketModel
{
    /// <summary>
    /// 消费明细（就是在数据库中记录一下本次消费的详细商品）
    /// </summary>
    [Serializable]
    public class SalesListDetailModel
    {
        public int Id { get; set; }
        public string SerialNum { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public double Discount { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotalMoney { get; set; }
    }
}
