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
public partial class Problem_Update : System.Web.UI.Page
{
    protected bool canRead;
    protected bool canUpdate;

    protected Problem problem;
    protected ProblemRevision revision;
    protected void Page_Load(object sender, EventArgs e)
    {
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

                if (problem.LatestRevision.ID != revision.ID)
                {
                    if (!Permission.Check("problem.history.read", true)) return;
                }

                canRead = problem.Hidden ? Permission.Check("problem.hidden.read", false,false) : Permission.Check("problem.read", true,false);

                canUpdate = problem.Lock ? Permission.Check("problem.locked.update", false, false) : Permission.Check("problem.update", false, false);

                ViewState["problemID"] = problem.ID;
                Page.DataBind();
            }
        }
    }
    void CollectEntityByRevision(MooDB db, int id)
    {
        revision = (from r in db.ProblemRevisions
                    where r.ID == id
                    select r).SingleOrDefault<ProblemRevision>();
        problem = revision == null ? null : revision.Problem;
    }
    void CollectEntityById(MooDB db, int id)
    {
        problem = (from p in db.Problems
                   where p.ID == id
                   select p).SingleOrDefault<Problem>();
        revision = problem == null ? null : problem.LatestRevision;
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
            if (problem.Lock)
            {
                if (!Permission.Check("problem.locked.update", false)) return;
            }
            else
            {
                if (!Permission.Check("problem.update", false)) return;
            }

            User user = ((SiteUser)User.Identity).GetDBUser(db);
            ProblemRevision revision = new ProblemRevision()
            {
                Content = txtContent.Text,
                Problem = problem,
                Reason = txtReason.Text,
                CreatedBy = user
            };
            db.ProblemRevisions.AddObject(revision);
            problem.LatestRevision = revision;
            db.SaveChanges();

            Logger.Info(db, string.Format("更新题目#{0}，新版本为#{1}", problem.ID, revision.ID));
        }
        PageUtil.Redirect("更新成功", "~/Problem/?id=" + problemID);
    }
}