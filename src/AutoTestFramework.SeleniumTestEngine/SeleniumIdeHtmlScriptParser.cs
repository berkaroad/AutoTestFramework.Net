using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutoTestFramework;
using AutoTestFramework.Models;

namespace AutoTestFramework.SeleniumTestEngine
{
    /// <summary>
    /// Selenium IDE 保存的Html脚本格式解析器
    /// </summary>
    public class SeleniumIdeHtmlScriptParser : ITestScriptParser
    {
        private Regex regHead = new Regex(@"<head\s*[^>]*>(\s|\S)+</head>", RegexOptions.IgnoreCase);
        private Regex regLinkStartUrl = new Regex(@"<link rel=""selenium\.base"" href=""[^""]*"" />", RegexOptions.IgnoreCase);
        private Regex regHrefStartUrl = new Regex(@"href=""[^""]*""", RegexOptions.IgnoreCase);
        private Regex regAttrValue = new Regex(@"""[^""]*""", RegexOptions.IgnoreCase);
        private Regex regTitle = new Regex(@"<title>[^<]*</title>", RegexOptions.IgnoreCase);

        private Regex regTBody = new Regex(@"<tbody>(\s|\S)+</tbody>", RegexOptions.IgnoreCase);
        private Regex regTr = new Regex(@"<tr>[^<]*<td>[^<]+</td>[^<]*<td>[^<]*</td>[^<]*<td>[^<]*</td>[^<]*</tr>", RegexOptions.IgnoreCase);
        private Regex regTd = new Regex(@"<td>[^<]*</td>", RegexOptions.IgnoreCase);

        private Regex regTag = new Regex(@"<[^>]+>", RegexOptions.IgnoreCase);

        public TestScript Parse(string scriptString)
        {
            TestScript result = new TestScript();
            var matchHead = regHead.Match(scriptString);
            if (matchHead.Success)
            {
                var matchLinkStartUrl = regLinkStartUrl.Match(matchHead.Value);
                if (matchLinkStartUrl.Success)
                {
                    var matchHrefStartUrl = regHrefStartUrl.Match(matchLinkStartUrl.Value);
                    if (matchHrefStartUrl.Success)
                    {
                        var matchStartUrl = regAttrValue.Match(matchHrefStartUrl.Value);
                        if (matchStartUrl.Success)
                        {
                            result.StartUrl = System.Web.HttpUtility.HtmlDecode(matchStartUrl.Value.Replace("\"","")).Trim();
                        }
                    }
                }
                var matchTitle = regTitle.Match(matchHead.Value);
                if (matchTitle.Success)
                {
                    result.Title = System.Web.HttpUtility.HtmlDecode(regTag.Replace(matchTitle.Value, "")).Trim();
                }
            }

            var matchTBody = regTBody.Match(scriptString);
            if (matchTBody.Success)
            {
                var matchTrColl = regTr.Matches(matchTBody.Value);
                foreach (Match matchTr in matchTrColl)
                {
                    var matchTdColl = regTd.Matches(matchTr.Value);
                    if (matchTdColl.Count == 3)
                    {
                        string command = System.Web.HttpUtility.HtmlDecode(regTag.Replace(matchTdColl[0].Value, "")).Trim();
                        string target = System.Web.HttpUtility.HtmlDecode(regTag.Replace(matchTdColl[1].Value, "")).Trim();
                        string value = System.Web.HttpUtility.HtmlDecode(regTag.Replace(matchTdColl[2].Value, "")).Trim();
                        result.AddCommand(command, target, value);
                    }
                }
            }
            return result;
        }
    }
}
