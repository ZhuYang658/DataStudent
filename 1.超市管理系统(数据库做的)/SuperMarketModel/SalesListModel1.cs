using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketModel
{
    /// <summary>
    /// 本次消费总计（给购物者的小票）
    /// </summary>
    [Serializable]
    public class SalesListModel1
    {
        public SalesListModel1()
        {
            ListDetail = new List<SalesListDetailModel>();
        }
        public string SerialNum { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal RealReceive { get; set; }
        public decimal ReturnMoney { get; set; }
        public int SalesPersonId { get; set; }
        public DateTime SaleDate { get; set; }
        //本次消费总计里面消费明系表
        //两个对象属于1对多的关系
        /// <summary>
        /// 【解释：通过拿SalesListModel1里面的值，可以给SalesListDetailModel的list里面赋值，一次性可以拿多表，在DAL里面方便实现多表的实现】
        /// </summary>
        public List<SalesListDetailModel> ListDetail { get; set; }
    }
}
