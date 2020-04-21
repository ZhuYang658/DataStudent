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
using StudentMangerBLL;//业务逻辑层:获取添加的方法
using StudentManagerModel.ObjExt;//获取表的组合
using StudentManagerModel;
using System.IO;

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// ADDstudent.xaml 的交互逻辑
    /// </summary>
    public partial class ADDstudent : Window
    {
        StudentClassManager SCM = new StudentClassManager();
        public ADDstudent()
        {
            InitializeComponent();
            //获取StudentClass下拉列表的内容
            List<StudentClass> classes = SCM.GetClasses();
            cmbClassName.ItemsSource = classes;
            cmbClassName.DisplayMemberPath = "ClassName";
            cmbClassName.SelectedValuePath = "ClassId";
            
        }
        StudentsManager manager = new StudentsManager();
        //给这个BitmapImg先一个空值
        common.BitmapImg image = null;
        /// <summary>
        /// 确认添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSureUpdate_Click(object sender, RoutedEventArgs e)
        {
            Students students = new Students();
            students.StudentName = txtName.Text;
            students.Gender = (radBoy.IsChecked == true ? "男" : "女");//性别
            students.Birthday = datePkBirthday.Text;
            students.Age =Convert.ToInt32(txtAge.Text);//年龄
            students.CardNo = txtCardNo.Text;//卡号
            students.ClassId = (int)cmbClassName.SelectedValue;//选择科目
            students.StudentAddress = txtAddress.Text;
            students.PhoneNumber = txtPhoneNumber.Text;
            students.StudentIdNo = txtStuNoId.Text;
            
            //获取图片的路径
            if (stuImg.Source == new BitmapImage(new Uri("img1/bg/zwzp.jpg", UriKind.RelativeOrAbsolute)))
            {
                students.StuImage = null;
            }
            //判断数据库中的图片是否和目前的上传图片一样
            else
            {
                //证明未修改图片,目前的图片和原来数据库中的一致
                if (image != null && img1.Buffer == image.Buffer)
                {
                    //序列化
                    students.StuImage = Common.BinaryStuObjcet.ZBinaryForStu(image);
                }
                else
                {
                    students.StuImage = Common.BinaryStuObjcet.ZBinaryForStu(img1);
                }
                // GetAddStudent修改的方法，students把上面赋给他的值全部赋给GetAddStudent这个方法
                if (manager.GetAddStudent(students)>0)//因为用的时增改删所以用的是ExecuteNonQuery这个方法返回数字大于0证明执行成功了
                {
                    System.Windows.MessageBox.Show("添加成功！", "提示");
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("添加失败，请稍后再试！", "提示");
                    this.Close();
                }
            }
            //添加进数据库中
            manager.GetAddStudent(students);
        }
        //从新拍照
        private void btnOpenVideo_Click(object sender, RoutedEventArgs e)
        {
            //打开一个文件
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            //获取这个文件以什么类型打开
            fileDialog.Filter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;*.bmp)|*.jpg;*.jpeg;*.gif;*.png,*.bmp";
            //当打开文本框的这个按钮，点击确定是选中这个图片
            if (fileDialog.ShowDialog() == true)
            {
                //获取选中图片的路径
                string path = fileDialog.FileName;
                //用一个图片类型就收一下选中的图片
                stuImg.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                stuImg.Stretch = Stretch.UniformToFill;//自己调试合适大小
                //用在通用层中创建一个属性类型为BitmapImg图片路径类型；在这里将其实例化
                img1.Buffer = File.ReadAllBytes(path);//Buffer记录图片路径
            }
        }
        //保存拍照的图片时保存的路径
        public static string imgPath;
        //从上传照片，在通用层中记录一下这个图片路径
        common.BitmapImg img1 = new common.BitmapImg();
        //从新上传图片
        private void btnUploadPic_Click(object sender, RoutedEventArgs e)
        {
            //要在这个地方获取一个窗口用来拍照
            StudentPhoto photo = new StudentPhoto();
            //展开这个窗口
            photo.ShowDialog();
            if (!string.IsNullOrEmpty(imgPath))
            {
                //照片刷新了之后对新照片进行序列化;表示把路径放在数据库中；反序列化时从数据库中拿出图片路径
                stuImg.Source = new BitmapImage(new Uri(imgPath, UriKind.RelativeOrAbsolute));
                //把这个图片路径给img1.Buffer(这样表示放在图片路径里面了)
                img1.Buffer = File.ReadAllBytes(imgPath);//File这个是访问IO流的一种访问方式，所以定要加IO流命名空间
            }
        }
    }
}
