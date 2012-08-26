using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Text;
using Moo.Utility;
public partial class Compare : System.Web.UI.Page
{
    protected HomepageRevision revisionOld;
    protected HomepageRevision revisionNew;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("homepage.history.read", false)) return;
        using (MooDB db = new MooDB())
        {
            if (Request["revisionOld"] != null && Request["revisionNew"] != null)
            {
                CollectEntity(db, int.Parse(Request["revisionOld"]), int.Parse(Request["revisionNew"]));
            }
            
            if(revisionOld==null || revisionNew==null){
                PageUtil.Redirect("找不到相关内容", "~/");
                return;
            }

            Page.DataBind();
        }
    }

    void CollectEntity(MooDB db, int oldID, int newID)
    {
        revisionOld = (from r in db.HomepageRevisions
                       where r.ID == oldID
                       select r).Single<HomepageRevision>();
        revisionNew = (from r in db.HomepageRevisions
                       where r.ID == newID
                       select r).Single<HomepageRevision>();
        if (revisionOld.ID > revisionNew.ID)
        {
            HomepageRevision tmp = revisionNew;
            revisionNew = revisionOld;
            revisionOld = tmp;
        }
    }
}