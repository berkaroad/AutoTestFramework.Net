using System;

namespace AutoTestFramework
{
    /// <summary>
    /// 断言相等未达到预期异常
    /// </summary>
    public class AssertEqualUnexpectedException : AssertUnexpectedException
    {
        /// <summary>
        /// 断言相等未达到预期异常
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public AssertEqualUnexpectedException(string assertName, string expected, string actual) : this(assertName, expected, actual, null) { }

        /// <summary>
        /// 断言相等未达到预期异常
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="innerException"></param>
        public AssertEqualUnexpectedException(string assertName, string expected, string actual, Exception innerException) : base(assertName, expected, actual, innerException) { }

        /// <summary>
        /// 错误消息
        /// </summary>
        public override string Message
        {
            get
            {
                return String.Format("Assert \"{0}\" is unexpected, expected is \"{1}\", actual is \"{2}\".", AssertName, Expected, Actual);
            }
        }
    }
}
