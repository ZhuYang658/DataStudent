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
    public partial class FrmAddSale营业员添加 : Form
    {
        public FrmAddSale营业员添加()
        {
            InitializeComponent();
            txtAdmPwd.GotFocus += TxtAdmPwd_GotFocus;
        }


        //获取管理者和营业员中的各种方法
        ISuperMarketAdminManager adminManager = new SuperMarketAdminManager();


        private void TxtAdmPwd_GotFocus(object sender, EventArgs e)
        {
            txtAdmPwd.SelectAll();
        }
        /// <summary>
        /// 添加营业员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSure_Click(object sender, EventArgs e)
        {
            if (txtAdmName.CheckNullOrEmpty() * txtAdmPwd.CheckData(@"^\w{6,}$", "密码必须为6位字母、数字、下划线组合") == 0)
            {
                return;
            }
            else
            {
                SelesPersonSModel person = new SelesPersonSModel()
                {
                    LoginPwd = txtAdmPwd.Text.Trim(),
                    SPName = txtAdmName.Text.Trim()
                };
                //添加营业员的方法
                person = adminManager.InsertSales(person);
                if (person == null)
                {
                    MessageBox.Show("添加营业员失败！", "提示");
                }
                else
                {
                    if (MessageBox.Show($"添加营业员成功！\r\n编号：【{person.SalesPersonId}】\r\n是否继续添加", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        txtAdmName.Text = "";
                        txtAdmPwd.Text = "123456";
                        return;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 取消添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
