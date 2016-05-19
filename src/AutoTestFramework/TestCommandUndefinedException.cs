using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试命令在特定的测试引擎中未定义
    /// </summary>
    public class TestCommandUndefinedException : Exception
    {
        /// <summary>
        /// 测试命令在特定的测试引擎中未定义
        /// </summary>
        /// <param name="command"></param>
        /// <param name="testEngineType"></param>
        public TestCommandUndefinedException(string command, Type testEngineType) : base(String.Format("The command \"{0}\" is undefined in specific TestEngine \"{1}\"!", command, testEngineType.FullName))
        {
            Command = command;
            TestEngineType = testEngineType;
        }

        /// <summary>
        /// 测试命令
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// 测试引擎类型
        /// </summary>
        public Type TestEngineType { get; private set; }
    }
}
