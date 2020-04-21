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

namespace 后台SuperMarketManager
{
    public partial class FrmIntoProduct商品入库 : Form
    {
        public FrmIntoProduct商品入库()
        {
            InitializeComponent();
            //txtProductId控件的获取
            txtProductId.Focus();
            txtProductId.LostFocus += TxtProductId_LostFocus;
        }
        ISuperMarketProductManager2 manager2 = new SuperMarketProductManager2();
        private void TxtProductId_LostFocus(object sender, EventArgs e)
        {
            if (txtProductId.CheckData(@"^\d+$", "商品编号录入有误") == 1)
            {
                ProdutsModel produts = manager2.GetProductWithId(txtProductId.Text.Trim());
                if (produts == null)
                {
                    MessageBox.Show("商品编号录入有误,未查询到对应商品！", "提示");
                    txtProductId.SelectAll();
                    txtProductId.Focus();
                    return;
                }
                else
                {
                    txtProductName.Text = produts.ProductName;
                }
            }
        }

        //入库确认
        private void btnSure_Click(object sender, EventArgs e)
        {
            if (txtProductId.CheckNullOrEmpty() * txtProductName.CheckNullOrEmpty() != 0)
            {
                if (txtCount.CheckData(@"^(-?[1-9]\d*)$", "入库数量为整数") != 0)
                {
                    if (manager2.InventoryProduct(txtProductId.Text.Trim(), Convert.ToInt32(txtCount.Text.Trim())))
                    {
                        if (MessageBox.Show("商品入库成功！是否继续入库", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        {
                            txtCount.Text = "0";
                            txtProductId.Text = txtProductName.Text = "";
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("商品入库失败！", "提示");
                    }
                }
            }
        }
        //取消入库
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
