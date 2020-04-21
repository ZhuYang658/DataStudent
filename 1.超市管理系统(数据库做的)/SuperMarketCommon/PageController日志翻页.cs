using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;//Regex正则表达式

namespace SuperMarketCommon
{
    //枚举类型的设置
    //如果什么都没赋值，则默认为从0开始往上累加
    public enum SoreType
    {
        ASC,     //0
        DESC     //1
    }
    //事件委托
    public delegate void PagerQueryDelegate(int CurrentPageIndex);
    public partial class PageController日志翻页 : UserControl
    {
        public PageController日志翻页()
        {
            InitializeComponent();
            //按钮是禁用状态
            FristBtn.Enabled = false;
            LastBtn.Enabled = false;
            NextBtn.Enabled = false;
            ReturnBtn.Enabled = false;
            UpBtn.Enabled = false;
        }
        //事件委托（PagerQueryDelegate）的声明（ ExceuteSetPageEvent声明的名字）
        public event PagerQueryDelegate ExceuteSetPageEvent;
        /// <summary>
        /// 查询分页的储存过程
        /// </summary>
        public string ProcName { get; set; }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int CurrentPageIndex { get; set; } = 1;
        /// <summary>
        /// （SoreType）顺序查询或者倒序查询
        /// </summary>
        public SoreType SoreType { get; set; } = SoreType.ASC;
        /// <summary>
        /// 总数据条数
        /// </summary>
        public int RecordCount { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 每页的数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 表示一共有多少页数
        /// </summary>
        public int PageCount
        {
            get
            {
                //总数据条数% 每页的数量==0表示一共有多少页数
                if (RecordCount % PageSize == 0)
                {
                    //返回一页有多少条数据
                    return RecordCount / PageSize;
                }
                else
                {
                    //RecordCount % PageSize != 0如果不等于0，则在原有的页数加1页
                    return (RecordCount / PageSize) + 1;
                }
            }
        }
        //当前页码,获取当前页码
        void ShowInfor()
        {
            //获取当页数label：PageIndex.Text
            PageIndex.Text = CurrentPageIndex.ToString();
            //表示一共有多少页数
            lblPageCount.Text = PageCount.ToString();
        }
        /// <summary>
        /// 设置按钮的状态
        /// </summary>
        public void SetButtonEnable()
        {
            //获取当前页码
            ShowInfor();
            //PageCount表示一共有多少页数，表示页数如果<=1时进入
            if (PageCount <= 1)
            {
                //关闭所有的按钮
                FristBtn.Enabled = false;
                LastBtn.Enabled = false;
                NextBtn.Enabled = false;
                ReturnBtn.Enabled = false;
                UpBtn.Enabled = false;
                return;//下面不在执行
            }
            //打开所有按钮
            FristBtn.Enabled = true;
            LastBtn.Enabled = true;
            NextBtn.Enabled = true;
            ReturnBtn.Enabled = true;
            UpBtn.Enabled = true;
           //如果当前页面只有1个
            if (CurrentPageIndex == 1)
            {
                //第一页和上一页关闭按钮
                FristBtn.Enabled = false;
                UpBtn.Enabled = false;
            }
            //当前页码==一共有多少页数
            else if (CurrentPageIndex == PageCount)
            {
                //上一页和最后一页的按钮关闭 
                LastBtn.Enabled = false;
                NextBtn.Enabled = false;
            }
        }
        
        //首次查询的方法
        public void FirstSearh()
        {
            //当前页码
            CurrentPageIndex = 1;
            ExceuteSetPageEvent(CurrentPageIndex);//当前页码把当前页码放在ExceuteSetPageEvent事件委托中
        }
        //上一页按钮
        private void FristBtn_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = 1;
            ExceuteSetPageEvent(CurrentPageIndex);
        }
        //下一页按钮
        private void UpBtn_Click(object sender, EventArgs e)
        {
            CurrentPageIndex--;
            ExceuteSetPageEvent(CurrentPageIndex);
        }
        //下一页按钮
        private void NextBtn_Click(object sender, EventArgs e)
        {
            CurrentPageIndex++;
            ExceuteSetPageEvent(CurrentPageIndex);
        }
        //最后一页按钮
        private void LastBtn_Click(object sender, EventArgs e)
        {
            CurrentPageIndex = PageCount;
            ExceuteSetPageEvent(CurrentPageIndex);
        }
        //跳转按钮
        private void ReturnBtn_Click(object sender, EventArgs e)
        {
            //表示写跳转页面的数字的文本框
            string input = txtNum.Text.Trim();
            //文本框中的数字>=1时进入
            if (input.Length >= 1)
            {
                //判断文本框中的内容，是否满足条件
                Regex regex = new Regex(@"^[1-9]\d*$");
                if (!regex.IsMatch(input))//表示文本内容是否正确
                {
                    MessageBox.Show("请输入有效数字！", "提示");
                    txtNum.Text = "";
                    return;
                }
                else
                {
                    //获取文本框中的内容
                    int pageIndex = Convert.ToInt32(input);
                    //文本框的数字不能超过总页数的数字
                    if (pageIndex > PageCount)
                    {
                        MessageBox.Show($"最大页数不能超出{PageCount}", "提示");
                        return;
                    }
                    else
                    {
                        //当前页码
                        CurrentPageIndex = pageIndex;
                        ExceuteSetPageEvent(CurrentPageIndex);//把当前跳转的页码给事件委托
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
}
