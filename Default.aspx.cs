using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using System.Security.Cryptography;
using System.Text;
using Moo.Utility;
using Moo.Tester.MooTester;
using Moo.DB;
public partial class _Default : System.Web.UI.Page
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
                    PageUtil.Redirect("找不到相关内容", "~/");
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
}