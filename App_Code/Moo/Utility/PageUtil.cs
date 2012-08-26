using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
namespace Moo.Utility
{
    /// <summary>
    /// 网页显示工具
    /// </summary>
    public static class PageUtil
    {
        public static string Truncate(string str,int len){
            return str.Length <= len ? str : str.Substring(0, len)+"...";
        }

        public static void Redirect(string info, string url)
        {
            HttpContext.Current.Response.Redirect("~/Special/Redirect.aspx?url="+HttpUtility.UrlEncode(url)+"&info="+HttpUtility.UrlEncode(info),true);
        }

        public static T GetEntity<T>(object obj) where T:class
        {
            T directCast = obj as T;
            if (directCast != null)
            {
                return directCast;
            }
            return ((ICustomTypeDescriptor)obj).GetPropertyOwner(null) as T;
        }
    }
}