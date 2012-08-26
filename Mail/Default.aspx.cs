using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class Mail_Default : System.Web.UI.Page
{
    protected Mail mail;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("mail.read", false)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    CollectEntityByID(db, int.Parse(Request["id"]));
                }
                if (mail == null)
                {
                    PageUtil.Redirect("找不到相关内容", "~/");
                    return;
                }

                int myUserID = ((SiteUser)User.Identity).ID;
                if (myUserID != mail.From.ID && myUserID != mail.To.ID)
                {
                    Permission.Check("i'm superman", false);
                    return;
                }
                mail.IsRead = true;
                db.SaveChanges();
                Page.DataBind();
            }
        }
    }

    void CollectEntityByID(MooDB db, int id)
    {
        mail = (from m in db.Mails
                where m.ID == id
                select m).SingleOrDefault<Mail>();
    }
}