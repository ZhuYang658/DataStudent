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
using StudentMangerBLL;//业务逻辑层
using StudentManagerModel;//实体层

namespace 项目_SMDB_学生管理系统_知识点总结
{
    /// <summary>
    /// AdminADDfrom.xaml 的交互逻辑
    /// </summary>
    public partial class AdminADDfrom : Window
    {
        public AdminADDfrom()
        {
            InitializeComponent();
        }
        AdminsManager manager = new AdminsManager();
        private void yanzheng_Click(object sender, RoutedEventArgs e)
        {
            Admins admins = new Admins();
            admins.AdminName = nametxt.Text;
            admins.loginPwd = mima.Text;
            manager.GetAdminADD(admins);
        }
    }
}
