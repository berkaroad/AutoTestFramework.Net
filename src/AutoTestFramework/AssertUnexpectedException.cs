using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 断言未预期异常
    /// </summary>
    public class AssertUnexpectedException : Exception
    {
        /// <summary>
        /// 断言未达到预期异常
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="innerException"></param>
        public AssertUnexpectedException(string assertName, string expected, string actual, Exception innerException) : base(String.Format("Assert \"{0}\" is unexpected.", assertName), innerException)
        {
            AssertName = assertName;
            Expected = expected;
            Actual = actual;
        }

        /// <summary>
        /// 断言名称
        /// </summary>
        public string AssertName { get; protected set; }

        /// <summary>
        /// 预期值
        /// </summary>
        public string Expected { get; protected set; }

        /// <summary>
        /// 实际值
        /// </summary>
        public string Actual { get; protected set; }
    }
}
