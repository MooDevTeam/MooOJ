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
public partial class Solution_Compare : System.Web.UI.Page
{
    protected Problem problem;
    protected SolutionRevision revisionOld;
    protected SolutionRevision revisionNew;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("solution.history.read", true)) return;
        using (MooDB db = new MooDB())
        {
            if (Request["revisionOld"] != null && Request["revisionNew"] != null)
            {
                CollectEntity(db, int.Parse(Request["revisionOld"]), int.Parse(Request["revisionNew"]));
            }
            
            if(revisionOld==null || revisionNew==null){
                PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                return;
            }

            Page.DataBind();
        }
    }

    void CollectEntity(MooDB db, int oldID, int newID)
    {
        revisionOld = (from r in db.SolutionRevisions
                       where r.ID == oldID
                       select r).Single<SolutionRevision>();
        revisionNew = (from r in db.SolutionRevisions
                       where r.ID == newID
                       select r).Single<SolutionRevision>();
        if (revisionOld.ID > revisionNew.ID)
        {
            SolutionRevision tmp = revisionNew;
            revisionNew = revisionOld;
            revisionOld = tmp;
        }

        if (revisionOld.Problem.ID != revisionNew.Problem.ID)
        {
            throw new Exception("试图比较不同题目之间的版本");
        }

        problem = revisionNew.Problem;
    }
}