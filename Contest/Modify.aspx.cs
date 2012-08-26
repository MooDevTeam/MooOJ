using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Text;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Contest_Modify : System.Web.UI.Page
{
    protected Contest contest;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("contest.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    int contestID = int.Parse(Request["id"]);
                    contest = (from c in db.Contests
                               where c.ID == contestID
                               select c).SingleOrDefault<Contest>();
                }

                if (contest == null)
                {
                    PageUtil.Redirect("找不到相关内容", "~/");
                    return;
                }

                ViewState["contestID"] = contest.ID;
                Page.DataBind();
            }
        }
    }
    protected void ValidatePositiveTimeSpan(object source, ServerValidateEventArgs args)
    {
        args.IsValid = timeEnd.Value >= timeStart.Value;
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = true;
        trPreview.Visible = true;
        divPreview.InnerHtml = WikiParser.Parse(txtDescription.Text);
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("contest.modify", false)) return;

        int contestID = (int)ViewState["contestID"];
        using (MooDB db = new MooDB())
        {
            Contest contest = (from c in db.Contests
                               where c.ID == contestID
                               select c).Single<Contest>();

            contest.Title = txtTitle.Text;
            contest.Description = txtDescription.Text;
            contest.StartTime = timeStart.Value;
            contest.EndTime = timeEnd.Value;

            contest.AllowTestingOnStart = chkAllowTestingOnStart.Checked;
            contest.HideTestCaseOnStart = chkHideTestCaseOnStart.Checked;
            contest.LockProblemOnStart = chkLockProblemOnStart.Checked;
            contest.LockTestCaseOnStart = chkLockTestCaseOnStart.Checked;
            contest.LockSolutionOnStart = chkLockSolutionOnStart.Checked;
            contest.LockPostOnStart = chkLockPostOnStart.Checked;
            contest.LockRecordOnStart = chkLockRecordOnStart.Checked;
            contest.HideProblemOnStart = chkHideProblemOnStart.Checked;

            contest.AllowTestingOnEnd = chkAllowTestingOnEnd.Checked;
            contest.HideTestCaseOnEnd = chkHideTestCaseOnEnd.Checked;
            contest.LockProblemOnEnd = chkLockProblemOnEnd.Checked;
            contest.LockTestCaseOnEnd = chkLockTestCaseOnEnd.Checked;
            contest.LockSolutionOnEnd = chkLockSolutionOnEnd.Checked;
            contest.LockPostOnEnd = chkLockPostOnEnd.Checked;
            contest.LockRecordOnEnd = chkLockRecordOnEnd.Checked;
            contest.HideProblemOnEnd = chkHideProblemOnEnd.Checked;

            db.SaveChanges();
        }

        PageUtil.Redirect("操作成功", "~/Contest/?id=" + contestID);
    }
}