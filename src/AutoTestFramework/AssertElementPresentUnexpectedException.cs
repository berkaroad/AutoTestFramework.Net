using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 断言元素存在未预期异常
    /// </summary>
    public class AssertElementPresentUnexpectedException : AssertEqualUnexpectedException
    {
        /// <summary>
        /// 断言元素存在未预期异常
        /// </summary>
        /// <param name="locator"></param>
        public AssertElementPresentUnexpectedException(string locator) : this(locator, null) { }

        /// <summary>
        /// 断言元素存在未预期异常
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="innerException"></param>
        public AssertElementPresentUnexpectedException(string locator, Exception innerException) : base(String.Format("Element {0} is present", locator), "Present", "Unpresent", innerException)
        {
            Locator = locator;
        }

        /// <summary>
        /// 元素定位器
        /// </summary>
        public string Locator { get; private set; }
    }
}
