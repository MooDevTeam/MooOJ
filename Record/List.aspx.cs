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
    protected string userName;
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
                if (userID != null)
                {
                    userName = (from u in db.Users
                                where u.ID == userID
                                select u).Single<User>().Name;
                }
                grid.DataSource = GetDataToBind(db);
                Page.DataBind();
            }
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        if (txtUserName.Text != "")
        {
            using (MooDB db = new MooDB())
            {
                userID = (from u in db.Users
                          where u.Name == txtUserName.Text
                          select u).Single<User>().ID;
            }
        }

        Response.Redirect("~/Record/List.aspx?"
            + (txtProblemID.Text.Length > 0 ? "problemID=" + txtProblemID.Text : "")
            + (userID != null ? "&userID=" + userID : "")
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
                DeleteJudgeInfoAndRefresh(db, record, record.JudgeInfo);
                db.JudgeInfos.DeleteObject(record.JudgeInfo);
                db.SaveChanges();
            }
            DeleteRecordAndRefresh(db, record);
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
        if (!Permission.Check("record.judgeinfo.delete.limited", false)) return;

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
            //Refresh Score
            DeleteJudgeInfoAndRefresh(db, record, info);

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

    void DeleteJudgeInfoAndRefresh(MooDB db, Record record, JudgeInfo info)
    {
        if (info.Score >= 0)
        {
            record.User.Score -= info.Score;
            record.Problem.ScoreSum -= info.Score;
            var hisRecords = from r in db.Records
                             where r.ID != record.ID
                                   && r.User.ID == record.User.ID && r.Problem.ID == record.Problem.ID
                                   && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                             select r;
            int hisMaxScore = hisRecords.Any() ? hisRecords.Max(r => r.JudgeInfo.Score) : 0;
            record.User.Score += hisMaxScore;
            record.Problem.ScoreSum += hisMaxScore;

            if (record.Problem.MaximumScore == info.Score)
            {
                var problemRecords = from r in db.Records
                                     where r.ID != record.ID
                                           && r.Problem.ID == record.Problem.ID
                                           && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                     select r;
                if (problemRecords.Any())
                {
                    record.Problem.MaximumScore = problemRecords.Max(r => r.JudgeInfo.Score);
                }
                else
                {
                    record.Problem.MaximumScore = null;
                }
            }
        }
    }

    void DeleteRecordAndRefresh(MooDB db, Record record)
    {
        record.Problem.SubmissionCount--;
        var hisRecords = from r in db.Records
                         where r.ID != record.ID
                               && r.User.ID == record.User.ID && r.Problem.ID == record.Problem.ID
                               && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                         select r;
        if (!hisRecords.Any())
        {
            record.Problem.SubmissionUser--;
        }
    }
    protected void validateUserName_ServerValidate(object source, ServerValidateEventArgs args)
    {
        using (MooDB db = new MooDB())
        {
            args.IsValid = (from u in db.Users
                            where u.Name == txtUserName.Text
                            select u).Any();
        }
    }
}