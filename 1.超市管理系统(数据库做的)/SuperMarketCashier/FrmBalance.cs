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

namespace SuperMarketCashier
{
    public partial class FrmBalance : Form
    {
        //传递进来钱数
        public FrmBalance(string totalMoney)//【三.2】
        {
            InitializeComponent();
            //显示购买人员购买的商品的总价钱 
            //获取购买商品的所有的钱数
            txtPay.Text = totalMoney;//【三.3】
            //获取窗口的位置
            this.StartPosition = FormStartPosition.CenterScreen;

            txtVip.GotFocus += TxtVip_GotFocus;
            txtVip.LostFocus += TxtVip_LostFocus;
            txtAmount.GotFocus += TxtVip_GotFocus;
            txtAmount.LostFocus += TxtVip_LostFocus;
        }
        ISuperMarketMemberManager3 memberManager = new SuperMarketMemberManager3();


        private void FrmBalance_Load_1(object sender, EventArgs e)
        {
            SuperTextbox txt = sender as SuperTextbox;
            /*txt.BackColor = Color.White;*/
            if (txtAmount.Text.Contains(".") && txtAmount.Text.IndexOf(".") == txtAmount.Text.Length)
            {
                txtAmount.Text += "00";
            }
            else if (!txtAmount.Text.Contains("."))
            {
                txtAmount.Text += ".00";
            }
            txtAmount.Text = Convert.ToDecimal(txtAmount.Text).ToString("F2");
        }

        private void TxtVip_LostFocus(object sender, EventArgs e)
        {
            SuperTextbox txt = sender as SuperTextbox;
            txt.BackColor = Color.White;
            if (txtAmount.Text.Contains(".") && txtAmount.Text.IndexOf(".") == txtAmount.Text.Length)
            {
                txtAmount.Text += "00";
            }
            else if (!txtAmount.Text.Contains("."))
            {
                txtAmount.Text += ".00";
            }
            txtAmount.Text = Convert.ToDecimal(txtAmount.Text).ToString("F2");
        }

        private void TxtVip_GotFocus(object sender, EventArgs e)
        {
            SuperTextbox txt = sender as SuperTextbox;
            txt.BackColor = Color.Cyan;
            txt.SelectAll();
        }

