using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
using Moo.Text;
public partial class User_Modify : System.Web.UI.Page
{
    protected bool canModify;

    protected User user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("user.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    int id = int.Parse(Request["id"]);
                    user = (from u in db.Users
                            where u.ID == id
                            select u).SingleOrDefault<User>();
                }

                if (user == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                if (!User.Identity.IsAuthenticated)
                {
                    canModify = false;
                }
                else
                {
                    SiteUser siteUser = (SiteUser)User.Identity;
                    canModify = siteUser.ID == user.ID
                        || Permission.Check("user.modify", false, false)
                            && siteUser.Role.Type < SiteRoles.ByID[user.Role.ID].Type;
                }

                ViewState["userID"] = user.ID;
                Page.DataBind();
                ddlRole.SelectedIndex = ddlRole.Items.IndexOf(ddlRole.Items.FindByValue(user.Role.ID.ToString()));
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        if (!User.Identity.IsAuthenticated)
        {
            Permission.Check("i'm super man.", false);
            return;
        }

        int userID = (int)ViewState["userID"];
        using (MooDB db = new MooDB())
        {
            user = (from u in db.Users
                    where u.ID == userID
                    select u).Single<User>();

            SiteUser siteUser = (SiteUser)User.Identity;
            canModify = siteUser.ID == user.ID
                || Permission.Check("user.modify", false, false)
                    && siteUser.Role.Type < SiteRoles.ByID[user.Role.ID].Type;
            if (!canModify)
            {
                Permission.Check("i'm super man.", false);
                return;
            }

            if (Permission.Check("user.name.modify", false, false))
            {
                user.Name = txtName.Text;
            }

            if (txtPassword.Text.Length > 0)
            {
                user.Password = Converter.ToSHA256Hash(txtPassword.Text);
            }

            user.Role = SiteRoles.ByID[int.Parse(ddlRole.SelectedValue)].GetDBRole(db);
            user.ImageURL = txtImageURL.Text;
            user.BriefDescription = txtBriefDescription.Text;
            user.Description = txtDescription.Text;

            db.SaveChanges();
        }

        PageUtil.Redirect("操作成功", "~/User/?id=" + userID);
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        trPreview.Visible = true;
        btnSubmit.Enabled = true;
        divPreview.InnerHtml = WikiParser.Parse(txtDescription.Text);
    }
    protected void btnForceLogout_Click(object sender, EventArgs e)
    {
        if (!Permission.Check("user.forcelogout", false)) return;

        int userID = (int)ViewState["userID"];
        SiteUsers.ByID.Remove(userID);

        PageUtil.Redirect("操作成功", "~/User/Modify.aspx?id=" + userID);
    }
    protected void validateRole_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Permission.Check("user.role.modify", false, false))
        {
            args.IsValid = true;
        }
        else
        {
            int userID = (int)ViewState["userID"];
            using (MooDB db = new MooDB())
            {
                user = (from u in db.Users
                        where u.ID == userID
                        select u).Single<User>();
                args.IsValid = SiteRoles.ByID[int.Parse(ddlRole.SelectedValue)].Type >= SiteRoles.ByID[user.Role.ID].Type;
            }
        }
    }
    protected void validateName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int userID = (int)ViewState["userID"];
        using (MooDB db = new MooDB())
        {
            args.IsValid = !(from u in db.Users
                            where u.ID != userID && u.Name == txtName.Text
                            select u).Any();
        }
    }
}