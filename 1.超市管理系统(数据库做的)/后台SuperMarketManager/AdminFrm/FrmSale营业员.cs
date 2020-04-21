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

namespace 后台SuperMarketManager.AdminFrm
{
    public partial class FrmSale营业员 : Form
    {
        public FrmSale营业员()
        {
            InitializeComponent();
            //不能自动创建dataGridView1的列
            dataGridView1.AutoGenerateColumns = false;
            //调用数据库中营业员的所有数据
            InitializeSale();
            //封装器在当前绑定项更改时发生
            source.CurrentChanged += Source_CurrentChanged;
        }

        

        //获取管理者和营业员
        ISuperMarketAdminManager adminManager = new SuperMarketAdminManager();
        //营业员属性可以返回值
        public SelesPersonSModel Person { get; set; }
        //营业员类泛型
        List<SelesPersonSModel> list = null;
        //封装器
        BindingSource source = new BindingSource();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Source_CurrentChanged(object sender, EventArgs e)
        {
            //获取列表中的当前项
            Person = source.Current as SelesPersonSModel;
        }
        /// <summary>
        /// 添加营业员按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSysAdm_Click(object sender, EventArgs e)
        {
            FrmAddSale营业员添加 addSale = new FrmAddSale营业员添加();
            addSale.ShowDialog();
            //调用数据库中营业员的所有数据,并刷新
            InitializeSale();
        }
        //修改营业员按钮
        private void btnUpdateSysAdm_Click(object sender, EventArgs e)
        {
            FrmUpdateSale营业员修改 updateSale = new FrmUpdateSale营业员修改(Person);
            if (updateSale.ShowDialog() == DialogResult.OK)
            {
                //调用数据库中营业员的所有数据,并刷新
                InitializeSale();
            }
        }
        //调用数据库中营业员的所有数据
        private void InitializeSale()
        {
            //从数据库中获取营业员的全部内容，并显示在窗口中
            list = adminManager.GetSales();
            //用封装器装数据（可以防止修改添加及时更改）
            source.DataSource = list;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = source;
        }

        //关闭按钮
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