        //【四.1】
        //【1.1】到【1.11】搭建大概矿建还没有显现存储过程
        //【1.12】到【1.22】会员和普通客户给支付界面tag里面放值，方便主窗口获取:客户的收的钱数和Vip数据或只是客户的收的钱数
        /// <summary>
        /// KeyDown首次按下某个键发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtVip_KeyDown(object sender, KeyEventArgs e)
        {
            //换行符进行处理，原因是会获取\r\n，所以用Replace处理\r\n
            txtAmount.Text = txtAmount.Text.Replace("\r\n", "");
            txtVip.Text = txtVip.Text.Replace("\r\n", "");
            //【1.1】
            //【四.2】当按F1时
            if (e.KeyCode == Keys.F1)
            {
                //当按F1时获取‘现金’是否被选中
                radMoney.Checked = true;
            }
            //【1.2】
            //当按下F2时刷卡
            else if (e.KeyCode == Keys.F2)
            {
                //表示该空间被选中
                radCard.Checked = true;
            }
            //【1.3】
            //当按下F2时扫二维码
            else if (e.KeyCode == Keys.F3)
            {
                //表示该空间被选中
                radQRCode.Checked = true;
            }
            //【1.4】
            //回车键：表示正常结算
            else if (e.KeyCode == Keys.Enter)
            {
                //【1.5】
                //txtAmount输入购物人给的钱数,必须满足条件，!=0
                if (txtAmount.CheckData(@"^(([1-9]\d*)|(\d*.\d{0,2}))$", "输入金额有误") != 0)
                {
                    //【1.6】
                    //‘.’符号是否出现过txtAmount文本框中的字符串，和‘.’获取以后的从0开始的索引是否==txtAmount字符串中的长度
                    //如果FrmBalance里面的应付只输入数字后面有.就会自动补上00
                    if (txtAmount.Text.Contains(".") && txtAmount.Text.IndexOf(".") == txtAmount.Text.Length)
                    {
                        //满足上面条件进入，给txtAmount.Text多加00
                        txtAmount.Text += "00";
                    }
                    //【1.7】
                    //如果FrmBalance里面的应付只输入数字后面没有.就会自动补上.00
                    else if (!txtAmount.Text.Contains("."))
                    {
                        txtAmount.Text += ".00";
                    }
                    //把获取的输入钱数转换成ToDecimal类型再给txtAmount.Text里面放
                    txtAmount.Text = Convert.ToDecimal(txtAmount.Text).ToString("F2");

                    //【1.8】
                    //判断不是会员
                    if (txtVip.Text.Length == 0)//0不是会员
                    {
                        //【1.9】
                        //txtVip文本框中的Tag中直接赋txtAmount的实付的钱数
                        this.Tag = txtAmount.Text.Trim();
                    }
                    //【1.10】
                    else//有会员卡号
                    {
                        //【1.12】
                        if (txtVip.CheckData(@"^[1-9]\d*$", "会员卡号有误") != 0)
                        {
                            //【1.13】
                            //进一步判断会员是否正常自己完成
                            //GetMembersById:通过ID获取会员；并接收会员的所有信息
                            SMMembersModel10 members = memberManager.GetMembersById(txtVip.Text.Trim());
                            //【1.14】表示拿到会员
                            if (members != null)
                            {
                                //【检查会员状态】
                                //【1.15】
                                //members.MemberStatus属性表示会员状态
                                //members.MemberStatus == 1表示拿到会员
                                if (members.MemberStatus == 1)
                                {
                                    //用【&】连接(1)和(2)最后用表示框架的tag给里面负(1)和(2)的值
                                    //（1）txtAmount.Text.Trim()拿到应支付的商品总价钱
                                    //（2）txtVip.Text.Trim()拿到客户的VIP编号
                                    this.Tag = $"{txtAmount.Text.Trim()}&{txtVip.Text.Trim()}";
                                }
                                //【1.16】
                                else
                                {
                                    //【1.17】
                                    //会员XXX被冻结！请联系超市相关工作人员！
                                    if (members.MemberStatus == 0)
                                    {
                                        MessageBox.Show($"会员【{txtVip.Text.Trim()}】被冻结！请联系超市相关工作人员！", "提示");
                                    }
                                    //【1.18】
                                    //会员XXX已被注销！
                                    else
                                    {
                                        MessageBox.Show($"会员【{txtVip.Text.Trim()}】已被注销！", "提示");
                                    }
                                    //【1.19】
                                    //清空会员文本框
                                    txtVip.Text = "";
                                    return;
                                }
                            }
                            //【1.20】
                            else
                            {
                                //【1.21】
                                MessageBox.Show($"会员【{txtVip.Text.Trim()}】不存在！请检查会员账号！", "提示");
                                txtVip.Text = "";
                                return;
                            }
                        }
                        //【1.22】
                        else
                        {
                            //没有会员卡，tag直接用txtAmount.Text.Trim()实际收客户的钱数
                            this.Tag = txtAmount.Text.Trim();
                        }
                    }
                    //【1.6】
                    //证明客户付钱够了，比较应付的金额小于等于应付金额；表示客户拿的钱够了，可以付钱
                    if (Convert.ToDecimal(txtPay.Text) <= Convert.ToDecimal(txtAmount.Text))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    ////【1.11】
                    else
                    {
                        MessageBox.Show("客户实际付款金额不足！", "注意");
                    }
                }
            }
            //【1.12】
            else if (e.KeyCode == Keys.Escape)
            {
                //如果键盘上按了Escape按钮，给Tag传了一个Tag数据，这个Tag数据方便给主窗口显示其他的功能
                this.Tag = "Esc";
                this.Close();//关闭这个窗口
            }
            //【1.13】按Tab只让输入钱数和vip账号获取焦点，来回获取焦点
            else if (e.KeyCode == Keys.Tab)
            {
                //按Tab判断文本框的Tag是什么值
                SuperTextbox txt = sender as SuperTextbox;
                //【1.14】
                if (txt.Tag.ToString() == "vip")//如果是vip
                {
                    //获取焦点
                    txtAmount.Focus();
                }
                //【1.15】
                else if (txt.Tag.ToString() == "pay")//如果是pay
                {
                    //获取焦点
                    txtVip.Focus();
                }
            }
        }
    }
}
