using System.Collections.Generic;

namespace AutoTestFramework.Templates
{
    /// <summary>
    /// 表单模板，通过<see cref="TestDataTemplate"/>访问
    /// </summary>
    public class FormTemplate
    {
        private List<string> fields = new List<string>();

        internal FormTemplate(string formName)
        {
            FormName = formName;
        }

        /// <summary>
        /// 表单名
        /// </summary>
        public string FormName { get; private set; }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fieldName"></param>
        public void AddField(string fieldName)
        {
            if (!fields.Contains(fieldName))
            {
                fields.Add(fieldName);
            }
        }

        /// <summary>
        /// 获取表单字段列表
        /// </summary>
        public IList<string> GetFieldList()
        {
            return fields.AsReadOnly();
        }
    }
}
