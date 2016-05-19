using AutoTestFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试命令上下文
    /// </summary>
    public class TestCommandContext
    {
        private List<AssertUnexpectedException> testWarnings = new List<AssertUnexpectedException>();

        internal TestCommandContext(TestCommand commandModel)
        {
            CommandModel = commandModel;
        }

        /// <summary>
        /// 测试命令模型
        /// </summary>
        public TestCommand CommandModel { get; private set; }

        /// <summary>
        /// 测试错误
        /// </summary>
        public AssertUnexpectedException TestError { get; internal set; }

        /// <summary>
        /// 记录测试警告
        /// </summary>
        /// <param name="ex"></param>
        private void RecordTestWarning(AssertUnexpectedException ex)
        {
            testWarnings.Add(ex);
        }
    }
}
