using System;
using System.Collections.Generic;
using AutoTestFramework;
using System.Text.RegularExpressions;

namespace AutoTestFrameworkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string scriptFile = System.Configuration.ConfigurationManager.AppSettings["scriptFile"];
            string screenShotFolder = System.Configuration.ConfigurationManager.AppSettings["screenShotFolder"];
            string seleniumGridHub = System.Configuration.ConfigurationManager.AppSettings["seleniumGridHub"];

            // 测试引擎
            ITestEngine testEngine = new AutoTestFramework.SeleniumTestEngine.WebDriverBackedSelenium();
            testEngine.ScreenShotSavedFolder = screenShotFolder.TrimEnd('\\') + System.IO.Path.DirectorySeparatorChar;
            testEngine.EnableAutoScreenShot();

            // 获取并解析测试脚本
            string scriptString = "";
            using (System.IO.StreamReader sr = System.IO.File.OpenText(scriptFile))
            {
                scriptString = sr.ReadToEnd();
            }
            ITestScriptParser parser = new AutoTestFramework.SeleniumTestEngine.SeleniumIdeHtmlScriptParser();
            var testScript = parser.Parse(scriptString);

            // 创建测试用例，并执行
            TestCase testCase = TestCase.Create(testScript, WebBrowserType.Chrome);
            var testResult = testCase.Run(testEngine, new Uri(seleniumGridHub));

            /*
            testEngine.Prepare(new Uri(seleniumGridHub), new Uri(testScript.StartUrl), WebBrowserType.Chrome, new TimeSpan(0, 0, 30));
            testEngine.Start();
            testEngine.ExecuteScript(testScript);
            testEngine.Stop();
            */

            /*
            test.ExecuteCommand("open", target: "/Account/Login?ReturnUrl=%2F");
            test.ExecuteCommand("type", target: "id=UserName", value: "test");
            test.ExecuteCommand("type", target: "id=Password", value: "test");
            test.ExecuteCommand("clickAndWait", target: "css=button[type=\"submit\"]");
            test.ExecuteCommand("clickAndWait", target: "css=i.ico.ico-set");
            test.ExecuteCommand("clickAndWait", target: "link=添加成员");
            test.ExecuteCommand("type", target: "id=Mobile", value: "13300000000");
            test.ExecuteCommand("type", target: "id=EmpName", value: "Test User1");
            test.ExecuteCommand("type", target: "id=EMail", value: "test@abc.com");
            test.ExecuteCommand("type", target: "id=Password", value: "12345678");
            test.ExecuteCommand("type", target: "id=PasswordConfirm", value: "12345678");
            test.ExecuteCommand("type", target: "id=Mobile", value: "13300000000");
            test.ExecuteCommand("type", target: "id=Mobile", value: "13300000000");
            test.ExecuteCommand("type", target: "id=Mobile", value: "13300000000");
            test.ExecuteCommand("type", target: "id=Mobile", value: "13300000000");
            test.ExecuteCommand("type", target: "id=Mobile", value: "13300000000");
            test.ExecuteCommand("select", target: "id=DeptID", value: "label=运维部");
            test.ExecuteCommand("clickAndWait", target: "id=btnSave");
            */
            
        }
    }
}
