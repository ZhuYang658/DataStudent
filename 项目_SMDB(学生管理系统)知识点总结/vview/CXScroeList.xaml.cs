using System;
using System.Collections.Generic;
using System.IO;
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
using Common;
using StudentManagerModel.ObjExt;
using StudentMangerBLL;
using 项目_SMDB_学生管理系统_知识点总结.View;

namespace 项目_SMDB_学生管理系统_知识点总结.vview
{
    /// <summary>
    /// CXScroeList.xaml 的交互逻辑
    /// </summary>
    public partial class CXScroeList : Window
    {
        //成绩的业务逻辑
        ScroeListManager manager = new ScroeListManager();
        List<ExtScroeList> listscroe = null;
        public CXScroeList()
        {
            InitializeComponent();

            //获取学生的所有值,也就是显示获取成绩表
            listscroe = manager.GetExtScroeLists();
            //DataGrid进行数据绑定，DataGrid的名字叫smDgStudentLsit
            CjDataGrid.ItemsSource = listscroe;
        }
        /// <summary>
        /// 查找C#成绩按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dancz_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            if (!int.TryParse(txtCX.Text, out a))
            {
                MessageBox.Show("只能输入数字");
                txtCX.Text = "";
            }
            listscroe = manager.GetExtScroeCJ(a);
            CjDataGrid.ItemsSource = listscroe;
        }
        /// <summary>
        /// 查找DB成绩按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBcz_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            if (!int.TryParse(txtCX.Text, out a))
            {
                MessageBox.Show("只能输入数字");
                txtCX.Text = "";
            }
            listscroe = manager.GetExtScroeDB(a);
            CjDataGrid.ItemsSource = listscroe;
        }
        /// <summary>
        /// C#范围查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Duocx_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            if (!int.TryParse(txt1.Text, out a))
            {
                MessageBox.Show("只能输入数字");
                txt1.Text = "";
            }
            int b = 0;
            if (!int.TryParse(txt2.Text, out b))
            {
                MessageBox.Show("只能输入数字");
                txt2.Text = "";
            }
            listscroe = manager.GetExtScroeListsCC(a, b);
            CjDataGrid.ItemsSource = listscroe;
        }
        /// <summary>
        /// DB范围查找
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBFW_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            if (!int.TryParse(txt1.Text, out a))
            {
                MessageBox.Show("只能输入数字");
                txt1.Text = "";
            }
            int b = 0;
            if (!int.TryParse(txt2.Text, out b))
            {
                MessageBox.Show("只能输入数字");
                txt2.Text = "";
            }
            listscroe = manager.GetExtScroeListsDB(a, b);
            CjDataGrid.ItemsSource = listscroe;
        }
        /// <summary>
        /// 关闭成绩
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            /*//关闭所有窗口
            Environment.Exit(0);*/
            this.Close();
        }
        /// <summary>
        /// C#排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CCPX_Click(object sender, RoutedEventArgs e)
        {
            if (CjDataGrid.ItemsSource == null)
            {
                return;
            }
            if ((sender as Button).Tag.ToString() == "True")
            {
                //简单的排序方式，本身排序//如果里面没参数就默认排序第一个属性
                listscroe.Sort(new ScroeList(true));
                groupBySNameImg.Source = new BitmapImage(new Uri("/img1/ico/down.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "False";
            }
            else if ((sender as Button).Tag.ToString() == "False")
            {
                listscroe.Sort(new ScroeList(false));
                groupBySNameImg.Source = new BitmapImage(new Uri("/img1/ico/up.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "True";
            }
            //上面排序完后先清空
            CjDataGrid.ItemsSource = null;
            //然后把students的值从新赋给显示学生相信的框里DataGrid里面
            CjDataGrid.ItemsSource = listscroe;
        }
       
        /// <summary>
        /// DB排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DBPX_Click(object sender, RoutedEventArgs e)
        {
            if (CjDataGrid.ItemsSource == null)
            {
                return;
            }
            if ((sender as Button).Tag.ToString() == "True")
            {
                //简单的排序方式，本身排序//如果里面没参数就默认排序第一个属性
                listscroe.Sort(new ScroeList(true));
                groupBySNameImg.Source = new BitmapImage(new Uri("/img1/ico/jiang.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "False";
            }
            else if ((sender as Button).Tag.ToString() == "False")
            {
                listscroe.Sort(new ScroeList(false));
                groupBySNameImg.Source = new BitmapImage(new Uri("/img1/ico/sheng.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "True";
            }
            //上面排序完后先清空
            CjDataGrid.ItemsSource = null;
            //然后把students的值从新赋给显示学生相信的框里DataGrid里面
            CjDataGrid.ItemsSource = listscroe;
        }
        //比较数字的接口，用类来写，继承IComparer<StudentsExt>表示Sort的一个参数，这个参数用来进行排序，所以继承接口这个排序方式<比较这里面的实体对象>，左后直接(把鼠标放在红线上点击实现皆空)实现这个接口
        //1.声明比较器
        class ScroeList : IComparer<ExtScroeList>
        {
            //定义一个字段B表示用来记录当这个B字段的值为多少时B会选择进入那个排序顺序
            public bool B { get; set; }
            //值只有-1表示逆着排序，0表示没排序，1表示正着排序
            public ScroeList(bool b)
            {
                B = b;
            }
            public int Compare(ExtScroeList x, ExtScroeList y)
            {
                if (B)
                {
                    return y.StudentName.CompareTo(x.StudentName);
                }
                else
                {
                    return x.StudentName.CompareTo(y.StudentName);
                }
            }
        }
        //用来记录当前的选择的学员
        StudentsExt selectStu = null;
        /// <summary>
        /// 打印学员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CJDAExcl_Click(object sender, RoutedEventArgs e)
        {
            //点击选中的学员，按下打印学员信息的buttom按钮，这样就获取具体学员的信息了（用一个实体类接收一下在smDgStudentLsit里面数据，当点击时把所有的值用selectStu接收一下）
            selectStu = CjDataGrid.SelectedItem as StudentsExt;//（现在selectStu里面就装了点击时所包含的值）
            //当StudentsExt的值等于等于空值时，表示
            if (selectStu == null)
            {
                MessageBox.Show("请选择您要打印的学员", "提示");
                return;
            }
            //实例化放图片的空数据
            common.BitmapImg image = null;
            if (string.IsNullOrEmpty(selectStu.StuImage))
            {
                //最后在StudentsExt多添加了一个属性
                selectStu.ImgPath = "/img1/bg/zwzp.jpg";
            }
            //如果有图片执行下面获取的图片
            else
            {
                //(获取数据库中图片的反序列化)用空的图片类来接收数据库中的这个图片（当点击要打印学生信息就可以获取selectStu.StuImage的数据）   
                image = BinaryStuObjcet.FBinaryForStu(selectStu.StuImage) as common.BitmapImg;
                //实例化BitmapImage用来封装序列化和反序列化的转换路径的*记录方式*
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();//用信号通知BitmapImage初始化开始
                //图像流:  对象的集合编码为图像流（new无法自己出来，所以自己输入）
                BitmapEncoder encoder = new PngBitmapEncoder();
                /*1.向图像流中添加
                 * 2.BitmapFrame图像数据属于静态类
                 * 3.创建这个BitmapImage路径并添加给图像流
                 * 4.就相当于给这个图像流输入照片路径
                 * 总结：encoder.Frames.Add相当于给图像流中添加图片的路径
                 */
                encoder.Frames.Add(BitmapFrame.Create(bitmap));//把图片从数据库拿到的图片路径拿到，并创建

                //获取事件
                long sc = DateTime.Now.Ticks;
                //using表示方式这个流被其他替代，在using里绝对不会被替代相当一个封闭环境
                using (MemoryStream stream = new MemoryStream())
                {
                    //**encoder里面获取到的内容给了stream流**：将位图图像编码为指定的流；把
                    encoder.Save(stream);
                    //用一个byte数组来装这个流
                    byte[] buffer = stream.ToArray();
                    /*1.file是一个静态类可以直接使用里面的方法
                     * 2.使用WriteAllBytes这个方法创建一个新文件在其中写入指定的字节数组，然后关闭改文件。如果目标问价以存在，则覆盖改文件
                     * 3.AppDomain应用程序中独立执行的环境
                     * 4.CurrentDomain获取当前程序域
                     * 5.BaseDirectory获取基目录
                     * 6.sc表示当前时间
                     * 7.printImg文件名、
                     * 8.buffer表示记录stream流中的所有内容
                     * 总结创建了一个图片文件
                     */
                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "/printImg/" + sc + ".png", buffer);//???
                    stream.Close();//关闭流
                }
                //获取学生图片的路径
                selectStu.ImgPath = AppDomain.CurrentDomain.BaseDirectory + "/printImg/" + sc + ".png";
            }
            /*1.创建打印学生信息界面
             * 2.参数1（PrintModel.xaml：框架形式是流文档所以创建流文档写打印界面）：表示对打印界面的设计
             * 3.参数2（selectStu）：表示获取学生的所有数据
             */
            View.PrintWindow frmPrint = new PrintWindow("PrintModel.xaml", selectStu);
            //获取或设置一个值，该值指示窗口是否具有任务栏按钮
            frmPrint.ShowInTaskbar = false;
            //打开打印学生窗口窗口
            frmPrint.ShowDialog();
        }
        //实例化BLL里面的StudentsManager,不加public公用，时通过StudentMangerBLL.点出来
        StudentsManager sm = new StudentsManager();
        /// <summary>
        /// 导出学员信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutExcl_Click(object sender, RoutedEventArgs e)
        {
            //1.（1）创建一个打开对话框
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            //（2）默认自动确定的文件所选后缀名（给打开的文件框起名）
            fileDialog.Filter = "Excel工作簿(*.xlsx;*.xls)|*.xlsx;*.xls";
            //（3）默认打开文件自动确定文件名（FileName设置文件默认的名字）
            fileDialog.FileName = "学生信息表.xlsx";
            //（4）在标题栏显示的文本
            fileDialog.Title = "导出到Excel表";
            //（5）当点击文件弹框的确定键时进入获取
            if (fileDialog.ShowDialog() == true)
            {
                //（6）用string接收这个文件的名字
                string path = fileDialog.FileName;

                //2.把获取到某个班级(smclassCmb班级名字的对应SelectedValue)的所有学生查到在ADL里面用DataTable类型方法接收
                //先找到GetDataTable方法表
                System.Data.DataTable table = sm.GetDataTable(Convert.ToInt32(txtCX.Text));
                //这个表的行如果<=0表示该表什么也没有
                if (table.Rows.Count <= 0)
                {
                    MessageBox.Show("该班级暂无学生数据！", "提示");
                    return;
                }


                //3.Common.ExportData.ExportToExcel(table, path)获取加入表格的方法
                //3.判断将DataTable这个表的数据导入进某个文件（wps等文件）；第一个参数表示整个表；第二个参数是文件的路径
                if (Common.ExportData.ExportToExcel(table, path))
                {
                    MessageBox.Show("导出数据完成！", "提示");
                }
                else
                {
                    MessageBox.Show("导出数据失败，请稍后再试！", "提示");
                }

            }
        }
    }
}
