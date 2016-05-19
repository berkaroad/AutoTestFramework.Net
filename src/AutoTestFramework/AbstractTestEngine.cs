using AutoTestFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoTestFramework
{
    /// <summary>
    /// 测试引擎
    /// </summary>
    public abstract class AbstractTestEngine : ITestEngine
    {
        private Dictionary<string, System.Reflection.MethodInfo> commandInfos = new Dictionary<string, System.Reflection.MethodInfo>();

        private AssertUnexpectedException testErrorInfo = null;

        /// <summary>
        /// 已经准备完毕
        /// </summary>
        protected bool IsPrepared { get; private set; }

        /// <summary>
        /// 已经启动
        /// </summary>
        protected bool IsStarted { get; private set; }

        /// <summary>
        /// 自动屏幕截屏启用状态
        /// </summary>
        protected bool AutoScreenShotEnabled { get; private set; }

        /// <summary>
        /// 屏幕截屏保存的文件夹
        /// </summary>
        public string ScreenShotSavedFolder { get; set; }

        /// <summary>
        /// 启用自动屏幕截屏
        /// </summary>
        public void EnableAutoScreenShot()
        {
            this.AutoScreenShotEnabled = !String.IsNullOrEmpty(ScreenShotSavedFolder);
        }

        /// <summary>
        /// 禁用自动屏幕截屏
        /// </summary>
        public void DisableAutoScreenShot()
        {
            this.AutoScreenShotEnabled = false;
        }

        /// <summary>
        /// 初始化准备
        /// </summary>
        /// <param name="testEngineUri"></param>
        /// <param name="testSiteStartUri"></param>
        /// <param name="browserType"></param>
        /// <param name="commandTimeout"></param>
        public void Prepare(Uri testEngineUri, Uri testSiteStartUri, WebBrowserType browserType, TimeSpan commandTimeout)
        {
            if (IsStarted)
            {
                Stop();
            }
            InternalPrepare(testEngineUri, testSiteStartUri, browserType, commandTimeout);
            IsPrepared = true;
        }
        
        /// <summary>
        /// 初始化准备
        /// </summary>
        /// <param name="testEngineUri"></param>
        /// <param name="testSiteStartUri"></param>
        /// <param name="browserType"></param>
        /// <param name="commandTimeout"></param>
        protected abstract void InternalPrepare(Uri testEngineUri, Uri testSiteStartUri, WebBrowserType browserType, TimeSpan commandTimeout);

        /// <summary>
        /// 启动测试引擎
        /// </summary>
        public void Start()
        {
            if (IsPrepared)
            {
                foreach (var methodInfo in this.GetType().GetMethods(System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.InvokeMethod
                    | System.Reflection.BindingFlags.NonPublic))
                {
                    foreach (CommandAttribute commandAttr in methodInfo.GetCustomAttributes(typeof(CommandAttribute), false))
                    {
                        string commandName = commandAttr.Command.ToUpper();
                        if (!commandInfos.ContainsKey(commandName))
                        {
                            commandInfos.Add(commandName, methodInfo);
                        }
                    }
                }
                InternalStart();
                IsStarted = true;
            }
        }

        /// <summary>
        /// 内部启动测试引擎
        /// </summary>
        protected abstract void InternalStart();

        /// <summary>
        /// 获取测试错误信息
        /// </summary>
        /// <returns></returns>
        public AssertUnexpectedException GetTestErrorInfo()
        {
            return testErrorInfo;
        }

        /// <summary>
        /// 捕获屏幕快照
        /// </summary>
        public virtual void CaptureScreenshot()
        { }

        /// <summary>
        /// 断言预期值与实际值相等
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        protected void AssertEqual(string assertName, string expected, string actual)
        {
            try
            {
                Xunit.Assert.Equal(expected, actual);
            }
            catch (Xunit.Sdk.EqualException ex)
            {
                throw new AssertEqualUnexpectedException(String.IsNullOrEmpty(assertName) ? ex.UserMessage : assertName, expected, actual, ex);
            }
        }

        /// <summary>
        /// 断言预期值与实际值不相等
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        protected void AssertNotEqual(string assertName, string expected, string actual)
        {
            try
            {
                Xunit.Assert.NotEqual(expected, actual);
            }
            catch (Xunit.Sdk.NotEqualException ex)
            {
                throw new AssertNotEqualUnexpectedException(String.IsNullOrEmpty(assertName)? ex.UserMessage : assertName, expected, actual, ex);
            }
        }

        /// <summary>
        /// 断言为真
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="condition"></param>
        protected void AssertTrue(string assertName, bool condition)
        {
            try
            {
                Xunit.Assert.True(condition);
            }
            catch (Xunit.Sdk.TrueException ex)
            {
                throw new AssertUnexpectedException(String.IsNullOrEmpty(assertName) ? ex.UserMessage : assertName, "True", condition.ToString(), ex);
            }
        }

        /// <summary>
        /// 断言为假
        /// </summary>
        /// <param name="assertName"></param>
        /// <param name="condition"></param>
        protected void AssertFalse(string assertName, bool condition)
        {
            try
            {
                Xunit.Assert.False(condition);
            }
            catch (Xunit.Sdk.FalseException ex)
            {
                throw new AssertUnexpectedException(String.IsNullOrEmpty(assertName) ? ex.UserMessage : assertName, "False", condition.ToString(), ex);
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandModel"></param>
        public void ExecuteCommand(TestCommand commandModel)
        {
            if (IsStarted)
            {
                if (commandModel == null || String.IsNullOrEmpty(commandModel.Command))
                {
                    throw new TestCommandInvalidException();
                }
                var command = commandModel.Command.ToUpper();
                if (commandInfos.ContainsKey(command))
                {
                    try
                    {
                        var commandMethodInfo = commandInfos[command];
                        UnwrappedExecuteCommand(commandMethodInfo, commandModel);
                    }
                    catch (System.Exception innerEx)
                    {
                        throw new TestCommandInvokeException(commandModel.Command, commandModel.Target, commandModel.Value, this.GetType(), innerEx);
                    }
                }
                else
                {
                    throw new TestCommandUndefinedException(commandModel.Command, this.GetType());
                }
            }
        }

        /// <summary>
        /// 执行测试脚本
        /// </summary>
        /// <param name="command"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        public void ExecuteCommand(string command, string target = "", string value = "")
        {
            ExecuteCommand(new TestCommand(command, target, value));
        }

        private void UnwrappedExecuteCommand(System.Reflection.MethodInfo commandMethodInfo, TestCommand command)
        {
            var context = new TestCommandContext(command);
            try
            {
                commandMethodInfo.Invoke(this, System.Reflection.BindingFlags.InvokeMethod
                    | System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.Instance
                    , null, new object[] { context }, null);
            }
            catch (System.Reflection.TargetInvocationException invokeEx)
            {
                if (invokeEx.InnerException != null)
                {
                    if(invokeEx.InnerException is AssertUnexpectedException)
                    {
                        testErrorInfo = (AssertUnexpectedException)(invokeEx.InnerException);
                    }
                    else
                    {
                        throw invokeEx.InnerException;
                    }
                }
                else
                {
                    throw invokeEx;
                }
            }
        }

        /// <summary>
        /// 终止测试引擎
        /// </summary>
        public void Stop()
        {
            commandInfos.Clear();
            testErrorInfo = null;
            InternalStop();
            IsStarted = false;
        }

        /// <summary>
        /// 终止测试引擎
        /// </summary>
        public abstract void InternalStop();

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            Stop();
        }
    }
}
