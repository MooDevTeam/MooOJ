using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class Contest_Result : System.Web.UI.Page
{
    protected double averageScore;

    protected Contest contest;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("contest.result.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    int id = int.Parse(Request["id"]);
                    contest = (from c in db.Contests
                               where c.ID == id
                               select c).SingleOrDefault<Contest>();
                }

                if (contest == null)
                {
                    PageUtil.Redirect("找不到相关内容", "~/");
                    return;
                }

                averageScore = (from u in contest.User
                                let maxScores = from p in contest.Problem
                                                let records = from r in u.Record
                                                              where r.CreateTime > contest.StartTime && r.CreateTime < contest.EndTime
                                                                    && r.Problem.ID == p.ID
                                                                    && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                                              select r.JudgeInfo.Score
                                                let maxScore = records.DefaultIfEmpty(0).Max(s => s)
                                                select maxScore
                                let score = maxScores.Sum(maxScore => maxScore)
                                select score).DefaultIfEmpty(0).Average(s => (double)s);

                AddColumns();
                BindGridView();

                ViewState["contestID"] = contest.ID;
                Page.DataBind();
            }
        }
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            int contestID = (int)ViewState["contestID"];
            contest = (from c in db.Contests
                       where c.ID == contestID
                       select c).Single<Contest>();
            grid.PageIndex = e.NewPageIndex;
            BindGridView();
            grid.DataBind();
        }
    }

    void AddColumns()
    {
        int counter = 0;
        foreach (Problem problem in contest.Problem)
        {
            grid.Columns.Add(new TemplateField()
            {
                HeaderText = HttpUtility.HtmlEncode(problem.Name),
                ItemTemplate = new ScoreFieldTemplate()
                {
                    FieldID = counter++,
                    ProblemID = problem.ID,
                    ContestID = contest.ID
                }
            });
        }
    }

    void BindGridView()
    {
        grid.DataSource = (from u in contest.User
                           let maxScores = from p in contest.Problem
                                           let records = from r in u.Record
                                                         where r.CreateTime > contest.StartTime && r.CreateTime < contest.EndTime
                                                               && r.Problem.ID == p.ID
                                                               && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                                         select r.JudgeInfo.Score
                                           let maxScore = records.DefaultIfEmpty(0).Max(s => s)
                                           select maxScore
                           let score = maxScores.Sum(maxScore => maxScore)
                           let Scores = maxScores.ToArray<int>()
                           orderby score descending
                           select new
                           {
                               ID = u.ID,
                               Name = u.Name,
                               Score = score,
                               Scores = Scores
                           }).ToList();
    }

    class ScoreFieldTemplate : ITemplate
    {
        public int FieldID { get; set; }
        public int ProblemID { get; set; }
        public int ContestID { get; set; }

        public void InstantiateIn(Control container)
        {
            HyperLink link = new HyperLink();
            link.DataBinding +=
                (sender, e) =>
                {
                    HyperLink theLink = (HyperLink)sender;
                    GridViewRow row = (GridViewRow)theLink.NamingContainer;
                    int[] scores = (int[])DataBinder.Eval(row.DataItem, "Scores");
                    int userID = (int)DataBinder.Eval(row.DataItem, "ID");

                    theLink.Text = scores[FieldID].ToString();
                    theLink.NavigateUrl = "~/Record/List.aspx?contestID=" + ContestID + "&problemID=" + ProblemID + "&userID=" + userID;
                };
            container.Controls.Add(link);
        }
    }

}