using AutoTestFramework.Models;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试脚本解析器
    /// </summary>
    public interface ITestScriptParser
    {
        /// <summary>
        /// 解析为测试脚本模型
        /// </summary>
        /// <param name="scriptString"></param>
        /// <returns></returns>
        TestScript Parse(string scriptString);
    }
}
