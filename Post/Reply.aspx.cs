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
            initialContent = "\n\n@" + postItem.CreatedBy.Name + " 写道：\n:{\n" + postItem.Content + "\n:}";
            post = postItem.Post;
        }
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

            //Send Mails
            SortedSet<User> userBeAt;
            string parsed = WikiParser.ParseAt(db, txtContent.Text, out userBeAt);
            foreach (User user in userBeAt)
            {
                db.Mails.AddObject(new Mail()
                {
                    Title = "我@了您哦~",
                    Content = "我在帖子[url:" + post.Name + "|../Post/?id=" + post.ID + "]中*@*了您哦~快去看看！\r\n\r\n*原文如下*：\r\n" + parsed,
                    From = currentUser,
                    To = user,
                    IsRead = false
                });
            }

            PostItem postItem = new PostItem()
            {
                Post = post,
                Content = txtContent.Text,
                CreatedBy = currentUser
            };
            db.PostItems.AddObject(postItem);
            db.SaveChanges();

            Logger.Info(db, string.Format("创建帖子#{0}的新项#{1}", post.ID, postItem.ID));
        }

        PageUtil.Redirect("回复成功", "~/Post/?id=" + postID);
    }
}