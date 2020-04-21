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
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketBLL.后台SuperMarketManager;
using SuperMarketCommon;

namespace 后台SuperMarketManager.ProducFrm
{
    public partial class FrmUpdateProduct : Form
    {
        //ProdutsModel produts从商品信息维护获取数据
        public FrmUpdateProduct(ProdutsModel produts)
        {
            InitializeComponent();
            
            //商品名字获取焦点
            txtProductName.Focus();
            //GetCategories()获取【商品分类表中的所有数据库】并给categories商品类型泛型中添加内容
            categories = manager2.GetCategories();
            //GetUnit()获取【商品的计量单位从数据库中获取所有数据】并给units商品计量单位泛型中添加内容
            units = manager2.GetUnit();
            
            //【商品类型】和【商品计量单位】不能为空
            if (categories.Count == 0 || units.Count == 0)
            {
                //如果【商品类型】和【商品计量单位】为空则return下面的代码不再执行
                return;
            }
            source1.DataSource = categories;
            //cmbCategory商品类型的Combox的名字
            cmbCategory.DataSource = source1;
            //【cmbCategory.ValueMember】和【cmbCategory.DisplayMember】分别记录一个标记，到时候数据库的商品类型表可以一一对应拿到的数据对应的标记位置
            //key：CategoryId获取商品类型Id（就是给Combox里面放CategoryId的key名字，用来记录Combox的内容）
            cmbCategory.ValueMember = "CategoryId";
            //Combox的值的显示（就是给Combox里面放CategoryName属性名字，用来记录Combox的内容）
            cmbCategory.DisplayMember = "CategoryName";
            //获取或设置指定当前选定的索引项
            cmbCategory.SelectedIndex = produts.CategoryId - 1;

            //【用两个封装器装【商品类型】和【商品计量单位】的数据】
            source2.DataSource = units;
            //给Combox计量单位里面放封装器接收数据库获取的数据
            cmbUnit.DataSource = source2;
            //Combox中对应位置放入对应的所有数据
            cmbUnit.ValueMember = "Id";
            cmbUnit.DisplayMember = "Unit";
            //通过linq查询返回第一个Id值
            cmbUnit.SelectedIndex = ((from item in units where item.Unit == produts.Unit select item.Id).FirstOrDefault() - 1);

            //给txtProductId.Text文本框添加Id数据produts.ProductId（通过另一个页面获取到商品Id的数据）
            txtProductId.Text = produts.ProductId;
            //给txtProductName.Text文本框添加商品名字（通过另一个页面获取到商品Id的数据）
            txtProductName.Text = produts.ProductName;
            //给txtUnitPrice.Text添加折扣的修改
            txtUnitPrice.Text = produts.UnitPrice.ToString("F2");
            //produts商品表内容把从另一个页面获取的值给CurrentProduct商品属性记录
            CurrentProduct = produts;
            //把获取焦点是触发数据
            txtProductId.GotFocus += TxtProductId_GotFocus;
            txtProductName.GotFocus += TxtProductId_GotFocus;
            txtUnitPrice.GotFocus += TxtProductId_GotFocus;
        }
        #region【各种实例化】
        //商品的BLL业务逻辑层；DAL数据访问层；Model实体层
        ISuperMarketProductManager2 manager2 = new SuperMarketProductManager2();
        //categories商品类型泛型
        List<ProductCategoryModel> categories = null;
        //封装器：方便窗口数据和数据库数据值的互相使用增删改，这样可以防止转换不及时；相当于中间站
        BindingSource source1 = new BindingSource();
        //封装器：方便窗口数据和数据库数据值的互相使用增删改，这样可以防止转换不及时；相当于中间站
        BindingSource source2 = new BindingSource();
        //商品计量单位表的类
        List<ProductUnitModel> units = null;
        #endregion

        //商品表的属性
        public ProdutsModel CurrentProduct { get; set; }

        //获取焦点时触发
        private void TxtProductId_GotFocus(object sender, EventArgs e)
        {
            SuperTextbox text = sender as SuperTextbox;
            text.SelectAll();
        }

        #region 确认修改
        private void btnSure_Click(object sender, EventArgs e)
        {
            //判断文本框输入数据的格式是否正确
            if (txtProductId.CheckData(@"^\d{6,}$", "商品编格式错误") * txtProductName.CheckNullOrEmpty() * txtUnitPrice.CheckData(@"^\d*(.\d\d?)+$", "单价格式错误！") != 0)
            {
                //把窗口中的文本框商品Id内容给商品表的属性存数据
                CurrentProduct.ProductId = txtProductId.Text.Trim();
                //把窗口中的文本框商品名字内容给商品表的属性存数据
                CurrentProduct.ProductName = txtProductName.Text.Trim();
                //把窗口中的文本框商品折扣内容给商品表的属性存数据
                CurrentProduct.UnitPrice = Convert.ToDecimal(txtUnitPrice.Text.Trim());
                //把窗口中的文本框商品商品分类内容给商品表的属性存数据
                CurrentProduct.CategoryId = (from item in categories where item.CategoryId == Convert.ToInt32(cmbCategory.SelectedValue) select item.CategoryId).FirstOrDefault();
                //把窗口中的文本框商品商品计量内容给商品表的属性存数据
                CurrentProduct.Unit = (from uitem in units where uitem.Id == cmbUnit.SelectedIndex + 1 select uitem.Unit).FirstOrDefault();
                //调用商品的修改储存过程
                if (manager2.UpdateProduct(CurrentProduct))
                {
                    MessageBox.Show("修改商品成功！");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("修改商品失败！");
                }
            }
        }
        #endregion

        #region 取消修改
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
