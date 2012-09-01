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
            if (problem.Type == "Tranditional")
            {
                testCase = new TranditionalTestCase()
                {
                    Score = int.Parse(txtScore.Text),
                    TimeLimit = int.Parse(txtTimeLimit.Text),
                    MemoryLimit = int.Parse(txtMemoryLimit.Text),
                    Input = fileInput.FileBytes,
                    Answer = fileAnswer.FileBytes,
                    Problem = problem
                };
            }
            else if (problem.Type == "SpecialJudged")
            {
                int judgerID = int.Parse(txtJudger.Text);
                UploadedFile judger = (from f in db.UploadedFiles
                                       where f.ID == judgerID
                                       select f).Single<UploadedFile>();
                testCase = new SpecialJudgedTestCase()
                {
                    TimeLimit = int.Parse(txtTimeLimit.Text),
                    MemoryLimit = int.Parse(txtMemoryLimit.Text),
                    Input = fileInput.FileBytes,
                    Answer = fileAnswer.FileBytes,
                    Judger = judger,
                    Problem = problem
                };
            }
            else if (problem.Type == "Interactive")
            {
                int invokerID = int.Parse(txtInvoker.Text);
                UploadedFile invoker = (from f in db.UploadedFiles
                                        where f.ID == invokerID
                                        select f).Single<UploadedFile>();
                testCase = new InteractiveTestCase()
                {
                    Invoker = invoker,
                    MemoryLimit = int.Parse(txtMemoryLimit.Text),
                    TimeLimit = int.Parse(txtTimeLimit.Text),
                    Problem = problem,
                    TestData = fileTestData.FileBytes,
                };
            }
            else if (problem.Type == "AnswerOnly")
            {
                int judgerID = int.Parse(txtJudger.Text);
                UploadedFile judger = (from f in db.UploadedFiles
                                       where f.ID == judgerID
                                       select f).Single<UploadedFile>();
                testCase = new AnswerOnlyTestCase()
                {
                    Judger = judger,
                    Problem = problem,
                    TestData = fileTestData.FileBytes,
                };
            }
            else
            {
                PageUtil.Redirect("未知的题目类型", "~/");
                return;
            }
            db.TestCases.AddObject(testCase);
            db.SaveChanges();

            testCaseID = testCase.ID;
        }

        PageUtil.Redirect("操作成功", "~/TestCase/?id=" + testCaseID);
    }
    protected void ValidateFileID(object source, ServerValidateEventArgs args)
    {
        CustomValidator validator = (CustomValidator)source;
        TextBox toValidate = (TextBox)validator.Parent.FindControl(validator.ControlToValidate);
        using (MooDB db = new MooDB())
        {
            int fileID = int.Parse(toValidate.Text);
            UploadedFile theFile = (from f in db.UploadedFiles
                                    where f.ID == fileID
                                    select f).SingleOrDefault<UploadedFile>();
            args.IsValid = theFile != null;
        }
    }
}