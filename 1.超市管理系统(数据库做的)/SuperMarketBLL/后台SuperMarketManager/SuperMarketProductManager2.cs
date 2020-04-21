using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketIDAL.后台SuperMarketManager;
using SuperMarketDAL.后台SuperMarketManager;
using SuperMarketModel;

namespace SuperMarketBLL.后台SuperMarketManager
{
    public class SuperMarketProductManager2 : ISuperMarketProductManager2
    {
        //实例化SuperMarketProductManager2的IDAL
        ISuperMarketProductServer2 GetManager2 = new SuperMarketProductServer2();

        /// <summary>
        /// 获取数据库中商品分类表中的所有数据
        /// </summary>
        /// <returns></returns>
        public List<ProductCategoryModel> GetCategories()
        {
            return GetManager2.GetCategories();
        }
        /// <summary>
        /// 获取数据库中商品计量单位表中的所有数据
        /// </summary>
        /// <returns></returns>
        public List<ProductUnitModel> GetUnit()
        {
            return GetManager2.GetUnit();
        }
        /// <summary>
        /// 把添加商品的窗口中的数据给数据库中的商品表添加
        /// </summary>
        /// <param name="produt"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        public bool InsertProduct(ProdutsModel produt, ProductInventoryModel inventory)
        {
            return GetManager2.InsertProduct(produt, inventory);
        }
        /// <summary>
        /// 商品的各种数据形成多表链接：商品Products表，商品分类表ProductCategory，商品库存表ProductInventory等多表链接
        /// </summary>
        /// <returns></returns>
        public List<ProdutsModel> GetAllProduct()
        {
            return GetManager2.GetAllProduct();
        }
        /// <summary>
        /// 从数据库中获取所有商品内容，并把商品Id进行排序
        /// </summary>
        /// <returns></returns>
        public ProdutsModel GetProductWithId(string pid)
        {
            return GetManager2.GetProductWithId(pid);
        }
        public bool InventoryProduct(string pid, int count)
        {
            if (GetManager2.InventoryProduct(pid, count)>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 【4Day15】

        /// <summary>
        /// 【关键字查询】商品的各种数据形成多表链接：商品Products表，商品分类表ProductCategory，商品库存表ProductInventory等多表链接
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ProdutsModel> GetProductsWithWhere(int categoryId, string where)
        {
            return GetManager2.GetProductsWithWhere(categoryId, where);
        }
        /// <summary>
        /// 商品表：通过商品Id编号修改折扣Discount
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        public bool SetProductDiscount(string pid, float discount)
        {
            if (GetManager2.SetProductDiscount(pid, discount) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 商品表修改名字、单价、商品分类、计量单位，通过Id商品编号修改商品表的内容
        /// </summary>
        /// <param name="produts"></param>
        /// <returns></returns>
        public bool UpdateProduct(ProdutsModel produts)
        {
            if (GetManager2.UpdateProduct(produts) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //【4Day19自己写的】
        /// <summary>
        /// 商品库存
        ///  把｛｝里面的数据用一个List<ProductInventoryModel>接收
        /// </summary>
        /// <returns></returns>
        public List<ProductInventoryModel> IPSQLku()
        {
            return GetManager2.IPSQLku();
        }
    }
}
