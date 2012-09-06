using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Mail_List : System.Web.UI.Page
{
    protected string otherName;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("mail.list", false)) return;

        if (!Page.IsPostBack)
        {
            if (Request["otherID"] != null)
            {
                int otherID = int.Parse(Request["otherID"]);
                using (MooDB db = new MooDB())
                {
                    otherName = (from u in db.Users
                                 where u.ID == otherID
                                 select u).Single<User>().Name;
                }
            }
            dataSource.WhereParameters.Add(new Parameter("currentUserID", System.Data.DbType.Int32, ((SiteUser)User.Identity).ID.ToString()));
            Page.DataBind();
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int mailID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            Mail mail = (from m in db.Mails
                         where m.ID == mailID
                         select m).SingleOrDefault<Mail>();
            bool allow = mail.To.ID == ((SiteUser)User.Identity).ID
                || mail.From.ID == ((SiteUser)User.Identity).ID && !mail.IsRead;
            e.Cancel = !allow;
            if (!allow)
            {
                deletingFailure.Visible = true;
            }
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        using (MooDB db = new MooDB())
        {
            var other = (from u in db.Users
                         where u.Name == txtOtherName.Text
                         select u).Single<User>();
            Response.Redirect("~/Mail/List.aspx?otherID=" + other.ID, true);
        }
    }
    protected void validateOtherName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (MooDB db = new MooDB())
        {
            args.IsValid = (from u in db.Users
                            where u.Name == txtOtherName.Text
                            select u).Any();
        }
    }
    protected void grid_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            Logger.Warning(db, "删除邮件#" + e.Keys[0]);
        }
    }
}