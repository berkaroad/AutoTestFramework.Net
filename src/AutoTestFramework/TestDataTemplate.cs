using AutoTestFramework.Templates;
using System.Collections.Generic;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试数据模板
    /// </summary>
    public class TestDataTemplate
    {
        private List<FormTemplate> formTmplList = new List<FormTemplate>();
        private List<AssertTemplate> assertTmplList = new List<AssertTemplate>();

        /// <summary>
        /// 测试数据模板
        /// </summary>
        public TestDataTemplate() { }

        /// <summary>
        /// 查找表单模板
        /// </summary>
        /// <param name="formName"></param>
        /// <returns></returns>
        public FormTemplate FindFormTemplate(string formName)
        {
            return formTmplList.Find(m => m.FormName.Equals(formName));
        }

        /// <summary>
        /// 添加表单模板
        /// </summary>
        /// <param name="formName"></param>
        public void AddFormTemplate(string formName)
        {
            var formTmpl = FindFormTemplate(formName);
            if (formTmpl == null)
            {
                formTmpl = new FormTemplate(formName);
                formTmplList.Add(formTmpl);
            }
        }

        /// <summary>
        /// 查找断言模板
        /// </summary>
        /// <param name="assertName"></param>
        /// <returns></returns>
        public AssertTemplate FindAssertTemplate(string assertName)
        {
            return assertTmplList.Find(m => m.AssertName.Equals(assertName));
        }

        /// <summary>
        /// 添加断言模板
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expectedVarName"></param>
        public void AddAssertTemplate(string assertName, string expectedVarName)
        {
            var assertTmpl = FindAssertTemplate(assertName);
            if (assertTmpl == null)
            {
                assertTmpl = new AssertTemplate(assertName, expectedVarName);
                assertTmplList.Add(assertTmpl);
            }
        }

        /// <summary>
        /// 获取表单模板列表
        /// </summary>
        /// <returns></returns>
        public IList<FormTemplate> GetFormTemplateList()
        {
            return formTmplList.AsReadOnly();
        }

        /// <summary>
        /// 获取断言模板列表
        /// </summary>
        /// <returns></returns>
        public IList<AssertTemplate> GetAssertTemplateList()
        {
            return assertTmplList.AsReadOnly();
        }
    }
}
