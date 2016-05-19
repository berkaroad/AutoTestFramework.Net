using AutoTestFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试用例
    /// </summary>
    [Serializable]
    public class TestCase
    {
        private List<TestCommand> commandList = new List<TestCommand>();

        private TestCase(string name, Uri testSiteStartUri, WebBrowserType browserType)
        {
            Name = name;
            TestSiteStartUri = testSiteStartUri;
            BrowserType = browserType;
        }

        /// <summary>
        /// 测试用例名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 测试站点URL
        /// </summary>
        public Uri TestSiteStartUri { get; private set; }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public WebBrowserType BrowserType { get; set; }

        /// <summary>
        /// 添加命令模型
        /// </summary>
        /// <param name="testCommand"></param>
        public void AddCommand(TestCommand testCommand)
        {
            if (testCommand != null && !String.IsNullOrEmpty(testCommand.Command))
            {
                commandList.Add(testCommand);
            }
        }

        /// <summary>
        /// 命令模型是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsCommandExists()
        {
            return commandList.Count > 0;
        }

        /// <summary>
        /// 获取命令模型列表
        /// </summary>
        /// <returns></returns>
        public IList<TestCommand> GetCommandList()
        {
            return commandList.AsReadOnly();
        }

        /// <summary>
        /// 运行测试用例
        /// </summary>
        /// <param name="testEngine"></param>
        /// <param name="testEngineUri"></param>
        /// <returns></returns>
        public TestResult Run(ITestEngine testEngine, Uri testEngineUri)
        {
            return Run(testEngine, testEngineUri, new TimeSpan(0, 0, 30));
        }

        /// <summary>
        /// 运行测试用例
        /// </summary>
        /// <param name="testEngine"></param>
        /// <param name="testEngineUri"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public TestResult Run(ITestEngine testEngine, Uri testEngineUri, TimeSpan commandTimeout)
        {
            TestResult result = new TestResult(Name);
            if (testEngine != null)
            {
                testEngine.Prepare(testEngineUri, TestSiteStartUri, BrowserType, commandTimeout);
                testEngine.Start();
                foreach (var command in GetCommandList())
                {
                    testEngine.ExecuteCommand(command);
                    var errorInfo = testEngine.GetTestErrorInfo();
                    if (errorInfo != null)
                    {
                        result.AddResultItem(errorInfo.AssertName, errorInfo);
                        break;
                    }
                    else if(command.IsAssertMark())
                    {
                        result.AddResultItem(command.AssertName, null);
                    }
                }
                testEngine.Stop();
            }
            return result;
        }

        /// <summary>
        /// 创建测试用例
        /// </summary>
        /// <param name="testScript"></param>
        /// <param name="browserType"></param>
        /// <returns></returns>
        public static TestCase Create(TestScript testScript, WebBrowserType browserType)
        {
            TestCase result = null;
            if (testScript != null)
            {
                result = new TestCase(testScript.Title, new Uri(testScript.StartUrl), browserType);
                foreach (var command in testScript.GetCommandList())
                {
                    var clonedCommand = command.Clone();
                    result.AddCommand(clonedCommand);
                }
            }
            return result;
        }

        /// <summary>
        /// 创建测试用例
        /// </summary>
        /// <param name="name"></param>
        /// <param name="testSiteStartUri"></param>
        /// <param name="browserType"></param>
        /// <param name="commands"></param>
        /// <param name="testData"></param>
        /// <returns></returns>
        public static TestCase Create(string name, Uri testSiteStartUri, WebBrowserType browserType, IEnumerable<TestCommand> commands, TestData testData)
        {
            TestCase result = new TestCase(name, testSiteStartUri, browserType);
            if (commands != null && testData != null)
            {
                string currentFormName = "";
                FormData currentFormData = null;
                foreach (var command in commands)
                {
                    var clonedCommand = command.Clone();
                    if (!String.IsNullOrEmpty(clonedCommand.TargetVarName))
                    {
                        clonedCommand.Target = "";// 原有值必须清空
                    }
                    if (!String.IsNullOrEmpty(clonedCommand.ValueVarName))
                    {
                        clonedCommand.Value = "";// 原有值必须清空
                    }
                    clonedCommand.Value = "";
                    if (!String.IsNullOrEmpty(clonedCommand.FormMark))
                    {
                        if (clonedCommand.IsStartFormMark())
                        {
                            currentFormName = clonedCommand.GetFormName();
                            currentFormData = testData.FindFormData(currentFormName);
                        }
                        else if (clonedCommand.IsEndFormMark())
                        {
                            currentFormName = "";
                        }
                    }

                    if (!String.IsNullOrEmpty(currentFormName) && clonedCommand.IsFormFieldMark())
                    {
                        var fieldName = clonedCommand.ValueVarName;
                        if (currentFormData != null)
                        {
                            clonedCommand.Value = currentFormData.FindFieldValue(fieldName);
                        }
                    }

                    if (clonedCommand.IsAssertMark())
                    {
                        var assertName = clonedCommand.AssertName;
                        var expectedValueName = clonedCommand.GetAssertVarName();
                        var currentAssertData = testData.FindAssertData(assertName);
                        if (currentAssertData != null)
                        {
                            clonedCommand.SetAssertVarValue(currentAssertData.ExpectedValue);
                        }
                    }

                    result.AddCommand(clonedCommand);
                }
            }
            return result;
        }
    }
}
