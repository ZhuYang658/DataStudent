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

//【二】
namespace 后台SuperMarketManager.AdminFrm
{
    public partial class FrmAdmin管理者和营业员 : Form
    {
        public FrmAdmin管理者和营业员()
        {
            InitializeComponent();
            //窗口居中
            this.StartPosition = FormStartPosition.CenterScreen;
            //是否自动创建列
            dataGridView1.AutoGenerateColumns = false;
            //给dataGridView1里面获取管理者的数据
            InitializeUser();
            //在当前绑定项更改时发生
            //source封装器
            source.CurrentChanged += Source_CurrentChanged;
        }

        
        //获取IBLL中的方法SuperMarketAdminManager
        ISuperMarketAdminManager adminManager = new SuperMarketAdminManager();
        //管理者的泛型储存
        List<SysAdminsModel> adminList = null;
        //source封装器
        BindingSource source = new BindingSource();
        //管理者类
        SysAdminsModel currentAdm = null;

        //在当前绑定项更改时发生
        private void Source_CurrentChanged(object sender, EventArgs e)
        {
            //source封装器
            //获取列表中的当前项
            currentAdm = source.Current as SysAdminsModel;
        }


        #region 添加系统用户管理
        //添加系统用户管理
        private void button1_Click(object sender, EventArgs e)
        {
            FrmAddAdmin管理者添加 addAdmin = new FrmAddAdmin管理者添加();
            addAdmin.ShowDialog();
            //调用管理者和营业员的表显示：相当于刷新dataGridView1里面的数据
            InitializeUser();
        }
        #endregion


        //用户主要有后台系统管理员，前台收银员（也可以用来刷新）
        private void InitializeUser()
        {
            //获取数据库中管理者的所有内容，并通过adminList接收管理者的所有数据，并把所有管理者的内容放在dataGridView1里面
            adminList = adminManager.GetAdmins();
            source.DataSource = adminList;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = source;
        }

        #region 修改系统用户管理
        //修改系统用户管理
        private void button2_Click(object sender, EventArgs e)
        {
            //在dataGridView1.RowCount点击选择按钮，则可以修改；否则请选择要修改的用户
            if (dataGridView1.RowCount == 0 || currentAdm == null)
            {
                MessageBox.Show("请选择要修改的用户！");
                return;
            }
            //管理者的修改
            FrmUpdateAdmin管理者修改 updateAdmin = new FrmUpdateAdmin管理者修改(currentAdm);
            if (updateAdmin.ShowDialog() == DialogResult.OK)
            {
                //调用管理者和营业员的表显示：相当于刷新dataGridView1里面的数据
                InitializeUser();
            }
        }
        #endregion

        #region 禁用系统用户管理
        //改禁用系统用户管理
        private void button3_Click(object sender, EventArgs e)
        {
            //dataGridView1.RowCount == 0没有选中按钮；currentAdm == null管理者类里面没有数据；currentAdm.Roleld == 1管理者角色类型1超级2一般
            if (dataGridView1.RowCount == 0 && currentAdm == null && currentAdm.Roleld == 1)
            {
                return;
            }
            //管理者当前状态1启0禁
            currentAdm.AdminStatus = 0;
            //在禁用状态时，进入数据库改成禁用状态
            if (adminManager.SetSysStatus(currentAdm))
            {
                //调用管理者和营业员的表显示：相当于刷新dataGridView1里面的数据
                InitializeUser();
            }
        }
        #endregion

        #region 启动系统用户管理
        //改启动系统用户管理
        private void button4_Click(object sender, EventArgs e)
        {
            //dataGridView1.RowCount == 0没有选中按钮；currentAdm == null管理者类里面没有数据；currentAdm.Roleld == 1管理者角色类型1超级2一般
            if (dataGridView1.RowCount == 0 && currentAdm == null && currentAdm.Roleld == 1)
            {
                return;
            }
            //管理者当前状态1启0禁
            currentAdm.AdminStatus = 1;
            //在禁用状态时，进入数据库改成禁用状态
            if (adminManager.SetSysStatus(currentAdm))
            {
                //调用管理者和营业员的表显示：相当于刷新dataGridView1里面的数据
                InitializeUser();
            }
        }
        #endregion

        #region 关闭窗口
        //关闭窗口
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
