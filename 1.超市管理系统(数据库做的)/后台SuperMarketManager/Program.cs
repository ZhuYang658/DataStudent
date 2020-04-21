using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;//Process禁止程序多次启动
using SuperMarketModel;

namespace 后台SuperMarketManager
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //【一：登录获取窗口】
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //禁止程序多次启动，一个程序运行一个进程，当this.Close()主线程关闭,而进程并没有彻底关闭，然而重启是由主线程中的一个方法支持，继续启动了一个重复的线程
            Process[] processList = Process.GetProcesses();
            int currentCount = 0;
            foreach (Process item in processList)
            {
                if (item.ProcessName == Process.GetCurrentProcess().ProcessName)
                {
                    currentCount += 1;
                }
            }
            if (currentCount > 1)
            {
                Application.Exit();
                return;
            }
            //登录者登录密码的窗口
            FrmLogin frm = new FrmLogin();
            DialogResult result = frm.ShowDialog();
            if (result == DialogResult.OK)
            {
                frm.DialogResult = DialogResult.No;
                Application.Run(new FrmMain());
            }
            else
            {
                //停止调用
                Application.Exit();
            }
        }
        /// <summary>
        /// 管理者的登录：把管理者的登录内容记录一下
        /// </summary>
        public static SysAdminsModel CurrentAdmin { get; set; } = null;
        /// <summary>
        /// 商品分类表
        /// </summary>
        public static ProductCategoryModel ClockCategory { get; set; } = null;
    }
}
