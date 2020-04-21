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
using StudentManagerModel;//实体层
using StudentManagerModel.ObjExt;
using StudentMangerBLL;//业务逻辑层
using Common;
using System.IO;//MemoryStream流

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// FrmStuManager.xaml 的交互逻辑
    /// </summary>
    public partial class FrmStuManager : UserControl
    {//表示层

        //先实例化业务逻辑层中的StudentClassManager
        StudentClassManager csm = new StudentClassManager();

        //实例化BLL里面的StudentsManager,不加public公用，时通过StudentMangerBLL.点出来
        StudentsManager sm = new StudentsManager();
        //现实list
        List<StudentsExt> students = null;

        //用来记录当前的选择的学员
        StudentsExt selectStu = null;
        public FrmStuManager()
        {
            InitializeComponent();

            //下拉框中内容填写
            List<StudentClass> list = csm.GetClasses();
            //现在现在combox绑定要给里面放的值
            smclassCmb.ItemsSource = list;
            //设置放在某个具体的对象上，可视表示设置再combox上面可以看见的,要显示ClassName的属性
            smclassCmb.DisplayMemberPath = "ClassName";//设置下拉框显示文本
            //这个表示给过来的数据一一对应，才可以填写进去
            smclassCmb.SelectedValuePath = "ClassId";//设置下拉框显示对应文本的value
            //并获取索引为0的选项
            smclassCmb.SelectedIndex = 0;

            //获取当选中某个科目时选中学生ID值对应显示绑定
           students=sm.GetStudentsExts(Convert.ToInt32(smclassCmb.SelectedValue));
            //DataGrid进行数据绑定，DataGrid的名字叫smDgStudentLsit
            smDgStudentLsit.ItemsSource = students;//把获取到的路径students赋给DataGrid表里
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
            //Environment.Exit(0);
        }
        /// <summary>
        /// 提交查询按钮，更具班级查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectByCId_Click(object sender, RoutedEventArgs e)
        {
            //如果这个选项小于0表示没有选中内容
            if (smclassCmb.SelectedIndex<0)
            {
                MessageBox.Show("请选择班级！", "提示");
                return;
            }
            //把选中的值GetStudentsExts传进去判断
            students = sm.GetStudentsExts(Convert.ToInt32(smclassCmb.SelectedValue));
            //把获取的值给smDgStudentLsit这个路径,smDgStudentLsit表示显示给DataGrid的名字，然后把找到的学生全放在这里面的网格
            smDgStudentLsit.ItemsSource = students;
        }
        /// <summary>
        /// 学员管理里面的提交查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectBySIN_Click(object sender, RoutedEventArgs e)
        {
            //btnSelectBySIN某个单个id或姓式查询的button名字
            smclassCmb.SelectedIndex = -1;
            string str = mstxtIdorName.Text.Trim();
            List<StudentsExt> liststu= sm.GetStudentsIdorName(str);
            if (liststu.Count<=0)
            {
                MessageBox.Show("根据条件没查询到","提示！");
                return;
            }
            students = liststu;
            //点击先清空路径里面的内容
            smDgStudentLsit.ItemsSource = null;
            //在从新查找
            smDgStudentLsit.ItemsSource = students;
        }
        
        /// <summary>
        /// 学号排列按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupBySid_Click(object sender, RoutedEventArgs e)
        {
            //排序方式list中有默认的排序方式3
            //先实现比较器
            //1.先声明比较顺序
            if (smDgStudentLsit.ItemsSource == null)
            {
                return;
            }
            if ((sender as Button).Tag.ToString() == "True")//为倒序排列从小到大
            {
                students.Sort(new StudentidDESC(true));
                smDgStudentLsit.ItemsSource = null;
                smDgStudentLsit.ItemsSource = students;
                //groupBySidImg向下箭头的名字
                groupBySidImg.Source = new BitmapImage(new Uri("/img1/ico/up.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "False";
            }
            else if ((sender as Button).Tag.ToString() == "False")//从大到小排序
            {
                students.Sort(new StudentidDESC(false));
                groupBySidImg.Source = new BitmapImage(new Uri("/img1/ico/down.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "True";
            }

            //相当于刷新
            smDgStudentLsit.ItemsSource = null;
            smDgStudentLsit.ItemsSource = students;
            /* students.Sort(new StudentidDESC());
             smDgStudentLsit.ItemsSource = null;
             smDgStudentLsit.ItemsSource = students;*/
        }
        /// <summary>
        /// 姓名排列按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGroupBySName_Click(object sender, RoutedEventArgs e)
        {
            if (smDgStudentLsit.ItemsSource == null)
            {
                return;
            }
            if ((sender as Button).Tag.ToString() == "True")
            {
                //简单的排序方式，本身排序//如果里面没参数就默认排序第一个属性
                students.Sort(new StudentNameDESC(true));
                groupBySNameImg.Source = new BitmapImage(new Uri("/img1/ico/jiang.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "False";
            }
            else if ((sender as Button).Tag.ToString() == "False")
            {
                students.Sort(new StudentNameDESC(false));
                groupBySNameImg.Source = new BitmapImage(new Uri("/img1/ico/sheng.ico", UriKind.RelativeOrAbsolute));
                (sender as Button).Tag = "True";
            }
            //上面排序完后先清空
            smDgStudentLsit.ItemsSource = null;
            //然后把students的值从新赋给显示学生相信的框里DataGrid里面
            smDgStudentLsit.ItemsSource = students;
            //相当于刷新
            /*students.Sort(new StudentNameDESC());
            smDgStudentLsit.ItemsSource = null;
            smDgStudentLsit.ItemsSource = students;*/
        }

        //比较数字的接口，用类来写，继承IComparer<StudentsExt>表示Sort的一个参数，这个参数用来进行排序，所以继承接口这个排序方式<比较这里面的实体对象>，左后直接(把鼠标放在红线上点击实现皆空)实现这个接口
        //1.声明比较器
        class StudentidDESC : IComparer<StudentsExt>
        {
            //定义一个字段B表示用来记录当这个B字段的值为多少时B会选择进入那个排序顺序
            public StudentidDESC(bool b)
            {
                B = b;
            }
            public bool B { get; set; }
            public int Compare(StudentsExt x, StudentsExt y)
            {
                //值只有-1表示逆着排序，0表示没排序，1表示正着排序
                //ID顺序排序
                if (B)
                {
                    return x.StudentId.CompareTo(y.StudentId);
                }
                else
                {
                    return y.StudentId.CompareTo(x.StudentId);
                }
            }
        }
        //比较文字的接口
        class StudentNameDESC : IComparer<StudentsExt>
        {
            //值只有-1表示逆着排序，0表示没排序，1表示正着排序
            public StudentNameDESC(bool b)
            {
                B = b;
            }
            //定义一个字段B表示用来记录当这个B字段的值为多少时B会选择进入那个排序顺序
            public bool B { get; set; }
            //Name顺序排序
            public int Compare(StudentsExt x, StudentsExt y)
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

        //用一个List装StudentsExt的数据
        //List<StudentsExt> DoubleList = new List<StudentsExt>();
        List<int> DoubleList = new List<int>();
        //用list给WindowDoubleStu成一个属性,相当于把一个窗口封装成属性了
        List<WindowDoubleStu> WindowDouStu = new List<WindowDoubleStu>();
        /// <summary>
        /// 当鼠标双击某个学员查看这个学员的详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void smDgStudentLsit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //接收查看是什么双击进入
            //object o = sender;

            //SelectedItem选中单个元素选中第一项,如果有多个也会只选第一个;SelectedItems选中多个元素
            //如果StudentsExt转成这个类型,表示我们点击时的这一行数据是StudentsExt这个类型
            StudentsExt ext = smDgStudentLsit.SelectedItem as StudentsExt;//只能现在只能双击不能拿到学员的信息

            //判断点击这个按钮是否为空,就是没点击到这行数据,这行没数据,如为空返回
            if (ext==null)
            {
                return;
            }
            #region//对以下代码解释
            /*//1.如何拿到学员的ID,通过ID拿到整行数据
            //2.ext要通过这个点击查询到所有学生ID,为什么ext可以点出StudentId,原因是Binding StudentId的绑定来获取的
            //3.双击后把ext.StudentId获取到的ID带入sm中GetDoubleStudentsExt的这个方法
            //4.如何把DoubleStu传递进WindowDoubleStu这个窗口里面,通过WindowDoubleStu窗口中函数的参数
            //5.老师:（1）当这个学员的完整信息已经存在的话,证明已经打开了一个窗口；（2）除非是打开新的学员窗口，否则只能把之前的窗口呈现出来
            StudentsExt DoubleStu = sm.GetDoubleStudentsExt(ext.StudentId);

            //如何展示用户信息
            View.WindowDoubleStu WindowdoubleStu = new WindowDoubleStu(DoubleStu);//这样就把含有某一个学生ID的全部值拿到了
            //Show可以打开多个窗口;ShowDialog不能同时打开多个窗口
            WindowdoubleStu.Show();*/
            #endregion

            //list有一个属性判断list确定某元素是否存在
            //DoubleList只存放学生ID
            if (DoubleList.Contains(ext.StudentId))//存在
            {
                foreach (WindowDoubleStu item in WindowDouStu)
                {//遍历WindowDoubleStu窗口的数据
                    //item表示把WindowDoubleStu这个窗口的Stuext数据拿到了，当被触发时两个数据一样，表示已经打开了窗口，然后进入if判断让其item(表示整个窗口)在激活的状态下
                    if (item.Stuext==ext.StudentId)
                    {
                        //只能激活窗口，不能创建多个窗口
                        item.Activate();
                    }
                }

            }
            else//不存在,下面的窗口没关，上面的作判断
            {
                StudentsExt DoubleStu = sm.GetDoubleStudentsExt(ext.StudentId);//给DoubleStu赋值
                //（1）然后把DoubleList里面的属性改成int,（2）然后在DoubleStu把学生ID添加名字为DoubleList的list
                DoubleList.Add(DoubleStu.StudentId);

                View.WindowDoubleStu WindowdoubleStu = new WindowDoubleStu(DoubleStu);//实例化窗口并传入学生数据

                //问题再次双击关闭窗口学生的行就点不出来，原因是DoubleList；里面已经有一个学生id了，所以再次双击就出不来了；所以我们要关闭的时候在把他对应的数据移除掉
                WindowdoubleStu.Closed += WindowdoubleStu_Closed;


                WindowDouStu.Add(WindowdoubleStu);//用list把窗口装起来了
                WindowdoubleStu.Show();
            }
        }
        //关闭WindowDoubleStu这个窗口时触发这个事件，可以再次双击这个学生行
        /// <summary>
        /// 移除关闭的窗口对应的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowdoubleStu_Closed(object sender, EventArgs e)
        {
            //获取事件的发起者,并拿到对应获取到的学生ID值
            int stuId = (sender as WindowDoubleStu).Stuext;

            //然后DoubleList名字为list里面的内容移除
            DoubleList.Remove(stuId);
            //WindowDoubleStu里面Stuext学生ID移除
            foreach (WindowDoubleStu item in WindowDouStu)
            {
                //使遍历出来的装学生ID的属性与关闭后拿到的学生ID属性一致时，关闭下面的名字为WindowDouStu的list范性
                if (item.Stuext == stuId)
                {
                    WindowDouStu.Remove(item);
                    return;
                }
            }
        }
        /// <summary>
        /// 修改学员信息：是以UpdateStuInfor窗口WPF创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdataStuButton_Click(object sender, RoutedEventArgs e)
        {
            //表示在DataGrid里面选中的一行
            StudentsExt selectStu = smDgStudentLsit.SelectedItem as StudentsExt;
            //没有选中提示，返回
            if (selectStu == null)
            {
                MessageBox.Show("请选择要修改的学员！", "提示");
                return;
            }
            //1.sm.GetDoubleStudentsExt(selectStu.StudentId)---》selectStu表示选中一行的所以数据----》只要点击后的学生ID值----》然后通过这个点击的ID值传进GetDoubleStudentsExt获取整个行，获取到行后用一个盒子接收
           StudentsExt studentsExt= sm.GetDoubleStudentsExt(selectStu.StudentId);

            //2.创建或实例化一下修改的页面,并传递参数
            UpdateStuInfor updateStuInfor = new UpdateStuInfor(studentsExt);
            //点击修改按钮显示这个窗口
            updateStuInfor.ShowDialog();
            //每次修改完后刷新DG中这个学员的信息
            RefreshDG();
        }

        private void RefreshDG()
        {
            //students是一个空范性
            students = sm.GetStudentsExts(Convert.ToInt32(smclassCmb.SelectedValue));//选的科目id号，绑定后从新刷新smDgStudentLsit表面临的内容（显示同一种多个科目不同人，相当于从新输出刷新，也相当于从数据库从新获取）
            smDgStudentLsit.ItemsSource = students;
        }
        /// <summary>
        /// 删除学员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //点击确定后获取这一行点击的数据
            selectStu = smDgStudentLsit.SelectedItem as StudentsExt;
            //判断DoubleList范性里面是否获取到//用来记录当前的选择的学员StudentsExt
            if (DoubleList.Contains(selectStu.StudentId))
            {
                MessageBox.Show("请关闭正在查看的学员信息界面", "提示");
                return;
            }
            if (selectStu == null)
            {
                MessageBox.Show("请选择要删除的学员！", "提示");
                return;
            }
            //实例化把这一行学生ID全部拿到，判断这行不为空
            StudentsExt students = sm.GetDoubleStudentsExt(selectStu.StudentId);
            if (students == null)
            {
                MessageBox.Show("您选择的学员信息已经被删除！", "提示");
                return;
            }

            MessageBoxResult mbr = MessageBox.Show("您确定要删除【" + students.StudentName + "】", "警告", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (mbr == MessageBoxResult.OK)
            {
                if (sm.DeleteStudentById(students.StudentId))
                {
                    MessageBox.Show("删除成功！", "提示");
                }
                else
                {
                    MessageBox.Show("删除失败请稍后再试！", "提示");
                }
            }
        }
        /// <summary>
        /// 导出到Excel文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_Click(object sender, RoutedEventArgs e)
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
                System.Data.DataTable table = sm.GetDataTable((int)smclassCmb.SelectedValue);
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
        //打印学员信息（ 实现打印及打印预览）
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //点击选中的学员，按下打印学员信息的buttom按钮，这样就获取具体学员的信息了（用一个实体类接收一下在smDgStudentLsit里面数据，当点击时把所有的值用selectStu接收一下）
            selectStu = smDgStudentLsit.SelectedItem as StudentsExt;//（现在selectStu里面就装了点击时所包含的值）
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
    }
}
