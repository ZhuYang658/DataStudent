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
using System.Windows.Shapes;
using StudentManagerModel;//1.一定切记要把StudentManagerModel任意类(一个也行)改成public,这样才会显示StudentManagerModel这个类名否则一定不会显示
using Common;//2.验证类,用于检验格子里的内容是否规范
using StudentMangerBLL;//3.业务逻辑层

namespace 项目_SMDB_学生管理系统_知识点总结
{
    /// <summary>
    /// FrmLogin.xaml 的交互逻辑
    /// </summary>
    public partial class FrmLogin : Window
    {
        public FrmLogin()
        {
            InitializeComponent();
            //给账号那文本框获取焦点
            txtLogId.Focus();
        }
        //推出程序
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //推出进程,此处进程已经完成，终止线程
            Environment.Exit(0);
        }
        //最小化窗口
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            //最小化窗口
            this.WindowState = WindowState.Minimized;
        }
        //登录
        private void DL_Click(object sender, RoutedEventArgs e)
        {
            //1.对账号的文本框(txtLogId)数据验证
            if (txtLogId.Text.Trim().Length==0)//在这个文本框中如果输入的账号长度0，则这个文本框为空
            {
                MessageBox.Show("请输入登录账号","登录提示!");
                //让文本框获取焦点
                txtLogId.Focus();
                return;
            }
            //3.用正则表达式判断文本框是否满足，不满足进入下面的if里面
            if (DataValidate.Isinteger(txtLogId.Text.Trim())==false)//调用这个类的Isinteger这个方法表示验证某个文本框是否为正整数
            {
                MessageBox.Show("请输入正确账号！","登录提示");
                txtLogId.Focus();
                return;
            }
            //2.密码验证
            if (txtLogPwd.Password.Length==0)
            {
                MessageBox.Show("请输入登录密码", "登录提示!");
                //获取焦点
                txtLogPwd.Focus();
                return;
            }

            #region//第一种登录方式
            //首先调用三层
            //1.先给命名空间添加StudentManagerModel类，在StudentManagerModel里面获取到Admins这个类，
            //（1.）创建/实例化Admins登录的表格
            Admins admins = new Admins()
            {
                //2.在LoginId 这个之前一定要用if验证txtLogId.Text里面的东西是数字，验证Common里面的DataValidate,所以又要在命名空间加Common类
                LoginId = Convert.ToInt32(txtLogId.Text.Trim()),
                loginPwd = txtLogPwd.Password
                //上面把输入的账号密码给了Admins对应的属性，保证这两个数一致
            };

            //和后台交互查询，判断登录信息是否正确
            //3导入业务逻辑层BLL，再引用里加BLL层
            try
            {
                //GetAdmins(admins)表示检测对象拿到，看里面的值是否正确；要把admins1传给主界面MainWindow
                Admins admins1 = new AdminsManager().GetAdmins(admins);
                //admins1真正的数据库中实体对象拿到了
                if (admins1!=null)//登录信息不能为空进来，就是登录成功，进入后对登录信息的保存
                {
                    //保存登录信息，把密码和账号赋给AppxamlAdmins属性
                    App.AppxamlAdmins = admins1;//用admins1这个原因是把admins1获取的值赋给AppxamlAdmins，表示AppxamlAdmins不为空不为空后this.DialogResult = true;//这个属性为ture时关闭这个窗口
                    //密码和账号都拿到了，this.DialogResult = true，给他赋一个true值
                    this.DialogResult = true;//这个属性为ture时给他一个判断
                    this.Close();//关闭这个窗口
                }
                else
                {
                    MessageBox.Show("用户账号或秘密错误","提示信息");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("数据库异常","提示信息");
                throw;
            }
            #endregion
        }

        AdminsManager Manager = new AdminsManager();
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZC_Click(object sender, RoutedEventArgs e)
        {
            //两个框都为空时，点击一下，把对应的lable改变名称
            if (txtLogPwd.Password==""&& txtLogId.Text=="")
            {
                nameZC.Content = "注册名：";
                pwdZC.Content = "注册密码";
                MessageBox.Show("转成注册界面");
                return;
            }
            Admins admins = new Admins();
            admins.loginPwd = txtLogPwd.Password;
            admins.AdminName = txtLogId.Text;
            Manager.AddAdmins(admins);
            MessageBox.Show("注册成功");
            nameZC.Content = "登录账号：";
            pwdZC.Content = "登录密码：";
        }
    }
}
