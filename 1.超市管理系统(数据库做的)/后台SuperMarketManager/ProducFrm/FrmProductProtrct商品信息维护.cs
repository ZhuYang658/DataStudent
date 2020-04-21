using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMarketModel;
using SuperMarketBLL.后台SuperMarketManager;
using SuperMarketIBLL.后台SuperMarketManager;

namespace 后台SuperMarketManager.ProducFrm
{
    public partial class FrmProductProtrct商品信息维护 : Form
    {
        //【dataGridView1要和数据库里面的数据绑定，必须给每一个列绑定对应的Model实体类中对应获取到的数据放在dataGridView1】
        //商品封装方法
        ISuperMarketProductManager2 productManager = new SuperMarketProductManager2();
        //装商品分类表的数据泛型
        List<ProductCategoryModel> categories = null;
        //装商品表的数据泛型
        List<ProdutsModel> list = null;
        //封装器：防止删除修改时直接的报错
        BindingSource source = new BindingSource();
        //商品表属性，属性可以返回值
        public ProdutsModel CurrentProduct { get; set; }
        public FrmProductProtrct商品信息维护()
        {
            InitializeComponent();
            //dataGridView1列表的自动创建
            dgvProductList.AutoGenerateColumns = false;
            // List<ProductCategoryModel> categories = null;商品维护信息
            categories = productManager.GetCategories();
            //商品分类后面的combox为toolCmbType的名字，添加内容
            toolCmbType.Items.Add("所有");
            
            //把categories（商品分类）的内容全部遍历给ProductCategoryModel item（商品分类）里面
            foreach (ProductCategoryModel item in categories)
            {
                //把商品分类的商品类型（名字）给商品分类后面的combox为toolCmbType的名字，添加商品类型名
                //Combox添加内容
                toolCmbType.Items.Add(item.CategoryName);
            }
            //Combox的选定当前的选定项的索引
            toolCmbType.SelectedIndex = 0;
            //从数据库中获取所有商品内容，并把商品Id进行排序,用list接收商品内容
            list = productManager.GetAllProduct();
            //数据库中【商品表】数据的实现和刷新
            InitializeProduct();//封装器source里面获取值了：有的商品表的所有值获取到
            //封装器在当前绑定项更改时发生
            source.CurrentChanged += Source_CurrentChanged;
            //查询的文本框名字toolTxtWhere；TextChanged是Text属性值更改时发生
            toolTxtWhere.TextChanged += ToolTxtWhere_TextChanged;
            //查询的文本框名字toolTxtWhere；GotFocus获取焦点时发生
            toolTxtWhere.GotFocus += ToolTxtWhere_GotFocus;
            //查询的文本框名字toolTxtWhere；LostFocus失去焦点时发生
            toolTxtWhere.LostFocus += ToolTxtWhere_LostFocus;
        }

        #region 查询文本框toolTxtWhere【获取焦点和失去焦点】
        //查询的文本框名字toolTxtWhere；LostFocus失去焦点时发生
        private void ToolTxtWhere_LostFocus(object sender, EventArgs e)
        {
            //查询文本框toolTxtWhere【失去焦点时改变文本框颜色】
            toolTxtWhere.ForeColor = Color.Black;
        }
        //查询的文本框名字toolTxtWhere；GotFocus获取焦点时发生
        private void ToolTxtWhere_GotFocus(object sender, EventArgs e)
        {
            //查询文本框toolTxtWhere中选定文本框的所有内容
            toolTxtWhere.SelectAll();
            //查询文本框toolTxtWhere获取焦点时改变文本框颜色
            toolTxtWhere.ForeColor = Color.FromArgb(100, 100, 100);
        }
        #endregion

        #region 文本框名字toolTxtWhere，【修改文本框tag=1，原来=0】
        //查询的文本框名字toolTxtWhere；TextChanged是Text属性值更改时发生
        //toolTxtWhere文本框中的数据获取，放在CurrentProduct里面
        private void ToolTxtWhere_TextChanged(object sender, EventArgs e)
        {
            //文本框名字toolTxtWhere,TextChanged是Text属性值更改时发生,tag就会改成1，原本tag0
            toolTxtWhere.Tag = "1";
        }
        #endregion

        #region 封装器在当前绑定【项更改时发生】
        //封装器在当前绑定项【更改时发生】
        private void Source_CurrentChanged(object sender, EventArgs e)
        {
            //source是封装器名字，封装器source里面获取值了：有的商品表的所有值获取到
            //获取列表中的当前项，这样就可以获取值放在CurrentProduct里面
            CurrentProduct = source.Current as ProdutsModel;
        }
        #endregion

        #region 数据库中【商品表】数据的实现和刷新
        //数据库中【商品表】数据的实现和刷新
        private void InitializeProduct()
        {
            //给封装器的路径放数据库中商品表的所有数据
            source.DataSource = list;
            //dgvProductList是dataGridView1的名字，把dataGridView1的内容清空
            //dgvProductList里面的列数据名和数据中的名字对应才可获取值
            dgvProductList.DataSource = null;
            //给dataGridView1里面添加商品表数据，通过封装器放进去的值
            dgvProductList.DataSource = source;
        }
        #endregion

