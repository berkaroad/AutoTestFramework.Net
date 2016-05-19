using System.Collections.Generic;
using System.Linq;

namespace AutoTestFramework.Models
{
    /// <summary>
    /// 表单数据模型，通过<see cref="TestData"/>访问
    /// </summary>
    public class FormData
    {
        private Dictionary<string, string> fieldNameValues = new Dictionary<string, string>();

        internal FormData(string formName)
        {
            FormName = formName;
        }

        /// <summary>
        /// 表单名
        /// </summary>
        public string FormName { get; private set; }

        /// <summary>
        /// 查找字段值
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string FindFieldValue(string fieldName)
        {
            if (fieldNameValues.ContainsKey(fieldName))
            {
                return fieldNameValues[fieldName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public void AddField(string fieldName, string fieldValue)
        {
            if (!fieldNameValues.ContainsKey(fieldName))
            {
                fieldNameValues.Add(fieldName, fieldValue);
            }
        }

        /// <summary>
        /// 获取字段列表
        /// </summary>
        /// <returns></returns>
        public IList<KeyValuePair<string, string>> GetFieldList()
        {
            return fieldNameValues.ToList().AsReadOnly();
        }
    }
}
