using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试命令调用异常
    /// </summary>
    public class TestCommandInvokeException: Exception
    {
        /// <summary>
        /// 测试命令调用异常
        /// </summary>
        /// <param name="command"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="testEngineType"></param>
        public TestCommandInvokeException(string command, string target, string value, Type testEngineType) : this(command,target, value, testEngineType, null) { }

        /// <summary>
        /// 测试命令调用异常
        /// </summary>
        /// <param name="command"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="testEngineType"></param>
        /// <param name="innerException"></param>
        public TestCommandInvokeException(string command, string target, string value, Type testEngineType, Exception innerException) : base(String.Format("The command \"{0}:{1},{2}\" invoke fail in specific TestEngine \"{3}\"!", command, target, value, testEngineType.FullName), innerException)
        {
            Command = command;
            Target = target;
            Value = value;
            TestEngineType = testEngineType;
        }

        /// <summary>
        /// 测试命令
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// 参数1
        /// </summary>
        public string Target { get; private set; }

        /// <summary>
        /// 参数2
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 测试引擎类型
        /// </summary>
        public Type TestEngineType { get; private set; }
    }
}
