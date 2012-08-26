<%@ WebHandler Language="C#" Class="Special_Logout" %>

using System;
using System.Web;
using System.Web.Security;
using Moo.Authorization;
using Moo.Utility;

public class Special_Logout : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        if (!context.User.Identity.IsAuthenticated)
        {
            PageUtil.Redirect("请先登录，然后才能登出。", "~/Special/Login.aspx");
            return;
        }

        FormsAuthentication.SignOut();
        SiteUsers.ByID.Remove(((SiteUser)context.User.Identity).ID);
        PageUtil.Redirect("已登出，现在跳转到登录页面", "~/Special/Login.aspx");
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}