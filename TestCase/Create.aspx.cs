using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.Utility;
using Moo.DB;
public partial class TestCase_Create : System.Web.UI.Page
{
    protected bool canCreate;

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

                canCreate = problem.LockTestCase ? Permission.Check("testcase.locked.create", false, false) : Permission.Check("testcase.create", false, false);

                ViewState["problemID"] = problem.ID;
                Page.DataBind();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int problemID = (int)ViewState["problemID"];
        int testCaseID;
        using (MooDB db = new MooDB())
        {
            problem = (from p in db.Problems
                       where p.ID == problemID
                       select p).Single<Problem>();

            if (problem.LockTestCase)
            {
                if (!Permission.Check("testcase.locked.create", false)) return;
            }
            else
            {
                if (!Permission.Check("testcase.create", false)) return;
            }

            TestCase testCase;
            switch (problem.Type)
            {
                case "Tranditional":
                    testCase = new TranditionalTestCase()
                    {
                        Score = int.Parse(txtScore.Text),
                        TimeLimit = int.Parse(txtTimeLimit.Text),
                        MemoryLimit = int.Parse(txtMemoryLimit.Text),
                        Input = fileInput.FileBytes,
                        Answer = fileAnswer.FileBytes,
                        Problem = problem
                    };
                    break;
                case "SpecialJudged":
                    int judgerID = int.Parse(txtJudger.Text);
                    UploadedFile judger = (from f in db.UploadedFiles
                                           where f.ID == judgerID
                                           select f).Single<UploadedFile>();
                    testCase = new SpecialJudgedTestCase()
                    {
                        Score = int.Parse(txtScore.Text),
                        TimeLimit = int.Parse(txtTimeLimit.Text),
                        MemoryLimit = int.Parse(txtMemoryLimit.Text),
                        Input = fileInput.FileBytes,
                        Answer = fileAnswer.FileBytes,
                        Judger = judger,
                        Problem = problem
                    };
                    break;
                default:
                    PageUtil.Redirect("未知的题目类型", "~/");
                    return;
            }
            db.TestCases.AddObject(testCase);
            db.SaveChanges();

            testCaseID = testCase.ID;
        }

        PageUtil.Redirect("操作成功", "~/TestCase/?id=" + testCaseID);
    }
    protected void validateJudger_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (MooDB db = new MooDB())
        {
            int fileID = int.Parse(txtJudger.Text);
            UploadedFile theFile = (from f in db.UploadedFiles
                                    where f.ID == fileID
                                    select f).SingleOrDefault<UploadedFile>();
            args.IsValid = theFile != null;
        }
    }
}