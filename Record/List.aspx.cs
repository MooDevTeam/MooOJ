using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Record_List : System.Web.UI.Page
{
    protected int? problemID;
    protected int? userID;
    protected int? contestID;

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
    
}