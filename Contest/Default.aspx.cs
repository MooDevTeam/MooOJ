using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Contest_Default : System.Web.UI.Page
{
    protected bool attended;

    protected Contest contest;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("contest.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    int contestID = int.Parse(Request["id"]);
                    contest = (from c in db.Contests
                               where c.ID == contestID
                               select c).SingleOrDefault<Contest>();
                }

                if (contest == null)
                {
                    PageUtil.Redirect("找不到相关内容", "~/");
                    return;
                }

                if (User.Identity.IsAuthenticated)
                {
                    attended = contest.User.Contains(((SiteUser)User.Identity).GetDBUser(db));
                }

                ViewState["contestID"] = contest.ID;
                grid.DataSource = contest.Problem;
                Page.DataBind();
            }
        }
    }
    protected void btnAttend_Click(object sender, EventArgs e)
    {
        if (!Permission.Check("contest.attend", false)) return;

        int contestID = (int)ViewState["contestID"];
        using (MooDB db = new MooDB())
        {
            contest = (from c in db.Contests
                       where c.ID == contestID
                       select c).Single<Contest>();

            if (DateTimeOffset.Now > contest.EndTime)
            {
                return;
            }

            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
            if (contest.User.Contains(currentUser))
            {
                return;
            }

            contest.User.Add(currentUser);
            db.SaveChanges();
        }

        Response.Redirect("~/Contest/?id=" + contestID, true);
    }
    protected void validateAdd_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (MooDB db = new MooDB())
        {
            int problemID = int.Parse(txtProblemID.Text);
            Problem problem = (from p in db.Problems
                               where p.ID == problemID
                               select p).SingleOrDefault<Problem>();
            if (problem == null)
            {
                args.IsValid = false;
                validateAdd.Text = "无此题目";
                return;
            }

            int contestID = (int)ViewState["contestID"];
            contest = (from c in db.Contests
                       where c.ID == contestID
                       select c).Single<Contest>();
            if (contest.Problem.Contains(problem))
            {
                args.IsValid = false;
                validateAdd.Text = "不能重复添加题目";
                return;
            }
        }
        args.IsValid = true;
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        if (!Permission.Check("contest.modify", false)) return;

        int contestID = (int)ViewState["contestID"];
        int problemID = int.Parse(txtProblemID.Text);
        using (MooDB db = new MooDB())
        {
            Problem problem = (from p in db.Problems
                               where p.ID == problemID
                               select p).Single<Problem>();
            contest = (from c in db.Contests
                       where c.ID == contestID
                       select c).Single<Contest>();

            contest.Problem.Add(problem);
            db.SaveChanges();
        }
        Response.Redirect("~/Contest/?id=" + contestID);
    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Permission.Check("contest.modify", false)) return;

        int problemID = (int)e.Keys[0];
        int contestID = (int)ViewState["contestID"];
        using (MooDB db = new MooDB())
        {
            (from c in db.Contests
             where c.ID == contestID
             select c).Single<Contest>().Problem.Remove((from p in db.Problems
                                                         where p.ID == problemID
                                                         select p).Single<Problem>());
            db.SaveChanges();
        }
        Response.Redirect("~/Contest/?id=" + contestID);
    }
}