        #region 提交查询
        private void toolBtnQuery_Click(object sender, EventArgs e)
        {
            //【1】商品名称CategoryName
            //【2】categories装商品分类表的数据泛型
            //【3】toolCmbType是Combox的名字
            //【总结：】通过linq查询把商品分类后的Combox进行查询选定toolCmbType名字；查询出对应商品Id
            var selectIndex = from item in categories where item.CategoryName == toolCmbType.SelectedItem.ToString() select item.CategoryId;
            int index = 0;
            //toolTxtWhere查询文本框内容
            string where = toolTxtWhere.Text.Trim();
            //没有查询文本框内容条件；selectIndex.FirstOrDefault() == 0表示选定的文本框
            if (toolTxtWhere.Tag.ToString() == "0" && selectIndex.FirstOrDefault() == 0)
            {
                //获取多表连接的内容给list存放
                list = productManager.GetAllProduct();
                //dataGridView1获取商品多表链接数据，刷新多表链接内容
                InitializeProduct();
                return;
            }
            //查询文本框的tag==0    and   选定文本框！=0
            else if (toolTxtWhere.Tag.ToString() == "0" && selectIndex.FirstOrDefault() != 0)
            {
                where = "";
                index = selectIndex.FirstOrDefault();
            }
            //查询文本框的tag!=0    and   选定文本框==0   :就是第一个条件不满足，第二个条件满足
            else if (toolTxtWhere.Tag.ToString() != "0" && selectIndex.FirstOrDefault() == 0)
            {
                where = toolTxtWhere.Text.Trim();
                index = 0;
            }
            //查询文本框的tag!=0    and   选定文本框==0   :就是第一个条件不满足，第二个条件不满足
            else if (toolTxtWhere.Tag.ToString() != "0" && selectIndex.FirstOrDefault() != 0)
            {
                where = toolTxtWhere.Text.Trim();
                index = selectIndex.FirstOrDefault();
            }
            //list获取商品的GetProductsWithWhere多表连接的【查询】
            list = productManager.GetProductsWithWhere(index, where);
            //dataGridView1获取商品多表链接数据，刷新多表链接内容
            InitializeProduct();

            toolTxtWhere.Text = "商品编号、商品名称";
            toolCmbType.SelectedIndex = 0;
            toolTxtWhere.Tag = "0";
        }
        #endregion

        #region 商品入库
        private void toolBtnInto_Click(object sender, EventArgs e)
        {
            //商品入库代码已写
            FrmIntoProduct商品入库 intoProduct = new FrmIntoProduct商品入库();
            intoProduct.ShowDialog();//打开窗口
            //dataGridView1获取商品多表链接数据，刷新多表链接内容
            InitializeProduct();
        }
        #endregion

        #region 添加商品
        private void toolBtnAdd_Click(object sender, EventArgs e)
        {
            //添加商品已写
            FrmAddProduct添加商品 addProduct = new FrmAddProduct添加商品();
            addProduct.ShowDialog();//打开窗口
            //dataGridView1获取商品多表链接数据，刷新多表链接内容
            InitializeProduct();
        }
        #endregion

        #region 修改商品
        private void toolBtnUpdate_Click(object sender, EventArgs e)
        {
            //CurrentProduct商品表属性，属性可以返回值
            ////list装商品表的数据泛型
            //如果CurrentProduct商品表属性==null，或list的内容==0；则返回return
            if (list.Count == 0 || CurrentProduct == null)
            {
                MessageBox.Show("请选择要修改的商品！");
                return;
            }
            //修改商品窗口，CurrentProduct的值获取来源是上面的按钮获取到商品中的数据，传递到修改商品窗口中
            FrmUpdateProduct updateProduct = new FrmUpdateProduct(CurrentProduct);
            if (updateProduct.ShowDialog() == DialogResult.OK)
            {
                //用list装商品表的数据泛型获取多表连接的商品表
                list = productManager.GetAllProduct();
                //dataGridView1获取商品多表链接数据，刷新多表链接内容
                InitializeProduct();
            }
        }
        #endregion

        #region 删除商品
        private void toolBtnDelete_Click(object sender, EventArgs e)
        {
            if (list.Count <= 0)
            {
                return;
            }
            dgvProductList.DataSource = null;
            dgvProductList.DataSource = source;
        }
        #endregion

        #region 更新折扣
        private void btnRefreshDiscount_Click(object sender, EventArgs e)
        {
            //更新折扣内容
            if (txtDiscount.CheckData(@"^\d(.\d)?$", "输入有误") != 0)
            {
                if (CurrentProduct != null)
                {
                    //修改折扣：根据商品Id和商品从新更改的折扣，进行折扣修改
                    if (productManager.SetProductDiscount(CurrentProduct.ProductId, Convert.ToSingle(txtDiscount.Text.Trim())))
                    {
                        //修改成功后，把修改后的折口，给商品表属性添加可以进行同步修改
                        CurrentProduct.Discount = Convert.ToSingle(txtDiscount.Text.Trim());
                        //dataGridView1获取商品多表链接数据，刷新多表链接内容
                        InitializeProduct();
                        txtDiscount.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("修改商品折扣失败！", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("请选择要更改折扣的商品！", "提示");
                }
            }
        }
        #endregion
    }
}
