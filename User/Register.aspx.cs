using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class User_Register : System.Web.UI.Page
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
            e.IsValid = (from u in db.Users
                         where u.Name == txtUserName.Text
                         select u).SingleOrDefault<User>() == null;
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("user.create", true)) return;

        using (MooDB db = new MooDB())
        {
            User user = new User()
            {
                Name = txtUserName.Text,
                Password = Converter.ToSHA256Hash(txtPassword.Text),
                Role = SiteRoles.ByType[RoleType.NormalUser].GetDBRole(db),
                BriefDescription = "我很懒，什么都没留下~",
                Description="我懒到头了，*真的*啥都没写",
                ImageURL = "",
                Score=0
            };
            db.Users.AddObject(user);
            db.SaveChanges();
        }
        Response.Redirect("~/Special/Login.aspx");
    }
}