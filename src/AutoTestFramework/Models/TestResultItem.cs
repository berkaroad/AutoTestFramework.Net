using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework.Models
{
    /// <summary>
    /// 测试结果项
    /// </summary>
    [Serializable]
    public class TestResultItem
    {
        /// <summary>
        /// 测试结果模型
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="errorInfo"></param>
        internal TestResultItem(string assertName, AssertUnexpectedException errorInfo)
        {
            AssertName = assertName;
            ErrorDetailInfo = errorInfo;
            ErrorMessage = errorInfo == null ? "" : errorInfo.Message;
        }

        /// <summary>
        /// 断言名
        /// </summary>
        [System.ComponentModel.DisplayName("断言")]
        public string AssertName { get; private set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        [System.ComponentModel.DisplayName("测试结果")]
        public string Result { get { return IsSuccessful ? "Y" : "N"; } }

        /// <summary>
        /// 是否成功
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public bool IsSuccessful { get { return String.IsNullOrEmpty(ErrorMessage); } }

        /// <summary>
        /// 错误信息
        /// </summary>
        [System.ComponentModel.DisplayName("错误信息")]
        public string ErrorMessage { get; private set; }

        /// <summary>
        /// 错误详细信息
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public AssertUnexpectedException ErrorDetailInfo { get; private set; }
    }
}
