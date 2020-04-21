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

namespace 后台SuperMarketManager
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }
        ISuperMarketAdminManager adminManager = new SuperMarketAdminManager();
        /// <summary>
        /// 登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtLoginId.CheckData(@"^\d+$", "账号输入有误！") * txtLoginPwd.CheckNullOrEmpty() != 0)
            {
                //获取窗口管理者的Id和密码
                SysAdminsModel sys = new SysAdminsModel()
                {
                    LoginId = Convert.ToInt32(txtLoginId.Text.Trim()),
                    LoginPwd = txtLoginPwd.Text.Trim()
                };
                try
                {
                    //把窗口的数据Id和pwd放在数据库查找整个管理者数据
                    sys = adminManager.AdminLogin(sys);
                    //写入老师创建的日志
                    Log4net类2Teacher.Info($"账号[{sys.LoginId}]开始登录");
                    if (sys != null)
                    {
                        //sys.AdminStatus当前状态1启0禁
                        if (sys.AdminStatus == 1)
                        {
                            Log4net类2Teacher.Info($"[{sys.LoginId}]登录成功！");
                            //Program主窗口中的属性,接收数据库中的所有数据
                            Program.CurrentAdmin = sys;
                            //并打开这个窗口
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            //写入老师创建的日志
                            Log4net类2Teacher.Info($"[{sys.LoginId}]账号被禁用");
                            MessageBox.Show("当前管理员账号已被禁用！", "登录提示");
                        }
                    }
                    else
                    {
                        //写入老师创建的日志
                        Log4net类2Teacher.Info($"[{sys.LoginId}]账号或密码错误登录失败");
                    }
                }
                catch (Exception ex)
                {
                    /*   throw ex;*/
                    //写入老师创建的日志
                    Log4net类2Teacher.Error($"[{sys.LoginId}]登录发生异常", ex);
                    return;
                }
            }
        }
        /// <summary>
        /// 取消事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
