using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;//倒入配制文件
using System.Data.SqlClient;
using System.Data;

namespace SuperMarketDAL
{
     class SQLHelper
    {
        static string Constr = ConfigurationManager.ConnectionStrings["Constr"].ConnectionString;
        //存储过程的增删改数据
        public static int ExecuteNonQuery(string ProcName,SqlParameter[] sp)
        {
            SqlConnection con = new SqlConnection(Constr);
           

            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = ProcName;
            if (sp!=null)
            {
                com.Parameters.AddRange(sp);
            }
            try
            {
                con.Open();
                int a = com.ExecuteNonQuery();
                return a;
            }
            catch (Exception)
            {
                return -1;
                /*//记入系统日志
                throw ex;*/
            }
            finally 
            {
                con.Close();
            }
        }
        /// <summary>
        /// 存储过程的单一数据
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static Object ExecuteScalar(string ProcName, SqlParameter[] sp)
        {
            SqlConnection con = new SqlConnection(Constr);


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            //获取数据库中的存储过程名称
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = ProcName;
            if (sp != null)
            {
                com.Parameters.AddRange(sp);
            }
            try
            {
                con.Open();
                object a = com.ExecuteScalar();
                return a;
            }
            catch (Exception ex)
            {
                //return null;
                //记入系统日志
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        /// <summary>
        /// 存储过程的的表一行一行拿到
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static SqlDataReader GetDataReader(string ProcName, SqlParameter[] sp)
        {
            SqlConnection con = new SqlConnection(Constr);
            SqlCommand com = new SqlCommand();
            //获取数据库中的存储过程名称
            com.CommandType = CommandType.StoredProcedure;
            com.Connection = con;
            com.CommandText = ProcName;
            if (sp != null)
            {
                com.Parameters.AddRange(sp);
            }
            try
            {
                con.Open();
                return com.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                //return null;
                //记入系统日志
                throw ex;
            }
        }
        /// <summary>
        /// 存储过程的整个表拿到
        /// </summary>
        /// <param name="ProcName"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string ProcName, SqlParameter[] sp)
        {
            SqlConnection con = new SqlConnection(Constr);


            SqlCommand com = new SqlCommand();
            com.Connection = con;
            //获取数据库中的存储过程名称
            com.CommandType = CommandType.StoredProcedure;
            com.CommandText = ProcName;
            if (sp != null)
            {
                com.Parameters.AddRange(sp);
            }
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter(com);
                DataTable data = new DataTable();
                adapter.Fill(data);
                return data;
            }
            catch (Exception)
            {
                return null;
                /*//记入系统日志
                throw ex;*/
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// 处理一个事务(事务里面时添加)
        /// </summary>
        /// <param name="procList">所有存储过程的名称容器</param>
        /// <param name="pslist">list放泛型集合的参数容器</param>
        /// <returns></returns>
        public static bool UpdateByTran(List<string> procList, List<SqlParameter[]> pslist)
        {
            SqlConnection sqlcon = new SqlConnection(Constr);
            SqlCommand cmd = new SqlCommand();
            //获取数据库中的存储过程名称
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlcon;
            try
            {
                sqlcon.Open();
                //BeginTransaction()开始数据库中的事务
                cmd.Transaction = sqlcon.BeginTransaction();
                //把所有获取的存储过程都要执行完，所以用foreach把每一个存储过程名字遍历出来
                foreach (string procName in procList)
                {
                    //获取接收进来的存储过程的名字
                    cmd.CommandText = procName;

                    //IndexOf()搜索指定的对象，并返回整个list泛型第一个匹配项的从0开始的索引位置
                    //procName表示存储过程名字
                    //procList.IndexOf(procName)表示找到procName存储过程这个名字的位置的参数
                    //pslist[procList.IndexOf(procName)]
                    //参数（获取存储过程名字里面的参数）具体的位置不为空
                    if (pslist[procList.IndexOf(procName)] != null)
                    {
                        //pslist[procList.IndexOf(procName)]的参数放在cmd.Parameters里面
                        //AddRange给数组末尾添加参数
                        cmd.Parameters.AddRange(pslist[procList.IndexOf(procName)]);
                    }
                    //每次影响（执行）一行
                    cmd.ExecuteNonQuery();
                    //把参数清除一下
                    cmd.Parameters.Clear();
                }
                //所有的存储过程执行完没有异常提交
                cmd.Transaction.Commit();//提交事务
                return true;
            }
            catch (Exception ex)
            {
                //如果抛异常事务回滚
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();//事务回滚
                }
                //注：可以放个日志
                return false;
            }
            finally
            {
                //不管事务是提交还是回滚,都要清空
                if (cmd.Transaction != null)
                {
                    
                    cmd.Transaction = null;
                }
                //关闭数据库
                sqlcon.Close();
                //注：可以放个日志
            }
        }
    }
}
