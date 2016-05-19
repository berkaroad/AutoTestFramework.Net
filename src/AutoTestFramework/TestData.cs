using AutoTestFramework.Models;
using System.Collections.Generic;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试数据模型
    /// </summary>
    public class TestData
    {
        private List<FormData> formDataList = new List<FormData>();
        private List<AssertData> assertDataList = new List<AssertData>();

        /// <summary>
        /// 测试数据模型
        /// </summary>
        public TestData() { }

        /// <summary>
        /// 查找表单数据模型
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public FormData FindFormData(string formName)
        {
            return formDataList.Find(m => m.FormName.Equals(formName));
        }

        /// <summary>
        /// 添加表单数据模型
        /// </summary>
        /// <param name="formName"></param>
        public void AddFormData(string formName)
        {
            var formDataModel = FindFormData(formName);
            if (formDataModel == null)
            {
                formDataModel = new FormData(formName);
                formDataList.Add(formDataModel);
            }
        }

        /// <summary>
        /// 获取表单数据模型列表
        /// </summary>
        /// <returns></returns>
        public IList<FormData> GetFormDataList()
        {
            return formDataList.AsReadOnly();
        }

        /// <summary>
        /// 查找断言数据模型
        /// </summary>
        /// <param name="assertName"></param>
        /// <returns></returns>
        public AssertData FindAssertData(string assertName)
        {
            return assertDataList.Find(m => m.AssertName.Equals(assertName));
        }

        /// <summary>
        /// 添加断言数据模型
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expectedValue"></param>
        public void AddAssertData(string assertName, string expectedValue)
        {
            var assertDataModel = FindAssertData(assertName);
            if (assertDataModel == null)
            {
                assertDataModel = new AssertData(assertName, expectedValue);
                assertDataList.Add(assertDataModel);
            }
        }

        /// <summary>
        /// 获取断言数据模型列表
        /// </summary>
        /// <returns></returns>
        public IList<AssertData> GetAssertDataList()
        {
            return assertDataList.AsReadOnly();
        }
    }
}
