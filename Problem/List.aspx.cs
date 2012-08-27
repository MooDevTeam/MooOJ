using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Moo.DB;
using Moo.Authorization;
public partial class Problem_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("problem.list", true)) return;
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["name"] != null)
                {
                    ViewState["name"] = Request["name"];
                }

                grid.DataSource = GetDataToBind(db);
                Page.DataBind();
            }
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (!Permission.Check("problem.delete", false)) e.Cancel = true;
        int problemID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            db.Problems.DeleteObject((from p in db.Problems
                                      where p.ID == problemID
                                      select p).Single<Problem>());
            db.SaveChanges();
        }

        grid.Rows[e.RowIndex].Visible = false;
    }

    IQueryable GetDataToBind(MooDB db)
    {
        int? currentUserID = User.Identity.IsAuthenticated ? (int?)((SiteUser)User.Identity).ID : null;
        string name = (string)ViewState["name"];

        IQueryable<Problem> query = db.Problems;
        if (name != null)
        {
            query = from p in query
                    where p.Name.Contains(name)
                    select p;
        }

        return from p in query
               let records = from r in db.Records
                             where r.User.ID == currentUserID && r.Problem.ID == p.ID
                                   && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                             select r
               let score = records.Any() ? (int?)records.Max(r => r.JudgeInfo.Score) : null
               let averageScore = p.SubmissionUser > 0 ? p.ScoreSum / (double?)p.SubmissionUser : null
               select new
               {
                   ID = p.ID,
                   Name = p.Name,
                   Type = p.Type,
                   Score = score,
                   SubmissionCount = p.SubmissionCount,
                   SubmissionUser = p.SubmissionUser,
                   AverageScore = averageScore,
                   MaximumScore = p.MaximumScore
               };
    }
    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            grid.PageIndex = e.NewPageIndex;
            grid.DataSource = GetDataToBind(db);
            grid.DataBind();
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        Response.Redirect("~/Problem/List.aspx?name=" + HttpUtility.UrlEncode(txtName.Text), true);
    }
}