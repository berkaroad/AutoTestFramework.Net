using AutoTestFramework.Models;
using System;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试引擎
    /// </summary>
    public interface ITestEngine : IScreenShot, IDisposable
    {
        /// <summary>
        /// 初始化准备
        /// </summary>
        /// <param name="testEngineUri"></param>
        /// <param name="testSiteStartUri"></param>
        /// <param name="browserType"></param>
        /// <param name="commandTimeout"></param>
        void Prepare(Uri testEngineUri, Uri testSiteStartUri, WebBrowserType browserType, TimeSpan commandTimeout);

        /// <summary>
        /// 启动测试引擎
        /// </summary>
        void Start();

        /// <summary>
        /// 获取测试错误信息
        /// </summary>
        /// <returns></returns>
        AssertUnexpectedException GetTestErrorInfo();

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        void ExecuteCommand(string command, string target = "", string value = "");

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        void ExecuteCommand(TestCommand command);

        /// <summary>
        /// 终止测试引擎
        /// </summary>
        void Stop();
    }
}
