using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 标记为测试命令
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// 命令名
        /// </summary>
        public string Command { get; set; }
    }
}
