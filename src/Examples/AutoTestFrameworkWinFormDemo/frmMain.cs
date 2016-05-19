using AutoTestFramework;
using AutoTestFramework.Models;
using AutoTestFramework.Templates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoTestFrameworkWinForm
{
    public partial class frmMain : Form
    {
        private string screenShotFolder = System.Configuration.ConfigurationManager.AppSettings["screenShotFolder"];
        private string seleniumGridHub = System.Configuration.ConfigurationManager.AppSettings["seleniumGridHub"];
        private ITestEngine testEngine = new AutoTestFramework.SeleniumTestEngine.WebDriverBackedSelenium();
        protected TestScript scriptModel;
        protected TestCase testCaseModel;
        protected TestResult testResultModel;

        public frmMain()
        {
            InitializeComponent();
            testEngine.ScreenShotSavedFolder = screenShotFolder.TrimEnd('\\') + System.IO.Path.DirectorySeparatorChar;
            testEngine.EnableAutoScreenShot();
        }

        /// <summary>
        /// 加载测试数据
        /// </summary>
        /// <param name="testScriptFileName"></param>
        private void LoadTestScript(string testScriptFileName)
        {
            string scriptString = "";
            using (System.IO.StreamReader sr = System.IO.File.OpenText(testScriptFileName))
            {
                scriptString = sr.ReadToEnd();
            }

            ITestScriptParser parser = new AutoTestFramework.SeleniumTestEngine.SeleniumIdeHtmlScriptParser();
            scriptModel = parser.Parse(scriptString);
            textBox3.Text = scriptModel.StartUrl;
            textBox4.Text = scriptModel.Title;
            dataGridView1.DataSource = new List<TestCommand>(scriptModel.GetCommandList());
        }

        /// <summary>
        /// 导出测试数据模板
        /// </summary>
        private void ExportTestDataTemplate()
        {
            if (scriptModel != null)
            {
                var testDataTemplate = new TestDataTemplate();
                FormTemplate currentForm = null;
                foreach (var command in scriptModel.GetCommandList())
                {
                    if (!String.IsNullOrEmpty(command.FormMark))
                    {
                        if (command.IsStartFormMark())
                        {
                            testDataTemplate.AddFormTemplate(command.GetFormName());
                            currentForm = testDataTemplate.FindFormTemplate(command.GetFormName());
                        }
                        else if (command.IsEndFormMark())
                        {
                            currentForm = null;
                        }
                    }

                    if (currentForm != null && command.IsFormFieldMark())
                    {
                        currentForm.AddField(command.ValueVarName);
                    }

                    if (command.IsAssertMark())
                    {
                        testDataTemplate.AddAssertTemplate(command.AssertName, command.GetAssertVarName());
                    }
                }
                SaveTestDataTemplateToExcel(testDataTemplate, saveFileDialog1.FileName);
            }
        }

        /// <summary>
        /// 保存测试数据模板到Excel
        /// </summary>
        /// <param name="testDataTemplate"></param>
        /// <param name="saveFileName"></param>
        private void SaveTestDataTemplateToExcel(TestDataTemplate testDataTemplate, string saveFileName)
        {
            if (testDataTemplate != null)
            {
                var formList = testDataTemplate.GetFormTemplateList();
                var assertList = testDataTemplate.GetAssertTemplateList();
                var excel = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(saveFileName));

                string tmpSheetName = "测试数据模板";
                if (excel.Workbook.Worksheets[tmpSheetName] == null)
                {
                    var sheet = excel.Workbook.Worksheets.Add(tmpSheetName);
                    int rowIndex = 1;
                    if (formList.Count > 0)
                    {
                        for (var i = 0; i < formList.Count; i++)
                        {
                            var formFieldList = formList[i].GetFieldList();
                            sheet.Row(rowIndex).Style.Font.Bold = true;
                            sheet.Cells[rowIndex, 1].Value = String.Format("Form:{0}", formList[i].FormName);
                            for (var j = 0; j < formFieldList.Count; j++)
                            {
                                sheet.Cells[rowIndex, j + 2].Value = formFieldList[j];
                            }
                            rowIndex += 3;
                        }
                    }
                    if (assertList.Count > 0)
                    {
                        for (var i = 0; i < assertList.Count; i++)
                        {
                            sheet.Row(rowIndex).Style.Font.Bold = true;
                            sheet.Cells[rowIndex, 1].Value = String.Format("Assert:{0}", assertList[i].AssertName);
                            sheet.Cells[rowIndex, 2].Value = assertList[i].ExpectedVariableName;
                            rowIndex += 3;
                        }
                    }

                    excel.Save();
                    MessageBox.Show("导出数据模板成功！");
                }
                else
                {
                    MessageBox.Show("已经存在数据模板，已忽略");
                }
            }
        }

        /// <summary>
        /// 加载测试数据
        /// </summary>
        private void LoadTestData(string testDataFileName)
        {
            if (scriptModel != null)
            {
                TestData dataModel = GetTestData(testDataFileName);
                testCaseModel = TestCase.Create(textBox4.Text, new Uri(textBox3.Text), WebBrowserType.Chrome, scriptModel.GetCommandList(), dataModel);
                dataGridView2.DataSource = testCaseModel.GetCommandList();
            }
        }

        private TestData GetTestData(string testDataFileName)
        {
            var testDataModel = new TestData();
            var excel = new OfficeOpenXml.ExcelPackage(new System.IO.FileInfo(testDataFileName));
            var sheetList = excel.Workbook.Worksheets.Where(m => m.Name != "测试数据模板").ToArray();
            if (sheetList.Length > 0)
            {

                var sheet = sheetList[0];// 测试仅读取第一个匹配的Sheet页
                int rowIndex = 1;
                while (true)
                {
                    string head = sheet.Cells[rowIndex, 1].Text;
                    if (head.Length > 5 && head.StartsWith("Form:", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string formName = head.Substring(5).Trim();
                        testDataModel.AddFormData(formName);
                        var formDataModel = testDataModel.FindFormData(formName);
                        int colIndex = 2;
                        string fieldName = sheet.Cells[rowIndex, colIndex].Text;
                        string fieldValue = sheet.Cells[rowIndex + 1, colIndex].Text;
                        while (!String.IsNullOrEmpty(fieldName))
                        {
                            formDataModel.AddField(fieldName, fieldValue);
                            colIndex++;
                            fieldName = sheet.Cells[rowIndex, colIndex].Text;
                            fieldValue = sheet.Cells[rowIndex + 1, colIndex].Text;
                        }
                    }
                    else if (head.Length > 7 && head.StartsWith("Assert:", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string assertName = head.Substring(7).Trim();
                        string expectedValue = sheet.Cells[rowIndex + 1, 2].Text;
                        testDataModel.AddAssertData(assertName, expectedValue);
                    }
                    else
                    {
                        break;
                    }

                    rowIndex += 3;
                }
            }
            return testDataModel;
        }

        /// <summary>
        /// 开始测试
        /// </summary>
        private void StartTest()
        {
            if (testCaseModel != null)
            {
                testResultModel = testCaseModel.Run(testEngine, new Uri(seleniumGridHub));
                dataGridView3.DataSource = testResultModel.GetResultItemList();
                if (testResultModel.Result)
                {
                    MessageBox.Show("测试通过！");
                }
                else
                {
                    MessageBox.Show("测试未通过！");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "打开测试脚本文件";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Html文件(*.html)|*.html";
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.Title == "打开测试脚本文件")
            {
                textBox1.Text = openFileDialog1.FileName;
            }
            else if (openFileDialog1.Title == "打开测试数据文件")
            {
                textBox2.Text = openFileDialog1.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                LoadTestScript(textBox1.Text);
            }
            MessageBox.Show("加载测试脚本完成！");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (scriptModel != null)
            {
                saveFileDialog1.Title = "导出数据模板";
                saveFileDialog1.FileName = "";
                saveFileDialog1.Filter = "Excel文件(*.xlsx)|*.xlsx";
                saveFileDialog1.ShowDialog();
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (saveFileDialog1.Title == "导出数据模板")
            {
                ExportTestDataTemplate();
                MessageBox.Show("导出数据模板完成！");
            }
            else if (saveFileDialog1.Title == "导出测试结果")
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "打开测试数据文件";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Excel文件(*.xlsx)|*.xlsx";
            openFileDialog1.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "导出测试结果";
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "Excel文件(*.xlsx)|*.xlsx";
            saveFileDialog1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                LoadTestData(textBox2.Text);
            }
            MessageBox.Show("加载测试数据完成！");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                StartTest();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "测试发生异常");
            }
        }
    }
}
