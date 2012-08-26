using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class Problem_History : System.Web.UI.Page
{
    protected Problem problem;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("problem.history.read", true)) return;
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
                    problem = PageUtil.GetEntity<ProblemRevision>(o).Problem;
                }
            });

            if (problem == null)
            {
                PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                return;
            }

            if (problem.Hidden)
            {
                if (!Permission.Check("problem.hidden.read", false)) return;
            }

            Page.DataBind();
        }
    }
    protected void chkCompare_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow currenRow = (GridViewRow)chk.Parent.Parent;
        if (chk.Checked)
        {
            int count = 0;
            foreach (GridViewRow row in grid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (((CheckBox)row.FindControl("chkCompare")).Checked)
                    {
                        count++;
                    }
                }
            }
            if (count == 2)
            {
                foreach (GridViewRow row in grid.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (!((CheckBox)row.FindControl("chkCompare")).Checked)
                        {
                            ((CheckBox)row.FindControl("chkCompare")).Enabled = false;
                        }
                    }
                }
                btnCompare.Enabled = true;
            }
        }
        else
        {
            int count = 0;
            foreach (GridViewRow row in grid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    if (((CheckBox)row.FindControl("chkCompare")).Checked)
                    {
                        count++;
                    }
                }
            }
            if (count == 1)
            {
                foreach (GridViewRow row in grid.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {

                        ((CheckBox)row.FindControl("chkCompare")).Enabled = true;
                    }
                }
                btnCompare.Enabled = false;
            }
        }
    }
    protected void btnCompare_Click(object sender, EventArgs e)
    {
        int? oldId = null, newId = null;
        foreach (GridViewRow row in grid.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow && ((CheckBox)row.FindControl("chkCompare")).Checked)
            {
                if (newId == null)
                {
                    newId = (int)grid.DataKeys[row.RowIndex].Value;
                }
                else
                {
                    oldId = (int)grid.DataKeys[row.RowIndex].Value;
                }
            }
        }
        Response.Redirect("~/Problem/Compare.aspx?revisionOld=" + oldId + "&revisionNew=" + newId);
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Permission.Check("problem.history.delete", false)) e.Cancel = true;
        int revisionID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            ProblemRevision revision = (from r in db.ProblemRevisions
                                        where r.ID == revisionID
                                        select r).Single<ProblemRevision>();
            if (revision.Problem.LatestRevision.ID == revision.ID)
            {
                e.Cancel = true;
                infoDeletingLatest.Visible = true;
            }
        }
    }
}