using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketModel
{
    [Serializable]
    //【三.1】
    //商品分类
    public class ProductCategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsClock { get; set; }
    }
}
