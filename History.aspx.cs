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
public partial class History : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("homepage.history.read", false)) return;
        if (!Page.IsPostBack)
        {
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
        Response.Redirect("~/Compare.aspx?revisionOld=" + oldId + "&revisionNew=" + newId);
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Permission.Check("homepage.history.delete", false)) e.Cancel = true;
        int revisionID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            HomepageRevision revision = (from r in db.HomepageRevisions
                                        where r.ID == revisionID
                                        select r).Single<HomepageRevision>();
            if (db.HomepageRevisions.Count()<=1)
            {
                e.Cancel = true;
                infoDeletingLast.Visible = true;
            }
        }
    }
    protected void grid_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            Logger.Warning(db, "删除主页版本#" + e.Keys[0]);
        }
    }
}