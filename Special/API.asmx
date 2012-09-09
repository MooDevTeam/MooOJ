<%@ WebService Language="C#" Class="API" %>

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Security;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Moo.Utility;
using Moo.DB;
using Moo.Authorization;

[WebService(Namespace = "http://moo.imeng.de/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class API : System.Web.Services.WebService
{
    static object loginLock = new object();
    static ISet<int> trustedUsers = new HashSet<int>();
    static IDictionary<int, DateTimeOffset> lastAccess = new Dictionary<int, DateTimeOffset>();

    bool Authenticate(string sToken)
    {
        string[] splited = sToken.Split(',');
        int userID = int.Parse(splited[0]), token = int.Parse(splited[1]);
        if (!SiteUsers.ByID.ContainsKey(userID) || SiteUsers.ByID[userID].Token != token) return false;
        
        if (SiteUsers.ByID[userID].Role.Type != RoleType.Organizer && !trustedUsers.Contains(userID))
        {
            lock (SiteUsers.ByID[userID])
            {
                if (lastAccess.ContainsKey(userID))
                {
                    if (lastAccess[userID] > DateTimeOffset.Now.AddSeconds(-10))
                    {
                        Thread.Sleep(lastAccess[userID].AddSeconds(10) - DateTimeOffset.Now);
                    }
                }
                else
                {
                    lastAccess[userID] = DateTimeOffset.Now;
                }
                
                lastAccess[userID] = DateTimeOffset.Now;
            }
        }
        Thread.CurrentPrincipal = new CustomPrincipal() { Identity = SiteUsers.ByID[userID] };
        return true;
    }

    bool Authorize(string permission)
    {
        SiteUser siteUser = (SiteUser)((CustomPrincipal)Thread.CurrentPrincipal).Identity;
        return siteUser.Role.AllowedFunction.Contains(permission);
    }

    SiteUser CurrentUser
    {
        get
        {
            return (SiteUser)((CustomPrincipal)Thread.CurrentPrincipal).Identity;
        }
    }

    [WebMethod]
    public string Login(string userName, string password)
    {
        lock (loginLock)
        {
            password = Converter.ToSHA256Hash(password);
            using (MooDB db = new MooDB())
            {
                User user = (from u in db.Users
                             where u.Name == userName && u.Password == password
                             select u).SingleOrDefault<User>();
                if (user == null)
                {
                    Thread.Sleep(5000);
                    return null;
                }

                int token = Rand.RAND.Next();
                if (SiteUsers.ByID.ContainsKey(user.ID))
                {
                    SiteUsers.ByID[user.ID].Initialize(user);
                    SiteUsers.ByID[user.ID].Token = token;
                }
                else
                {
                    SiteUsers.ByID[user.ID] = new SiteUser(user) { Token = token };
                }
                return user.ID + "," + token;
            }
        }
    }

    [WebMethod]
    public void AddTrustedUser(string sToken, int userID)
    {
        if (!Authenticate(sToken)) throw new SecurityException();
        if (CurrentUser.Role.Type != RoleType.Organizer) throw new SecurityException();
        if (!trustedUsers.Contains(userID))
        {
            trustedUsers.Add(userID);
        }
    }

    [WebMethod]
    public void RemoveTrustedUser(string sToken, int userID)
    {
        if (!Authenticate(sToken)) throw new SecurityException();
        if (CurrentUser.Role.Type != RoleType.Organizer) throw new SecurityException();
        if (trustedUsers.Contains(userID))
        {
            trustedUsers.Remove(userID);
        }
    }

    [WebMethod]
    public int CreateProblem(string sToken, string name, string type, string content)
    {
        if (!Authenticate(sToken)) throw new SecurityException();
        if (!Authorize("problem.create")) throw new SecurityException();
        using (MooDB db = new MooDB())
        {
            User currentUser = CurrentUser.GetDBUser(db);

            Problem problem = new Problem()
            {
                Name = name,
                Type = type,
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
                Content = content,
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

            return problem.ID;
        }
    }

    [WebMethod]
    public int CreateTranditionalTestCase(string sToken, int problemID, byte[] input, byte[] answer, int timeLimit, int memoryLimit, int score)
    {
        if (!Authenticate(sToken)) throw new SecurityException();

        using (MooDB db = new MooDB())
        {
            Problem problem = (from p in db.Problems
                               where p.ID == problemID
                               select p).Single<Problem>();

            if (problem.LockTestCase)
            {
                if (!Authorize("testcase.locked.create")) throw new SecurityException();
            }
            else
            {
                if (!Authorize("testcase.create")) throw new SecurityException();
            }

            User currentUser = CurrentUser.GetDBUser(db);

            TestCase testCase = new TranditionalTestCase()
            {
                Answer = answer,
                CreatedBy = currentUser,
                Input = input,
                MemoryLimit = memoryLimit,
                TimeLimit = timeLimit,
                Problem = problem,
                Score = score
            };

            db.TestCases.AddObject(testCase);
            db.SaveChanges();

            return testCase.ID;
        }
    }
}