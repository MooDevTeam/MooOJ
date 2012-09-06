using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
using Moo.Text;
public partial class Update : System.Web.UI.Page
{
    protected bool isLatest;

    protected HomepageRevision revision;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("homepage.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["revision"] != null)
                {
                    int revisionID = int.Parse(Request["revision"]);
                    revision = (from r in db.HomepageRevisions
                                where r.ID == revisionID
                                select r).SingleOrDefault<HomepageRevision>();
                    HomepageRevision latestRevision = (from r in db.HomepageRevisions
                                                       orderby r.ID descending
                                                       select r).First<HomepageRevision>();
                    isLatest = revision.ID == latestRevision.ID;
                }
                else
                {
                    revision = (from r in db.HomepageRevisions
                                orderby r.ID descending
                                select r).First<HomepageRevision>();
                    isLatest = true;
                }

                if (revision == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                if (!isLatest)
                {
                    if (!Permission.Check("homepage.history.read", false)) return;
                }

                Page.DataBind();
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = true;
        trPreview.Visible = true;
        divPreview.InnerHtml = WikiParser.Parse(txtContent.Text);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("homepage.update", false)) return;

        using (MooDB db = new MooDB())
        {
            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
            HomepageRevision revision=new HomepageRevision()
            {
                Title = txtTitle.Text,
                Content = txtContent.Text,
                Reason = txtReason.Text,
                CreatedBy = currentUser
            };

            db.HomepageRevisions.AddObject(revision);

            db.SaveChanges();
            Logger.Info(db, "更新主页，新版本为#" + revision.ID);
        }

        PageUtil.Redirect("操作成功", "~/");
    }
}