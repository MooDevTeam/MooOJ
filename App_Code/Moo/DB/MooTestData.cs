using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moo.Utility;
namespace Moo.DB
{
    public class MooTestData
    {
        static Role Organizer = new Role() { Name = "Organizer", DisplayName = "组织者" };
        static Role Worker = new Role() { Name = "Worker", DisplayName = "工作者" };
        static Role NormalUser = new Role() { Name = "NormalUser", DisplayName = "普通用户" };
        static Role Malefactor = new Role() { Name = "Malefactor", DisplayName = "作恶者" };

        static Function ReadHomepage = new Function() { Name = "homepage.read" };
        static Function UpdateHomepage = new Function() { Name = "homepage.update" };
        static Function ReadHomepageHistory = new Function() { Name = "homepage.history.read" };
        static Function DeleteHomepageHistory = new Function() { Name = "homepage.history.delete" };

        static Function ReadProblem = new Function() { Name = "problem.read" };
        static Function ReadHiddenProblem = new Function() { Name = "problem.hidden.read" };
        static Function ModifyProblem = new Function() { Name = "problem.modify" };
        static Function UpdateProblem = new Function() { Name = "problem.update" };
        static Function UpdateLockedProblem = new Function() { Name = "problem.locked.update" };
        static Function ListProblem = new Function() { Name = "problem.list" };
        static Function ReadProblemHistory = new Function() { Name = "problem.history.read" };
        static Function CreateProblem = new Function() { Name = "problem.create" };
        static Function DeleteProblem = new Function() { Name = "problem.delete" };
        static Function DeleteProblemHistory = new Function() { Name = "problem.history.delete" };

        static Function ReadTestCase = new Function() { Name = "testcase.read" };
        static Function ListTestCase = new Function() { Name = "testcase.list" };
        static Function CreateTestCase = new Function() { Name = "testcase.create" };
        static Function CreateLockedTestCase = new Function() { Name = "testcase.locked.create" };
        static Function DeleteTestCase = new Function() { Name = "testcase.delete" };

        static Function ReadSolution = new Function() { Name = "solution.read" };
        static Function UpdateSolution = new Function() { Name = "solution.update" };
        static Function UpdateLockedSolution = new Function() { Name = "solution.locked.update" };
        static Function ReadSolutionHistory = new Function() { Name = "solution.history.read" };
        static Function DeleteSolutionHistory = new Function() { Name = "solution.history.delete" };

        static Function ReadPost = new Function() { Name = "post.read" };
        static Function ListPost = new Function() { Name = "post.list" };
        static Function ModifyPost = new Function() { Name = "post.modify" };
        static Function ReplyPost = new Function() { Name = "post.reply" };
        static Function ReplyLockedPost = new Function() { Name = "post.locked.reply" };
        static Function CreatePost = new Function() { Name = "post.create" };
        static Function CreateLockedPost = new Function() { Name = "post.locked.create" };
        static Function DeletePost = new Function() { Name = "post.delete" };
        static Function DeletePostItem = new Function() { Name = "post.item.delete" };

        static Function ReadRecord = new Function() { Name = "record.read" };
        static Function ReadRecordCode = new Function() { Name = "record.code.read" };
        static Function ListRecord = new Function() { Name = "record.list" };
        static Function CreateRecord = new Function() { Name = "record.create" };
        static Function CreateLockedRecord = new Function() { Name = "record.locked.create" };
        static Function DeleteRecord = new Function() { Name = "record.delete" };

        static Function CreateUser = new Function() { Name = "user.create" };
        static Function ListUser = new Function() { Name = "user.list" };
        static Function ReadUser = new Function() { Name = "user.read" };
        static Function ModifyUser = new Function() { Name = "user.modify" };
        static Function ForceUserLogout = new Function() { Name = "user.forcelogout" };
        static Function ModifyUserRole = new Function() { Name = "user.role.modify" };

