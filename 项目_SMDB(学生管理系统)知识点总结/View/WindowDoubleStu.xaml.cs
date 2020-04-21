using System;
using System.Collections.Generic;
using System.IO;//MemoryStream
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
using StudentManagerModel.ObjExt;//实体层中把ObjExt里面的继承的Student和Cladss的合成显示出来
using Common;//要对序列化和反序列化使用

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// WindowDoubleStu.xaml 的交互逻辑
    /// </summary>
    public partial class WindowDoubleStu : Window
    {
        /// <summary>
        /// 这个也是构造函数所以里面可以传参数
        /// </summary>
        // 下面的函数有了StudentsExt参数类型,这样我们只需要把在FrmStuManager这个窗口的数据以参数的形式传递进来;这个函数就可以访问到数据了
        public WindowDoubleStu(StudentsExt ext)
        {
            InitializeComponent();
            //把参数类型StudentsExt的学生ID赋给Stuext
            Stuext = ext.StudentId;

            //在主窗口放一个lable名字Id测试
            //Id.Content = ext.StudentName;
            this.Title = ext.StudentName + "-信息";
            //地址
            lblAddress.Content = ext.StudentAddress;
            //年龄
            lblAge.Content = ext.Age;
            //出生日期
            lblBirthday.Content = ext.Birthday.ToString();
            //考勤卡号
            lblCardNo.Content = ext.CardNo;
            //课程名字
            lblClassName.Content = ext.ClassName;
            //性别
            lblGender.Content = ext.Gender;
            //学生名字
            lblName.Content = ext.StudentName;
            //学生电话
            lblPhoneNumber.Content = ext.PhoneNumber;
            //学生ID
            lblStuId.Content = ext.StudentId;
            //学生身份证号
            lblStuNoId.Content = ext.StudentIdNo;


            
            //学生图片
            //if这个图片为空时进入绑定这个图片
            if (string.IsNullOrEmpty(ext.StuImage))
            {
                //在创建的时候直接用image图片装
                stuImg.Source = new BitmapImage(new Uri("/img1/bg/zwzp.jpg", UriKind.RelativeOrAbsolute));
            }
            else
            {
                //1.第一步给通用层Common中添加序列化和反序列化
                //2.先反序列化

                //1.现在通用层里面添加一个类,用里面的属性来实现序列化的转变
                //2.反序列化在数据库中获取
                //(1)如果学员的Iamge字段中能够查询到数据，那么就可以直接将这个数据反序列化成BitmapImage对象
                //(2)反序列化就可以从数据库中那图片了
                //如果学员的Iamge字段中能够查询到数据，那么就可以直接将这个数据反序列化成BitmapImage对象
                //(3)要求封装好的
                /*BitmapImage image = Common.BinaryStuObjcet.FBinaryForStu(ext.StuImage) as BitmapImage;*/
                //2.获取到ext.StuImage的名字
                common.BitmapImg image = BinaryStuObjcet.FBinaryForStu(ext.StuImage) as common.BitmapImg;
                //3.实例化一下这个装图片盒子
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                //获取反序列化的内容
                bitmap.StreamSource = new MemoryStream(image.Buffer);
                bitmap.EndInit();
                //图片名字的路径上赋一个bitmap
                stuImg.Source = bitmap;

                /* image.Buffer = image.Buffer;
                 BitmapImage bitmap = new BitmapImage();
                 bitmap.BeginInit();
                 bitmap.StreamSource = new MemoryStream(image.Buffer);
                 bitmap.EndInit();
                 stuImg.Source = bitmap;*/

                /* //如果学员的Iamge字段中能够查询到数据，那么就可以直接将这个数据反序列化成BitmapImage对象
                 common.BitmapImg image = SerializeObjectTostring.DeserializeObject(stu.StuImage) as common.BitmapImg;
                 BitmapImage bitmap = new BitmapImage();
                 bitmap.BeginInit();
                 bitmap.StreamSource = new MemoryStream(image.Buffer);
                 bitmap.EndInit();
                 stuImg.Source = bitmap;*/
            }
        }
        /// <summary>
        /// 在这里创建一个属性，这个属性用来接收WindowDoubleStu的参数
        /// </summary>
        public int Stuext { get; set; }
    }
}
