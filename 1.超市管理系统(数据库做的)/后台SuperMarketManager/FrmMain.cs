using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMarketIBLL.前台SuperMarketCashier;
using SuperMarketBLL.前台SuperMarketCashier;
using SuperMarketCommon;
using SuperMarketModel;

namespace 后台SuperMarketManager
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.AutoSize = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            //关闭窗体前发生
            this.FormClosing += FrmMain_FormClosing;
            this.FormClosed += FrmMain_FormClosed;
            //在底下的栏里获取管理者的名字
            toollblUser.Text = $"【{Program.CurrentAdmin.AdminName}】";
            //将主窗体设置为MDI容器(父容器上设置这样可以让另一个窗口在父容器上显示)
            this.IsMdiContainer = true;
        }


        #region 负责开启功能窗体
        //记录当前开启的功能窗体（就是声明一个窗口）
        Form currentMDIChild = null;
        void ShowMDIChild(Form mdiForm)
        {
            //如果声明的窗口不为空，就是表明已经有这个窗口在显示了，这时候先关闭窗口，在从新执行下面的内容
            if (currentMDIChild != null)
            {
                currentMDIChild.Close();
            }
            //获取引进来的窗口
            currentMDIChild = mdiForm;
            //获取或设置此窗体当前多文档界面(MDI)父窗体
            //在父级上获取mdiForm.MdiParent这个窗口
            mdiForm.MdiParent = this;
            //获取这个窗口应放在对应的父容器上
            mdiForm.Parent = panel1;
            //子容器在父容器上的位置
            mdiForm.Left = panel1.Width / 2 - mdiForm.Width / 2;
            mdiForm.Top = panel1.Height / 2 - mdiForm.Height / 2;
            //获取或设置边框样式为单边框
            mdiForm.FormBorderStyle = FormBorderStyle.FixedSingle;
            //显示子窗口
            mdiForm.Show();
        }
        #endregion

        #region 主程序退出的时候做什么事情:【用数据库里的日志记录管理者退出时间】或【直接退出】
        //关闭窗体前发生
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //程序退出到数据库日志里面记录管理者退出的时间
            ISelesPersonManager saleManager = new SelesPersonManager();
            //老师写的日志log4net
            Log4net类2Teacher.Info($"[{Program.CurrentAdmin.LoginId}]退出程序！");
            saleManager.BllWriteSelesExitLog(Program.CurrentAdmin.LoginLogId);
        }

        #endregion
        

        #region 获取时间的更新
        private void timer1_Tick(object sender, EventArgs e)
        {
            toollblTime.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
        #endregion


        #region 退出系统 和 关闭时触发
        //菜单栏框：退出系统
        private void toolMenuExitSys_Click(object sender, EventArgs e)
        {
            //关闭该窗口
            this.Close();
        }
        //关闭时触发
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath, "restart");
        }
        #endregion

        #region 菜单栏框修改管理者的密码
        private void toolMenuUpdatePwd_Click(object sender, EventArgs e)
        {
            FrmUpdatePwd pwd = new FrmUpdatePwd();
            DialogResult Restart = pwd.ShowDialog();
            //密码修改成功，意味着需要重新登录
            if (Restart == DialogResult.OK)
            {
                //老师写的日志文件
                Log4net类2Teacher.Info($"[{Program.CurrentAdmin.LoginId}]成功修改密码");

                this.Close();//主线程关闭
                //修改密码之后重启

            }
        }
        #endregion

        #region 用户管理模块（没有添加功能)
        private void toolMenuUserManager_Click(object sender, EventArgs e)
        {
            //用户管理模块(没有添加功能)
            AdminFrm.FrmAdmin管理者和营业员 admin = new AdminFrm.FrmAdmin管理者和营业员();
            //负责开启功能窗体的方法，并显示子窗口
            ShowMDIChild(admin);
        }

        #endregion


        #region 添加商品功能
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            //【1】创建添加商品窗口
            FrmAddProduct添加商品 addProduct = new FrmAddProduct添加商品();
            //【2.1】当添加商品FrmAddProduct关闭窗体后发生
            addProduct.FormClosed += AddProduct_FormClosed;
            //【3】在父级容器获取此窗口，并显示子窗口
            ShowMDIChild(addProduct);
        }


        #endregion

        #region 商品入库功能
        ProdutsModel currentProduct = null;
        //【2.2】
        //当添加商品FrmAddProduct关闭窗体后发生，
        private void AddProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            //实例化添加商品窗口
            FrmAddProduct添加商品 frmAdd = sender as FrmAddProduct添加商品;
            if (frmAdd.DialogResult == DialogResult.OK)
            {
                currentProduct = frmAdd.Tag as ProdutsModel;
                frmAdd.DialogResult = DialogResult.Cancel;
                btnIntoProduct_Click(frmAdd, null);
            }
        }
        //添加商品的数量
        private void btnIntoProduct_Click(object sender, EventArgs e)
        {
            //添加商品数量的窗口
            FrmIntoProduct商品入库 intoProduct = new FrmIntoProduct商品入库();
            if (currentProduct != null)
            {
                //【重点：】intoProduct.txtProductId.Text的获取一定要改FrmIntoProduct窗口中对应文本框的Modifiers为public
                intoProduct.txtProductId.Text = currentProduct.ProductId;
                intoProduct.txtProductName.Text = currentProduct.ProductName;
            }
            ShowMDIChild(intoProduct);
        }
        #endregion


        //4Day14
        #region 添加会员
        private void btnAddMember_Click(object sender, EventArgs e)
        {
            //添加会员窗口
            FrmAddMember frm = new FrmAddMember();
            //负责开启功能窗体
            ShowMDIChild(frm);
        }
        #endregion

        #region 日志查询
        private void btnCheckLog_Click(object sender, EventArgs e)
        {
            AdminFrm.FrmLogCheck日志 logCheck = new AdminFrm.FrmLogCheck日志();
            ShowMDIChild(logCheck);
        }
        #endregion

        
        #region 用户管理模块 
        /// <summary>
        /// 营业员管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSaleManager_Click(object sender, EventArgs e)
        {
            AdminFrm.FrmSale营业员 sale = new AdminFrm.FrmSale营业员();
            ShowMDIChild(sale);
        }
        /// <summary>
        /// 系统管理者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolSysManager_Click(object sender, EventArgs e)
        {
            //如果当前角色是超级管理员才可以禁用其他普通管理员，否则无权限
            if (Program.CurrentAdmin.Roleld == 1)
            {
                AdminFrm.FrmAdmin管理者和营业员 admin = new AdminFrm.FrmAdmin管理者和营业员();
                ShowMDIChild(admin);
            }
            else
            {
                MessageBox.Show("您无权限操作该功能！", "提示");
            }
        }
        #endregion

        #region 库存管理
        private void btnInventory_Click(object sender, EventArgs e)
        {
            //商品库存查询（商品库存表的显示）
            ProducFrm.FrmInventory商品库存 inventory = new ProducFrm.FrmInventory商品库存();
            //调用负责开启功能窗体
            ShowMDIChild(inventory);
        }
        #endregion
        #region 商品维护
        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            ProducFrm.FrmProductProtrct商品信息维护 productProtrct = new ProducFrm.FrmProductProtrct商品信息维护();
            //调用负责开启功能窗体
            ShowMDIChild(productProtrct);
        }
        #endregion
    }
}
