using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;
namespace Moo.Authorization
{
    /// <summary>
    /// 检查权限
    /// </summary>
    public static class Permission
    {
        public static bool Check(string permission, bool allowAnonymous)
        {
            return Check(permission, allowAnonymous, true);
        }

        public static bool Check(string permission, bool allowAnonymous, bool autoRedirect)
        {
            bool result;
            IPrincipal currenPricipal = HttpContext.Current.User;
            if (!currenPricipal.Identity.IsAuthenticated)
            {
                result = allowAnonymous;
            }
            else
            {
                CustomPrincipal pricipal = (CustomPrincipal)currenPricipal;
                result = pricipal.IsAllowed(permission);
            }

            if (!result && autoRedirect)
            {
                FormsAuthentication.RedirectToLoginPage("noPermission=true");
                HttpContext.Current.Response.End();
            }
            return result;
        }
    }
}