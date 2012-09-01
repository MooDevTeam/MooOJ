using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;

public partial class TestCase_Default : System.Web.UI.Page
{
    protected const int DISPLAY_LENGTH = 1000;

    protected Problem problem;
    protected TestCase testCase;
    protected TranditionalTestCase asTranditional;
    protected SpecialJudgedTestCase asSpecialJudged;
    protected InteractiveTestCase asInteractive;
    protected AnswerOnlyTestCase asAnswerOnly;
    protected void Page_Load(object sender, EventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            if (Request["id"] != null)
            {
                CollectEntityByID(db, int.Parse(Request["id"]));
            }

            if (testCase == null)
            {
                PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                return;
            }

            if (testCase.Problem.TestCaseHidden)
            {
                if (!Permission.Check("testcase.hidden.read", false)) return;
            }
            else
            {
                if (!Permission.Check("testcase.read", true)) return;
            }

            if (testCase is TranditionalTestCase)
            {
                asTranditional = testCase as TranditionalTestCase;
                multiView.SetActiveView(viewTranditional);
            }
            else if (testCase is SpecialJudgedTestCase)
            {
                asSpecialJudged = testCase as SpecialJudgedTestCase;
                multiView.SetActiveView(viewSpecialJudged);
            }
            else if (testCase is InteractiveTestCase)
            {
                asInteractive = testCase as InteractiveTestCase;
                multiView.SetActiveView(viewInteractive);
            }
            else if (testCase is AnswerOnlyTestCase)
            {
                asAnswerOnly = testCase as AnswerOnlyTestCase;
                multiView.SetActiveView(viewAnswerOnly);
            }
            else
            {
                PageUtil.Redirect("未知的测试数据类型", "~/");
                return;
            }

            multiView.GetActiveView().DataBind();
            linkbar.DataBind();
            Header.DataBind();
        }
    }

    void CollectEntityByID(MooDB db, int id)
    {
        testCase = (from t in db.TestCases
                    where t.ID == id
                    select t).SingleOrDefault<TestCase>();
        problem = testCase.Problem;
    }
}