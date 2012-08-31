using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moo.Utility;
using Moo.Authorization;
namespace Moo.DB
{
    public class MooTestData
    {
        public static void AddTestData()
        {
            using (MooDB db = new MooDB())
            {
                AddTestData(db);
            }
        }
        public static void AddTestData(MooDB db)
        {
            //Users
            User MrPhone = new User()
            {
                Name = "onetwogoo",
                Password = Converter.ToSHA256Hash("123456"),
                Role = SiteRoles.ByType[RoleType.Organizer].GetDBRole(db),
                BriefDescription = "我觉得我写这么多应该够两行了",
                Description = "我是--屌丝--我骄傲，我为国家省钞票!",
                ImageURL = "https://www.google.com.hk/intl/zh-CN_cn/images/logos/images_logo_lg.gif",
                Score = 256,
            };
            db.Users.AddObject(MrPhone);

            User ShaBi = new User()
            {
                Name = "ShaBi",
                Password = Converter.ToSHA256Hash("ShaBi"),
                Role = SiteRoles.ByType[RoleType.Worker].GetDBRole(db),
                BriefDescription = "我觉得我写这么多应该够两行了",
                Description = "Moo*真*他妈的好！",
                ImageURL = "http://www.baidu.com/img/baidu_sylogo1.gif",
                Score = 128,
            };
            db.Users.AddObject(ShaBi);

            User Baby = new User()
            {
                Name = "Baby",
                Password = Converter.ToSHA256Hash("Baby"),
                Role = SiteRoles.ByType[RoleType.NormalUser].GetDBRole(db),
                BriefDescription = "我啥都不懂",
                Description = "真不懂",
                ImageURL = "http://www.9thome.com/ggb/upload/f86432898c354fec8915b0c03e368232.jpg",
                Score = 1000,
            };
            db.Users.AddObject(Baby);

            db.Users.AddObject(new User()
            {
                Name = "BeiJu",
                Password = Converter.ToSHA256Hash("BeiJu"),
                Role = SiteRoles.ByType[RoleType.Malefactor].GetDBRole(db),
                BriefDescription = "冤枉啊!啥都没干就被封了！",
                Description = "太他妈冤枉了！",
                ImageURL = "http://www.idaocao.com/daocaoeditor/2009_image/201104/201141910409550.jpg",
                Score = 0,
            });

            //Homepage
            db.HomepageRevisions.AddObject(new HomepageRevision()
            {
                Title = "欢迎测试",
                Content = "测试用户：\n"
                + "onetwogoo/123456 组织者\n"
                + "ShaBi/ShaBi 工作者\n"
                + "Baby/Baby 普通用户\n"
                + "BeiJu/BeiJu 作恶者",
                Reason = "测试用原因",
                CreatedBy = ShaBi
            });

            //Problems
            Problem APlusB = new Problem()
            {
                Name = "A+B问题",
                Type = "Tranditional",
                Lock = false,
                Hidden = false,
                TestCaseHidden = false,
                AllowTesting = true,
                LockPost = false,
                LockRecord = false,
                LockSolution = false,
                LockTestCase = false,
                SubmissionCount = 10,
                ScoreSum = 100,
                SubmissionUser = 1,
                MaximumScore = 30
            };

            Problem CPlusD = new Problem()
            {
                Name = "C+D问题",
                Type = "Tranditional",
                Lock = true,
                Hidden = false,
                TestCaseHidden = false,
                LockPost = true,
                LockTestCase = true,
                LockSolution = true,
                LockRecord = true,
                AllowTesting = true,
                SubmissionCount = 20,
                ScoreSum = 5,
                SubmissionUser = 2,
                MaximumScore = 120
            };
            db.Problems.AddObject(CPlusD);

            Problem EPlusF = new Problem()
            {
                Name = "E+F问题",
                Type = "Tranditional",
                Lock = false,
                LockPost = false,
                LockRecord = false,
                LockSolution = false,
                LockTestCase = false,
                AllowTesting = true,
                Hidden = true,
                TestCaseHidden = true,
                SubmissionCount = 40,
                ScoreSum = 300,
                SubmissionUser = 4,
                MaximumScore = 110
            };
            db.Problems.AddObject(EPlusF);

            Problem Cat = new Problem()
            {
                Name = "Cat",
                Type = "SpecialJudged",
                Lock = false,
                LockPost = false,
                LockRecord = false,
                LockSolution = false,
                LockTestCase = false,
                AllowTesting = true,
                Hidden = false,
                TestCaseHidden = false
            };
            db.Problems.AddObject(Cat);

            Problem EasyAPlusB = new Problem()
            {
                Name = "Easy A+B",
                Type = "Interactive",
                Lock = false,
                LockPost = false,
                LockRecord = false,
                LockSolution = false,
                LockTestCase = false,
                AllowTesting = true,
                Hidden = false,
                TestCaseHidden = false
            };
            db.Problems.AddObject(EasyAPlusB);

            //File
            UploadedFile file = new UploadedFile()
            {
                Name = "SPJ for Cat",
                Path = "D:\\Cat.exe"
            };
            db.UploadedFiles.AddObject(file);

            //Test Cases
            db.TestCases.AddObject(new TranditionalTestCase()
            {
                Problem = CPlusD,
                Input = Encoding.ASCII.GetBytes("qwertyuioplkjhgfdsazxcvbnm"),
                Answer = Encoding.ASCII.GetBytes("mnbvcxzasdfghjklpoiuytrewq"),
                TimeLimit = 1000,
                MemoryLimit = 1024 * 1024 * 6,
                Score = 12
            });
            db.SaveChanges();

            db.TestCases.AddObject(new TranditionalTestCase()
            {
                Problem = APlusB,
                Input = Encoding.ASCII.GetBytes("1 2"),
                Answer = Encoding.ASCII.GetBytes("3"),
                TimeLimit = 1000,
                MemoryLimit = 60 * 1024 * 1024,
                Score = 50
            });

            db.TestCases.AddObject(new TranditionalTestCase()
            {
                Problem = APlusB,
                Input = Encoding.ASCII.GetBytes("100 345"),
                Answer = Encoding.ASCII.GetBytes("445"),
                TimeLimit = 1000,
                MemoryLimit = 60 * 1024 * 1024,
                Score = 50
            });

            db.TestCases.AddObject(new SpecialJudgedTestCase()
            {
                Problem = Cat,
                Input = Encoding.ASCII.GetBytes("1 2"),
                Answer = Encoding.ASCII.GetBytes("Miao~"),
                TimeLimit = 1000,
                MemoryLimit = 60 * 1024 * 1024,
                Score = 100,
                Judger = file
            });

            file = new UploadedFile()
            {
                Name = "Invoker for EasyA+B",
                Path = "D:\\EasyA+B.o",
            };

            db.TestCases.AddObject(new InteractiveTestCase()
            {
                Problem = EasyAPlusB,
                TestData = Encoding.ASCII.GetBytes("1123 3212"),
                TimeLimit = 1000,
                MemoryLimit = 60 * 1024 * 1024,
                Invoker = file
            });

            db.TestCases.AddObject(new InteractiveTestCase()
            {
                Problem = EasyAPlusB,
                TestData = Encoding.ASCII.GetBytes("1 3"),
                TimeLimit = 1000,
                MemoryLimit = 60 * 1024 * 1024,
                Invoker = file
            });

            //Problem Revision
            db.ProblemRevisions.AddObject(new ProblemRevision()
             {
                 Problem = APlusB,
                 Content = "输入A,B。输出A+B。啊！输错了！",
                 Reason = "蛋疼",
                 CreatedBy = ShaBi
             });

            db.ProblemRevisions.AddObject(new ProblemRevision()
            {
                Problem = APlusB,
                Content = "输入A,B。输出它们的和。",
                Reason = "蛋疼",
                CreatedBy = MrPhone
            });

            db.ProblemRevisions.AddObject(new ProblemRevision()
            {
                Problem = APlusB,
                Content = "输入俩蛋，输出它们的和。",
                Reason = "蛋疼",
                CreatedBy = ShaBi
            });

            APlusB.LatestRevision = new ProblemRevision()
            {
                Problem = APlusB,
                Content = "输入两个Int32，输出它们的和。",
                Reason = "蛋疼",
                CreatedBy = MrPhone
            };

            CPlusD.LatestRevision = new ProblemRevision()
            {
                Problem = CPlusD,
                Content = "输入C,D。*注意是Int64*输出它们的和。",
                Reason = "蛋疼",
                CreatedBy = ShaBi
            };

            EPlusF.LatestRevision = new ProblemRevision()
            {
                Problem = EPlusF,
                Content = "输入E,F。输出它们的和。",
                Reason = "蛋疼",
                CreatedBy = ShaBi
            };

            Cat.LatestRevision = new ProblemRevision()
            {
                Problem = Cat,
                Content = "模拟Cat",
                Reason = "擦！",
                CreatedBy = MrPhone
            };

            EasyAPlusB.LatestRevision = new ProblemRevision()
            {
                Problem = EasyAPlusB,
                Content = "仅需编写一个int APlusB(int,int);即可。",
                Reason = "This is Interactive",
                CreatedBy = ShaBi
            };

            //Solution
            db.SolutionRevisions.AddObject(new SolutionRevision()
            {
                Problem = APlusB,
                Content = "很简单。水题不解释。",
                Reason = "太水了",
                CreatedBy = ShaBi
            });

            APlusB.LatestSolution = new SolutionRevision()
            {
                Problem = APlusB,
                Content = "var a,b:*int64*; begin read(a,b); write(a+b); end.",
                Reason = "上代码",
                CreatedBy = MrPhone
            };

            CPlusD.LatestSolution = new SolutionRevision()
            {
                Problem = CPlusD,
                Content = "这个好像跟A+B一样吧",
                Reason = "神犇题解",
                CreatedBy = ShaBi
            };

            EPlusF.LatestSolution = new SolutionRevision()
            {
                Problem = EPlusF,
                Content = "太难了，不会啊",
                Reason = "垃圾题解",
                CreatedBy = ShaBi
            };

            Cat.LatestSolution = new SolutionRevision()
            {
                Problem = Cat,
                Content = "抄！",
                Reason = "",
                CreatedBy = ShaBi
            };

            EasyAPlusB.LatestSolution = new SolutionRevision()
            {
                Problem = EasyAPlusB,
                Content = "就……就A了。",
                Reason = "",
                CreatedBy = MrPhone
            };

            //Post
            Post post = new Post()
            {
                Name = "讨论一下出题人心理",
                Problem = APlusB,
                Lock = false,
                OnTop = true,
            };
            db.Posts.AddObject(post);

            db.PostItems.AddObject(new PostItem()
            {
                Post = post,
                Content = "出--这么水--的题，找死啊",
                CreatedBy = ShaBi,
            });
            db.SaveChanges();

            db.PostItems.AddObject(new PostItem()
            {
                Post = post,
                Content = "靠！作为出题人我压力很大啊!",
                CreatedBy = MrPhone
            });

            post = new Post()
            {
                Name = "认真研究一下此题变形",
                Problem = APlusB,
                Lock = false,
                OnTop = false
            };
            db.Posts.AddObject(post);

            db.PostItems.AddObject(new PostItem()
            {
                Post = post,
                Content = "A+B能有什么变形呢？",
                CreatedBy = MrPhone
            });
            db.SaveChanges();

            db.PostItems.AddObject(new PostItem()
            {
                Post = post,
                Content = "靠！没人回答，我寂寞了~",
                CreatedBy = MrPhone
            });

            post = new Post()
            {
                Name = "讨论一下Moo好不好",
                Lock = true,
                OnTop = false
            };
            db.Posts.AddObject(post);

            db.PostItems.AddObject(new PostItem()
            {
                Post = post,
                Content = "Moo很好啊。*注意此贴没有对应题目且被锁*",
                CreatedBy = MrPhone
            });

            //Mail
            db.Mails.AddObject(new Mail()
            {
                Title = "咱把Moo黑了如何？",
                Content = "嘿！onetwogoo!把Moo--黑--了吧！",
                IsRead = true,
                From = ShaBi,
                To = MrPhone
            });
            db.SaveChanges();

            db.Mails.AddObject(new Mail()
            {
                Title = "找死啊！",
                Content = "我会去黑自己网站吗？",
                IsRead = true,
                From = MrPhone,
                To = ShaBi
            });
            db.SaveChanges();

            db.Mails.AddObject(new Mail()
            {
                Title = "没准嘞！",
                Content = "敢说你没这想法",
                IsRead = true,
                From = ShaBi,
                To = MrPhone
            });
            db.SaveChanges();

            db.Mails.AddObject(new Mail()
            {
                Title = "傻逼",
                Content = "不解释！",
                IsRead = false,
                From = MrPhone,
                To = ShaBi
            });
            db.SaveChanges();

            db.Mails.AddObject(new Mail()
            {
                Title = "把我弄成组织者如何啊",
                Content = "???",
                IsRead = false,
                From = Baby,
                To = MrPhone
            });
            db.SaveChanges();

            db.Mails.AddObject(new Mail()
            {
                Title = "美死你",
                Content = "怎么可能",
                IsRead = true,
                From = MrPhone,
                To = Baby
            });
            db.SaveChanges();

            //Contest
            Contest contest = new Contest()
            {
                StartTime = DateTimeOffset.Now.AddMinutes(1),
                EndTime = DateTimeOffset.Now.AddMinutes(2),
                Status = "Before",
                Title = "Moo水题大赛",
                Description = "全是--水--题啊！",
                LockProblemOnStart = true,
                LockPostOnStart = true,
                LockSolutionOnStart = true,
                LockTestCaseOnStart = true,
                AllowTestingOnStart = false,
                HideTestCaseOnStart = true,
                LockRecordOnStart = false,
                HideProblemOnStart = false,
                AllowTestingOnEnd = true,
                LockPostOnEnd = false,
                LockProblemOnEnd = false,
                LockRecordOnEnd = false,
                LockSolutionOnEnd = false,
                LockTestCaseOnEnd = false,
                HideProblemOnEnd = false,
                HideTestCaseOnEnd = false,
            };
            //contest.Problem.Add(APlusB);
            contest.Problem.Add(CPlusD);
            contest.User.Add(ShaBi);

            //Record
            JudgeInfo info = new JudgeInfo()
            {
                Info = "Test",
                Score = 10
            };

            Record record = new Record()
            {
                Code = "",
                Language = "cxx",
                CreateTime = DateTimeOffset.Now.AddMinutes(1.5),
                User = ShaBi,
                Problem = APlusB,
                PublicCode = true
            };
            record.JudgeInfo = info;
            info.Record = record;
            db.Records.AddObject(record);

            info = new JudgeInfo()
            {
                Info = "test",
                Score = 30
            };

            record = new Record()
            {
                Code = "",
                Language = "c++",
                CreateTime = DateTimeOffset.Now.AddMinutes(1.5),
                User = ShaBi,
                Problem = APlusB,
                PublicCode = true
            };
            record.JudgeInfo = info;
            info.Record = record;
            db.Records.AddObject(record);

            info = new JudgeInfo()
            {
                Info = "test",
                Score = 60
            };

            record = new Record()
            {
                Code = "",
                Language = "c++",
                CreateTime = DateTimeOffset.Now.AddMinutes(1.5),
                User = ShaBi,
                Problem = CPlusD,
                PublicCode = true
            };
            record.JudgeInfo = info;
            info.Record = record;
            db.Records.AddObject(record);

            db.Records.AddObject(new Record()
            {
                Code = "int APlusB(int x,int y){return 4;}",
                CreateTime = DateTimeOffset.Now,
                Language = "c++",
                Problem = EasyAPlusB,
                User = MrPhone,
                PublicCode = true
            });

            db.SaveChanges();
        }
    }
}