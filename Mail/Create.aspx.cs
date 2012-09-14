using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Text;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Mail_Create : System.Web.UI.Page
{
    protected string initialTitle="";
    protected string initialContent="";

    protected User theSender;
    protected User receiver;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("mail.create", false)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["to"] != null)
                {
                    CollectEntityByTo(db, int.Parse(Request["to"]));
                }

                if (Request["replyTo"] != null)
                {
                    int id=int.Parse(Request["replyTo"]);
                    Mail replyTo = (from m in db.Mails
                                    where m.ID == id
                                    select m).SingleOrDefault<Mail>();
                    if (replyTo == null)
                    {
                        receiver = null;
                    }
                    else
                    {
                        initialTitle = "回复:" + replyTo.Title;
                        initialContent = "\n\n在 *" + replyTo.Title + "* 中， *" + replyTo.From.Name + "* 写道:\n:{\n" + replyTo.Content+"\n:}";
                    }
                }
                
                theSender = ((SiteUser)User.Identity).GetDBUser(db);

                if (theSender == null || receiver == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                ViewState["receiverID"] = receiver.ID;
                Page.DataBind();
            }
        }
    }

    void CollectEntityByTo(MooDB db, int id)
    {
        receiver = (from u in db.Users
                    where u.ID == id
                    select u).SingleOrDefault<User>();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int receiverID=(int)ViewState["receiverID"];
        using (MooDB db = new MooDB())
        {
            Mail mail=new Mail()
            {
                From = ((SiteUser)User.Identity).GetDBUser(db),
                To = (from u in db.Users
                      where u.ID == receiverID
                      select u).Single<User>(),
                Title = txtTitle.Text,
                Content = txtContent.Text,
                IsRead = false,
            };

            db.Mails.AddObject(mail);

            db.SaveChanges();

            Logger.Info(db,string.Format("向用户#{0}发送邮件#{1}",receiverID,mail.ID));
        }

        PageUtil.Redirect("创建成功", "~/Mail/List.aspx");
    }
}