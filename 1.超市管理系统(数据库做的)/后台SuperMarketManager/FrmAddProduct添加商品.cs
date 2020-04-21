using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMarketCommon;
using SuperMarketModel;
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketBLL.后台SuperMarketManager;

namespace 后台SuperMarketManager
{
    public partial class FrmAddProduct添加商品 : Form
    {
        public FrmAddProduct添加商品()
        {
            InitializeComponent();
            //窗口创建之前的数据
            InitializeProduct();
            //商品编号
            txtProductId.GotFocus += TxtProductId_GotFocus;
            //商品折扣
            txtDiscount.GotFocus+= TxtProductId_GotFocus;
            //最大库存量
            txtMaxCount.GotFocus += TxtProductId_GotFocus;
            //最小库存量
            txtMinCount.GotFocus += TxtProductId_GotFocus;
            //商品名字
            txtProductName.GotFocus += TxtProductId_GotFocus;
            //销售单价
            txtProductUnitprice.GotFocus += TxtProductId_GotFocus;

        }
        //实例化：给数据库添加商品，或给数据库的商品添加数量，或获取商品分类表的类，或获取窗口创建之前的数据。。。
        ISuperMarketProductManager2 manager2 = new SuperMarketProductManager2();
        //初始化基本信息
        BindingSource categorysource = new BindingSource();//封装一个容器相当于转换器
        BindingSource unitsource = new BindingSource();

        //商品分类表的类
        List<ProductCategoryModel> categories = null;
        //商品计量单位表（商品单位）
        List<ProductUnitModel> units = null;
        //窗口创建之前的数据
        private void InitializeProduct()
        {
            txtMinCount.Text = "0";
            txtMaxCount.Text = "0";
            txtDiscount.Text = "0";
            txtProductId.Text = "";
            txtProductName.Text = "";
            txtProductUnitprice.Text = "0.00";
            //获取商品分类表在数据库的所有内容（现在DAL获取商品分类表拿到所有数据库中的数据）
            categories=manager2.GetCategories();//把获取的数据给categories
            //把获取的数据给categorysource.DataSource封装容器
            categorysource.DataSource = categories;
            //再把categorysource封装的容器给combox产品分类
            cmbCategory.DataSource = categorysource;
            //显示对于数据的Id
            cmbCategory.ValueMember = "CategoryId";
            //显示combox的具体的数据
            cmbCategory.DisplayMember = "CategoryName";
            //主窗口ClockCategory属性=！0进入
            if (Program.ClockCategory != null)
            {
                //cmbCategory是Combox商品分类的名字
                //cmbCategory.SelectedIndex获取选定的索引
                //Program.ClockCategory.CategoryId - 1:表示商品分类表的Id从0开始记录（数据库是从1开始所以-1）
                //选中的Id-1的原因是 cmbCategory.SelectedIndex从0开始计算，所以Program.ClockCategory.CategoryId - 1，因为Program.ClockCategory.CategoryId从1开始获取
                cmbCategory.SelectedIndex = Program.ClockCategory.CategoryId - 1;
                //按钮上的图片显示，0表示一种图片（关闭锁图片）
                btnClock.ImageIndex = 0;
                //Combox里面的内容不能在选了
                cmbCategory.Enabled = false;
            }
            else
            {
                //cmbCategory.SelectedIndex获取选定的索引
                cmbCategory.SelectedIndex = 0;
                //按钮上的图片显示，1表示一种图片
                btnClock.ImageIndex = 1;
                ///Combox里面的内容可以选了
                cmbCategory.Enabled = true;
            }
            //商品计量单位：获取数据库中商品计量单位表中的所有数据
            units = manager2.GetUnit();
            //放在一个容器用在做中间站
            unitsource.DataSource = units;
            //在把获取的值放在Combox计价单位
            cmbUnit.DataSource = unitsource;
            //分别获取对应的Id
            cmbUnit.ValueMember = "Id";
            //和对应的数据
            cmbUnit.DisplayMember = "Unit";
            //获取对应的combox选定的项目
            cmbUnit.SelectedIndex = 0;
        }
        //锁定按钮
        private void btnClock_Click(object sender, EventArgs e)
        {
            //主窗口ClockCategory属性==null进入
            if (Program.ClockCategory == null)//证明如果是非锁定状态，则应该改变成锁定状态
            {
                //数据库的Id和==combox的Value对应
                var obj = from item in categories where item.CategoryId == Convert.ToInt32(cmbCategory.SelectedValue) select item;
                //返回obj序列的第一个元素
                Program.ClockCategory = obj.FirstOrDefault();
                //获取对应的combox选定的项目
                btnClock.ImageIndex = 0;
                //Combox里面的内容不能在选了
                cmbCategory.Enabled = false;
            }
            else
            {
                //商品表分类==空
                Program.ClockCategory = null;
                //获取对应的combox选定的项目
                btnClock.ImageIndex = 1;
                //Combox里面的内容可以选了
                cmbCategory.Enabled = true;
            }
        }
        //失去焦点时发生
        private void TxtProductId_GotFocus(object sender, EventArgs e)
        {
            SuperTextbox text = sender as SuperTextbox;
            //选定文本框中的所有文本
            text.SelectAll();
        }

