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

namespace 后台SuperMarketManager.AdminFrm
{
    public partial class FrmUpdateSale营业员修改 : Form
    {
        public FrmUpdateSale营业员修改(SelesPersonSModel person)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            Sales = person;
            txtAdmName.Text = person.SPName;
            txtAdmPwd.Text = person.LoginPwd;
        }
        //营业员属性
        public SelesPersonSModel Sales { get; set; }
        //获取管理者和营业员中的各种方法
        ISuperMarketAdminManager adminManager = new SuperMarketAdminManager();

        //确认修改
        private void btnSure_Click(object sender, EventArgs e)
        {
            if (txtAdmName.CheckNullOrEmpty() * txtAdmPwd.CheckData(@"^\w{6,}$", "密码必须为6位字母、数字、下划线组合") != 0)
            {
                Sales.SPName = txtAdmName.Text.Trim();
                Sales.LoginPwd = txtAdmPwd.Text.Trim();
                //修改营业员的方法
                if (adminManager.UpdateSales(Sales) != null)
                {
                    MessageBox.Show("修改成功！", "提示");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("修改失败！", "提示");
                }
            }
        }
        //取消修改
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
