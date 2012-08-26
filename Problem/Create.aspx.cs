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
public partial class Problem_Create : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Page.DataBind();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("problem.create", false)) return;

        int problemID;
        using (MooDB db = new MooDB())
        {
            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
            Problem problem = new Problem()
            {
                Name = txtName.Text,
                Type = ddlType.SelectedValue,
                AllowTesting=true,
                Lock=false,
                LockPost=false,
                LockRecord=false,
                LockSolution=false,
                LockTestCase=false,
                Hidden=false,
                TestCaseHidden=false,
            };
            ProblemRevision revision = new ProblemRevision()
            {
                Problem = problem,
                Content = txtContent.Text,
                Reason = "创建题目",
                CreatedBy = currentUser
            };
            SolutionRevision solution = new SolutionRevision()
            {
                Problem = problem,
                Content = "暂无题解",
                Reason = "创建题解",
                CreatedBy = currentUser
            };
            problem.LatestRevision = revision;
            problem.LatestSolution = solution;

            db.ProblemRevisions.AddObject(revision);
            db.SaveChanges();

            problemID = revision.Problem.ID;
        }
        PageUtil.Redirect("操作成功", "~/Problem/?id=" + problemID);
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = true;
        trPreview.Visible = true;
        divPreview.InnerHtml = WikiParser.Parse(txtContent.Text);
    }
}