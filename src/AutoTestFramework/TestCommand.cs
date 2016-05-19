using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试命令模型
    /// </summary>
    [Serializable]
    public class TestCommand
    {
        /// <summary>
        /// 测试命令模型
        /// </summary>
        /// <param name="command"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public TestCommand(string command, string target, string value)
        {
            Command = command;
            Target = target;
            Value = value;
        }

        /// <summary>
        /// 命令
        /// </summary>
        public string Command { get; private set; }

        /// <summary>
        /// 参数1
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 参数2
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 参数1变量名
        /// </summary>
        [System.ComponentModel.DisplayName("Target变量")]
        public string TargetVarName { get; set; }

        /// <summary>
        /// 参数2变量名
        /// </summary>
        [System.ComponentModel.DisplayName("Value变量")]
        public string ValueVarName { get; set; }

        /// <summary>
        /// 表单标记
        /// </summary>
        /// <remarks>S:&lt;表单名&gt;；E:[&lt;表单名&gt;]</remarks>
        /// <example>S:登录；E:登录</example>
        [System.ComponentModel.DisplayName("表单标记")]
        [System.ComponentModel.Description("S:<表单名>；E:[<表单名>]")]
        public string FormMark { get; set; }

        /// <summary>
        /// 断言名
        /// </summary>
        [System.ComponentModel.DisplayName("断言名称")]
        public string AssertName { get; set; }

        /// <summary>
        /// 关联的测试脚本标题（可选）
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public string RelatedScriptTitle { get; internal set; }

        /// <summary>
        /// 是否是表单开始标记
        /// </summary>
        /// <returns></returns>
        public bool IsStartFormMark()
        {
            return !String.IsNullOrEmpty(FormMark) && FormMark.Length > 2 && FormMark.StartsWith("S:");
        }

        /// <summary>
        /// 是否是表单结束标记
        /// </summary>
        /// <returns></returns>
        public bool IsEndFormMark()
        {
            return !String.IsNullOrEmpty(FormMark) && FormMark.StartsWith("E:");
        }

        /// <summary>
        /// 获取真实的表单名
        /// </summary>
        /// <returns></returns>
        public string GetFormName()
        {
            return IsStartFormMark() ? FormMark.Substring(2) : "";
        }

        /// <summary>
        /// 是否为表单字段标记
        /// </summary>
        /// <returns></returns>
        public bool IsFormFieldMark()
        {
            return !String.IsNullOrEmpty(ValueVarName) && String.IsNullOrEmpty(AssertName);
        }

        /// <summary>
        /// 是否是断言标记
        /// </summary>
        /// <returns></returns>
        public bool IsAssertMark()
        {
            return (!String.IsNullOrEmpty(TargetVarName) || !String.IsNullOrEmpty(ValueVarName))
                && !String.IsNullOrEmpty(AssertName);
        }

        /// <summary>
        /// 获取断言的变量名
        /// </summary>
        /// <returns></returns>
        public string GetAssertVarName()
        {
            return IsAssertMark() && !String.IsNullOrEmpty(TargetVarName) ? TargetVarName : ValueVarName;
        }

        /// <summary>
        /// 设置断言的变量值
        /// </summary>
        /// <param name="assertVarValue"></param>
        public void SetAssertVarValue(string assertVarValue)
        {
            if (IsAssertMark())
            {
                if (!String.IsNullOrEmpty(TargetVarName))
                {
                    Target = assertVarValue;
                }
                else
                {
                    Value = assertVarValue;
                }
            }
        }

        /// <summary>
        /// 克隆
        /// </summary>
        /// <returns></returns>
        public TestCommand Clone()
        {
            var command = new TestCommand(Command, Target, Value);
            command.TargetVarName = TargetVarName;
            command.ValueVarName = ValueVarName;
            command.FormMark = FormMark;
            command.AssertName = AssertName;
            command.RelatedScriptTitle = RelatedScriptTitle;
            return command;
        }

        /// <summary>
        /// 字符串形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}:{1}|{2}", Command, Target, Value);
        }
    }
}
