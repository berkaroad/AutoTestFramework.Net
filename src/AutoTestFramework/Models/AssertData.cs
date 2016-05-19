
namespace AutoTestFramework.Models
{
    /// <summary>
    /// 断言数据模型，通过<see cref="TestData"/>访问
    /// </summary>
    public class AssertData
    {
        /// <summary>
        /// 断言数据模型
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expectedValue"></param>
        internal AssertData(string assertName, string expectedValue)
        {
            AssertName = assertName;
            ExpectedValue = expectedValue;
        }

        /// <summary>
        /// 断言名
        /// </summary>
        public string AssertName { get; private set; }

        /// <summary>
        /// 预期值
        /// </summary>
        public string ExpectedValue { get; set; }
    }
}
