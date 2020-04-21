using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarketModel;
using SuperMarketDAL.后台SuperMarketManager;
using SuperMarketIDAL.后台SuperMarketManager;
using System.Data.SqlClient;

namespace SuperMarketDAL.后台SuperMarketManager
{
    public class SuperMarketProductServer2 : ISuperMarketProductServer2
    {
        /// <summary>
        /// 获取数据库中商品分类表中的所有数据
        /// </summary>
        /// <returns></returns>
        public List<ProductCategoryModel> GetCategories()
        {
            string procName = "GetProductCategory";
            List<ProductCategoryModel> list = new List<ProductCategoryModel>();
            SqlDataReader reader = SQLHelper.GetDataReader(procName, null);
            while (reader.Read())
            {
                list.Add(new ProductCategoryModel()
                {
                    CategoryId = Convert.ToInt32(reader["CategoryId"]),
                    CategoryName = reader["CategoryName"].ToString()
                });
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 获取数据库中商品计量单位表中的所有数据
        /// </summary>
        /// <returns></returns>
        public List<ProductUnitModel> GetUnit()
        {
            string procName = "GetProductUnit";
            List<ProductUnitModel> list = new List<ProductUnitModel>();
            SqlDataReader reader = SQLHelper.GetDataReader(procName, null);
            while (reader.Read())
            {
                list.Add(new ProductUnitModel()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Unit = reader["Unit"].ToString()
                });
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 把添加商品的窗口中的数据给数据库中的商品表添加
        /// 添加商品数量的存储过程
        /// </summary>
        /// <param name="produt">添加商品的存储过程</param>
        /// <param name="inventory">添加商品数量的存储过程</param>
        /// <returns></returns>
        public bool InsertProduct(ProdutsModel produt, ProductInventoryModel inventory)
        {
            List<string> procList = new List<string>()
            {
                "InsertProduct",//添加商品的存储过程
                "InsertInventory"//添加商品数量的存储过程
            };

            List<SqlParameter[]> psList = new List<SqlParameter[]>();
            SqlParameter[] prodPs = new SqlParameter[]
            {
                new SqlParameter("@productId",produt.ProductId),
                new SqlParameter("@productName",produt.ProductName),
                new SqlParameter("@unitPrice",produt.UnitPrice),
                new SqlParameter("@unit",produt.Unit),
                new SqlParameter("@discount",produt.Discount),
                new SqlParameter("@categoryId",produt.CategoryId)
            };

            SqlParameter[] inventPs = new SqlParameter[]
            {
                new SqlParameter("@productId",inventory.ProductId),
                new SqlParameter("@minCount",inventory.MinCount),
                new SqlParameter("@maxCount",inventory.MaxCount)
            };
            psList.Add(prodPs);
            psList.Add(inventPs);
            return SQLHelper.UpdateByTran(procList, psList);
        }
        /// <summary>
        /// 商品的各种数据形成多表链接：商品Products表，商品分类表ProductCategory，商品库存表ProductInventory等多表链接
        /// 商品Products表的多表链接，商品分类表ProductCategory，商品库存表ProductInventory等多表链接
        /// </summary>
        /// <returns></returns>
        public List<ProdutsModel> GetAllProduct()
        {
            //GetAllProducts
            string procName = "GetAllProducts"; 
            List<ProdutsModel> list = new List<ProdutsModel>();
            SqlDataReader reader = SQLHelper.GetDataReader(procName, null);
            while (reader.Read())
            {
                list.Add(new ProdutsModel()
                {
                    ProductId = reader["ProductId"].ToString(),
                    ProductName = reader["ProductName"].ToString(),
                    CategoryId = Convert.ToInt32(reader["Categoryld"]),
                    Discount = Convert.ToSingle(reader["Discount"]),
                    Unit = reader["Unit"].ToString(),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                    CategoryName = reader["CategoryName"].ToString(),
                    TotalCount = Convert.ToInt32(reader["TotalCount"])
                });
            }
            reader.Close();
            return list;
        }
        /// <summary>
        /// 商品数量表的存储过程：修改商品数量表的数量
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int InventoryProduct(string pid, int count)
        {
            string procName = "InventoryIn";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@productId",pid),
                new SqlParameter("@count",count)
            };
            return SQLHelper.ExecuteNonQuery(procName, sp);
        }

        /// <summary>
        /// 获取Products商品表的存储过程：根据商品Id，获取这行数据
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public ProdutsModel GetProductWithId(string pid)
        {
            string procName = "GetProductWithId";
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@productId",pid)
            };
            ProdutsModel produt = null;
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            while (reader.Read())
            {
                produt = new ProdutsModel()
                {
                    ProductId = reader["ProductId"].ToString(),
                    ProductName = reader["ProductName"].ToString(),
                    Discount = Convert.ToSingle(reader["Discount"]),
                    CategoryId = Convert.ToInt32(reader["Categoryld"]),
                    CategoryName = reader["CategoryName"].ToString(),
                    Unit = reader["Unit"].ToString(),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"])
                };
            }
            reader.Close();
            return produt;
        }


        // 【4Day15】
        /// <summary>
        /// 【关键字查询】商品的各种数据形成多表链接：商品Products表，商品分类表ProductCategory，商品库存表ProductInventory等多表链接
        /// GetProductsByWhere存储过程名称
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<ProdutsModel> GetProductsWithWhere(int categoryId, string where)
        {
            string procName = "GetProductsByWhere";
            SqlParameter[] sp =
            {
                new SqlParameter("@category",categoryId),
                new SqlParameter("@where",where)
            };
            List<ProdutsModel> list = new List<ProdutsModel>();
            SqlDataReader reader = SQLHelper.GetDataReader(procName, sp);
            while (reader.Read())
            {
                list.Add(new ProdutsModel()
                {
                    ProductId = reader["ProductId"].ToString(),
                    ProductName = reader["ProductName"].ToString(),
                    CategoryId = Convert.ToInt32(reader["Categoryld"]),
                    Discount = Convert.ToSingle(reader["Discount"]),
                    Unit = reader["Unit"].ToString(),
                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                    CategoryName = reader["CategoryName"].ToString(),
                    TotalCount = Convert.ToInt32(reader["TotalCount"])
                });
            }
            reader.Close();
            return list;

        }

        /// <summary>
        /// 商品表：通过商品Id编号修改折扣Discount
        /// SetProductDiscount存储过程名称
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="discount"></param>
        /// <returns></returns>
        public int SetProductDiscount(string pid, float discount)
        {
            string procName = "SetProductDiscount";
            SqlParameter[] sp =
            {
                new SqlParameter("@discount",discount),
                new SqlParameter("@productId",pid)
            };
            return SQLHelper.ExecuteNonQuery(procName, sp);
        }

        /// <summary>
        /// 商品表修改名字、单价、商品分类、计量单位，通过Id商品编号修改商品表的内容
        /// SetProductInfor存储过程名称
        /// </summary>
        /// <param name="produts"></param>
        /// <returns></returns>
        public int UpdateProduct(ProdutsModel produts)
        {
            string procName = "SetProductInfor";
            SqlParameter[] sp =
            {
                new SqlParameter("@productName",produts.ProductName),
                new SqlParameter("@unitPrice",produts.UnitPrice),
                new SqlParameter("@categoryId",produts.CategoryId),
                new SqlParameter("@unit",produts.Unit),
                new SqlParameter("@productId",produts.ProductId)
            };
            return SQLHelper.ExecuteNonQuery(procName, sp);
        }


        //【4Day19自己写的】
        /// <summary>
        /// 商品库存
        ///  把｛｝里面的数据用一个List<ProductInventoryModel>接收
        /// </summary>
        /// <returns></returns>
        public List<ProductInventoryModel> IPSQLku()
        {
            string procName = "PIku";
            List<ProductInventoryModel> list = new List<ProductInventoryModel>();
            SqlDataReader reader = SQLHelper.GetDataReader(procName, null);
            while (reader.Read())
            {
                list.Add(new ProductInventoryModel()
                {
                    ProductId = reader["ProductId"].ToString(),
                    TotalCount = Convert.ToInt32(reader["TotalCount"]),
                    MinCount = Convert.ToInt32(reader["MinCount"]),
                    MaxCount = Convert.ToInt32(reader["MaxCount"]),
                    StatusId =Convert.ToInt32(reader["Statusld"]),
                    ProductName= reader["ProductName"].ToString(),
                    Unit= reader["Unit"].ToString()
                });
            }
            reader.Close();
            return list;
        }
    }
}
