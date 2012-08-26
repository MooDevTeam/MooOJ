using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
public partial class Post_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("post.list", true)) return;
        if (!Page.IsPostBack)
        {
            if (Request["problemID"] != null)
            {
                int problemID=int.Parse(Request["problemID"]);
                dataSource.Where += " and it.[Problem].ID=" + problemID;
            }

            Page.DataBind();
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Permission.Check("post.delete", false)) e.Cancel = true;
    }
}