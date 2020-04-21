using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;//计时器的命名空间，主要用来获取当前系统时间
using System.Configuration;//导入配置文件命名空间,把配置文件的东西要引用在这里
using System.IO;//创建IO流命名控件
using System.Windows.Forms;//获取里面的功能所以要设置using System.Windows.Forms命名空间，现在引用里添加using System.Windows.Forms这个类
using StudentManagerModel.ObjExt;//获取组合的学生表

namespace 项目_SMDB_学生管理系统_知识点总结
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //这里面也可以获取密码和账号，还有只要是Admins的里面的属性都可以获取到，所以就可以获取管理员的名称了
            InitializeComponent();
            //登录窗口验证,如果登录成功,关闭FrmLogin打开MainWindow窗口
            FrmLogin login = new FrmLogin();
            //主窗口打开时加载出登录页面
            login.ShowDialog();//ShowDialog这是一个Bool如果true表示可以获取到值，true则关闭，false整个关闭

            if (login.DialogResult!=true)//如果是false直接关闭真=整个页面
            {
                //终止次进程，并退出代码返回到操作系统
                Environment.Exit(0);
            }

            //再App.Xaml这里面拿到密码和账号

            //MessageBox.Show("欢迎"+App.AppxamlAdmins.AdminName+"登录成功");

            //实例化计时器
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
            try
            {
                //登录时那个老师显示的名字，显示在主页面
                HY.Content = string.Format("欢迎{0}来到管理系统",App.AppxamlAdmins.AdminName);

                //拿到登录后管理员的名称，statusAdminLb管理员文本的名字
                statusAdminLb.Content = "操作管理员【"+App.AppxamlAdmins.AdminName+"】";

                //statusVersionLb版本号的名字，拿到版本的文本那一栏,版本号这个应在App.config配置文件中写方便更改,再这里使用先再引用里导入配置文件，然后给命名空间加配置文件
                statusVersionLb.Content = "版本号：" + ConfigurationManager.AppSettings["version"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        private void Timer_Tick(object sender, EventArgs e)
        {
            //先要获取系统的年
            DateTime now = DateTime.Now;
            string week = "星期天";
            switch (now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    week = "星期天";
                    break;
                case DayOfWeek.Monday:
                    week = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    week = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    week = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    week = "星期四";
                    break;
                case DayOfWeek.Friday:
                    week = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    week = "星期六";
                    break;
                default:
                    break;
            }
            //statusTimeLb状态栏中最右面装日期时间的Lable名字
            statusTimeLb.Content = string.Format("{0}年{1}月{2}日  {3}:{4}{5}",now.Year,now.Month,now.Day,now.Hour,now.Minute,now.Second,week);
           
        }
        //声明计时器
        DispatcherTimer timer = null;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BG.Background = Brushes.Pink;
        }
        /// <summary>
        /// 主窗口的关闭X标志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //终止次进程，并退出代码返回到操作系统
            Environment.Exit(0);
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        /// <summary>
        /// 系统中的修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatePwd_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 系统中的推出系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 学员管理中的添加学员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addsMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 学员管理中的信息管理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smMenu_Click(object sender, RoutedEventArgs e)
        {
            //1.如果添加学员管理中的信息管理先创建一个窗口再View里面添加一个页面
            //2.把原有的主窗口内面的字体清除
            //3.把View里面的页面放在这里显示
            //4.View文件中的WPF窗口如何显示：右键---点击--->添加---找--->用户控件(WPF)--就可以添加WPF窗口

            Gird_Content.Children.Clear();
            View.FrmStuManager frmStudent = new View.FrmStuManager();
            Gird_Content.Children.Add(frmStudent);//把实力化的WPF窗口添加到Gird_Content页面中
        }
        /// <summary>
        /// 成绩管理中的成绩录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void writesMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 成绩管理中的成绩分析
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checksMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 成绩管理中的成绩查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectsMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 考勤管理中的考勤查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void queryaMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 考勤管理中的考勤打卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adakaMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 帮助中的在线问答
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void lianxMenu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 退出系统按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tui_Click(object sender, RoutedEventArgs e)
        {
            //终止次进程，并退出代码返回到操作系统
            Environment.Exit(0);
        }
        /// <summary>
        /// 访问官网
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inlinhMenu_Click(object sender, RoutedEventArgs e)
        {
            //有两种方式
            //方法一：
            //1.首先再View添加用户控件(WPF),创建好后再Grid里面在工具箱里找WebBrowser控件，然后再Source里写网站地址
            //2.再这里先清除主页文字
            //3.再这里把链接的地址窗口实例化
            //4.添加到主页面里
            //5.这个网址一定要访问通
            /* Gird_Content.Children.Clear();
             View.FrmeWebBrowser frmeWeb = new View.FrmeWebBrowser();
             Gird_Content.Children.Add(frmeWeb);*/

            //法二：
            //1.可以把官网写到配置文件里(App.config)
            //2.在配置文件中写入网址
            //3.在要链接的网址中引用配置文件的类
            //4.在FrmeWebBrowser使用配置文件里面你添加的网址
            //5.通过WebBrowser工具名字的uir路径点出配置文件中含有网址的路径
            Gird_Content.Children.Clear();
            View.FrmeWebBrowser frmeWeb = new View.FrmeWebBrowser();
            Gird_Content.Children.Add(frmeWeb);

            //法三:
            //System.Diagnostics.Process.Start("explorer.exe",ConfigurationManager.AppSettings["webadd"].ToString());
            //explorer.exe只要在本机里面时.exe文件都可以被启动

        }
        
        /// <summary>
        /// 自定义图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZiDing_Click(object sender, RoutedEventArgs e)
        {
            //打开一个文件,因为打开这个文件只有
            OpenFileDialog open = new OpenFileDialog();
            //筛选的位置是打开一个文件的最文件对应的后面文件,只能筛选这几个
            open.Filter = "(*.png,*.jpg,*.jpeg,*.bmp)|*.png;*.jpg;*.jpeg;*.bmp;";
            open.Title = "请选择背景图";//给一个提示
            //给一个弹出框,弹框弹出,给一个选项
            DialogResult tan = open.ShowDialog();
            if (tan == System.Windows.Forms.DialogResult.OK)
            {
                string bj = open.FileName;//把筛选的名字记录给一个变量
                ZDBJ(bj);//b背景的路径+名字传递进去
            }
        }
        //森林背景
        private void senlin_Click(object sender, RoutedEventArgs e)
        {
            ZDBJ("img1\\bg\\bg3.jpg");
        }
        //天空背景
        private void Tiankong_Click(object sender, RoutedEventArgs e)
        {
            ZDBJ("img1\\bg\\bg2.jpg");
        }
        //火海背景
        private void xuanzhong_Click(object sender, RoutedEventArgs e)
        {
            ZDBJ("img1\\bg\\bg4.jpg");
        }
        //获取图片路径
        private void ZDBJ(string BJ)
        {
            Gird_Content.Children.Clear();//每次先清除原有的就不会叠加
            //创建一个存图的
            Image image = new Image();
            //给一个路径添加图路径，BJ表示图片的路径
            image.Source = new BitmapImage(new Uri(BJ, UriKind.RelativeOrAbsolute));
            image.Stretch = Stretch.Fill;
            Gird_Content.Children.Add(image);
        }
        /// <summary>
        /// 键盘的按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                //一下IsSubmenuOpen这个属性只有在Menu标签里有
                if (e.Key == Key.Escape)
                {
                    this.WindowState = WindowState.Minimized;
                }
                else if (e.Key == Key.S)
                {
                    //菜单的子菜单是否打开
                    itemS.IsSubmenuOpen = true;
                }
                else if (e.Key == Key.M)
                {
                    //菜单的子菜单是否打开
                    itemM.IsSubmenuOpen = true;
                }
                else if (e.Key == Key.J)
                {
                    itemJ.IsSubmenuOpen = true;
                }
                else if (e.Key == Key.A)
                {
                    itemA.IsSubmenuOpen = true;
                }
                else if (e.Key == Key.H)
                {
                    itemH.IsSubmenuOpen = true;
                }
                else if (e.Key == Key.Q)
                {
                    QeiHuan_Click(null, null);
                }
                else if (e.Key == Key.W)
                {
                    inlinhMenu_Click(null, null);
                }
                else if (e.Key == Key.T)
                {
                    Tui_Click(null, null);
                }
                else if (e.Key == Key.D)
                {
                    //ZiDing.IsSubmenuOpen = true;
                    DaoRuData_Click(null, null);
                }
                else if (e.Key == Key.F)
                {
                    Tupian.IsSubmenuOpen = true;
                }
                else if (e.Key == Key.E)
                {
                    DaoRuData_Click(null, null);
                }
                else if (e.Key == Key.X)
                {
                    AddStu_Click(null, null);
                }
                else if (e.Key == Key.C)
                {
                    ChengjiCX_Click(null, null);
                }
                else if (e.Key==Key.K) 
                {
                    Button_Click(null,null);
                }
                else if (e.Key==Key.N)
                {
                    Button_Click_1(null, null);
                }
                else if (e.Key==Key.M)
                {
                    Button_Click_2(null,null);
                }
                else if (e.Key == Key.Z)
                {
                    //打开学生信息栏
                    smMenu_Click(null, null);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 切换账号的button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QeiHuan_Click(object sender, RoutedEventArgs e)
        {
            //先清除网页的内容
            statusAdminLb.Content = "";//登录者清空
            statusVersionLb.Content = "";//版本清空
            HY.Content = "";//"欢迎{0}来到管理系统"清空

           

            //下面是从新打开一个页面，从新登录
            //登录窗口验证,如果登录成功,关闭FrmLogin打开MainWindow窗口
            FrmLogin login = new FrmLogin();
            //主窗口打开时加载出登录页面
            login.ShowDialog();//ShowDialog这是一个Bool如果true表示可以获取到值，true则关闭，false整个关闭

            if (login.DialogResult != true)//如果是false直接关闭真=整个页面
            {
                //终止次进程，并退出代码返回到操作系统
                Environment.Exit(0);
            }
            try
            {
                //登录时那个老师显示的名字，显示在主页面
                HY.Content = string.Format("欢迎{0}来到管理系统", App.AppxamlAdmins.AdminName);

                //拿到登录后管理员的名称，statusAdminLb管理员文本的名字
                statusAdminLb.Content = "操作管理员【" + App.AppxamlAdmins.AdminName + "】";

                //statusVersionLb版本号的名字，拿到版本的文本那一栏,版本号这个应在App.config配置文件中写方便更改,再这里使用先再引用里导入配置文件，然后给命名空间加配置文件
                statusVersionLb.Content = "版本号：" + ConfigurationManager.AppSettings["version"].ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 添加学生(自己做的)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddStu_Click(object sender, RoutedEventArgs e)
        {
            View.ADDstudent aDD = new View.ADDstudent();
             aDD.ShowDialog();
        }


        /// <summary>
        /// 批量导入：从Excel库导入到DaoRuDataWPF这个窗体的DataGrid这个表中，和从Excel库导入到DaoRuDataWPF这个窗体的DataGrid这个表中在从这个表中拿出数据放在数据库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaoRuData_Click(object sender, RoutedEventArgs e)
        {
            View.DaoRuDataWPF wPF = new View.DaoRuDataWPF();
            wPF.ShowDialog();
        }
        /// <summary>
        /// 成绩查询（自己做的）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChengjiCX_Click(object sender, RoutedEventArgs e)
        {
            vview.CXScroeList cXScroe = new vview.CXScroeList();
            cXScroe.ShowDialog();
        }
        /// <summary>
        /// 考勤打卡（K）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Gird_Content.Children.Clear();
            vview.KaoQinDaKa kaoQin = new vview.KaoQinDaKa();
            Gird_Content.Children.Add(kaoQin);
        }
        /// <summary>
        /// 考勤查询（N）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 成绩分析（M）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
