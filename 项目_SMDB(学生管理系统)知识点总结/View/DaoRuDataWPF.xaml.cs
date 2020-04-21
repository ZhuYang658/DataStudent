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
using StudentManagerModel.ObjExt;////实体层的某个文件

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// DaoRuDataWPF.xaml 的交互逻辑
    /// </summary>
    public partial class DaoRuDataWPF : Window
    {
        public DaoRuDataWPF()
        {
            InitializeComponent();
        }
        //实例化一下业务逻辑中的学生部分
        StudentsManager manager = new StudentsManager();
        //用一个list泛型装StudentExt的属性
        List<StudentsExt> list = null;
        /// <summary>
        /// 导入到主页面中的DataGrid展示表格中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaoRu_Click(object sender, RoutedEventArgs e)
        {
            //导入Excel数据预览，这块只是将Excel中的数据加载出来之后绑定到DATAGRID控件上
            //1.这是打开文件的一个弹框
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            //2.默认的文件后缀名类型
            fileDialog.Filter = "工作簿Excel文件(*.xlsx;*.xls)|*.xlsx;*.xls";
            //3.打开文件
            if (fileDialog.ShowDialog() == true)
            {
                //4.获取文件路径
                string path = fileDialog.FileName;
                //5.调用业务逻辑中学生：找到对应的方法获取从Excel中拿到所有的数据----》在找访问层把所有的在Excel库中学生的数据用list范性全部拿到----》在学生中找对应的属性赋值
                list = manager.GetStudentByExcel(path);

                //***先把DataGrid（名字时dgStudent）标签里面的东西全部清空
                dgStudent.ItemsSource = null;
                dgStudent.AutoGenerateColumns = false;
                //从新赋值给DataGrid（名字时dgStudent）标签
                dgStudent.ItemsSource = list;
            }
        }


        List<StudentsExt> lastlist = new List<StudentsExt>();
        /// <summary>
        /// 导出到SQL数据库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DaoChu_Click(object sender, RoutedEventArgs e)
        {
            #region//普通版
            /* //将DataGrid中的数据添加到数据库的数据表中
             ///两种上传方式：1.逐个上传-如果Excel中的某行数据有问题，可以针对这行数据先检查再进行添加
             ///2.一次性上传-将Excel中的所有数据一键上传至数据库，如果某个数据有问题可能导致系统崩溃
             if (list.Count > 0)
             {
                 for (int i = 0; i < list.Count; i++)
                 {
                     //业务逻辑中的方法获取学生ID数据和把每一个学生数据给数据库中添加
                     int res = manager.InsertStudent(list[i]);
                     //表示执行不成功，先暂时不执行这行先执行下面的行
                     if (res <= 0)
                     {
                         //保存这行数据不添加到数据库中
                         lastlist.Add(list[i]);
                         //先暂时不执行这行先执行下面的行
                         continue;
                     }
                 }
                 //所有成员上传成功
                 if (lastlist.Count <= 0)
                 {//lastlist范性里面的所有的元素数都为0表示上传成功
                     //***先把DataGrid（名字时dgStudent）标签里面的东西全部清空
                     dgStudent.ItemsSource = null;
                     MessageBox.Show("所有数据上传成功！", "提示");
                 }
                 else
                 {
                     //***先把DataGrid（名字时dgStudent）标签里面的东西全部清空
                     dgStudent.ItemsSource = null;
                     //显示生于信息
                     dgStudent.ItemsSource = lastlist;
                     //提示剩余信息
                     MessageBox.Show("剩余学员信息上传失败！请检查！", "提示");
                 }
             }
             else
             {
                 MessageBox.Show("当前没有任何数据！", "提示");
             }*/
            #endregion

            #region//事务使用版
            if (list.Count>0)
            {
                int res = manager.TranStuDaTa(list);
                //如果执行的数据的数字==list的总共数据
                if (res==list.Count)
                {
                    //***先把DataGrid（名字时dgStudent）标签里面的东西全部清空
                    dgStudent.ItemsSource = null;
                    MessageBox.Show("所有数据上传成功！", "提示");
                }
                else
                {
                    MessageBox.Show("所有数据上传失败！", "提示");
                }
            }
            else
            {
                MessageBox.Show("当前没有任何数据！", "提示");
            }
            #endregion
        }
    }
}
