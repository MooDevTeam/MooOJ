<%@ WebHandler Language="C#" Class="Recalc" %>

using System;
using System.Linq;
using System.Web;
using Moo.DB;

public class Recalc : IHttpHandler
{
    HttpResponse Response;
    public void ProcessRequest(HttpContext context)
    {
        Response = context.Response;
        context.Response.ContentType = "text/html";
        using (MooDB db = new MooDB())
        {
            RecalcProblems(db);
            RecalcUsers(db);

            Response.Write("Start Saving<br/>");
            Response.Flush();
            db.SaveChanges();
            Response.Write("Over!<br/>");
        }
    }

    public void RecalcProblems(MooDB db)
    {
        foreach (Problem p in db.Problems)
        {
            p.SubmissionCount = (from r in db.Records
                                 where r.Problem.ID == p.ID
                                 select r).Count();
            p.SubmissionUser = (from u in db.Users
                                let hisRecords = from r in db.Records
                                                 where r.Problem.ID == p.ID && r.User.ID == u.ID
                                                 select r
                                where hisRecords.Any()
                                select u).Count();
            var userScores = from u in db.Users
                             let scores = from r in db.Records
                                          where r.Problem.ID == p.ID && r.User.ID == u.ID
                                              && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                          select r.JudgeInfo.Score
                             where scores.Any()
                             select scores.Max();
            p.MaximumScore = userScores.Any() ? (int?)userScores.Max() : null;
            p.ScoreSum = userScores.DefaultIfEmpty().Sum();
            Response.Write("OK Problem " + p.ID + "<br/>");
            Response.Flush();
        }
    }

    public void RecalcUsers(MooDB db)
    {
        foreach (User u in db.Users)
        {
            var hisScores = from p in db.Problems
                            let scores = from r in db.Records
                                         where r.User.ID == u.ID && r.Problem.ID == p.ID
                                          && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                         select r.JudgeInfo.Score
                            where scores.Any()
                            select scores.Max();
            u.Score = hisScores.DefaultIfEmpty().Sum();
            Response.Write("OK User " + u.ID + "<br/>");
            Response.Flush();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}