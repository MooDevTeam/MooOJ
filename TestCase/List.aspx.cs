using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class TestCase_List : System.Web.UI.Page
{
    protected Problem problem;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("testcase.list", true)) return;
        if (!Page.IsPostBack)
        {
            string viewName = null;
            foreach (string s in ((IDataSource)dataSource).GetViewNames())
            {
                viewName = s;
            }
            DataSourceView view = ((IDataSource)dataSource).GetView(viewName);
            view.Select(new DataSourceSelectArguments(0, 1), delegate(IEnumerable data)
            {
                foreach (object o in data)
                {
                    problem = PageUtil.GetEntity<TestCase>(o).Problem;
                }
            });

            if (problem == null)
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

                    Page.DataBind();
                }
            }
            else
            {
                Page.DataBind();
            }
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        int testCaseID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            TestCase testCase = (from t in db.TestCases
                                 where t.ID == testCaseID
                                 select t).Single<TestCase>();
            bool allowed = Permission.Check("testcase.delete", false, false)
                   || User.Identity.IsAuthenticated && ((SiteUser)User.Identity).ID == testCase.CreatedBy.ID;
            if (!allowed)
            {
                Permission.Check("i'm superman.", false);
                return;
            }
            db.TestCases.DeleteObject(testCase);
            db.SaveChanges();
        }

        grid.Rows[e.RowIndex].Visible = false;
    }
}