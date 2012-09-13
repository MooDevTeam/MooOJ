using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
namespace Moo.DB
{
    /// <summary>
    /// 安装数据库
    /// </summary>
    public static class DatabaseInstaller
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
        static Function ReadHiddenTestCase = new Function() { Name = "testcase.hidden.read" };
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
        static Function ModifyRecord = new Function() { Name = "record.modify" };
        static Function CreateRecord = new Function() { Name = "record.create" };
        static Function CreateLockedRecord = new Function() { Name = "record.locked.create" };
        static Function DeleteRecord = new Function() { Name = "record.delete" };
        static Function DeleteRecordJudgeInfoLimited = new Function() { Name = "record.judgeinfo.delete.limited" };
        static Function DeleteRecordJudgeInfo = new Function() { Name = "record.judgeinfo.delete" };

        static Function CreateUser = new Function() { Name = "user.create" };
        static Function ListUser = new Function() { Name = "user.list" };
        static Function ReadUser = new Function() { Name = "user.read" };
        static Function ModifyUser = new Function() { Name = "user.modify" };
        static Function ForceUserLogout = new Function() { Name = "user.forcelogout" };
        static Function ModifyUserRole = new Function() { Name = "user.role.modify" };
        static Function ModifyUserName = new Function() { Name = "user.name.modify" };

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

        static User owner;

        public static void Install()
        {
            using (MooDB db = new MooDB())
            {
                Install(db);
            }
        }

        public static void Install(MooDB db)
        {
            db.CreateDatabase();
            FixDatabase(db);
            AddRequiredData(db);
        }

        static void FixDatabase(MooDB db)
        {
            db.ExecuteStoreCommand("CREATE UNIQUE INDEX IX_Users_Name ON [dbo].[Users] ([Name])");

            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_TranditionalTestCase] DROP CONSTRAINT [FK_TranditionalTestCase_inherits_TestCase];");
            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_TranditionalTestCase] ADD CONSTRAINT [FK_TranditionalTestCase_inherits_TestCase] FOREIGN KEY ([ID]) REFERENCES [dbo].[TestCases]([ID]) ON DELETE CASCADE;");

            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase] DROP CONSTRAINT [FK_SpecialJudgedTestCase_inherits_TestCase];");
            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase] ADD CONSTRAINT [FK_SpecialJudgedTestCase_inherits_TestCase] FOREIGN KEY ([ID]) REFERENCES [dbo].[TestCases]([ID]) ON DELETE CASCADE;");

            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_InteractiveTestCase] DROP CONSTRAINT [FK_InteractiveTestCase_inherits_TestCase];");
            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_InteractiveTestCase] ADD CONSTRAINT [FK_InteractiveTestCase_inherits_TestCase] FOREIGN KEY ([ID]) REFERENCES [dbo].[TestCases]([ID]) ON DELETE CASCADE;");

            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_AnswerOnlyTestCase] DROP CONSTRAINT [FK_AnswerOnlyTestCase_inherits_TestCase];");
            db.ExecuteStoreCommand("ALTER TABLE [dbo].[TestCases_AnswerOnlyTestCase] ADD CONSTRAINT [FK_AnswerOnlyTestCase_inherits_TestCase] FOREIGN KEY ([ID]) REFERENCES [dbo].[TestCases]([ID]) ON DELETE CASCADE;");
        }

        static void AddRequiredData(MooDB db)
        {
            AddRoles(db);
            AddOwner(db);
            AddHomepage(db);
        }

        static void AddRoles(MooDB db)
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

            Organizer.AllowedFunction.Add(ReadHiddenTestCase);
            Worker.AllowedFunction.Add(ReadHiddenTestCase);

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

            Organizer.AllowedFunction.Add(ModifyRecord);
            Worker.AllowedFunction.Add(ModifyRecord);

            Organizer.AllowedFunction.Add(CreateRecord);
            Worker.AllowedFunction.Add(CreateRecord);
            NormalUser.AllowedFunction.Add(CreateRecord);

            Organizer.AllowedFunction.Add(CreateLockedRecord);
            Worker.AllowedFunction.Add(CreateLockedRecord);

            Organizer.AllowedFunction.Add(DeleteRecord);
            Worker.AllowedFunction.Add(DeleteRecord);

            Organizer.AllowedFunction.Add(DeleteRecordJudgeInfoLimited);
            Worker.AllowedFunction.Add(DeleteRecordJudgeInfoLimited);
            NormalUser.AllowedFunction.Add(DeleteRecordJudgeInfoLimited);

            Organizer.AllowedFunction.Add(DeleteRecordJudgeInfo);
            Worker.AllowedFunction.Add(DeleteRecordJudgeInfo);

            //User
            Organizer.AllowedFunction.Add(CreateUser);
            Worker.AllowedFunction.Add(CreateUser);

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

            Organizer.AllowedFunction.Add(ModifyUserName);

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

        static void AddOwner(MooDB db)
        {
            owner = new User()
            {
                Name = WebConfigurationManager.AppSettings["install.owner.name"],
                Password = Moo.Utility.Converter.ToSHA256Hash(WebConfigurationManager.AppSettings["install.owner.password"]),
                BriefDescription = "这个账户为Moo的拥有者准备",
                Description = "",
                Email = "",
                Role = Organizer,
                Score = 0,
                PreferredLanguage = "c++",
            };
            db.Users.AddObject(owner);
            db.SaveChanges();
        }

        static void AddHomepage(MooDB db)
        {
            db.HomepageRevisions.AddObject(new HomepageRevision()
            {
                Title = "Moo的主页",
                Content = "请点击上方的更新，以便使这里显示您需要的内容。",
                CreatedBy = owner,
                Reason = "安装Moo"
            });
            db.SaveChanges();
        }
    }
}