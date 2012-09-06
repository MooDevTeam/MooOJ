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
                Logger.Warning(db, "初始化数据库");
            }
        }
        SiteRoles.Initialize();

        using (MooDB db = new MooDB())
        {
            if (db.Users.Count() <= 1)
            {
                MooTestData.AddTestData(db);
                Logger.Debug(db, "加入测试数据");
            }
        }

        TesterManager.Testers.Add(new Moo.Tester.MooTester.Tester());
        TesterManager.Start();
        ContestManager.Start();

        using (MooDB db = new MooDB())
        {
            Logger.Info(db, "Moo启动");
        }
    }

    void Application_End(object sender, EventArgs e)
    {
        //在应用程序关闭时运行的代码
        TesterManager.Stop();
        ContestManager.Stop();
        
        using (MooDB db = new MooDB())
        {
            Logger.Info(db, "Moo停止");
        }
    }

    void Application_Error(object sender, EventArgs e)
    {
        //在出现未处理的错误时运行的代码
        Exception ex = Context.Error;
        if (ex is HttpUnhandledException)
        {
            ex = ex.InnerException;
        }
        using(MooDB db=new MooDB()){
            Logger.Error(db, ex.ToString());
        }
    }

    void Session_Start(object sender, EventArgs e)
    {
        //在新会话启动时运行的代码
        using (MooDB db = new MooDB())
        {
            Logger.Info(db, "会话启动");
        }
    }

    void Session_End(object sender, EventArgs e)
    {
        //在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式 
        //设置为 StateServer 或 SQLServer，则不会引发该事件。
        using (MooDB db = new MooDB())
        {
            Logger.Info(db, "会话结束");
        }
    }

    void Application_AuthorizeRequest(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated && User.Identity is FormsIdentity)
        {
            FormsIdentity formsIdentity = User.Identity as FormsIdentity;
            string userData = formsIdentity.Ticket.UserData;
            string[] splited = userData.Split(',');
            int userID = int.Parse(splited[0]);
            int token = int.Parse(splited[1]);

            if (!SiteUsers.ByID.ContainsKey(userID) || ((Context.User = new CustomPrincipal() { Identity = SiteUsers.ByID[userID] }).Identity as SiteUser).Token != token)
            {
                using (MooDB db = new MooDB())
                {
                    Logger.Warning(db, "Token无效");
                }
                FormsAuthentication.SignOut();
                SiteUsers.ByID.Remove(userID);
                Response.Redirect("~/Special/Security.aspx", true);
                return;
            }

            Thread.CurrentPrincipal = Context.User;
        }
    }
</script>
