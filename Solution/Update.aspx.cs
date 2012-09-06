using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Text;
using Moo.Utility;
public partial class Solution_Update : System.Web.UI.Page
{
    protected bool canUpdate;

    protected Problem problem;
    protected SolutionRevision revision;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("solution.read", true)) return;
        if (!Page.IsPostBack)
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
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                if (problem.LatestSolution.ID != revision.ID)
                {
                    if (!Permission.Check("solution.history.read", true)) return;
                }

                canUpdate = problem.LockSolution ? Permission.Check("solution.locked.update", false, false) : Permission.Check("solution.update", false, false);

                ViewState["problemID"] = problem.ID;
                Page.DataBind();
            }
        }
    }
    void CollectEntityByRevision(MooDB db, int id)
    {
        revision = (from r in db.SolutionRevisions
                    where r.ID == id
                    select r).SingleOrDefault<SolutionRevision>();
        problem = revision == null ? null : revision.Problem;
    }
    void CollectEntityById(MooDB db, int id)
    {
        problem = (from p in db.Problems
                   where p.ID == id
                   select p).SingleOrDefault<Problem>();
        revision = problem == null ? null : problem.LatestSolution;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int problemID = (int)ViewState["problemID"];
        using (MooDB db = new MooDB())
        {
            Problem problem = (from p in db.Problems
                               where p.ID == problemID
                               select p).Single<Problem>();
            if (problem.LockSolution)
            {
                if (!Permission.Check("solution.locked.update", false)) return;
            }
            else
            {
                if (!Permission.Check("solution.update", false)) return;
            }
            User user = ((SiteUser)User.Identity).GetDBUser(db);
            SolutionRevision revision = new SolutionRevision()
            {
                Content = txtContent.Text,
                Problem = problem,
                Reason = txtReason.Text,
                CreatedBy = user
            };
            db.SolutionRevisions.AddObject(revision);
            problem.LatestSolution = revision;
            db.SaveChanges();

            Logger.Info(db, string.Format("更新题目#{0}的题解，新版本为#{1}", ViewState["problemID"], revision.ID));
        }
        PageUtil.Redirect("更新成功", "~/Solution/?id=" + problemID);
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        divPreview.InnerHtml = WikiParser.Parse(txtContent.Text);
        btnSubmit.Enabled = true;
        trPreview.Visible = true;
    }
}