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
    protected SolutionRevision revisionOldPrev;
    protected SolutionRevision revisionOldNext;
    protected SolutionRevision revisionNewPrev;
    protected SolutionRevision revisionNewNext;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("solution.history.read", true)) return;
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["revisionOld"] != null && Request["revisionNew"] != null)
                {
                    CollectEntity(db, int.Parse(Request["revisionOld"]), int.Parse(Request["revisionNew"]));
                }

                if (revisionOld == null || revisionNew == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

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

                revisionOldPrev = (from r in db.SolutionRevisions
                                   where r.Problem.ID == problem.ID && r.ID < revisionOld.ID
                                   orderby r.ID descending
                                   select r).FirstOrDefault<SolutionRevision>();
                revisionNewPrev = (from r in db.SolutionRevisions
                                   where r.Problem.ID == problem.ID && r.ID < revisionNew.ID
                                   orderby r.ID descending
                                   select r).FirstOrDefault<SolutionRevision>();
                revisionOldNext = (from r in db.SolutionRevisions
                                   where r.Problem.ID == problem.ID && r.ID > revisionOld.ID
                                   orderby r.ID
                                   select r).FirstOrDefault<SolutionRevision>();
                revisionNewNext = (from r in db.SolutionRevisions
                                   where r.Problem.ID == problem.ID && r.ID > revisionNew.ID
                                   orderby r.ID
                                   select r).FirstOrDefault<SolutionRevision>();

                Page.DataBind();
            }
        }
    }

    void CollectEntity(MooDB db, int oldID, int newID)
    {
        revisionOld = (from r in db.SolutionRevisions
                       where r.ID == oldID
                       select r).SingleOrDefault<SolutionRevision>();
        revisionNew = (from r in db.SolutionRevisions
                       where r.ID == newID
                       select r).SingleOrDefault<SolutionRevision>();
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        Response.Redirect("~/Solution/Compare.aspx?revisionOld=" + txtRevisionOld.Text + "&revisionNew=" + txtRevisionNew.Text);
    }
    protected void ValidateRevisionID(object sender, ServerValidateEventArgs e)
    {
        TextBox toValidate = (TextBox)fieldQuery.FindControl(((CustomValidator)sender).ControlToValidate);
        int revisionID = int.Parse(toValidate.Text);
        using (MooDB db = new MooDB())
        {
            e.IsValid = (from r in db.SolutionRevisions
                         where r.ID == revisionID
                         select r).Any();
        }
    }
    protected void validateSameProblem_ServerValidate(object source, ServerValidateEventArgs args)
    {
        int revisionOldID = int.Parse(txtRevisionOld.Text), revisionNewID = int.Parse(txtRevisionNew.Text);
        using (MooDB db = new MooDB())
        {
            revisionOld = (from r in db.SolutionRevisions
                           where r.ID == revisionOldID
                           select r).SingleOrDefault<SolutionRevision>();
            revisionNew = (from r in db.SolutionRevisions
                           where r.ID == revisionNewID
                           select r).SingleOrDefault<SolutionRevision>();
            args.IsValid = revisionOld == null || revisionNew == null || revisionOld.Problem.ID == revisionNew.Problem.ID;
        }
    }
}