        /// <summary>
        /// 添加商品
        /// （1）根据商品Id添加新商品
        /// （2）新加的Id不能和数据库Id重复
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            //判断每个文本框是否是正确的，如果不正确直接return下面不在执行
            if (txtProductId.CheckData("^\\d+$", "商品编号必须是至少6位数字") * txtProductName.CheckNullOrEmpty() * txtProductUnitprice.CheckData(@"^(([1-9]\d*)|(\d*.\d{1,2}))$", "输入金额有误") * txtDiscount.CheckData(@"^((\d)|(\d.\d))$", "折扣输入有误") * txtMinCount.CheckData(@"^\d+$", "最小库存输入有误") * txtMaxCount.CheckData(@"^\d+$", "最大库存输入有误") == 0)
            {
                return;
            }
            else
            {
                //【一】当录入商品的时候要做一些判断：判断商品编号必须是唯一的，其二判断商品的名称必须是唯一的

                //productList获取数据库中商品表的所有内容
                List<ProdutsModel> productList = manager2.GetAllProduct();
                var obj1 = from objPro in productList
                           where objPro.ProductId == txtProductId.Text
                           select objPro;
                //【1】商品的编号不能重复；判断商品编号必须是唯一的
                if (obj1.Count() > 0)
                {
                    MessageBox.Show("商品编号已经存在,请重新录入!", "提示");
                    txtProductId.SelectAll();
                    return;
                }
                //判断数据库中所有商品名objPro.ProductName是否==txtProductName.Text里面的名字有相同的
                //productList获取商品的所有名字；获取数据库中商品表的所有内容
                var obj2 = from objPro in productList
                           where objPro.ProductName == txtProductName.Text
                           select objPro;
                //【3】断商品的名称必须是唯一的；obj2.Count() > 0表示有重复的名字
                if (obj2.Count() > 0)
                {
                    MessageBox.Show("商品名称已经存在,请重新录入!", "提示");
                    txtProductName.SelectAll();
                    return;
                }
                //判断最大库存量不能小于最小库存量
                if (int.Parse(txtMinCount.Text) > int.Parse(txtMaxCount.Text))
                {
                    MessageBox.Show("最大库存量不能小于最小库存量", "提示");
                    return;
                }
                else//【二】添加商品
                {
                    //cmbUnit.SelectedValue单位是否==和数据库中的单位相同
                    var pu = from item in units where item.Id == Convert.ToInt32(cmbUnit.SelectedValue) select item;
                    //商品表
                    ProdutsModel produts = new ProdutsModel()
                    {
                        ProductId = txtProductId.Text.Trim(),
                        ProductName = txtProductName.Text.Trim(),
                        Discount = Convert.ToSingle(txtDiscount.Text.Trim()),
                        UnitPrice = Convert.ToDecimal(txtProductUnitprice.Text.Trim()),
                        CategoryId = Convert.ToInt32(cmbCategory.SelectedValue),
                        Unit = pu.FirstOrDefault().Unit
                    };
                    //商品数量表
                    ProductInventoryModel inventory = new ProductInventoryModel()
                    {
                        ProductId = txtProductId.Text.Trim(),
                        MinCount = Convert.ToInt32(txtMinCount.Text.Trim()),
                        MaxCount = Convert.ToInt32(txtMaxCount.Text.Trim())
                    };
                    //获取商品表和商品数量表
                    bool res = manager2.InsertProduct(produts, inventory);
                    if (res)
                    {
                        if (MessageBox.Show("添加商品成功，是否继续添加", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            InitializeProduct();
                            txtProductId.Focus();
                            return;
                        }
                        else
                        {
                            if (MessageBox.Show("是否对该商品进行入库？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                this.DialogResult = DialogResult.OK;
                                this.Tag = produts;
                            }
                            else
                            {
                                this.DialogResult = DialogResult.Cancel;
                            }
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("添加商品失败！", "提示");
                        return;
                    }
                }
            }
        }
        //取消商品
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
