using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Common;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Post_Default : System.Web.UI.Page
{
    protected Post post;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("post.read", true)) return;

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
                    post = PageUtil.GetEntity<PostItem>(o).Post;
                }
            });

            if (post == null)
            {
                using (MooDB db = new MooDB())
                {
                    if (Request["id"] != null)
                    {
                        int postID = int.Parse(Request["id"]);
                        post = (from p in db.Posts
                                where p.ID == postID
                                select p).SingleOrDefault<Post>();
                    }
                    if (post == null)
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
        if (!Permission.Check("post.item.delete", false)) e.Cancel = true;
    }
}