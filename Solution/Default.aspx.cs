using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class Solution_Default : System.Web.UI.Page
{
    protected SolutionRevision revision;
    protected Problem problem;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("solution.read", true)) return;

        using (MooDB db = new MooDB())
        {
            if (Request["revision"] != null)
            {
                CollectEntityByRevision(db,int.Parse(Request["revision"]));
            }
            else if (Request["id"] != null)
            {
                CollectEntityByID(db,int.Parse(Request["id"]));
            }

            if (problem == null || revision==null)
            {
                PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                return;
            }

            if (problem.LatestSolution.ID != revision.ID)
            {
                if (!Permission.Check("solution.history.read", true)) return;
            }

            Page.DataBind();
        }
    }

    void CollectEntityByID(MooDB db, int id)
    {
        problem = (from p in db.Problems
                   where p.ID == id
                   select p).SingleOrDefault<Problem>();
        revision = problem == null ? null : problem.LatestSolution;
    }

    void CollectEntityByRevision(MooDB db, int id)
    {
        revision = (from r in db.SolutionRevisions
                    where r.ID == id
                    select r).SingleOrDefault<SolutionRevision>();
        problem = revision == null ? null : revision.Problem;
    }
}