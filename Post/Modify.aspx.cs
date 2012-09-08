using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Post_Modify : System.Web.UI.Page
{
    protected Post post;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("post.read", true)) return;

        if (!Page.IsPostBack)
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

                ViewState["postID"] = post.ID;
                Page.DataBind();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("post.modify", false)) return;

        int postID = (int)ViewState["postID"];
        using (MooDB db = new MooDB())
        {
            post = (from p in db.Posts
                    where p.ID == postID
                    select p).SingleOrDefault<Post>();

            post.Name = txtName.Text;
            post.Lock = chkLock.Checked;
            post.OnTop = chkOnTop.Checked;
            db.SaveChanges();

            Logger.Info(db, "修改帖子#" + post.ID);
        }

        PageUtil.Redirect("修改成功", "~/Post/?id=" + postID);
    }
}