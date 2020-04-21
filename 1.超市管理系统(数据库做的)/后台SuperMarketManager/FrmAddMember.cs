using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperMarketModel;
using SuperMarketIBLL.后台SuperMarketManager;
using SuperMarketBLL.后台SuperMarketManager;

namespace 后台SuperMarketManager
{
    public partial class FrmAddMember : Form
    {
        public FrmAddMember()
        {
            InitializeComponent();
            //获取会员名字焦点
            txtmemberName.Focus();
        }
        //实例化添加会员的类，关于会员的实现
        ISuperMarketMemberManager3 manager3=new SuperMarketMemberManager3();


        //确认注册
        private void btnSure_Click(object sender, EventArgs e)
        {
            if (txtmemberName.CheckNullOrEmpty()*txtmembertel.CheckDataD(@"^1\d[10]$","手机号格式有误！")!=0)
            {
                //【1】manager3.GETMemberByIdOrPhone(txtmembertel.Text.Trim())：
                //【2】通过获取Id或手机号获取这个txtmembertel.Text文本框中的内容是否是空；（不等于空表示调用获取到数据）
                //【3】在C#中的作用是判断这个Id编号或手机号是否在数据库中可以拿到
                if (manager3.GETMemberByIdOrPhone(txtmembertel.Text.Trim()) != null)
                {
                    MessageBox.Show("改账号已经被注册", "提示");
                    return;
                }
                SMMembersModel10 members = new SMMembersModel10()
                {
                    MemberName = txtmemberName.Text.Trim(),
                    PhoneNumber = txtmembertel.Text.Trim(),
                    MemberAddress = string.IsNullOrEmpty( txtAddress.Text.Trim())?"地址不详":txtAddress.Text.Trim()
                };

                //获取添加会员的内容：把members原有的数据放在AddMember()方法里面，然后把添加会员的内容返回给members这样表示members不为空
                members = manager3.AddMember(members);
                //表示members已经添加成功，members不为空
                if (members != null)
                {
                    //members不为空，表示已经添加到数据库，然后把窗口里面的内容清空
                    if (MessageBox.Show($"注册成功！ 会员账号是【{members.MemberId}】\r\n是否继续注册？", "提示", MessageBoxButtons.YesNo) == DialogResult.OK)
                    {
                        txtmemberName.Text = "";
                        txtmembertel.Text = "";
                        txtAddress.Text = "";
                        txtmemberName.Focus();//获取焦点
                    }
                }
                else
                {
                    MessageBox.Show("注册失败请稍后重新试");
                }
            }
        }
        //取消注册
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
