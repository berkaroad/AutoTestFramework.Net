using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoTestFramework
{
    /// <summary>
    /// 屏幕截屏
    /// </summary>
    public interface IScreenShot
    {
        /// <summary>
        /// 屏幕快照文件夹
        /// </summary>
        string ScreenShotSavedFolder { get; set; }

        /// <summary>
        /// 启用自动屏幕快照
        /// </summary>
        void EnableAutoScreenShot();

        /// <summary>
        /// 禁用自动屏幕快照
        /// </summary>
        void DisableAutoScreenShot();

        /// <summary>
        /// 捕获屏幕快照
        /// </summary>
        void CaptureScreenshot();
    }
}
