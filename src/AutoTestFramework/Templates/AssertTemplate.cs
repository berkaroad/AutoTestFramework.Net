
namespace AutoTestFramework.Templates
{
    /// <summary>
    /// 断言模板，通过<see cref="TestDataTemplate"/>访问
    /// </summary>
    public class AssertTemplate
    {
        /// <summary>
        /// 断言模板
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expectedVariableName"></param>
        internal AssertTemplate(string assertName, string expectedVariableName)
        {
            AssertName = assertName;
            ExpectedVariableName = expectedVariableName;
        }

        /// <summary>
        /// 断言名
        /// </summary>
        public string AssertName { get; private set; }

        /// <summary>
        /// 预期值变量名
        /// </summary>
        public string ExpectedVariableName { get; set; }
    }
}
