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
using System.Configuration;//在这里用配置文件的原因是,我把链接地址放在配置文件了

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// FrmeWebBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class FrmeWebBrowser : UserControl
    {
        public FrmeWebBrowser()
        {
            InitializeComponent();
            web.Source = new Uri(ConfigurationManager.AppSettings["webadd"].ToString());
        }
    }
}
