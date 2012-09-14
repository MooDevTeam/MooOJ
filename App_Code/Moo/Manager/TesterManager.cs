﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Text.RegularExpressions;
using Moo.DB;
using Moo.Utility;
using Moo.Tester;
namespace Moo.Manager
{
    /// <summary>
    ///Tester 后台进程
    /// </summary>
    public static class TesterManager
    {
        static List<ITester> testers = new List<ITester>();
        public static List<ITester> Testers
        {
            get
            {
                return testers;
            }
            set
            {
                testers = value;
            }
        }

        static Thread testThread;
        static volatile bool shouldStop;

        public static void Start()
        {
            if (testThread != null)
            {
                Stop();
            }

            shouldStop = false;
            testThread = new Thread(new ThreadStart(ThreadMain));
            testThread.Start();
            using (MooDB db = new MooDB())
            {
                Logger.Info(db, "评测进程启动");
            }
        }

        public static void Stop()
        {
            shouldStop = true;
            testThread.Interrupt();
            testThread.Join();
            testThread = null;

            using (MooDB db = new MooDB())
            {
                Logger.Info(db, "评测进程停止");
            }
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
                        using (MooDB db = new MooDB())
                        {
                            Logger.Fatal(db, e.ToString());
                        }
                    }
                }
            }
        }

        static int MainLoop()
        {
            using (MooDB db = new MooDB())
            {
                Record record = (from r in db.Records
                                 where r.JudgeInfo == null && r.Problem.AllowTesting
                                 select r).FirstOrDefault<Record>();
                var a = (from r in db.Records
                         where r.JudgeInfo == null
                         select r);
                if (record == null)
                {
                    return 5 * 1000;
                }
                else
                {
                    record.JudgeInfo = new JudgeInfo()
                    {
                        Record = record,
                        Score = -1,
                        Info = "<color:blue>*正在评测*</color>"
                    };
                    db.SaveChanges();

                    Logger.Info(db, "开始评测记录#" + record.ID);

                    Test(db, record);
                    db.SaveChanges();

                    Logger.Info(db, "记录#" + record.ID + "评测完成");

                    return 0;
                }
            }
        }

        static void Test(MooDB db, Record record)
        {
            TestResult result;
            switch (record.Problem.Type)
            {
                case "Tranditional":
                    result = TestTranditional(db, record);
                    break;
                case "SpecialJudged":
                    result = TestSpecialJudged(db, record);
                    break;
                case "Interactive":
                    result = TestInteractive(db, record);
                    break;
                case "AnswerOnly":
                    result = TestAnswerOnly(db, record);
                    break;
                default:
                    result = new TestResult()
                    {
                        Score = 0,
                        Info = "<color:red>*未知的题目类型*</color>"
                    };
                    break;
            }

            int oldScore = (from r in db.Records
                            where r.User.ID == record.User.ID && r.Problem.ID == record.Problem.ID
                                && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                            select r.JudgeInfo.Score).DefaultIfEmpty().Max();
            int currentScore = Math.Max(oldScore, result.Score);

            record.User.Score -= oldScore;
            record.Problem.ScoreSum -= oldScore;
            record.User.Score += currentScore;
            record.Problem.ScoreSum += currentScore;

            if (record.Problem.MaximumScore == null)
            {
                record.Problem.MaximumScore = result.Score;
            }
            else
            {
                record.Problem.MaximumScore = Math.Max(result.Score, (int)record.Problem.MaximumScore);
            }

            record.JudgeInfo.Score = result.Score;
            record.JudgeInfo.Info = result.Info;
        }

        static TestResult TestTranditional(MooDB db, Record record)
        {
            ITester tester = Testers[0];
            IEnumerable<TranditionalTestCase> cases = from t in db.TestCases.OfType<TranditionalTestCase>()
                                                      where t.Problem.ID == record.Problem.ID
                                                      select t;
            return tester.TestTranditional(record.Code, record.Language, cases);
        }

        static TestResult TestSpecialJudged(MooDB db, Record record)
        {
            ITester tester = Testers[0];
            IEnumerable<SpecialJudgedTestCase> cases = from t in db.TestCases.OfType<SpecialJudgedTestCase>()
                                                       where t.Problem.ID == record.Problem.ID
                                                       select t;
            return tester.TestSpecialJudged(record.Code, record.Language, cases);
        }

        static TestResult TestInteractive(MooDB db, Record record)
        {
            ITester tester = Testers[0];
            IEnumerable<InteractiveTestCase> cases = from t in db.TestCases.OfType<InteractiveTestCase>()
                                                     where t.Problem.ID == record.Problem.ID
                                                     select t;
            return tester.TestInteractive(record.Code, record.Language, cases);
        }

        static TestResult TestAnswerOnly(MooDB db, Record record)
        {
            ITester tester = Testers[0];
            IEnumerable<AnswerOnlyTestCase> cases = from t in db.TestCases.OfType<AnswerOnlyTestCase>()
                                                    where t.Problem.ID == record.Problem.ID
                                                    select t;
            Dictionary<int, string> answers = new Dictionary<int, string>();
            MatchCollection matches = Regex.Matches(record.Code, @"<Moo:Answer testCase='(\d+)'>(.*?)</Moo:Answer>", RegexOptions.Singleline);
            foreach (Match match in matches)
            {
                int testCaseID = int.Parse(match.Groups[1].Value);
                string answer = match.Groups[2].Value;
                answers.Add(testCaseID, answer);
            }
            return tester.TestAnswerOnly(answers, cases);
        }
    }
}