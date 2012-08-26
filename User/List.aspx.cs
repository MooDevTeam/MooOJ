using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
public partial class User_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("user.list", true)) return;
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
        int? userAID = null, userBID = null;
        foreach (GridViewRow row in grid.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow && ((CheckBox)row.FindControl("chkCompare")).Checked)
            {
                if (userAID == null)
                {
                    userAID = (int)grid.DataKeys[row.RowIndex].Value;
                }
                else
                {
                    userBID = (int)grid.DataKeys[row.RowIndex].Value;
                }
            }
        }
        Response.Redirect("~/User/Compare.aspx?userA=" + userAID + "&userB=" + userBID);
    }
}