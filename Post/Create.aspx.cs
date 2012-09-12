using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Text;
using Moo.Utility;
public partial class Post_Create : System.Web.UI.Page
{
    protected bool canCreate;

    protected Problem problem;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["problemID"] != null)
                {
                    int problemID = int.Parse(Request["problemID"]);
                    problem = (from p in db.Problems
                               where p.ID == problemID
                               select p).SingleOrDefault<Problem>();
                }

                if (problem != null)
                {
                    ViewState["problemID"] = problem.ID;
                    canCreate = problem.LockPost ? Permission.Check("post.locked.create", false, false) : Permission.Check("post.create", false, false);
                }
                else
                {
                    canCreate = Permission.Check("post.create", false, false);
                }

                Page.DataBind();
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int postID;
        int? problemID = (int?)ViewState["problemID"];
        using (MooDB db = new MooDB())
        {
            problem = problemID == null ? null : (from p in db.Problems
                                                  where p.ID == problemID
                                                  select p).Single<Problem>();
            if (problem != null)
            {
                if (problem.LockPost)
                {
                    if (!Permission.Check("post.locked.create", false)) return;
                }
                else
                {
                    if (!Permission.Check("post.create", false)) return;
                }
            }
            else
            {
                if (!Permission.Check("post.create", false)) return;
            }

            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);

            Post post = new Post()
            {
                Name = txtName.Text,
                Problem = problem,
                Lock=false
            };
            db.Posts.AddObject(post);
            db.SaveChanges();

            PostItem item = new PostItem()
            {
                Post = post,
                Content = WikiParser.DoAt(db,txtContent.Text,post,currentUser,true),
                CreatedBy = currentUser
            };

            db.PostItems.AddObject(item);
            db.SaveChanges();

            postID = post.ID;

            Logger.Info(db, "创建帖子#" + postID);
        }
        PageUtil.Redirect("创建成功", "~/Post/?id=" + postID);
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        btnSubmit.Enabled = true;
        trPreview.Visible = true;
        using (MooDB db = new MooDB())
        {
            divPreview.InnerHtml = WikiParser.Parse(WikiParser.DoAt(db,txtContent.Text,null,null,false));
        }
    }
}