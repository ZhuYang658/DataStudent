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
using SuperMarketCommon;

//【一】
namespace 后台SuperMarketManager.AdminFrm
{
    public partial class FrmLogCheck日志 : Form
    {
        ISuperMarketLoginLogManager4 logManager4 = new SuperMarketLoginLogManager4();
        public FrmLogCheck日志()
        {
            InitializeComponent();
            startTime.Focus();//第一个文本框查询时间的焦点
            //在接收焦点时发生变化
            txtWhere.GotFocus += Txtwhere_GotFocus;
            //焦点失去时发生改变
            txtWhere.LostFocus += Txtwhere_LostFocus;

            //获取数据库中日志的所有数据，把数据库中的所有数据给logList泛型里面装数据库获取的内容
            logList = logManager4.GetLoginLog();
            //把数据库日志中的所有内容获取到，然后通过InitializePage方法分配下去值
            InitializePage();
            dataGridView1.AutoGenerateColumns = false;
        }
        //放所有获取数据库的日志
        List<LoginLogsModel> logList = null;
        List<LoginLogsModel> currentPageList = null;
        private void InitializePage()
        {
            if (logList == null)
            {
                MessageBox.Show("暂无任何登录信息！");
                return;
            }
            else
            {
                //pageNav表示组件的名字：把logList获取的数据给pageNav.RecordCount放数据
                pageNav.RecordCount = logList.Count;
                //获取页面的初始值;当前页码为1
                pageNav.CurrentPageIndex = 1;
                //每页的数量
                pageNav.PageSize = 15;
                //SoreType.ASC要获取必须SuperMarketCommon类
                //如果什么都没赋值，则默认为从0开始往上累加;获取0
                pageNav.SoreType = SoreType.ASC;
                //【一】currentPage获取日志内容
                //【logList中的数据分页查询】
                //【1.1】Skip()跳过指定的数量的序列中的元素，然后返回剩余元素
                //【1.2】pageNav.CurrentPageIndex - 1当前页码0开始：要减原来的数值
                //【1.3】(pageNav.CurrentPageIndex - 1) * pageNav.PageSize；每页的数量（pageNav.PageSize）
                //【2.1】Take()从系列的开始返回指定的数量的连续元素
                //【2.2】ToList()
                //currentPageList = logList.Skip((pageNav.CurrentPageIndex - 1) * pageNav.PageSize).Take(pageNav.PageSize).ToList();
                
                //
                pageNav.ExceuteSetPageEvent += PageNav_ExceuteSetPageEvent;
                //首次查询的方法（第一页的查询）
                pageNav.FirstSearh();
            }
        }
        //封装的中间站
        BindingSource source = new BindingSource();

        private void PageNav_ExceuteSetPageEvent(int CurrentPageIndex)
        {

            if (logList != null)
            {
                currentPageList = logList.Skip((pageNav.CurrentPageIndex - 1) * pageNav.PageSize).Take(pageNav.PageSize).ToList();
                source.DataSource = currentPageList;
                dataGridView1.DataSource = source;
                pageNav.SetButtonEnable();
            }
        }
        #region 分页查询
        //开始查询
        private void btnCheck_Click(object sender, EventArgs e)
        {
            //证明条件框中未输入内容,也不需要按区间查询，查询所有数据
            // txtWhere.Tag默认为1
            if (checkBox1.Checked == false && txtWhere.Tag.ToString() == "1")
            {
                logList = logManager4.GetLoginLog();
                pageNav.RecordCount = logList.Count;
                pageNav.FirstSearh();
            }
            else
            {
                DateTime start = DateTime.Now;
                DateTime end = DateTime.Now;
                string where = "";
                int check = 0;
                //需要按照查询区间进行查询
                if (checkBox1.Checked == true)
                {
                    check = 1;
                    if (startTime.Value == endTime.Value)//等于
                    {
                        check = 2;
                        start = end = Convert.ToDateTime(startTime.Value.ToShortDateString());
                    }
                    else if (startTime.Value < endTime.Value)//小于
                    {
                        start = Convert.ToDateTime(startTime.Value.ToShortDateString());
                        //'2020-04-14 0:00:00'
                        end = Convert.ToDateTime(endTime.Value.ToShortDateString()).AddDays(1);
                    }
                    else if (startTime.Value > endTime.Value)//大于
                    {
                        check = -1;
                        start = end = Convert.ToDateTime(startTime.Value.ToShortDateString());
                    }
                    if (txtWhere.Tag.ToString() == "1")//不带条件的查询
                    {
                        where = "";
                    }
                    else
                    {
                        where = txtWhere.Text.Trim();
                    }
                }
                else//不按照区间查询但是需要按照条件查询所有的
                {
                    check = 0;
                    where = txtWhere.Text.Trim();
                }
                //日志的模糊查询
                logList = logManager4.GetLoginLogBy(start, end, where, check);
                pageNav.RecordCount = logList.Count;
                pageNav.FirstSearh();
            }
        }
        #endregion

        #region 失去焦点和获取焦点
        private void Txtwhere_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWhere.Text))
            {
                txtWhere.Text = "姓名、账号、服务器";
                txtWhere.ForeColor = Color.FromArgb(100, 100, 100);
            }
        }

        //在接收焦点时发生变化
        private void Txtwhere_GotFocus(object sender, EventArgs e)
        {
            //选定文本框中的所有文本
            txtWhere.SelectAll();
            //当焦点放上去 其他条件后问文本框中改变文本框中的文本框颜色
            txtWhere.ForeColor = Color.Black;
        }
        #endregion


        //在控件上更改Text属性的值时引发的事件
        private void txtwhere_TextChanged(object sender, EventArgs e)
        {
            txtWhere.Tag = "0";
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
