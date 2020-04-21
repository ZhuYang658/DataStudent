using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;

namespace SuperMarketIBLL.后台SuperMarketManager
{
    public interface ISuperMarketProductManager2
    {
        /// <summary>
        /// 获取数据库中商品分类表中的所有数据
        /// </summary>
        /// <returns></returns>
        List<ProductCategoryModel> GetCategories();
        /// <summary>
        /// 获取数据库中商品计量单位表中的所有数据
        /// </summary>
        /// <returns></returns>
        List<ProductUnitModel> GetUnit();
        /// <summary>
        /// 把添加商品的窗口中的数据给数据库中的商品表添加
        /// </summary>
        /// <param name="produt"></param>
        /// <param name="inventory"></param>
        /// <returns></returns>
        bool InsertProduct(ProdutsModel produt, ProductInventoryModel inventory);
        /// <summary>
        /// 商品的各种数据形成多表链接：商品Products表，商品分类表ProductCategory，商品库存表ProductInventory等多表链接
        /// </summary>
        /// <returns></returns>
        List<ProdutsModel> GetAllProduct();

        /// <summary>
        /// 从数据库中获取所有商品内容，并把商品Id进行排序
        /// </summary>
        /// <returns></returns>
        ProdutsModel GetProductWithId(string pid);
        /// <summary>
        /// 商品数量表的存储过程：修改商品数量表的数量
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        bool InventoryProduct(string pid, int count);


        //【4Day15】

        /// <summary>
        ///  【关键字查询】商品的各种数据形成多表链接：商品Products表，商品分类表ProductCategory，商品库存表ProductInventory等多表链接
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        List<ProdutsModel> GetProductsWithWhere(int categoryId, string where);
        /// <summary>
        /// 商品表：通过商品Id编号修改折扣Discount
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        bool SetProductDiscount(string pid, float discount);

        /// <summary>
        /// 商品表修改名字、单价、商品分类、计量单位，通过Id商品编号修改商品表的内容
        /// </summary>
        /// <param name="produts"></param>
        /// <returns></returns>
        bool UpdateProduct(ProdutsModel produts);



        //【4Day19自己写的】
        /// <summary>
        /// 商品库存
        ///  把｛｝里面的数据用一个List<ProductInventoryModel>接收
        /// </summary>
        /// <returns></returns>
        List<ProductInventoryModel> IPSQLku();
    }
}
