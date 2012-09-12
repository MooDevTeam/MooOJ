using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Record_Create : System.Web.UI.Page
{
    protected bool canCreate;

    protected Problem problem;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                int problemID = int.Parse(Request["problemID"]);
                problem = (from p in db.Problems
                           where p.ID == problemID
                           select p).Single<Problem>();
                if (problem.Type == "AnswerOnly")
                {
                    AddToAnswerArea(db);
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["problemID"] != null)
                {
                    CollectEntityByProblemID(db, int.Parse(Request["problemID"]));
                }

                if (problem == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                canCreate = problem.LockRecord ? Permission.Check("record.locked.create", false, false) : Permission.Check("record.create", false, false);

                if (problem.Type == "AnswerOnly")
                {
                    AddToAnswerArea(db);
                }
                else if (User.Identity.IsAuthenticated)
                {
                    User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
                    ddlLanguage.SelectedIndex = ddlLanguage.Items.IndexOf(ddlLanguage.Items.FindByValue(currentUser.PreferredLanguage));
                }

                ViewState["problemID"] = problem.ID;
                Page.DataBind();
            }
        }
    }

    void CollectEntityByProblemID(MooDB db, int id)
    {
        problem = (from p in db.Problems
                   where p.ID == id
                   select p).SingleOrDefault<Problem>();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int problemID = (int)ViewState["problemID"];
        using (MooDB db = new MooDB())
        {
            problem = (from p in db.Problems
                       where p.ID == problemID
                       select p).Single<Problem>();
            if (problem.LockRecord)
            {
                if (!Permission.Check("record.locked.create", false)) return;
            }
            else
            {
                if (!Permission.Check("record.create", false)) return;
            }

            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
            Record record;
            if (problem.Type == "AnswerOnly")
            {
                record = new Record()
                {
                    Problem = problem,
                    User = currentUser,
                    Code = MergeAnswers(db),
                    Language = "plaintext",
                    PublicCode = chkPublicCode.Checked,
                    CreateTime = DateTimeOffset.Now
                };
            }
            else
            {
                record = new Record()
                {
                    Problem = problem,
                    User = currentUser,
                    Code = txtCode.Text,
                    Language = ddlLanguage.SelectedValue,
                    PublicCode = chkPublicCode.Checked,
                    CreateTime = DateTimeOffset.Now
                };
                currentUser.PreferredLanguage = ddlLanguage.SelectedValue;
            }
            db.Records.AddObject(record);

            problem.SubmissionCount++;
            if (!(from r in db.Records
                  where r.ID != record.ID && r.User.ID == currentUser.ID && r.Problem.ID == problem.ID
                  select r).Any())
            {
                problem.SubmissionUser++;
            }

            db.SaveChanges();

            Logger.Info(db, "创建记录#" + record.ID);
        }

        PageUtil.Redirect("创建成功", "~/Record/List.aspx?userID=" + ((SiteUser)User.Identity).ID);
    }

    string MergeAnswers(MooDB db)
    {
        var testCases = from t in db.TestCases.OfType<AnswerOnlyTestCase>()
                        where t.Problem.ID == problem.ID
                        select t;
        StringBuilder sb = new StringBuilder();
        foreach (AnswerOnlyTestCase testCase in testCases)
        {
            TextBox textBox = (TextBox)answerArea.FindControl("txtAnswer" + testCase.ID);
            string answer = textBox.Text;
            sb.AppendLine(string.Format("<Moo:Answer testCase='{0}'>{1}</Moo:Answer>", testCase.ID, answer));
        }
        return sb.ToString();
    }

    void AddToAnswerArea(MooDB db)
    {
        var testCases = from t in db.TestCases.OfType<AnswerOnlyTestCase>()
                        where t.Problem.ID == problem.ID
                        select t;
        foreach (AnswerOnlyTestCase testCase in testCases)
        {
            HtmlGenericControl fieldset = new HtmlGenericControl("fieldset");
            HtmlGenericControl legend = new HtmlGenericControl("legend");
            legend.Controls.Add(new HyperLink()
            {
                Text = "测试点#" + testCase.ID,
                NavigateUrl = "~/TestCase/?id=" + testCase.ID
            });
            legend.Controls.Add(new Literal()
            {
                Text = "的答案"
            });
            fieldset.Controls.Add(legend);
            fieldset.Controls.Add(new TextBox()
            {
                ID = "txtAnswer" + testCase.ID,
                ClientIDMode = ClientIDMode.Static,
                Width = new Unit(100, UnitType.Percentage),
                Rows = 10,
                TextMode = TextBoxMode.MultiLine
            });
            answerArea.Controls.Add(fieldset);
        }
    }
}