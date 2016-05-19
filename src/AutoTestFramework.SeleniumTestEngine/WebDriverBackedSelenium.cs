using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using AutoTestFramework.Models;
using System.Text.RegularExpressions;

namespace AutoTestFramework.SeleniumTestEngine
{
    /// <summary>
    /// 基于WebDriverBackedSelinium的测试引擎
    /// </summary>
    public class WebDriverBackedSelenium : AbstractTestEngine
    {
        private TimeSpan commandTimeout = new TimeSpan(0, 0, 30);
        private int defaultPauseTime = 3000;
        private Selenium.ISelenium selenium = null;

        /// <summary>
        /// 基于WebDriverBackedSelinium的测试引擎
        /// </summary>
        public WebDriverBackedSelenium() { }

        /// <summary>
        /// 初始化准备
        /// </summary>
        /// <param name="testEngineUri"></param>
        /// <param name="testSiteStartUri"></param>
        /// <param name="browserType"></param>
        /// <param name="commandTimeout"></param>
        protected override void InternalPrepare(Uri testEngineUri, Uri testSiteStartUri, WebBrowserType browserType, TimeSpan commandTimeout)
        {
            DesiredCapabilities aDesiredcap = null;
            switch (browserType)
            {
                case WebBrowserType.Chrome:
                    aDesiredcap = DesiredCapabilities.Chrome();
                    break;
                case WebBrowserType.Firefox:
                    aDesiredcap = DesiredCapabilities.Firefox();
                    break;
                case WebBrowserType.InternetExplorer:
                    aDesiredcap = DesiredCapabilities.InternetExplorer();
                    break;
                case WebBrowserType.Safari:
                    aDesiredcap = DesiredCapabilities.Safari();
                    break;
                default:
                    aDesiredcap = DesiredCapabilities.Chrome();
                    break;
            }
            aDesiredcap.Platform = new Platform(PlatformType.Any);
            this.commandTimeout = commandTimeout;
            var wd = new RemoteWebDriver(testEngineUri, aDesiredcap, commandTimeout);
            selenium = new Selenium.WebDriverBackedSelenium(wd, testSiteStartUri.Scheme + "://" + testSiteStartUri.Authority);
        }

        /// <summary>
        /// 启动测试引擎
        /// </summary>
        protected override void InternalStart()
        {
            selenium.Start();
        }

        /// <summary>
        /// 停止测试引擎
        /// </summary>
        public override void InternalStop()
        {
            selenium.Stop();
        }

        public override void CaptureScreenshot()
        {
            if (!String.IsNullOrEmpty(ScreenShotSavedFolder))
            {
                string fileName = "";
                string title = selenium.GetTitle();
                title = string.IsNullOrEmpty(title) ? "" : title;
                fileName = ScreenShotSavedFolder.TrimEnd(System.IO.Path.DirectorySeparatorChar) + System.IO.Path.DirectorySeparatorChar + System.DateTime.Now.Ticks.ToString() + "-" + title.Replace("\\", "").Replace("/", "").Replace("|", "").Replace(".", "").Replace("?", "").Replace("*", "").Replace(":", "").Replace("\"", "").Replace("<", "").Replace(">", "") + ".png";
                if (!System.IO.Directory.Exists(ScreenShotSavedFolder))
                {
                    System.IO.Directory.CreateDirectory(ScreenShotSavedFolder);
                }
                byte[] imgBuffer = Convert.FromBase64String(selenium.CaptureScreenshotToString());
                if (imgBuffer.Length > 0)
                {
                    using (var fs = System.IO.File.Create(fileName))
                    {
                        fs.Position = 0;
                        fs.Write(imgBuffer, 0, imgBuffer.Length);
                        fs.Flush();
                    }
                }
            }
        }

        #region Test Command

        [Command(Command = "addLocationStrategy")]
        private void AddLocationStrategy(TestCommandContext ctx)
        {
            selenium.AddLocationStrategy(ctx.CommandModel.Target, ctx.CommandModel.Value);
        }

        [Command(Command = "addLocationStrategyAndWait")]
        private void AddLocationStrategyAndWait(TestCommandContext ctx)
        {
            AddLocationStrategy(ctx);
            selenium.WaitForPageToLoad(commandTimeout.TotalMilliseconds.ToString());
        }

