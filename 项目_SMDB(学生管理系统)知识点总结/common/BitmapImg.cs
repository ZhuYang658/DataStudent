using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 项目_SMDB_学生管理系统_知识点总结.common
{
    //表识序列化
    [Serializable]
    class BitmapImg
    {
        /// <summary>
        /// 用这个 byte数组 属性来记录图片的路径
        /// 封装数据的序列化和反序列化的转换
        /// </summary>
        public byte[] Buffer { get; set; }
    }
}
