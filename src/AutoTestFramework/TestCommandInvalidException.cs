using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试命令无效
    /// </summary>
    public class TestCommandInvalidException : System.Exception
    {
        /// <summary>
        /// 测试命令无效
        /// </summary>
        public TestCommandInvalidException() : base("Command is invalid.") { }
    }
}
