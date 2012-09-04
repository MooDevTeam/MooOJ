<%@ WebHandler Language="C#" Class="API" %>

using System;
using System.Linq;
using System.Web;
using System.Xml;
using System.Text;
using System.Timers;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;

public class API : IHttpHandler
{
    static volatile bool allowAccess = true;
    static Timer accessTimer = new Timer(5 * 1000);

    static API()
    {
        accessTimer.AutoReset = false;
        accessTimer.Elapsed += (sender, e) =>
            {
                allowAccess = true;
            };
    }
    
    HttpResponse Response;
    HttpRequest Request;
    SiteRole role;
    User currentUser;
    MooDB db;

    public void ProcessRequest(HttpContext context)
    {
        if (!allowAccess)
        {
            return;
        }
        Response = context.Response;
        Request = context.Request;
        if (Request.HttpMethod != "POST")
        {
            Response.ContentType = "text/plain";
            Response.Write("Only POST is supported.");
            return;
        }
        string userName = Request.Form["userName"];
        string password = Request.Form["password"];
        password = Converter.ToSHA256Hash(password);

        using (db = new MooDB())
        {
            currentUser = (from u in db.Users
                           where u.Name == userName && u.Password == password
                           select u).SingleOrDefault<User>();
            if (currentUser == null)
            {
                allowAccess = false;
                accessTimer.Start();
                return;
            }
            role = SiteRoles.ByID[currentUser.Role.ID];
            string xmlString = Request.Form["xml"];
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            ProcessXML(xmlDocument);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    void ProcessXML(XmlDocument document)
    {
        XmlNode method = document.SelectSingleNode("/request/method");
        switch (method.InnerText)
        {
            case "problem.create":
                CreateProblem(document);
                break;
            case "testcase.create":
                CreateTranditionalTestCase(document);
                break;
        }
    }

    void CreateProblem(XmlDocument document)
    {
        if (!Permission.Check(role, "problem.create")) return;
        Problem problem = new Problem()
        {
            Name = document.SelectSingleNode("/request/name").InnerText,
            Type = document.SelectSingleNode("/request/type").InnerText,
            AllowTesting = true,
            Hidden = false,
            Lock = false,
            LockPost = false,
            LockRecord = false,
            LockSolution = false,
            LockTestCase = false,
            MaximumScore = null,
            ScoreSum = 0,
            SubmissionCount = 0,
            SubmissionUser = 0,
            TestCaseHidden = false
        };

        ProblemRevision revision = new ProblemRevision()
        {
            Content = document.SelectSingleNode("/request/content").InnerText,
            CreatedBy = currentUser,
            Reason = "创建题目",
            Problem = problem
        };

        SolutionRevision solution = new SolutionRevision()
        {
            Content = "暂无题解",
            CreatedBy = currentUser,
            Reason = "创建题解",
            Problem = problem
        };

        problem.LatestRevision = revision;
        problem.LatestSolution = solution;
        db.SaveChanges();

        Response.Write(problem.ID);
    }

    void CreateTranditionalTestCase(XmlDocument document)
    {
        int problemID = int.Parse(document.SelectSingleNode("/request/problemid").InnerText);

        Problem problem = (from p in db.Problems
                           where p.ID == problemID
                           select p).Single<Problem>();

        if (problem.LockTestCase)
        {
            if (!Permission.Check(role, "testcase.locked.create")) return;
        }
        else
        {
            if (!Permission.Check(role, "testcase.create")) return;
        }
        
        TestCase testCase = new TranditionalTestCase()
        {
            Answer =Convert.FromBase64String(document.SelectSingleNode("/request/answer").InnerText),
            CreatedBy = currentUser,
            Input = Convert.FromBase64String(document.SelectSingleNode("/request/input").InnerText),
            MemoryLimit = int.Parse(document.SelectSingleNode("/request/memorylimit").InnerText),
            TimeLimit = int.Parse(document.SelectSingleNode("/request/timelimit").InnerText),
            Problem = (from p in db.Problems
                       where p.ID == problemID
                       select p).Single<Problem>(),
            Score = int.Parse(document.SelectSingleNode("/request/score").InnerText)
        };
        db.TestCases.AddObject(testCase);
        db.SaveChanges();
        Response.Write(testCase.ID);
    }

}