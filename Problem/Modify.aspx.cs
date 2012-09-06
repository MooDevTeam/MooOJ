using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Utility;
using Moo.Authorization;
public partial class Problem_Modify : System.Web.UI.Page
{
    protected Problem problem;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    int problemID = int.Parse(Request["id"]);
                    problem = (from p in db.Problems
                               where p.ID == problemID
                               select p).SingleOrDefault<Problem>();
                }

                if (problem == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                ViewState["problemID"] = problem.ID;
                Page.DataBind();
                ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(problem.Type));
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("problem.modify", false)) return;

        int problemID = (int)ViewState["problemID"];
        using (MooDB db = new MooDB())
        {
            problem = (from p in db.Problems
                       where p.ID == problemID
                       select p).Single<Problem>();
            problem.Name = txtName.Text;
            problem.Type = ddlType.SelectedValue;
            problem.Hidden = chkHidden.Checked;
            problem.TestCaseHidden = chkTestCaseHidden.Checked;
            problem.AllowTesting = chkAllowTesting.Checked;
            problem.Lock = chkLock.Checked;
            problem.LockPost = chkLockPost.Checked;
            problem.LockRecord = chkLockRecord.Checked;
            problem.LockSolution = chkLockSolution.Checked;
            problem.LockTestCase = chkLockTestCase.Checked;

            db.SaveChanges();
            Logger.Info(db, "修改题目#" + problem.ID);
        }

        PageUtil.Redirect("操作成功", "~/Problem/?id=" + problemID);
    }
}