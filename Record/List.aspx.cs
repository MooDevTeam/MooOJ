using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Timers;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Record_List : System.Web.UI.Page
{
    protected int? problemID;
    protected int? userID;
    protected int? contestID;
    static volatile bool allowRejudge = true;
    static readonly System.Timers.Timer rejudgeTimer = new System.Timers.Timer(double.Parse(Resources.Moo.Record_RejudgeInterval));

    static Record_List()
    {
        rejudgeTimer.Elapsed += (sender, e) =>
        {
            allowRejudge = true;
        };
        rejudgeTimer.AutoReset = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("record.list", true)) return;

        if (!Page.IsPostBack)
        {
            problemID = Request["problemID"] == null ? null : (int?)int.Parse(Request["problemID"]);
            userID = Request["userID"] == null ? null : (int?)int.Parse(Request["userID"]);
            contestID = Request["contestID"] == null ? null : (int?)int.Parse(Request["contestID"]);
            ViewState["problemID"] = problemID;
            ViewState["userID"] = userID;
            ViewState["contestID"] = contestID;

            using (MooDB db = new MooDB())
            {
                grid.DataSource = GetDataToBind(db);
                Page.DataBind();
            }
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        Response.Redirect("~/Record/List.aspx?"
            + (txtProblemID.Text.Length > 0 ? "problemID=" + txtProblemID.Text : "")
            + (txtUserID.Text.Length > 0 ? "&userID=" + txtUserID.Text : "")
            + (txtContestID.Text.Length > 0 ? "&contestID=" + txtContestID.Text : ""));
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            RecoverViewState();
            grid.PageIndex = e.NewPageIndex;
            grid.DataSource = GetDataToBind(db);
            grid.DataBind();
        }
    }

    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Permission.Check("record.delete", false)) return;

        int recordID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            Record record = (from r in db.Records
                             where r.ID == recordID
                             select r).Single<Record>();
            if (record.JudgeInfo != null)
            {
                db.JudgeInfos.DeleteObject(record.JudgeInfo);
                db.SaveChanges();
            }
            db.Records.DeleteObject(record);
            db.SaveChanges();
        }

        grid.Rows[e.RowIndex].Visible = false;
    }

    void RecoverViewState()
    {
        problemID = (int?)ViewState["problemID"];
        userID = (int?)ViewState["userID"];
        contestID = (int?)ViewState["contestID"];
    }

    IQueryable<Record> GetDataToBind(MooDB db)
    {
        IQueryable<Record> query = db.Records;

        if (contestID != null)
        {
            query = from r in query
                    let contest = (from c in db.Contests
                                   where c.ID == contestID
                                   select c).FirstOrDefault<Contest>()
                    where r.Problem.Contest.Contains(contest)
                          && contest.User.Contains(r.User)
                          && r.CreateTime > contest.StartTime && r.CreateTime < contest.EndTime
                    select r;
        }

        if (problemID != null)
        {
            query = from r in query
                    where r.Problem.ID == problemID
                    select r;
        }

        if (userID != null)
        {
            query = from r in query
                    where r.User.ID == userID
                    select r;
        }

        return query.OrderByDescending<Record, int>(r => r.ID);
    }

    protected void btnRejudge_Click(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            Permission.Check("i'm super man.", false);
            return;
        }
        if (!Permission.Check("record.judgeinfo.delete", false, false))
        {
            if (!allowRejudge)
            {
                PageUtil.Redirect("请喝杯茶", "~/Record/List.aspx?" + Request.QueryString);
                return;
            }

            allowRejudge = false;
            rejudgeTimer.Start();
        }

        LinkButton theButton = (LinkButton)sender;
        GridViewRow row = (GridViewRow)theButton.Parent.Parent;
        int recordID = (int)grid.DataKeys[row.RowIndex][0];
        using (MooDB db = new MooDB())
        {
            Record record = (from r in db.Records
                             where r.ID == recordID
                             select r).Single<Record>();
            JudgeInfo info = record.JudgeInfo;
            if (info == null)
            {
                PageUtil.Redirect("已经提交重测，请耐心等候。", "~/Record/List.aspx?" + Request.QueryString);
                return;
            }

            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
            //Send A Mail
            db.Mails.AddObject(new Mail()
            {
                Title = "您提交的记录 #" + record.ID + " 被我提请重新测评",
                From = currentUser,
                To = record.User,
                Content = "您为 [url:" + record.Problem.Name + "|../Problem/?id=" + record.Problem.ID + "] 提交的记录已被我提请重新测评，请[url:点击这里|../Record/?id=" + record.ID + "]了解最新情况。\n\n"
                        + "*注意*：原始测评结果为*" + info.Score + "*分。详细测评信息为：\n"
                        + info.Info,
                IsRead = false
            });

            record.JudgeInfo = null;
            db.JudgeInfos.DeleteObject(info);
            db.SaveChanges();
        }

        PageUtil.Redirect("操作成功", "~/Record/List.aspx?" + Request.QueryString);
    }
}