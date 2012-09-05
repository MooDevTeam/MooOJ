using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using Moo.DB;
using Moo.Authorization;
public partial class UserSign : System.Web.UI.UserControl
{
    //protected string userEmail;

    protected User user;
    public User User
    {
        set
        {
            user = value;
            switch (SiteRoles.ByID[user.Role.ID].Type)
            {
                case RoleType.Organizer:
                    signWrap.Style.Add(HtmlTextWriterStyle.BackgroundColor, "lightblue");
                    signWrap.Style.Add(HtmlTextWriterStyle.BorderColor, "blue");
                    break;
                case RoleType.Worker:
                    signWrap.Style.Add(HtmlTextWriterStyle.BackgroundColor, "pink");
                    signWrap.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                    break;
                case RoleType.NormalUser:
                    signWrap.Style.Add(HtmlTextWriterStyle.BackgroundColor, "lightgreen");
                    signWrap.Style.Add(HtmlTextWriterStyle.BorderColor, "green");
                    break;
                case RoleType.Malefactor:
                    signWrap.Style.Add(HtmlTextWriterStyle.BackgroundColor, "lightgray");
                    signWrap.Style.Add(HtmlTextWriterStyle.BorderColor, "black");
                    break;
            }
            IIdentity identity = HttpContext.Current.User.Identity;
            signControlSelf.Visible = identity.IsAuthenticated && ((SiteUser)identity).ID == user.ID;
            signControlOther.Visible = !signControlSelf.Visible;
        }
    }

    public int UserID
    {
        set
        {
            using (MooDB db = new MooDB())
            {
                User = (from u in db.Users
                        where u.ID == value
                        select u).Single<User>();
            }
        }
    }

    public string Style
    {
        set
        {
            signWrap.Style.Value += value;
        }
    }

    public bool Vertical
    {
        get
        {
            return ViewState["vertical"] == null ? false : (bool)ViewState["vertical"];
        }
        set
        {
            ViewState["vertical"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            signImage.DataBind();
        }
    }
}