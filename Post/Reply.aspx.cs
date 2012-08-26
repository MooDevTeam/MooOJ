using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
using Moo.Text;
public partial class Post_Reply : System.Web.UI.Page
{
    protected string initialContent = "";
    protected bool canReply;

    protected Post post;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["replyTo"] != null)
                {
                    CollectEntityByReplyTo(db, int.Parse(Request["replyTo"]));
                }
                else if (Request["id"] != null)
                {
                    CollectEntityByID(db, int.Parse(Request["id"]));
                }

                if (post == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                if (post.Lock || post.Problem != null && post.Problem.LockPost)
                {
                    canReply = Permission.Check("post.locked.reply", false, false);
                }
                else
                {
                    canReply = Permission.Check("post.reply", false, false);
                }

                ViewState["postID"] = post.ID;
                Page.DataBind();
            }
        }
    }

    void CollectEntityByID(MooDB db, int id)
    {
        post = (from p in db.Posts
                where p.ID == id
                select p).SingleOrDefault<Post>();
    }

    void CollectEntityByReplyTo(MooDB db, int id)
    {
        PostItem postItem = (from p in db.PostItems
                             where p.ID == id
                             select p).SingleOrDefault<PostItem>();
        if (postItem == null)
        {
            post = null;
        }
        else
        {
            initialContent = "\n\n[B:" + postItem.CreatedBy.Name + "] 写道：\n[BQ " + postItem.Content + " BQ]";
            post = postItem.Post;
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = true;
        trPreview.Visible = true;
        divPreview.InnerHtml = WikiParser.Parse(txtContent.Text);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int postID = (int)ViewState["postID"];
        using (MooDB db = new MooDB())
        {
            Post post = (from p in db.Posts
                         where p.ID == postID
                         select p).Single<Post>();

            if (post.Lock || post.Problem != null && post.Problem.LockPost)
            {
                if (!Permission.Check("post.locked.reply", false)) return;
            }
            else
            {
                if (!Permission.Check("post.reply", false)) return;
            }

            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
            db.PostItems.AddObject(new PostItem()
            {
                Post = post,
                Content = txtContent.Text,
                CreatedBy = currentUser
            });
            db.SaveChanges();
        }

        PageUtil.Redirect("操作成功", "~/Post/?id=" + postID);
    }
}