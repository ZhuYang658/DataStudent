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
using System.IO;//MemoryStream
using System.IO.Packaging;//Package
using System.Windows.Threading;//DispatcherOperation
using System.Windows.Xps.Packaging;//XpsDocument的命名空间,这个空间需要在引用里添加Reachframework控件
using System.Windows.Xps;//XpsDocumentWriter这个空间需要在引用里添加Reachframework控件
using StudentManagerModel.ObjExt;

namespace 项目_SMDB_学生管理系统_知识点总结.View
{
    /// <summary>
    /// PrintWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PrintWindow : Window
    {
        delegate void LoadXpsMethod();
        //
        FlowDocument doc;
        //表示用于与已发送到的操作进行交互的对象
        DispatcherOperation op = null;
        //参数1：字符串的获取一个窗口；参数2：表示获取到的StudentsExt学生的数据
        public PrintWindow(string strTmpname, object data)
        {
            InitializeComponent();
            //1.FlowDocument：表示载入一个FlowDocument模版；就相当于创建Flow Document流文档中的东西在这里显示
            //2.Application.LoadComponent表示加载的时候获取这个窗口
            //3.总结:创建实例化FlowDocument类型的意思就是表示这里可以放Flow Document流文档
            doc = (FlowDocument)Application.LoadComponent(new Uri("/common/" + strTmpname, UriKind.RelativeOrAbsolute));
            //设置边框据
            doc.PagePadding = new Thickness(50);
            //1.把学生的数据传给模版;2.模版就是学生的空框架;3.data就是把学生获取的所有数据给FlowDocument(流文档的窗口形成的)模版框架装
            doc.DataContext = data;
            /*1.设置延后的调用
             * 2.Dispatcher:获取Dispatcher这DispatcherObject与相关联
             * 3.BeginInvoke委托使用指定的参数
             */
            op = Dispatcher.BeginInvoke(new LoadXpsMethod(LoadXps), DispatcherPriority.ApplicationIdle);
            //“延后”调用，不然刚刚更改的数据不会马上更新，也就是说打印或者预览不到更新后的数据
            op.Completed += Op_Completed;//操作完成时发生
        }

        private void Op_Completed(object sender, EventArgs e)
        {
            if (op.Status == DispatcherOperationStatus.Completed)
            {
                doc.DataContext = null;
                op.Abort();
            }
        }

        void LoadXps()
        {
            MemoryStream stream = new MemoryStream();
            Package package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);
            Uri DocumentUri = new Uri("pack://InMemoryDocument.xps");
            PackageStore.RemovePackage(DocumentUri);
            PackageStore.AddPackage(DocumentUri, package);
            /*XpsDocument/XpsDocumentWriter这两个类型需要在引用里添加Reachframework控件：用户要求现场不允许安装office、页面预览显示必须要与文档完全一致，xps文档来对数据进行处理。Wpf的DocumentView 控件可以直接将数据进行显示，xps也是一种开放式的文档，如果我们能够替换里面的标签就最终实现了我们想要的效果。*/
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Fast, DocumentUri.AbsoluteUri);
            //将flow document写入基于内存的xps document中去
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);//在这里需要添加对.NET 4.0 的一些应用
            writer.Write(((IDocumentPaginatorSource)doc).DocumentPaginator);
            //获取这个基于内存的xps document的fixed documen
            docViewer.Document = xpsDocument.GetFixedDocumentSequence();
            //关闭基于内存的xps document
            xpsDocument.Close();
        }
    }
}