        static Function ReadMail = new Function() { Name = "mail.read" };
        static Function ListMail = new Function() { Name = "mail.list" };
        static Function CreateMail = new Function() { Name = "mail.create" };

        static Function ReadContest = new Function() { Name = "contest.read" };
        static Function ReadContestResult = new Function() { Name = "contest.result.read" };
        static Function CreateContest = new Function() { Name = "contest.create" };
        static Function ModifyContest = new Function() { Name = "contest.modify" };
        static Function ListContest = new Function() { Name = "contest.list" };
        static Function AttendContest = new Function() { Name = "contest.attend" };
        static Function DeleteContest = new Function() { Name = "contest.delete" };

        static Function CreateFile = new Function() { Name = "file.create" };
        static Function ListFile = new Function() { Name = "file.list" };
        static Function ReadFile = new Function() { Name = "file.read" };
        static Function DeleteFile = new Function() { Name = "file.delete" };

        static Function SkipCaptcha = new Function { Name = "captcha.skip" };
        static Function ReadHelp = new Function() { Name = "help.read" };

        public static void InitDatabase()
        {
            using (MooDB db = new MooDB())
            {
                ReGenerateDatabase(db);
                AddRequiredData(db);
                AddTestData(db);
            }
        }
        static void ReGenerateDatabase(MooDB db)
        {
            try
            {
                db.DeleteDatabase();
            }
            catch
            {

            }
            db.CreateDatabase();
            FixDatabase(db);
        }
        static void FixDatabase(MooDB db)
        {
            ///FIXME !!!!!
            //db.ExecuteStoreCommand("ALTER TABLE [dbo].[Users] ADD UNIQUE ([Name])");

            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_TranditionalTestCase] DROP CONSTRAINT [FK_TranditionalTestCase_inherits_TestCase];");
            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_TranditionalTestCase] ADD CONSTRAINT [FK_TranditionalTestCase_inherits_TestCase] FOREIGN KEY ([ID]) REFERENCES [dbo].[TestCases]([ID]) ON DELETE CASCADE;");

            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase] DROP CONSTRAINT [FK_SpecialJudgedTestCase_inherits_TestCase];");
            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase] ADD CONSTRAINT [FK_SpecialJudgedTestCase_inherits_TestCase] FOREIGN KEY ([ID]) REFERENCES [dbo].[TestCases]([ID]) ON DELETE CASCADE;");
        }
        static void AddRequiredData(MooDB db)
        {
            //Homepage
            Organizer.AllowedFunction.Add(ReadHomepage);
            Worker.AllowedFunction.Add(ReadHomepage);
            NormalUser.AllowedFunction.Add(ReadHomepage);
            Malefactor.AllowedFunction.Add(ReadHomepage);

            Organizer.AllowedFunction.Add(UpdateHomepage);
            Worker.AllowedFunction.Add(UpdateHomepage);

            Organizer.AllowedFunction.Add(ReadHomepageHistory);
            Worker.AllowedFunction.Add(ReadHomepageHistory);

            Organizer.AllowedFunction.Add(DeleteHomepageHistory);
            Worker.AllowedFunction.Add(DeleteHomepageHistory);

            //Problem
            Organizer.AllowedFunction.Add(ReadProblem);
            Worker.AllowedFunction.Add(ReadProblem);
            NormalUser.AllowedFunction.Add(ReadProblem);

            Organizer.AllowedFunction.Add(ReadHiddenProblem);
            Worker.AllowedFunction.Add(ReadHiddenProblem);

            Organizer.AllowedFunction.Add(ModifyProblem);
            Worker.AllowedFunction.Add(ModifyProblem);

            Organizer.AllowedFunction.Add(UpdateProblem);
            Worker.AllowedFunction.Add(UpdateProblem);
            NormalUser.AllowedFunction.Add(UpdateProblem);

            Organizer.AllowedFunction.Add(UpdateLockedProblem);
            Worker.AllowedFunction.Add(UpdateLockedProblem);

            Organizer.AllowedFunction.Add(ListProblem);
            Worker.AllowedFunction.Add(ListProblem);
            NormalUser.AllowedFunction.Add(ListProblem);

            Organizer.AllowedFunction.Add(ReadProblemHistory);
            Worker.AllowedFunction.Add(ReadProblemHistory);
            NormalUser.AllowedFunction.Add(ReadProblemHistory);

            Organizer.AllowedFunction.Add(CreateProblem);
            Worker.AllowedFunction.Add(CreateProblem);
            NormalUser.AllowedFunction.Add(CreateProblem);

            Organizer.AllowedFunction.Add(DeleteProblem);
            Worker.AllowedFunction.Add(DeleteProblem);

            Organizer.AllowedFunction.Add(DeleteProblemHistory);
            Worker.AllowedFunction.Add(DeleteProblemHistory);

            //TestCase
            Organizer.AllowedFunction.Add(ReadTestCase);
            Worker.AllowedFunction.Add(ReadTestCase);
            NormalUser.AllowedFunction.Add(ReadTestCase);

            Organizer.AllowedFunction.Add(ListTestCase);
            Worker.AllowedFunction.Add(ListTestCase);
            NormalUser.AllowedFunction.Add(ListTestCase);

            Organizer.AllowedFunction.Add(CreateTestCase);
            Worker.AllowedFunction.Add(CreateTestCase);
            NormalUser.AllowedFunction.Add(CreateTestCase);

            Organizer.AllowedFunction.Add(CreateLockedTestCase);
            Worker.AllowedFunction.Add(CreateLockedTestCase);

            Organizer.AllowedFunction.Add(DeleteTestCase);
            Worker.AllowedFunction.Add(DeleteTestCase);

            //Solution
            Organizer.AllowedFunction.Add(ReadSolution);
            Worker.AllowedFunction.Add(ReadSolution);
            NormalUser.AllowedFunction.Add(ReadSolution);

            Organizer.AllowedFunction.Add(UpdateSolution);
            Worker.AllowedFunction.Add(UpdateSolution);
            NormalUser.AllowedFunction.Add(UpdateSolution);

            Organizer.AllowedFunction.Add(UpdateLockedSolution);
            Worker.AllowedFunction.Add(UpdateLockedSolution);

            Organizer.AllowedFunction.Add(ReadSolutionHistory);
            Worker.AllowedFunction.Add(ReadSolutionHistory);
            NormalUser.AllowedFunction.Add(ReadSolutionHistory);

            Organizer.AllowedFunction.Add(DeleteSolutionHistory);
            Worker.AllowedFunction.Add(DeleteSolutionHistory);

            //Post
            Organizer.AllowedFunction.Add(ReadPost);
            Worker.AllowedFunction.Add(ReadPost);
            NormalUser.AllowedFunction.Add(ReadPost);

            Organizer.AllowedFunction.Add(ListPost);
            Worker.AllowedFunction.Add(ListPost);
            NormalUser.AllowedFunction.Add(ListPost);

            Organizer.AllowedFunction.Add(ModifyPost);
            Worker.AllowedFunction.Add(ModifyPost);

            Organizer.AllowedFunction.Add(ReplyPost);
            Worker.AllowedFunction.Add(ReplyPost);
            NormalUser.AllowedFunction.Add(ReplyPost);

            Organizer.AllowedFunction.Add(ReplyLockedPost);
            Worker.AllowedFunction.Add(ReplyLockedPost);

            Organizer.AllowedFunction.Add(CreatePost);
            Worker.AllowedFunction.Add(CreatePost);
            NormalUser.AllowedFunction.Add(CreatePost);

            Organizer.AllowedFunction.Add(CreateLockedPost);
            Worker.AllowedFunction.Add(CreateLockedPost);

            Organizer.AllowedFunction.Add(DeletePost);
            Worker.AllowedFunction.Add(DeletePost);

            Organizer.AllowedFunction.Add(DeletePostItem);
            Worker.AllowedFunction.Add(DeletePostItem);

            //Record
            Organizer.AllowedFunction.Add(ReadRecord);
            Worker.AllowedFunction.Add(ReadRecord);
            NormalUser.AllowedFunction.Add(ReadRecord);

            Organizer.AllowedFunction.Add(ReadRecordCode);
            Worker.AllowedFunction.Add(ReadRecordCode);

            Organizer.AllowedFunction.Add(ListRecord);
            Worker.AllowedFunction.Add(ListRecord);
            NormalUser.AllowedFunction.Add(ListRecord);

            Organizer.AllowedFunction.Add(CreateRecord);
            Worker.AllowedFunction.Add(CreateRecord);
            NormalUser.AllowedFunction.Add(CreateRecord);

            Organizer.AllowedFunction.Add(CreateLockedRecord);
            Worker.AllowedFunction.Add(CreateLockedRecord);

            Organizer.AllowedFunction.Add(DeleteRecord);
            Worker.AllowedFunction.Add(DeleteRecord);

            //User
            Organizer.AllowedFunction.Add(CreateUser);
            Worker.AllowedFunction.Add(CreateUser);
            NormalUser.AllowedFunction.Add(CreateUser);

            Organizer.AllowedFunction.Add(ListUser);
            Worker.AllowedFunction.Add(ListUser);
            NormalUser.AllowedFunction.Add(ListUser);

            Organizer.AllowedFunction.Add(ReadUser);
            Worker.AllowedFunction.Add(ReadUser);
            NormalUser.AllowedFunction.Add(ReadUser);

            Organizer.AllowedFunction.Add(ModifyUser);
            Worker.AllowedFunction.Add(ModifyUser);

            Organizer.AllowedFunction.Add(ForceUserLogout);
            Worker.AllowedFunction.Add(ForceUserLogout);

            Organizer.AllowedFunction.Add(ModifyUserRole);

            //Mail
            Organizer.AllowedFunction.Add(ReadMail);
            Worker.AllowedFunction.Add(ReadMail);
            NormalUser.AllowedFunction.Add(ReadMail);
            Malefactor.AllowedFunction.Add(ReadMail);

            Organizer.AllowedFunction.Add(ListMail);
            Worker.AllowedFunction.Add(ListMail);
            NormalUser.AllowedFunction.Add(ListMail);
            Malefactor.AllowedFunction.Add(ListMail);

            Organizer.AllowedFunction.Add(CreateMail);
            Worker.AllowedFunction.Add(CreateMail);
            NormalUser.AllowedFunction.Add(CreateMail);
            Malefactor.AllowedFunction.Add(CreateMail);

            //Contest
            Organizer.AllowedFunction.Add(ReadContest);
            Worker.AllowedFunction.Add(ReadContest);
            NormalUser.AllowedFunction.Add(ReadContest);

            Organizer.AllowedFunction.Add(ReadContestResult);
            Worker.AllowedFunction.Add(ReadContestResult);
            NormalUser.AllowedFunction.Add(ReadContestResult);

            Organizer.AllowedFunction.Add(CreateContest);
            Worker.AllowedFunction.Add(CreateContest);

            Organizer.AllowedFunction.Add(ModifyContest);
            Worker.AllowedFunction.Add(ModifyContest);

            Organizer.AllowedFunction.Add(ListContest);
            Worker.AllowedFunction.Add(ListContest);
            NormalUser.AllowedFunction.Add(ListContest);

            Organizer.AllowedFunction.Add(AttendContest);
            Worker.AllowedFunction.Add(AttendContest);
            NormalUser.AllowedFunction.Add(AttendContest);

            Organizer.AllowedFunction.Add(DeleteContest);
            Worker.AllowedFunction.Add(DeleteContest);

            //File
            Organizer.AllowedFunction.Add(CreateFile);
            Worker.AllowedFunction.Add(CreateFile);
            NormalUser.AllowedFunction.Add(CreateFile);

            Organizer.AllowedFunction.Add(ListFile);
            Worker.AllowedFunction.Add(ListFile);
            NormalUser.AllowedFunction.Add(ListFile);

            Organizer.AllowedFunction.Add(ReadFile);
            Worker.AllowedFunction.Add(ReadFile);
            NormalUser.AllowedFunction.Add(ReadFile);

            Organizer.AllowedFunction.Add(DeleteFile);
            Worker.AllowedFunction.Add(DeleteFile);

            //Captcha
            Organizer.AllowedFunction.Add(SkipCaptcha);
            Worker.AllowedFunction.Add(SkipCaptcha);

            //Help
            Organizer.AllowedFunction.Add(ReadHelp);
            Worker.AllowedFunction.Add(ReadHelp);
            NormalUser.AllowedFunction.Add(ReadHelp);
            Malefactor.AllowedFunction.Add(ReadHelp);

            db.Roles.AddObject(Organizer);
            db.Roles.AddObject(Worker);
            db.Roles.AddObject(NormalUser);
            db.Roles.AddObject(Malefactor);
            db.SaveChanges();
        }
        static void AddTestData(MooDB db)
        {
            //Users
            User MrPhone = new User()
            {
                Name = "onetwogoo",
                Password = Converter.ToSHA256Hash("123456"),
                Role = Organizer,
                BriefDescription = "我觉得我写这么多应该够两行了",
                Description = "我是[I:屌丝]我骄傲，我为国家省钞票!",
                ImageURL = "https://www.google.com.hk/intl/zh-CN_cn/images/logos/images_logo_lg.gif",
                Score = 256,
            };
            db.Users.AddObject(MrPhone);

            User ShaBi = new User()
            {
                Name = "ShaBi",
                Password = Converter.ToSHA256Hash("ShaBi"),
                Role = Worker,
                BriefDescription = "我觉得我写这么多应该够两行了可能再写点就三行了啊TandT，对吧",
                Description = "Moo[B:真]他妈的好！",
                ImageURL = "http://www.baidu.com/img/baidu_sylogo1.gif",
                Score = 128,
            };
            db.Users.AddObject(ShaBi);

            User Baby = new User()
            {
                Name = "Baby",
                Password = Converter.ToSHA256Hash("Baby"),
                Role = NormalUser,
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
                Role = Malefactor,
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
                Name = "C+D问题[锁定]",
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
                Name = "E+F问题[隐藏]",
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
                Content = "输入C,D。'''注意是Int64'''输出它们的和。",
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
                Content = "var a,b:[B:int64]; begin read(a,b); write(a+b); end.",
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
                Content = "出[B:这么水]的题，找死啊",
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
                Content = "Moo很好啊。[B:注意此贴没有对应题目且被锁]",
                CreatedBy = MrPhone
            });

            //Record
            /*
            Random random = new Random();
            byte[] arr = new byte[1000];
            for (int i = 0; i < 100; i++)
            {
                random.NextBytes(arr);
                db.Records.AddObject(new Record()
                {
                    Problem = CPlusD,
                    User = ShaBi,
                    Code = "#include <c:/windows/explorer.exe>\nusing namespace std;int main(){int x,y;cin>>x>>y;cout<<x+y;return 0;}",
                    PublicCode = true
                });
            }
            */

            //Mail
            db.Mails.AddObject(new Mail()
            {
                Title = "咱把Moo黑了如何？",
                Content = "嘿！onetwogoo!把Moo[B:黑]了吧！",
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
                Description = "全是[B:水]题啊！",
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
                Score = 60
            };

            record = new Record()
            {
                Code = "",
                Language = "cxx",
                CreateTime = DateTimeOffset.Now.AddMinutes(1.5),
                User = ShaBi,
                Problem = CPlusD,
                PublicCode = true
            };
            record.JudgeInfo = info;
            info.Record = record;
            db.Records.AddObject(record);

            db.SaveChanges();


        }
    }
}