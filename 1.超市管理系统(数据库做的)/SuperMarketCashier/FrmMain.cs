using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMarketBLL;
using SuperMarketIBLL;
using SuperMarketCommon;
using System.Configuration;
using SuperMarketModel;
using SuperMarketIBLL.前台SuperMarketCashier;
using SuperMarketBLL.前台SuperMarketCashier;


namespace SuperMarketCashier
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            
            InitializeComponent();
            //关闭窗口前一刻触发（作用：写入数据库的日志里面）
            this.FormClosing += FrmMain_FormClosing;
            this.FormBorderStyle = FormBorderStyle.None;//设置窗口边框
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;//不能手动调整边框大小
            //this.StartPosition = FormStartPosition.CenterScreen;//窗口居中

            //【必须初始化】
            //初始化log4net【重点：必须初始化不然获取不到Info和Error路径地址】
            log4net.Config.XmlConfigurator.Configure();

            toollblName.Text = $"收银员【{Program.Seles.SPName}】";

            //【开始实现主窗口中：商品销售统计--流水表】
            txtSerialNum.Text = CreateSerialNum();
            //dgvProductList(dataGridView1)里面不自动添加数据，手动添加
            dgvProductList.AutoGenerateColumns = false;

            //系统的核心计算功能主要取决于获取商品的编号
            //获取商品编号
            //a.采用扫描枪扫描条形码  
            //b.无法获得商品条形码要提供手动录入条形码功能  
            //通过按键来实现事件；txtProductId是商品编号对应的文本框
            //焦点在次控件上当按键动时触发
            txtProductId.KeyDown += TxtProductId_KeyDown;
            //在控件接收焦点时发生
            txtProductId.GotFocus += TxtProductId_GotFocus;
            //在控件失去焦点时发生
            txtProductId.LostFocus += TxtProductId_LostFocus;
            //数量的焦点和失去获取时的事件
            txtQuantity.GotFocus += TxtProductId_GotFocus;
            txtQuantity.LostFocus += TxtProductId_LostFocus;
            //销售单价的焦点和失去获取时的事件
            txtUnitPrice.GotFocus += TxtProductId_GotFocus;
            txtUnitPrice.LostFocus += TxtProductId_LostFocus;
            //折扣的焦点和失去获取时的事件
            txtDiscount.GotFocus += TxtProductId_GotFocus;
            txtDiscount.LostFocus += TxtProductId_LostFocus;

        }
        //实例化IBll调用Bll里面写入数据库日志的方法
        SuperMarketIBLL.前台SuperMarketCashier.ISelesPersonManager manager = new SuperMarketBLL.前台SuperMarketCashier.SelesPersonManager();
        //Product表示的BLL层
        ISuperMarketProductManager manager2 = new SuperMarketProductManager();
        //用泛型装ProdutsModel的属性
        List<ProdutsModel> productList = new List<ProdutsModel>();
        //起一个缓冲作用，用来调解数据源和容器之间的数据变换关系
        BindingSource bs = new BindingSource();
        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Width = 1000;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;
        }



        #region 文本框设计（文本框失去焦点和获取焦点时发生改变）
        /// <summary>
        /// 获取焦点时，文本框发生改变
        /// </summary>
        /// <param name="sender">事件的接收者(谁触发事件,这个焦点上的文本框就是谁)</param>
        /// <param name="e">事件的触发者</param>
        private void TxtProductId_GotFocus(object sender, EventArgs e)
        {
            //实例化自己创建的组件
            SuperTextbox text = sender as SuperTextbox;
            text.BackColor = Color.Pink;
            text.SelectAll();//选定文本框中的所有文本
        }
        /// <summary>
        /// 失去焦点发生(对应的文本框发生改变)
        /// </summary>
        /// <param name="sender">事件的接收者(谁触发事件,这个焦点上的文本框就是谁)</param>
        /// <param name="e">事件的触发者</param>
        private void TxtProductId_LostFocus(object sender, EventArgs e)
        {
            SuperTextbox text = sender as SuperTextbox;
            text.BackColor = Color.Gray;
        }
        #endregion


        #region 系统核心操作模块
        //【二、当系统读取到条形码开始将商品加入购物车】
        //1.如果购物车中没有该商品，在购物车中新建一栏加入
        //2.如果购物车中该商品已经存在了，直接改变购物车中该商品的数量
        private void TxtProductId_KeyDown(object sender, KeyEventArgs e)
        {
            //【1】按下会车进行商品编号的录入，进行商品添加的功能
            //Enter键按下表示检查商品编号对应的商品
            if (e.KeyCode == Keys.Enter)
            {
                //商品添加
                BindProduct();
             }
            //【2】按上移动
            else if (e.KeyCode == Keys.Up)
            {
                if (dgvProductList.RowCount == 0 || dgvProductList.RowCount == 1)
                {
                    return;
                }
                bs.MovePrevious();
            }
            //【3】按下移动
            else if (e.KeyCode == Keys.Down)
            {
                if (dgvProductList.RowCount == 0 || dgvProductList.RowCount == 1)
                {
                    return;
                }
                bs.MoveNext();
            }
            //【4】按下Del键移除
            else if (e.KeyCode == Keys.Delete)
            {
               //结算：如果结算未空，则直接返回return
                if (dgvProductList.RowCount == 0)
                {
                    return;
                }
                bs.RemoveCurrent();
                //问题：1.数据表要刷新
                //问题：2.更新序号
                dgvProductList.DataSource = null;
                dgvProductList.DataSource = bs;
            }
            //【5】按下的是F1键开始结算
            else if (e.KeyCode == Keys.F1)
            {
                //如果按F1在主页面没有的RowCount行数==0，表示没有行数直接返回
                if (dgvProductList.RowCount == 0)
                {
                    return;
                }
                else
                {
                    //购物结算方法【一】
                    Balance();
                }
            }
            //【6】按下的是退出系统按键
            else if (e.KeyCode == Keys.Escape)
            {
                if (MessageBox.Show("是否确认关闭系统", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
        #endregion


        #region 结算【一】
        //【1.1】到【1.5】先搭建构架;还没有实现存储过程
        //【1.1】到【1.4】取消结算
        //【1.5】到【1.20】正式开始结算（[包含了客户卖东西的数据库中的表]和[收银员记录客户消费具体的信息表在数据库中显示]和[VIP会员的积分记录]）
        private void Balance()
        {
            //【1.1】
            //显示结算窗口，考虑支付被取消或修改，并传递txtPay应付多少钱文本框
            FrmBalance frm = new FrmBalance(txtPay.Text);
            //【1.2】
            //如果打开FrmBalance不成功进入
            if (frm.ShowDialog() != DialogResult.OK)
            {
                //客户放弃购买(忘记带钱等)；在购买界面时键盘上按了Escape按钮，给Tag传了一个Tag数据，则frm.Tag.ToString() == "Esc"现在相等，进入if语句
                //【1.3】
                if (frm.Tag.ToString() == "Esc")
                {
                    //【1.4】
                    //支付完成重置界面:就是把原有的界面里面的商品清空，从新开始
                    RestForm();//【二】弄完后进入【三.1】FrmBalance窗口里
                }
            }
            //【1.5】
            else//正式进入结算环节
            {
                //【1.6】
                //实例化会员类
                SMMembersModel10 members = null;
                //Contains("&")表示&这个字符是否出现在此字符串中
                //【1.7】
                //获取含有&tag值
                if (frm.Tag.ToString().Contains("&"))//输入了会员卡号
                {
                    //【1.8】
                    //把含有&的Tag值,分割两部分,用string数组记录
                    //这两部分分别是【客户实际付的钱数】和【VIP的编号】
                    string[] info = frm.Tag.ToString().Split('&');
                    //【1.9】【客户实际付的钱数】
                    txtAmount.Text = info[0];
                    //【1.10】
                    members = new SMMembersModel10()
                    {
                        //【1.11】【VIP的编号】
                        MemberId = info[1],
                        //【1.12】给具体购买的商品的总金额/10
                        Points = (int)(Convert.ToDouble(txtPay.Text) / 10.0)
                    };
                }
                else
                {
                    //【1.13】没有会员，直接获取【客户实际付的钱数】
                    txtAmount.Text = frm.Tag.ToString();
                }
                //【1.14】
                //找零
                //【客户实际付的钱数】-【具体购买的商品的总金额】
                txtChange.Text = (Convert.ToDecimal(txtAmount.Text) - Convert.ToDecimal(txtPay.Text)).ToString();
                /*txtChange.Text = (Convert.ToDecimal(txtPay.Text) - Convert.ToDecimal(txtAmount.Text)).ToString();*/
                //【1.15】上面弄完后，saleObj的每个数据就会附上具体的数据
                //创建消费对象
                SalesListModel1 saleObj = new SalesListModel1()
                {
                    SerialNum = txtSerialNum.Text,
                    TotalMoney = Convert.ToDecimal(txtPay.Text.Trim()),
                    RealReceive = Convert.ToDecimal(txtAmount.Text.Trim()),
                    ReturnMoney = Convert.ToDecimal(txtChange.Text.Trim()),
                    SalesPersonId = Program.Seles.SalesPersonId
                };
                //【1.16】
                //封装消费明细列表
                //productList放商品的具体数据盒子
                foreach (ProdutsModel item in productList)
                {
                    //【1.17】
                    //给saleObj里面的ListDetail属性里面添加方法
                    //ListDetail就是SalesListDetailModel的接收容器
                    //给 saleObj.ListDetail.Add里面添加new SalesListDetailModel(){}的原因是saleObj.ListDetail这个方法接收的是SalesListDetailModel类型的泛型所以用new SalesListDetailModel(){}添加
                    saleObj.ListDetail.Add(
                        //【1.18】
                        new SalesListDetailModel()
                        {
                            //把商品列表中的所有信息给了SalesListDetailModel类里面
                            SerialNum = txtSerialNum.Text,
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            Discount = item.Discount,
                            SubTotalMoney = item.SubTotal
                        }
                    );
                }
                try
                {
                    //【1.19】
                    /// <summary>
                    /// SaveSalerInfo：
                    /// 【1】AddSalesList销售主表：以发票的形式记录消费的客户的小票数据，放在SalesList表里面记录客户的买的东西
                    /// 【2】AddSalesListDetail商品销售统计表：就是顾客买的东西记录一下，放在AddSalesListDetail表里面记录收银员的给客户卖东西是的账本
                    /// 【3】RefreshMemberPoint修改会员积分：如果有会员则修改会员积分
                    /// </summary>
                    //把结算的内容相互通过数据库事务相互牵连
                    //saleObj（包含了在主窗口中文本框中的所有数据，还获取了ListDetail里面添加的所有数据）获取所有的商品表并数据传递到数据库里
                    //members把会员所需要的数据传递到数据库里
                    manager2.SaveSalerInfo(saleObj, members);
                }
                //【1.20】
                catch (Exception ex)
                {
                    MessageBox.Show($"保存销售数据的时候发生异常！{ex.Message}", "异常提示");
                    return;
                }
                //小票打印
                //【1】在工具箱里获取打印工具printDocument（printDocument打印工具的名字）
                //【3】Print开始文档的打印进程
                printDocument.Print();
                //【4】小票打印预览
                PrintPriview();

                //重置主界面
                RestForm();
            }
        }
        #endregion


        #region 支付完成重置界面
        void RestForm()
        {
            txtSerialNum.Text = CreateSerialNum();
            dgvProductList.DataSource = null;
            productList.Clear();
            txtPay.Text = "0.00";
            txtProductId.Focus();
        }
        #endregion



        #region 商品添加
        private void BindProduct()
        {
            //商品编号正确(把换行符号改成空字符串：原因是获取时会包含换行符)
            txtProductId.Text = txtProductId.Text.Replace("\r\n", "");
            //商品编号正确格式的验证
            if (txtProductId.CheckData(@"^[1-9]\d*$", "商品编号为10-15纯数字") != 0)
            {

                //【1】查询对应的商品的详细信息
                //【2】检查这个商品是否已经在购物车中存在了，如果已经有了这个商品则直接添加数量，如果没有才往购物车中添加
                //【3】product记录获取的值，在productList中获取，Id对应的值 
                //【4】p.ProductId.Equals(txtProductId.Text.Trim())值并确定这个Id是否有相同的值
                //【5】product接收这个值
                var product = from p in productList
                              where p.ProductId.Equals(txtProductId.Text.Trim())
                              select p;
                //在购物车中未找到该商品，则进行重新添加
                if (product.Count() == 0)//product.Count()泛型里面的内容总数
                {
                    //第一次添加数据库数据，肯定dgvProductList(dataGridView1)没有任何值，所以第一次添加数据进入这里AddNewProductToList，productList就会有数据了
                    //【通过商品表的Id获取表中的内容在dgvProductList(dataGridView1)框里面显示】
                    AddNewProductToList();
                }
                else//商品已经存在，则只需要更新数量和小计金额即可
                {
                    //product如果有多个Id相同的商品，只返回序列的第一个值
                    ProdutsModel pro = product.FirstOrDefault();
                    //并在Quantity商品数量对应的文本框累加1，赋给pro.Quantity里面，现在productList里面就有值了
                    pro.Quantity += Convert.ToInt32(txtQuantity.Text.Trim());
                    //pro.SubTotal记录总钱数（已有的数量*单价）
                    pro.SubTotal = pro.Quantity * pro.UnitPrice;
                    //打折的位置是否为空
                    if (pro.Discount != 0)
                    {
                        //不为空进来打折，把打折后的钱赋给 pro.SubTotal记录总钱数
                        pro.SubTotal *= (Convert.ToDecimal(pro.Discount) / 10);
                    }
                }
                //【3】整个商品加入购物车完成，刷新界面显示
                //先把productList数据放在BindingSource缓冲容器里面，防止直接删除数据报错
                bs.DataSource = productList;
                //清空原来的数据，方便下一次添加
                dgvProductList.DataSource = null;
                //从新添加数据
                dgvProductList.DataSource = bs;


                //【重点：下面的4、5表示给主窗口添加商品数据后全部恢复初始化】
                //【4】更新购物车的应付金额数量
                txtPay.Text = (from p in productList select p.SubTotal).Sum().ToString();

                //【5】清空商品的相关信息
                txtProductId.Text = "";
                txtQuantity.Text = "1";
                txtDiscount.Text = "0";
                txtUnitPrice.Text = "0.00";
                txtAmount.Text = "0.00";
                txtChange.Text = "0.00";
                txtProductId.Focus();
            }
            else
            {
                MessageBox.Show("商品编号为10-15纯数字", "错误编号");
            }
        }
        #endregion


       
        #region 添加商品到购物车列表
        /// <summary>
        /// 【通过商品表的Id获取表中的内容在dgvProductList(dataGridView1)框里面显示】
        /// </summary>
        private void AddNewProductToList()
        {
            //【一】根据商品编号查询商品
            //【1.在数据库中根据商品编号查询商品】
            //objProduct就拿到到所有的商品表内容
            ProdutsModel objProduct = manager2.GetProductWithId(txtProductId.Text.Trim());
            //【2】检查商品是否未查到，如果没有商品临时创建一个商品 
            //objProduct如果没有拿到值，则进入
            if (objProduct == null)//商品编号未查到对应的商品信息，要么是临时商品
            {
                //【3】假设是临时商品
                objProduct = new ProdutsModel()
                {
                    ProductName = "暂未提供商品名称",//商品名字
                    ProductId = txtProductId.Text.Trim(),//商品ID
                    UnitPrice = Convert.ToDecimal(txtUnitPrice.Text.Trim()),//商品单价
                    Discount = Convert.ToInt32(txtDiscount.Text.Trim())//商品折扣
                };
            }
            //【3】扫到的商品（就是数据库里面通过Id查到商品）
            else
            {
                //把从数据库获取的， 内容给窗口上的文本框
                txtUnitPrice.Text = objProduct.UnitPrice.ToString();
                txtDiscount.Text = objProduct.Discount.ToString();
            }
            //【5】根据商品的数量折扣计算小计金额
            objProduct.Quantity = Convert.ToInt32(txtQuantity.Text.Trim());
            objProduct.SubTotal = objProduct.Quantity * objProduct.UnitPrice;
            //【6】如果这个商品有折扣
            if (objProduct.Discount != 0)
            {
                objProduct.SubTotal *= (Convert.ToDecimal(objProduct.Discount) / 10);
            }
            //【7】商品列表序号问题(每次添加序号改变，增加一个)
            objProduct.ProductNo = productList.Count + 1;
            productList.Add(objProduct);
            //【8】添加商品之后应该立刻让最新添加的商品作为选中项
            //bs是缓冲容器的名字，MoveLast()是获取移至列表中的最后一项
            bs.MoveLast();
        }
        #endregion


        #region 生成流水账号
        //获取流水号//生成流水账号
        private string CreateSerialNum()//返回一个str组合值
        {
            string str= manager.GetSysTime().ToString("yyyyMMddHHmmssms");
            //随机一个数
            Random random = new Random();
            //时间拼接随机(10-100;不包含100)数
            str += random.Next(10, 100).ToString();
            return str;
        }
        #endregion
        //写入数据日志里面
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            manager.BllWriteSelesExitLog(Program.Seles.LogId);
            //写入日志
            Log4net类.WriteInfo(string.Format("" + Program.Seles.LogId + ""));
        }
        //获取电脑系统时间
        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel7.Text = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }


        #region 小票打印预览【4】{没认真了解}
        public void PrintPriview()
        {
            try
            {
                var printPriview = new PrintPreviewDialog
                {
                    Document = printDocument,
                    WindowState = FormWindowState.Maximized
                };
                printPriview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("打印错误，请检查打印设置！");
            }
        }
        #endregion

        #region 小票事件的实现（每一次触发都要对于打印的每一页发生事件）
        //【2】在工具里面找打印printDocument1工具，在事件里 PrintPage表示（对于要打印的每一页发生一次）
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //【2.1】创建USBPrint
            //【2.2】调用USBPrint里面的Print方法
            //【2.3】productList, txtSerialNum.Text.Trim(), toollblName.Text.Trim()都是要打印的内容
            USBPrint.Print(e, productList, txtSerialNum.Text.Trim(), toollblName.Text.Trim());
        }
        #endregion
    }
}
