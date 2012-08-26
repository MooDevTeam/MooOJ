using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
public partial class Mail_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("mail.list", false)) return;

        if (!Page.IsPostBack)
        {
            int myUserID = ((SiteUser)User.Identity).ID;
            dataSource.Where = "(it.[To].ID=" + myUserID + " or it.[From].ID=" + myUserID + ")";
            if (Request["otherID"] != null)
            {
                int otherID = int.Parse(Request["otherID"]);
                dataSource.Where += " and (it.[From].ID=" + otherID + " or it.[To].ID=" + otherID + ")";
            }
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
}