        [Command(Command = "addScript")]
        private void AddScript(TestCommandContext ctx)
        {
            selenium.AddScript(ctx.CommandModel.Target, ctx.CommandModel.Value);
        }

        [Command(Command = "addScriptAndWait")]
        private void AddScriptAndWait(TestCommandContext ctx)
        {
            AddScript(ctx);
            selenium.WaitForPageToLoad(commandTimeout.TotalMilliseconds.ToString());
        }

        private void AddSelection(TestCommandContext ctx)
        {
            selenium.AddSelection(ctx.CommandModel.Target, ctx.CommandModel.Value);
        }

        [Command(Command = "assertAlert")]
        private void AssertAlert(TestCommandContext ctx)
        {
            AssertEqual(ctx.CommandModel.AssertName, ctx.CommandModel.Target, selenium.IsAlertPresent() ? selenium.GetAlert() : "");
        }

        [Command(Command = "assertConfirmation")]
        private void AssertConfirmation(TestCommandContext ctx)
        {
            AssertEqual(ctx.CommandModel.AssertName, ctx.CommandModel.Target, selenium.IsConfirmationPresent() ? selenium.GetConfirmation() : "");
        }

        [Command(Command = "assertElementPresent")]
        private void AssertElementPresent(TestCommandContext ctx)
        {
            AssertTrue(ctx.CommandModel.AssertName, selenium.IsElementPresent(ctx.CommandModel.Target));
        }

        [Command(Command = "assertNotText")]
        private void AssertNotText(TestCommandContext ctx)
        {
            AssertNotEqual(ctx.CommandModel.AssertName, ctx.CommandModel.Value, selenium.IsElementPresent(ctx.CommandModel.Target) ? selenium.GetText(ctx.CommandModel.Target) : "");
        }

        [Command(Command = "assertText")]
        private void AssertText(TestCommandContext ctx)
        {
            AssertEqual(ctx.CommandModel.AssertName, ctx.CommandModel.Value, selenium.IsElementPresent(ctx.CommandModel.Target) ? selenium.GetText(ctx.CommandModel.Target) : "");
        }

        [Command(Command = "click")]
        private void Click(TestCommandContext ctx)
        {
            if (!selenium.IsElementPresent(ctx.CommandModel.Target))
            {
                throw new AssertElementPresentUnexpectedException(ctx.CommandModel.Target);
            }
            selenium.Click(ctx.CommandModel.Target);
        }

        [Command(Command = "clickAndWait")]
        private void ClickAndWait(TestCommandContext ctx)
        {
            if (this.AutoScreenShotEnabled)
            {
                CaptureScreenshot();
            }
            Click(ctx);
            System.Threading.Thread.Sleep(defaultPauseTime);
            if (this.AutoScreenShotEnabled)
            {
                CaptureScreenshot();
            }
        }

        [Command(Command = "open")]
        private void Open(TestCommandContext ctx)
        {
            selenium.Open(ctx.CommandModel.Target);
            if (this.AutoScreenShotEnabled)
            {
                CaptureScreenshot();
            }
        }

        [Command(Command = "pause")]
        private void Pause(TestCommandContext ctx)
        {
            System.Threading.Thread.Sleep(Int32.Parse(ctx.CommandModel.Target));
        }

        [Command(Command = "select")]
        private void Select(TestCommandContext ctx)
        {
            if (!selenium.IsElementPresent(ctx.CommandModel.Target))
            {
                throw new AssertElementPresentUnexpectedException(ctx.CommandModel.Target);
            }
            selenium.Select(ctx.CommandModel.Target, ctx.CommandModel.Value);
            if (this.AutoScreenShotEnabled)
            {
                CaptureScreenshot();
            }
        }

        [Command(Command = "type")]
        private void Type(TestCommandContext ctx)
        {
            if (!selenium.IsElementPresent(ctx.CommandModel.Target))
            {
                throw new AssertElementPresentUnexpectedException(ctx.CommandModel.Target);
            }
            selenium.Type(ctx.CommandModel.Target, ctx.CommandModel.Value);
        }

        #endregion
    }
}
