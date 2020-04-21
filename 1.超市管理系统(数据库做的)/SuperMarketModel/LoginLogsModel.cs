using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarketModel
{
    /// <summary>
    /// 在日志记录销售员的各种信息
    /// </summary>
    [Serializable]
    public class LoginLogsModel
    {
        public int LogId { get; set; }
        public int LoginId { get; set; }
        public string SPName { get; set; }
        public string ServerName { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? ExitTime { get; set; }
    }
}
