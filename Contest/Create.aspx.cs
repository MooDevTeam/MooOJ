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
public partial class Contest_Create : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Page.DataBind();
        }
    }
    protected void ValidatePositiveTimeSpan(object source, ServerValidateEventArgs args)
    {
        args.IsValid = timeEnd.Value >= timeStart.Value;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("contest.create", false)) return;

        int contestID;
        using (MooDB db = new MooDB())
        {
            Contest contest = new Contest()
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                Status = "Before",
                StartTime = timeStart.Value,
                EndTime = timeEnd.Value,
                AllowTestingOnStart = chkAllowTestingOnStart.Checked,
                HideTestCaseOnStart = chkHideTestCaseOnStart.Checked,
                LockProblemOnStart = chkLockProblemOnStart.Checked,
                LockTestCaseOnStart = chkLockTestCaseOnStart.Checked,
                LockSolutionOnStart = chkLockSolutionOnStart.Checked,
                LockPostOnStart = chkLockPostOnStart.Checked,
                LockRecordOnStart=chkLockRecordOnStart.Checked,
                HideProblemOnStart=chkHideProblemOnStart.Checked,

                AllowTestingOnEnd=chkAllowTestingOnEnd.Checked,
                HideTestCaseOnEnd=chkHideTestCaseOnEnd.Checked,
                LockProblemOnEnd=chkLockProblemOnEnd.Checked,
                LockTestCaseOnEnd=chkLockTestCaseOnEnd.Checked,
                LockSolutionOnEnd=chkLockSolutionOnEnd.Checked,
                LockPostOnEnd=chkLockPostOnEnd.Checked,
                LockRecordOnEnd=chkLockRecordOnEnd.Checked ,
                HideProblemOnEnd=chkHideProblemOnEnd.Checked
            };

            db.Contests.AddObject(contest);
            db.SaveChanges();
            contestID = contest.ID;

            Logger.Info(db, "创建比赛#" + contestID);
        }

        PageUtil.Redirect("创建成功", "~/Contest/?id=" + contestID);
    }
}