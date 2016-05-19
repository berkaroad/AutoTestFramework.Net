using AutoTestFramework.Models;
using System;
using System.Collections.Generic;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试结果模型
    /// </summary>
    [Serializable]
    public class TestResult
    {
        private bool testResult = true;
        private List<TestResultItem> resultItems = new List<TestResultItem>();

        /// <summary>
        /// 测试结果模型
        /// </summary>
        /// <param name="testCaseName"></param>
        internal TestResult(string testCaseName)
        {
            TestCaseName = testCaseName;
        }

        /// <summary>
        /// 测试用例名称
        /// </summary>
        [System.ComponentModel.DisplayName("测试用例")]
        public string TestCaseName { get; private set; }

        /// <summary>
        /// 添加测试结果项
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="errorInfo"></param>
        internal void AddResultItem(string assertName, AssertUnexpectedException errorInfo)
        {
            var resultItem = new TestResultItem(assertName, errorInfo);
            testResult = testResult && resultItem.IsSuccessful;
            resultItems.Add(resultItem);
        }

        /// <summary>
        /// 测试结果
        /// </summary>
        public bool Result
        {
           get { return testResult; }
        }

        /// <summary>
        /// 获取测试结果项列表
        /// </summary>
        /// <returns></returns>
        public IList<TestResultItem> GetResultItemList()
        {
            return resultItems.AsReadOnly();
        }
    }
}
