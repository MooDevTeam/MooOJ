<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Configuration" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="Moo.Authorization" %>
<%@ Import Namespace="Moo.DB" %>
<%@ Import Namespace="Moo.Utility" %>
<%@ Import Namespace="Moo.Manager" %>
<script RunAt="server">

    void Application_Start(object sender, EventArgs e)
    {
        //在应用程序启动时运行的代码
        using (MooDB db = new MooDB())
        {
            if (!db.DatabaseExists())
            {
                DatabaseInstaller.Install(db);
            }
        }
        SiteRoles.Initialize();

        //MooTestData.AddTestData();

        TesterManager.Testers.Add(new Moo.Tester.MooTester.Tester());
        TesterManager.Start();
        ContestManager.Start();
    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码
        TesterManager.Stop();
        ContestManager.Stop();
    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码
    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码

    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。

    }

    void Application_AuthorizeRequest(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated && User.Identity.AuthenticationType == "Forms")
        {
            FormsIdentity formsIdentity = User.Identity as FormsIdentity;
            string userData = formsIdentity.Ticket.UserData;
            string[] splited = userData.Split(',');
            int userID = int.Parse(splited[0]);
            int token = int.Parse(splited[1]);

            SiteUser siteUser;
            if (!SiteUsers.ByID.ContainsKey(userID) || (siteUser = SiteUsers.ByID[userID]).Token != token)
            {
                FormsAuthentication.SignOut();
                SiteUsers.ByID.Remove(userID);
                Response.Redirect("~/Special/Security.aspx", true);
                return;
            }

            CustomPrincipal principal = new CustomPrincipal() { Identity = siteUser };
            HttpContext.Current.User = principal;
            Thread.CurrentPrincipal = principal;
        }
    }
</script>
