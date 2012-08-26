using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class Problem_Default : System.Web.UI.Page
{
    protected bool canRead;

    protected Problem problem;
    protected ProblemRevision revision;
    protected void Page_Load(object sender, EventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            if (Request["revision"] != null)
            {
                CollectEntityByRevision(db, int.Parse(Request["revision"]));
            }
            else if (Request["id"] != null)
            {
                CollectEntityById(db, int.Parse(Request["id"]));
            }

            if (problem == null || revision == null)
            {
                PageUtil.Redirect("找不到相关内容", "~/");
                return;
            }

            if (problem.LatestRevision.ID != revision.ID)
            {
                if (!Permission.Check("problem.history.read", true)) return;
            }

            canRead = problem.Hidden ? Permission.Check("problem.hidden.read", false, false) : Permission.Check("problem.read", true, false);

            Page.DataBind();
        }
    }

    void CollectEntityById(MooDB db, int id)
    {
        problem = (from p in db.Problems
                   where p.ID == id
                   select p).SingleOrDefault<Problem>();
        revision = problem == null ? null : problem.LatestRevision;
    }

    void CollectEntityByRevision(MooDB db, int id)
    {
        revision = (from r in db.ProblemRevisions
                    where r.ID == id
                    select r).SingleOrDefault<ProblemRevision>();
        problem = revision == null ? null : revision.Problem;
    }
}