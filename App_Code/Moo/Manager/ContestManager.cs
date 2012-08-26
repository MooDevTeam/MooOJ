using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Moo.Utility;
using Moo.DB;
namespace Moo.Manager
{
    /// <summary>
    /// 比赛服务进程
    /// </summary>
    public static class ContestManager
    {
        static Thread daemonThread;

        static volatile bool shouldStop;

        public static void Start()
        {
            if (daemonThread != null)
            {
                Stop();
            }

            shouldStop = false;
            daemonThread = new Thread(new ThreadStart(ThreadMain));
            daemonThread.Start();
        }

        public static void Stop()
        {
            shouldStop = true;
            daemonThread.Interrupt();
            daemonThread.Join();
            daemonThread = null;
        }

        static void ThreadMain()
        {
            while (!shouldStop)
            {
                try
                {
                    Thread.Sleep(MainLoop());
                }
                catch (Exception e)
                {
                    if (!(e is ThreadInterruptedException))
                    {
                        Logger.Log(e);
                    }
                }
            }
        }

        static int MainLoop()
        {
            using (MooDB db = new MooDB())
            {
                Contest contest = (from c in db.Contests
                                   where c.Status == "Before" && c.StartTime <= DateTimeOffset.Now
                                   select c).FirstOrDefault<Contest>();
                if (contest != null)
                {
                    contest.Status = "During";
                    foreach (Problem problem in contest.Problem)
                    {
                        problem.AllowTesting=contest.AllowTestingOnStart;
                        problem.TestCaseHidden=contest.HideTestCaseOnStart;
                        problem.LockPost=contest.LockPostOnStart;
                        problem.LockTestCase = contest.LockTestCaseOnStart;
                        problem.LockSolution = contest.LockSolutionOnStart;
                        problem.Lock = contest.LockProblemOnStart;
                        problem.Hidden = contest.HideProblemOnStart;
                        problem.LockRecord = contest.LockRecordOnStart;
                    }
                    db.SaveChanges();
                    return 0;
                }

                contest = (from c in db.Contests
                           where c.Status == "During" && c.EndTime <= DateTimeOffset.Now
                           select c).FirstOrDefault<Contest>();
                if (contest != null)
                {
                    contest.Status = "After";
                    foreach (Problem problem in contest.Problem)
                    {
                        problem.AllowTesting = contest.AllowTestingOnEnd;
                        problem.TestCaseHidden = contest.HideTestCaseOnEnd;
                        problem.LockPost = contest.LockPostOnEnd;
                        problem.LockTestCase = contest.LockTestCaseOnEnd;
                        problem.LockSolution = contest.LockSolutionOnEnd;
                        problem.Lock = contest.LockProblemOnEnd;
                        problem.Hidden = contest.HideProblemOnEnd;
                        problem.LockRecord = contest.LockRecordOnEnd;
                    }
                    db.SaveChanges();
                    return 0;
                }

                return 60 * 1000;
            }
        }
    }
}