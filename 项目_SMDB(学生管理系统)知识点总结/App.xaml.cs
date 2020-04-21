using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using StudentManagerModel;//先导入这个命名空间，获取Admins

namespace 项目_SMDB_学生管理系统_知识点总结
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //先执行这个程序App.XML里面的，用一个属性把获取到的数据装在里面，方便引用另一个框的密码和账号
        public static Admins AppxamlAdmins { get; set; }
    }
}
