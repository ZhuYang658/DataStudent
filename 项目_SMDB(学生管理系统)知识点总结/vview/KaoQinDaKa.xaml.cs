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
using StudentManagerModel;
using StudentMangerBLL;
using StudentManagerModel.ObjExt;

namespace 项目_SMDB_学生管理系统_知识点总结.vview
{
    /// <summary>
    /// KaoQinDaKa.xaml 的交互逻辑
    /// </summary>
    public partial class KaoQinDaKa : UserControl
    {
        StudentsManager Manager = new StudentsManager();
        List<Attendance> att = null;
        List<KaoQinAndStu> kaoQins = null;
        public KaoQinDaKa()
        {
            InitializeComponent();
            //下拉框中内容填写
            List<Students> list = Manager.GetKaHaoCombox();
            KAhao.ItemsSource = list;
            //设置放在某个具体的对象上，可视表示设置再combox上面可以看见的,要显示ClassName的属性
            KAhao.DisplayMemberPath = "CardNo";//设置下拉框显示文本
            KAhao.SelectedValuePath = "CardNo";
            /*KAhao.SelectedIndex = 0;//并获取索引为0的选项

            //获取当选中某个科目时选中学生ID值对应显示绑定
            students = Manager.GetKaHaoAddStudent(KAhao.SelectedItem.ToString());
            //DataGrid进行数据绑定，DataGrid的名字叫smDgStudentLsit
            DataGridKC.ItemsSource = students;//把获取到的路径students赋给DataGrid表里*/
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //没有close关闭，只有窗口才能进行打开和关闭
            //Hidden(保留控件)和Visible(不保留空间)
            this.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 打卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaKa_Click(object sender, RoutedEventArgs e)
        {
            //有问题
            int a= Manager.GetKaHaoCombox(KAhao.SelectedValue.ToString());
            if (a>0)
            {
                MessageBox.Show("添加成功");
            }
            kaoQins = Manager.GetKaHaoAddStudent(KAhao.SelectedValue.ToString());
            //DataGrid进行数据绑定，DataGrid的名字叫smDgStudentLsit
            DataGridKC.ItemsSource = kaoQins;//把获取到的路径students赋给DataGrid表里

            Manager.GetHuiFu();
        }
        /// <summary>
        /// 查询时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CXtime_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 卡号排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kahaopaixu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 学号排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Xuehaopaixu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaoChu_Click(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 打印DateGrid的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaYin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
