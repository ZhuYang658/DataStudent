using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketBLL.后台SuperMarketManager;
using SuperMarketModel;

namespace 后台SuperMarketManager.ProducFrm
{
    public partial class FrmInventory商品库存 : Form
    {
        //【dataGridView1要和数据库里面的数据绑定，必须给每一个列绑定对应的Model实体类中对应获取到的数据放在dataGridView1】
        public FrmInventory商品库存()
        {
            InitializeComponent();
            //先给封装器放入商品库存的所有数据
            source01.DataSource = manager2.IPSQLku();
            //给dataGridView1起名字为dgvProductList
            ////【dataGridView1要和数据库里面的数据绑定，必须给每一个列绑定对应的Model实体类中对应获取到的数据放在dataGridView1】
            dgvProductList.DataSource = source01;
            //获取manager2.GetCategories()商品分类
            source02.DataSource = manager2.GetCategories();
            //商品分类后面的combox为toolCmbType的名字，添加内容
            cmbCategory.Items.Add("所有");

            //把categories（商品分类）的内容全部遍历给ProductCategoryModel item（商品分类）里面
            foreach (ProductCategoryModel item in source02)
            {
                //把商品分类的商品类型（名字）给商品分类后面的combox为toolCmbType的名字，添加商品类型名
                //Combox添加内容
                cmbCategory.Items.Add(item.CategoryName);
            }
            //Combox的选定当前的选定项的索引
            cmbCategory.SelectedIndex = 0;


        }

        //商品库存方法显示类
        ISuperMarketProductManager2 manager2 = new SuperMarketProductManager2();
        //封装器：装在dataGridView1
        BindingSource source01 = new BindingSource();
        //封装器：商品分类
        BindingSource source02 = new BindingSource();
        #region 提交查询
        private void btnQueryWhere_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 低于库存
        private void btnLowMin_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 超出库存
        private void btnHeightMax_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 库存更新设置
        private void btnRefreshCount_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 修改库存
        private void btnUpdateCount_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 关闭窗口
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
