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
using WPFMediaKit.DirectShow.Controls;//摄像头控件
using System.IO;

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// StudentPhoto.xaml 的交互逻辑
    /// </summary>
    public partial class StudentPhoto : Window
    {
        /// <summary>
        /// 拍照窗口
        /// </summary>
        public StudentPhoto()
        {
            InitializeComponent();
        }
        //1.在主窗口---右键---》NuGet程序包
        //2.打开NuGet后在浏览里搜索WPFMediaKit安装包（先下载一个第三方组件（作用调用摄像头））
        //3.下载的第三方组件叫WPFMediaKit安装包
        //4.下载完成后(FMediaKit安装包在这个项目里面)到主文件的目录里面----找----->packages文件(所有NuGet下载的文件都在里面)---在lib文件找--->WPFMediaKit文件WPF---找--->MediaKit.dll--->复制这个文件放到根目录里面
        //5以上弄完后把(下载时会新出现一个WPFMediaKit.dll)引用里把它删除---在右键引用----》在浏览器中找到MediaKit.dll添加（如果找不到在最右下角添加浏览）
        //6.可以在设计界面添加命名空间，这样就可以用这个摄像组件，中的东西

        /// <summary>
        /// 加载框时判断系统是否有摄像头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //检测系统连接的摄像头
            //判断大于0表示系统至少有1个以上摄像头可以用
            if (MultimediaUtil.VideoInputNames.Length > 0)
            {
                //当前的拍照设备采用默认摄像头
                picture.VideoCaptureSource = MultimediaUtil.VideoInputNames[0];
            }
            else
            {
                MessageBox.Show("您的设备暂无摄像设备链接！", "提示");
                this.Close();
            }
        }
        /// <summary>
        /// 从拍按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //重新拍照先把摄像头显示，把预览关闭
            picture.Visibility = Visibility.Visible;
            pictrueYulan.Visibility = Visibility.Hidden;
        }
        //照片纸：照相机所照到的地方保存起来
        RenderTargetBitmap RTB = null;
        /// <summary>
        /// 拍照按钮；RTB=照相机，原因RTB.Render(picture);相当于放进去照相机
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClickPhoto_Click(object sender, RoutedEventArgs e)
        {
            //创建照片显示
            //（1）参数2：摄像头区域的宽度（2）参数2：摄像头区域的高度（3）参数3/4的像素都是七（4）参数5：像素集采用彩色照片
            RTB = new RenderTargetBitmap((int)picture.Width, (int)picture.Height, 96, 96, PixelFormats.Default);
            //将(picture)摄像头中的拍到的内容画到纸上
            RTB.Render(picture);
            //图片预览放在一个image(pictrueYulan名字)标签的路径里面；就是把RTB获取到的图片放在pictrueYulan里面
            pictrueYulan.Source = RTB;
            //（1）预览图显示，（2）摄像头隐藏
            pictrueYulan.Visibility = Visibility.Visible;
            picture.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSavePic_Click(object sender, RoutedEventArgs e)
        {
            //如果照片纸还是NULL则是未拍照
            if (RTB == null)
            {
                MessageBox.Show("请重新拍照！", "提示");
                //显示摄像头(picture)；隐藏预览区(pictrueYulan):就是把一张图片显示在后面，如果确定里面传递进去照片，则显示在前面
                picture.Visibility = Visibility.Visible;
                pictrueYulan.Visibility = Visibility.Hidden;
                return;
            }
            //选择路径保存照片
            Microsoft.Win32.SaveFileDialog fileDialog = new Microsoft.Win32.SaveFileDialog();
            //这是文件可以满足的文件条件
            fileDialog.Filter = "图像文件(*.jpg;*.jpeg;*.gif;*.png;*.bmp)|*.jpg;*.jpeg;*.gif;*.png;*.bmp";
            //文件默认框的名字
            fileDialog.Title = "保存照片";
            //文件默认的名字
            fileDialog.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            //当按确定时
            if (fileDialog.ShowDialog() == true)
            {
                //创建BitmapEncoder图像以流的形式
                //将刚才的照片以流的方式进行保存
                BitmapEncoder encoder = new PngBitmapEncoder();
                //把照片纸放在encoder这个里面
                encoder.Frames.Add(BitmapFrame.Create(RTB));
                using (MemoryStream stream = new MemoryStream())
                {
                    //将位图图像编码为指定的stream流
                    encoder.Save(stream);
                    //把这个流赋给byte数组
                    byte[] buffer = stream.ToArray();
                    //创建一个新文件给里面写只定的byte数组
                    File.WriteAllBytes(fileDialog.FileName, buffer);
                    MessageBox.Show("照片保存成功！", "提示");
                    //刷新修改界面的照片;就是修改时把这个图片的名字给UpdateStuInfor.imgPath这个窗口的路径传递进去；在修改装口中上传照相机拍照的路径
                    UpdateStuInfor.imgPath = fileDialog.FileName;
                    //摄像头显示；预览区隐藏
                    picture.Visibility = Visibility.Visible;
                    pictrueYulan.Visibility = Visibility.Hidden;
                }
            }
        }
    }
}
