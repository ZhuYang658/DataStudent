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
using StudentManagerModel.ObjExt;//要获取StudentsExt里的所有内容
using StudentManagerModel;//StudentClass的类
using StudentMangerBLL;//获取StudentClassManager类和StudentManager类
using Common;//要使用DataValidate之前先导入改命名空间
using System.IO;//MemoryStream

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// UpdateStuInfor.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateStuInfor : Window
    {
        StudentClassManager csm = new StudentClassManager();
        StudentsManager manager = new StudentsManager();

        //给这个BitmapImg先一个空值
        common.BitmapImg image = null;
        //创建这个属性是把构造函数参数的值赋给studentSJ名字叫这个StudentsExt类型的属性，方便下面要用到学生的所有属性，所以把参数赋给studentSJ
        public StudentsExt studentSJ { get; set; }
        public UpdateStuInfor(StudentsExt ext)
        {
            InitializeComponent();

            //从上传照片
            common.BitmapImg img = new common.BitmapImg();

            //把获取的元素放在一个属性里方便下面的调用
            studentSJ = ext;

            //开始在这个页面上显示修改按钮所点击的那个学生的所有信息在这里显示，点击是显示原有的
            txtAddress.Text = ext.StudentAddress;//地址
            txtAge.Text = ext.Age.ToString();//年龄
            txtCardNo.Text = ext.CardNo;//考勤卡号
            txtName.Text = ext.StudentName;//姓名
            txtPhoneNumber.Text = ext.PhoneNumber;//电话
            txtStuNoId.Text = ext.StudentIdNo;//ID
            if (ext.Gender == "男")//性别
            {
                radBoy.IsChecked = true;
            }
            else
            {
                radGirl.IsChecked = true;
            }
            /*//获取或设置要选定的日期
            datePkBirthday.DisplayDate =Convert.ToDateTime(ext.Birthday);
            //获取或设置当前要选定的日期
            datePkBirthday.SelectedDate = Convert.ToDateTime(ext.Birthday);*/
            datePkBirthday.Text = ext.Birthday;
            //图片的显示，如果当前图片为空时进入获取这下面zwzp图片
            if (string.IsNullOrEmpty(ext.StuImage))
            {
                stuImg.Source = new BitmapImage(new Uri("/img1/bg/zwzp.jpg", UriKind.RelativeOrAbsolute));
            }
            else
            {
                //如果学员的Iamge字段中能够查询到数据，那么就可以直接将这个数据反序列化成BitmapImage对象
                image = BinaryStuObjcet.FBinaryForStu(ext.StuImage) as common.BitmapImg;
                img1.Buffer = image.Buffer;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = new MemoryStream(image.Buffer);
                bitmap.EndInit();
                stuImg.Source = bitmap;
            }

            //获取下拉列表的内容
            List<StudentClass> classes = csm.GetClasses();
            cmbClassName.ItemsSource = classes;
            cmbClassName.DisplayMemberPath = "ClassName";
            cmbClassName.SelectedValuePath = "ClassId";
            cmbClassName.SelectedIndex = ext.ClassId - 1;
        }
        /// <summary>
        /// 取消修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 确认修改按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSureUpdate_Click(object sender, RoutedEventArgs e)
        {
            //CheckInfor这个个方法，改变数据之前的最终验证
            if (CheckInfor())
            {
                studentSJ.StudentName = txtName.Text;//姓名
                studentSJ.Age = Convert.ToInt32(txtAge.Text);//年龄
                studentSJ.Birthday = datePkBirthday.Text.ToString();//日期
                studentSJ.CardNo = txtCardNo.Text;//考勤卡号
                studentSJ.ClassId = (int)cmbClassName.SelectedValue;//科目
                studentSJ.Gender = (radBoy.IsChecked == true ? "男" : "女");//性别
                studentSJ.PhoneNumber = txtPhoneNumber.Text;//电话
                studentSJ.StudentAddress = (string.IsNullOrEmpty(txtAddress.Text) ? null : txtAddress.Text);//住址
                studentSJ.StudentIdNo = txtStuNoId.Text;//身份证号
                //判断是否重新选择了Image图片；路径
                if (stuImg.Source == new BitmapImage(new Uri("/img1/bg/zwzp.jpg", UriKind.RelativeOrAbsolute)))
                {
                    studentSJ.StuImage = null;
                }
                //判断数据库中的图片是否和目前的上传图片一样
                else
                {
                    //证明未修改图片,目前的图片和原来数据库中的一致
                    if (image != null && img1.Buffer == image.Buffer)
                    {
                        //序列化
                        studentSJ.StuImage = Common.BinaryStuObjcet.ZBinaryForStu(image);
                    }
                    else
                    {
                        studentSJ.StuImage = Common.BinaryStuObjcet.ZBinaryForStu(img1);
                    }

                }
                //UpdateStudentInfor修改的方法，studentSJ把上面赋给他的值全部赋给UpdateStudentInfor这个方法
                if (manager.UpdateStudentInfor(studentSJ))//修改的所有值传递进去，如果传递成功则系统提示并关闭
                {
                    System.Windows.MessageBox.Show("修改成功！", "提示");
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("修改失败，请稍后再试！", "提示");
                }
            }
        }
        //判断这些文本框的值失去焦点是否可以获取到
        bool CheckInfor()
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                System.Windows.MessageBox.Show("姓名不能为空！");
                txtName.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtAge.Text))
            {
                System.Windows.MessageBox.Show("年龄不能为空！");
                txtAge.Focus();
                return false;
            }
            else if (!DataValidate.Isinteger(txtAge.Text))
            {
                System.Windows.MessageBox.Show("年龄必须是纯数字！");
                txtAge.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCardNo.Text))
            {
                System.Windows.MessageBox.Show("打卡号不能为空！");
                txtCardNo.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtStuNoId.Text))
            {
                System.Windows.MessageBox.Show("身份证号不能为空！");
                txtStuNoId.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                System.Windows.MessageBox.Show("联系方式不能为空！");
                txtPhoneNumber.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 学生名字的文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            //判断含有学生的文本框不能为空
            if (string.IsNullOrEmpty(txtName.Text))
            {
                //如果为空提示姓名不能为空，返回焦点
                System.Windows.MessageBox.Show("姓名不能为空！");
                txtName.Focus();
            }
        }
        /// <summary>
        /// 年龄文本框的判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtAge_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAge.Text))
            {
                System.Windows.MessageBox.Show("年龄不能为空！");
                txtAge.Focus();
            }
            //DataValidate这个是在Common通用层里面封装用正则表达式判断的类，找里面关于数字封装的方式
            else if (!DataValidate.Isinteger(txtAge.Text))
            {
                //1.先倒入命名空间
                //2.又因为我们个这个类设置静态类所以直接通过里面的方法点出你需要的方法,Isinteger这个方法是封装类的
                System.Windows.MessageBox.Show("年龄必须是纯数字！");
                txtAge.Focus();
            }
        }
        /// <summary>
        /// 打卡号的按钮；LostFocus事件是刚一失去焦点就判断这里是否正确，这里面为空或错误时判断
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCardNo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCardNo.Text))
            {
                System.Windows.MessageBox.Show("打卡号不能为空！");
                txtCardNo.Focus();
            }
        }
        /// <summary>
        /// 当身份证失去焦点(lostFocus)时,判断文本框里面的数据是否满足条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtStuNoId_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtStuNoId.Text))
            {
                System.Windows.MessageBox.Show("身份证号不能为空！");
                txtStuNoId.Focus();
            }
        }
        /// <summary>
        /// 当联系方式失去焦点时就开始判断文本框里面的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPhoneNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                System.Windows.MessageBox.Show("联系方式不能为空！");
                txtPhoneNumber.Focus();
            }
        }
        //1.第一步：点击从新上传照片
        //从上传照片，在通用层中记录一下这个图片路径
        common.BitmapImg img1 = new common.BitmapImg();
        /// <summary>
        /// 从新上传照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUploadPic_Click(object sender, RoutedEventArgs e)
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
        /// <summary>
        /// 从新拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenVideo_Click(object sender, RoutedEventArgs e)
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
