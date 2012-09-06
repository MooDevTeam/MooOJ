using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Post_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("post.list", true)) return;
        if (!Page.IsPostBack)
        {
            Page.DataBind();
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Permission.Check("post.delete", false)) e.Cancel = true;
    }
    protected void grid_RowDeleted(object sender, GridViewDeletedEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            Logger.Warning(db, "删除帖子#" + e.Keys[0]);
        }
    }
}