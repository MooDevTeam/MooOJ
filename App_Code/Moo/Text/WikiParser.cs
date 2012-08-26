using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Moo.Text
{
    /// <summary>
    /// Wiki文本解析
    /// </summary>
    public static class WikiParser
    {
        public static string Parse(string text)
        {
            return new ProjectBase.Tools.Wiki.WikiConverter().ConvertToHtml(text);
        }
    }
}