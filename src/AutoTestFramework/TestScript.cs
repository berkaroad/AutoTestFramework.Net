using AutoTestFramework.Models;
using System;
using System.Collections.Generic;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试脚本模型
    /// </summary>
    public class TestScript
    {
        private List<TestCommand> commandList = new List<TestCommand>();

        /// <summary>
        /// 测试脚本模型
        /// </summary>
        public TestScript() { }

        /// <summary>
        /// 测试脚本模型
        /// </summary>
        /// <param name="title"></param>
        /// <param name="startUrl"></param>
        public TestScript(string title, string startUrl)
        {
            Title = title;
            StartUrl = startUrl;
        }

        /// <summary>
        /// 测试脚本名
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 测试脚本的启动URL
        /// </summary>
        public string StartUrl { get; set; }

        /// <summary>
        /// 添加命令模型
        /// </summary>
        /// <param name="command"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void AddCommand(string command, string target, string value)
        {
            if (!String.IsNullOrEmpty(command))
            {
                var testCommand = new TestCommand(command, target, value);
                testCommand.RelatedScriptTitle = Title;
                commandList.Add(testCommand);
            }
        }

        /// <summary>
        /// 添加命令模型
        /// </summary>
        /// <param name="testCommand"></param>
        public void AddCommand(TestCommand testCommand)
        {
            if (testCommand != null && !String.IsNullOrEmpty(testCommand.Command))
            {
                testCommand.RelatedScriptTitle = Title;
                commandList.Add(testCommand);
            }
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
        /// 命令模型是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsCommandExists()
        {
            return commandList.Count > 0;
        }

        /// <summary>
        /// 通过使用测试数据来创建脚本
        /// </summary>
        /// <param name="testData"></param>
        /// <returns></returns>
        private TestScript CreateScriptWithData(TestData testData)
        {
            TestScript result = new TestScript();
            if (testData != null)
            {
                string currentFormName = "";
                FormData currentFormData = null;
                foreach (var command in GetCommandList())
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
