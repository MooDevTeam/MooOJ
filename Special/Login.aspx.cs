using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class Special_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Page.DataBind();
        }
    }

    protected void ValidateUserName(object sender, ServerValidateEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            string userName = (loginView.FindControl("txtUserName") as TextBox).Text;
            e.IsValid = (from u in db.Users
                         where u.Name == userName
                         select u).Any();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        TextBox txtUserName = loginView.FindControl("txtUserName") as TextBox;
        TextBox txtPassword = loginView.FindControl("txtPassword") as TextBox;
        CheckBox chkPersistent = loginView.FindControl("chkPersistent") as CheckBox;

        string userName = txtUserName.Text;
        string password = Converter.ToSHA256Hash(txtPassword.Text);
        bool isPersistent = chkPersistent.Checked;

        using (MooDB db = new MooDB())
        {
            Login(db, userName, password, isPersistent);
        }
    }
    void Login(MooDB db, string userName, string password, bool isPersistent)
    {
        User user = GetUser(db, userName, password);
        if (user == null)
        {
            PageUtil.Redirect("密码错误", Request.RawUrl);
            return;
        }
        SetCookie(user, isPersistent);
        Context.User = new CustomPrincipal() { Identity = SiteUsers.ByID[user.ID] };
        Logger.Info(db, "登录");
        PageUtil.Redirect("登录成功", FormsAuthentication.GetRedirectUrl(user.Name, isPersistent));
    }
    User GetUser(MooDB db, string userName, string password)
    {
        return (from u in db.Users
                where u.Name == userName && u.Password == password
                select u).SingleOrDefault<User>();
    }
    void SetCookie(User user, bool isPersistent)
    {
        int token = Rand.RAND.Next();
        if (SiteUsers.ByID.ContainsKey(user.ID))
        {
            SiteUsers.ByID[user.ID].Initialize(user);
            SiteUsers.ByID[user.ID].Token = token;
        }
        else
        {
            SiteUsers.ByID[user.ID] = new SiteUser(user) { Token = token };
        }

        //string userData = new SiteUser(user).Serialize();
        string userData = user.ID + "," + token;

        FormsAuthenticationTicket ticket;
        ticket = new FormsAuthenticationTicket(1, user.Name, DateTime.Now, DateTime.Now + FormsAuthentication.Timeout, isPersistent, userData);
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
        cookie.Expires = isPersistent ? ticket.Expiration : DateTime.MinValue;
        cookie.HttpOnly = true;
        cookie.Path = FormsAuthentication.FormsCookiePath;
        Response.Cookies.Add(cookie);
    }
}