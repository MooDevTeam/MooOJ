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
            txtContent.Text = "!! 描述" + Environment.NewLine
                + "<在此填入题目描述>" + Environment.NewLine + Environment.NewLine
                + "!! 输入格式" + Environment.NewLine
                + "<在此填入样例输入格式>" + Environment.NewLine + Environment.NewLine
                + "!! 输出格式" + Environment.NewLine
                + "<在此填入样例输出格式>" + Environment.NewLine + Environment.NewLine
                + "!! 样例输入" + Environment.NewLine
                + "{code:plaintext}" + Environment.NewLine
                + "<在此填入样例输入>" + Environment.NewLine
                + "{code:plaintext}" + Environment.NewLine + Environment.NewLine
                + "!! 样例输出" + Environment.NewLine
                + "{code:plaintext}" + Environment.NewLine
                + "<在此填入样例输出>" + Environment.NewLine
                + "{code:plaintext}" + Environment.NewLine + Environment.NewLine
                + "!! 限制" + Environment.NewLine
                + "<在此填入各种限制>" + Environment.NewLine + Environment.NewLine
                + "!! 注释" + Environment.NewLine
                + "<在此填入注释>" + Environment.NewLine + Environment.NewLine
                + "!! 来源" + Environment.NewLine
                + "<在此填入题目来源>";
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
                AllowTesting = true,
                Lock = false,
                LockPost = false,
                LockRecord = false,
                LockSolution = false,
                LockTestCase = false,
                Hidden = false,
                TestCaseHidden = false,
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

            Logger.Info(db, "创建题目#" + problemID);
        }
        PageUtil.Redirect("创建成功", "~/Problem/?id=" + problemID);
    }
}