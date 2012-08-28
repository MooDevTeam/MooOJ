
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 08/28/2012 18:56:22
-- Generated from EDMX file: D:\WebSites\MooOJ\App_Code\Moo\DB\MooDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Moo];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_FunctionForRole_Function]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FunctionForRole] DROP CONSTRAINT [FK_FunctionForRole_Function];
GO
IF OBJECT_ID(N'[dbo].[FK_FunctionForRole_Role]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FunctionForRole] DROP CONSTRAINT [FK_FunctionForRole_Role];
GO
IF OBJECT_ID(N'[dbo].[FK_RecordProblem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Records] DROP CONSTRAINT [FK_RecordProblem];
GO
IF OBJECT_ID(N'[dbo].[FK_TestDataProblem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestCases] DROP CONSTRAINT [FK_TestDataProblem];
GO
IF OBJECT_ID(N'[dbo].[FK_ProblemProblemRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemRevisions] DROP CONSTRAINT [FK_ProblemProblemRevision];
GO
IF OBJECT_ID(N'[dbo].[FK_UserProblemRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemRevisions] DROP CONSTRAINT [FK_UserProblemRevision];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_ProblemSolutionRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolutionRevisions] DROP CONSTRAINT [FK_ProblemSolutionRevision];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCreateSolutionRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolutionRevisions] DROP CONSTRAINT [FK_UserCreateSolutionRevision];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCreatePostItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostItems] DROP CONSTRAINT [FK_UserCreatePostItem];
GO
IF OBJECT_ID(N'[dbo].[FK_PostItemPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostItems] DROP CONSTRAINT [FK_PostItemPost];
GO
IF OBJECT_ID(N'[dbo].[FK_ProblemPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posts] DROP CONSTRAINT [FK_ProblemPost];
GO
IF OBJECT_ID(N'[dbo].[FK_RecordJudgeInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JudgeInfos] DROP CONSTRAINT [FK_RecordJudgeInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_MailFrom]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Mails] DROP CONSTRAINT [FK_MailFrom];
GO
IF OBJECT_ID(N'[dbo].[FK_MailTo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Mails] DROP CONSTRAINT [FK_MailTo];
GO
IF OBJECT_ID(N'[dbo].[FK_LastestRevisionOfProblem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProblemRevisions] DROP CONSTRAINT [FK_LastestRevisionOfProblem];
GO
IF OBJECT_ID(N'[dbo].[FK_LatestSolutionOfProblem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SolutionRevisions] DROP CONSTRAINT [FK_LatestSolutionOfProblem];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAttendContest_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAttendContest] DROP CONSTRAINT [FK_UserAttendContest_User];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAttendContest_Contest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAttendContest] DROP CONSTRAINT [FK_UserAttendContest_Contest];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRecord]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Records] DROP CONSTRAINT [FK_UserRecord];
GO
IF OBJECT_ID(N'[dbo].[FK_ContestProblem_Contest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContestProblem] DROP CONSTRAINT [FK_ContestProblem_Contest];
GO
IF OBJECT_ID(N'[dbo].[FK_ContestProblem_Problem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContestProblem] DROP CONSTRAINT [FK_ContestProblem_Problem];
GO
IF OBJECT_ID(N'[dbo].[FK_SpecialJudgedTestCaseUploadedFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase] DROP CONSTRAINT [FK_SpecialJudgedTestCaseUploadedFile];
GO
IF OBJECT_ID(N'[dbo].[FK_UserCreateHomepageRevision]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HomepageRevisions] DROP CONSTRAINT [FK_UserCreateHomepageRevision];
GO
IF OBJECT_ID(N'[dbo].[FK_SpecialJudgedTestCase_inherits_TestCase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase] DROP CONSTRAINT [FK_SpecialJudgedTestCase_inherits_TestCase];
GO
IF OBJECT_ID(N'[dbo].[FK_TranditionalTestCase_inherits_TestCase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TestCases_TranditionalTestCase] DROP CONSTRAINT [FK_TranditionalTestCase_inherits_TestCase];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Functions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Functions];
GO
IF OBJECT_ID(N'[dbo].[Problems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Problems];
GO
IF OBJECT_ID(N'[dbo].[Records]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Records];
GO
IF OBJECT_ID(N'[dbo].[TestCases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestCases];
GO
IF OBJECT_ID(N'[dbo].[ProblemRevisions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProblemRevisions];
GO
IF OBJECT_ID(N'[dbo].[SolutionRevisions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SolutionRevisions];
GO
IF OBJECT_ID(N'[dbo].[Posts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posts];
GO
IF OBJECT_ID(N'[dbo].[PostItems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostItems];
GO
IF OBJECT_ID(N'[dbo].[JudgeInfos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JudgeInfos];
GO
IF OBJECT_ID(N'[dbo].[Mails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Mails];
GO
IF OBJECT_ID(N'[dbo].[Contests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contests];
GO
IF OBJECT_ID(N'[dbo].[UploadedFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UploadedFiles];
GO
IF OBJECT_ID(N'[dbo].[HomepageRevisions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HomepageRevisions];
GO
IF OBJECT_ID(N'[dbo].[TestCases_SpecialJudgedTestCase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestCases_SpecialJudgedTestCase];
GO
IF OBJECT_ID(N'[dbo].[TestCases_TranditionalTestCase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TestCases_TranditionalTestCase];
GO
IF OBJECT_ID(N'[dbo].[FunctionForRole]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FunctionForRole];
GO
IF OBJECT_ID(N'[dbo].[UserAttendContest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAttendContest];
GO
IF OBJECT_ID(N'[dbo].[ContestProblem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContestProblem];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(20)  NOT NULL,
    [Password] char(64)  NOT NULL,
    [BriefDescription] nvarchar(40)  NOT NULL,
    [ImageURL] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Score] int  NOT NULL,
    [Role_ID] int  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(12)  NOT NULL,
    [DisplayName] nvarchar(12)  NOT NULL
);
GO

-- Creating table 'Functions'
CREATE TABLE [dbo].[Functions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(40)  NOT NULL
);
GO

-- Creating table 'Problems'
CREATE TABLE [dbo].[Problems] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Type] varchar(20)  NOT NULL,
    [Lock] bit  NOT NULL,
    [LockSolution] bit  NOT NULL,
    [LockTestCase] bit  NOT NULL,
    [LockPost] bit  NOT NULL,
    [LockRecord] bit  NOT NULL,
    [AllowTesting] bit  NOT NULL,
    [Hidden] bit  NOT NULL,
    [TestCaseHidden] bit  NOT NULL,
    [SubmissionCount] int  NOT NULL,
    [ScoreSum] bigint  NOT NULL,
    [SubmissionUser] int  NOT NULL,
    [MaximumScore] int  NULL
);
GO

-- Creating table 'Records'
CREATE TABLE [dbo].[Records] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL,
    [PublicCode] bit  NOT NULL,
    [CreateTime] datetimeoffset  NOT NULL,
    [Language] varchar(12)  NOT NULL,
    [Problem_ID] int  NOT NULL,
    [User_ID] int  NOT NULL
);
GO

-- Creating table 'TestCases'
CREATE TABLE [dbo].[TestCases] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Problem_ID] int  NOT NULL
);
GO

-- Creating table 'ProblemRevisions'
CREATE TABLE [dbo].[ProblemRevisions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [Reason] nvarchar(40)  NOT NULL,
    [Problem_ID] int  NOT NULL,
    [CreatedBy_ID] int  NOT NULL,
    [LatestRevisionOf_ID] int  NULL
);
GO

-- Creating table 'SolutionRevisions'
CREATE TABLE [dbo].[SolutionRevisions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [Reason] nvarchar(40)  NOT NULL,
    [Problem_ID] int  NOT NULL,
    [CreatedBy_ID] int  NOT NULL,
    [LatestSolutionOf_ID] int  NULL
);
GO

-- Creating table 'Posts'
CREATE TABLE [dbo].[Posts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Lock] bit  NOT NULL,
    [OnTop] bit  NOT NULL,
    [Problem_ID] int  NULL
);
GO

-- Creating table 'PostItems'
CREATE TABLE [dbo].[PostItems] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [CreatedBy_ID] int  NOT NULL,
    [Post_ID] int  NOT NULL
);
GO

-- Creating table 'JudgeInfos'
CREATE TABLE [dbo].[JudgeInfos] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Score] int  NOT NULL,
    [Info] nvarchar(max)  NOT NULL,
    [Record_ID] int  NOT NULL
);
GO

-- Creating table 'Mails'
CREATE TABLE [dbo].[Mails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(40)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [IsRead] bit  NOT NULL,
    [From_ID] int  NOT NULL,
    [To_ID] int  NOT NULL
);
GO

-- Creating table 'Contests'
CREATE TABLE [dbo].[Contests] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StartTime] datetimeoffset  NOT NULL,
    [EndTime] datetimeoffset  NOT NULL,
    [Title] nvarchar(40)  NOT NULL,
    [LockProblemOnStart] bit  NOT NULL,
    [LockTestCaseOnStart] bit  NOT NULL,
    [LockSolutionOnStart] bit  NOT NULL,
    [LockPostOnStart] bit  NOT NULL,
    [HideTestCaseOnStart] bit  NOT NULL,
    [AllowTestingOnStart] bit  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Status] varchar(12)  NOT NULL,
    [HideProblemOnStart] bit  NOT NULL,
    [LockRecordOnStart] bit  NOT NULL,
    [LockProblemOnEnd] bit  NOT NULL,
    [LockSolutionOnEnd] bit  NOT NULL,
    [LockTestCaseOnEnd] bit  NOT NULL,
    [LockPostOnEnd] bit  NOT NULL,
    [LockRecordOnEnd] bit  NOT NULL,
    [AllowTestingOnEnd] bit  NOT NULL,
    [HideProblemOnEnd] bit  NOT NULL,
    [HideTestCaseOnEnd] bit  NOT NULL
);
GO

-- Creating table 'UploadedFiles'
CREATE TABLE [dbo].[UploadedFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(40)  NOT NULL,
    [Path] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'HomepageRevisions'
CREATE TABLE [dbo].[HomepageRevisions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(40)  NOT NULL,
    [Content] nvarchar(max)  NOT NULL,
    [Reason] nvarchar(40)  NOT NULL,
    [CreatedBy_ID] int  NOT NULL
);
GO

-- Creating table 'TestCases_SpecialJudgedTestCase'
CREATE TABLE [dbo].[TestCases_SpecialJudgedTestCase] (
    [Input] varbinary(max)  NOT NULL,
    [Answer] varbinary(max)  NOT NULL,
    [TimeLimit] int  NOT NULL,
    [MemoryLimit] int  NOT NULL,
    [Score] int  NOT NULL,
    [ID] int  NOT NULL,
    [Judger_ID] int  NOT NULL
);
GO

-- Creating table 'TestCases_TranditionalTestCase'
CREATE TABLE [dbo].[TestCases_TranditionalTestCase] (
    [Input] varbinary(max)  NOT NULL,
    [Answer] varbinary(max)  NOT NULL,
    [TimeLimit] int  NOT NULL,
    [MemoryLimit] int  NOT NULL,
    [Score] int  NOT NULL,
    [ID] int  NOT NULL
);
GO

-- Creating table 'FunctionForRole'
CREATE TABLE [dbo].[FunctionForRole] (
    [AllowedFunction_ID] int  NOT NULL,
    [FunctionForRole_Function_ID] int  NOT NULL
);
GO

-- Creating table 'UserAttendContest'
CREATE TABLE [dbo].[UserAttendContest] (
    [User_ID] int  NOT NULL,
    [UserAttendContest_User_ID] int  NOT NULL
);
GO

-- Creating table 'ContestProblem'
CREATE TABLE [dbo].[ContestProblem] (
    [Contest_ID] int  NOT NULL,
    [Problem_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Functions'
ALTER TABLE [dbo].[Functions]
ADD CONSTRAINT [PK_Functions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Problems'
ALTER TABLE [dbo].[Problems]
ADD CONSTRAINT [PK_Problems]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Records'
ALTER TABLE [dbo].[Records]
ADD CONSTRAINT [PK_Records]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TestCases'
ALTER TABLE [dbo].[TestCases]
ADD CONSTRAINT [PK_TestCases]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProblemRevisions'
ALTER TABLE [dbo].[ProblemRevisions]
ADD CONSTRAINT [PK_ProblemRevisions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SolutionRevisions'
ALTER TABLE [dbo].[SolutionRevisions]
ADD CONSTRAINT [PK_SolutionRevisions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [PK_Posts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PostItems'
ALTER TABLE [dbo].[PostItems]
ADD CONSTRAINT [PK_PostItems]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'JudgeInfos'
ALTER TABLE [dbo].[JudgeInfos]
ADD CONSTRAINT [PK_JudgeInfos]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Mails'
ALTER TABLE [dbo].[Mails]
ADD CONSTRAINT [PK_Mails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Contests'
ALTER TABLE [dbo].[Contests]
ADD CONSTRAINT [PK_Contests]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UploadedFiles'
ALTER TABLE [dbo].[UploadedFiles]
ADD CONSTRAINT [PK_UploadedFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HomepageRevisions'
ALTER TABLE [dbo].[HomepageRevisions]
ADD CONSTRAINT [PK_HomepageRevisions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TestCases_SpecialJudgedTestCase'
ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase]
ADD CONSTRAINT [PK_TestCases_SpecialJudgedTestCase]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TestCases_TranditionalTestCase'
ALTER TABLE [dbo].[TestCases_TranditionalTestCase]
ADD CONSTRAINT [PK_TestCases_TranditionalTestCase]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [AllowedFunction_ID], [FunctionForRole_Function_ID] in table 'FunctionForRole'
ALTER TABLE [dbo].[FunctionForRole]
ADD CONSTRAINT [PK_FunctionForRole]
    PRIMARY KEY NONCLUSTERED ([AllowedFunction_ID], [FunctionForRole_Function_ID] ASC);
GO

-- Creating primary key on [User_ID], [UserAttendContest_User_ID] in table 'UserAttendContest'
ALTER TABLE [dbo].[UserAttendContest]
ADD CONSTRAINT [PK_UserAttendContest]
    PRIMARY KEY NONCLUSTERED ([User_ID], [UserAttendContest_User_ID] ASC);
GO

-- Creating primary key on [Contest_ID], [Problem_ID] in table 'ContestProblem'
ALTER TABLE [dbo].[ContestProblem]
ADD CONSTRAINT [PK_ContestProblem]
    PRIMARY KEY NONCLUSTERED ([Contest_ID], [Problem_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AllowedFunction_ID] in table 'FunctionForRole'
ALTER TABLE [dbo].[FunctionForRole]
ADD CONSTRAINT [FK_FunctionForRole_Function]
    FOREIGN KEY ([AllowedFunction_ID])
    REFERENCES [dbo].[Functions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [FunctionForRole_Function_ID] in table 'FunctionForRole'
ALTER TABLE [dbo].[FunctionForRole]
ADD CONSTRAINT [FK_FunctionForRole_Role]
    FOREIGN KEY ([FunctionForRole_Function_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FunctionForRole_Role'
CREATE INDEX [IX_FK_FunctionForRole_Role]
ON [dbo].[FunctionForRole]
    ([FunctionForRole_Function_ID]);
GO

-- Creating foreign key on [Problem_ID] in table 'Records'
ALTER TABLE [dbo].[Records]
ADD CONSTRAINT [FK_RecordProblem]
    FOREIGN KEY ([Problem_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RecordProblem'
CREATE INDEX [IX_FK_RecordProblem]
ON [dbo].[Records]
    ([Problem_ID]);
GO

-- Creating foreign key on [Problem_ID] in table 'TestCases'
ALTER TABLE [dbo].[TestCases]
ADD CONSTRAINT [FK_TestDataProblem]
    FOREIGN KEY ([Problem_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TestDataProblem'
CREATE INDEX [IX_FK_TestDataProblem]
ON [dbo].[TestCases]
    ([Problem_ID]);
GO

-- Creating foreign key on [Problem_ID] in table 'ProblemRevisions'
ALTER TABLE [dbo].[ProblemRevisions]
ADD CONSTRAINT [FK_ProblemProblemRevision]
    FOREIGN KEY ([Problem_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProblemProblemRevision'
CREATE INDEX [IX_FK_ProblemProblemRevision]
ON [dbo].[ProblemRevisions]
    ([Problem_ID]);
GO

-- Creating foreign key on [CreatedBy_ID] in table 'ProblemRevisions'
ALTER TABLE [dbo].[ProblemRevisions]
ADD CONSTRAINT [FK_UserProblemRevision]
    FOREIGN KEY ([CreatedBy_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserProblemRevision'
CREATE INDEX [IX_FK_UserProblemRevision]
ON [dbo].[ProblemRevisions]
    ([CreatedBy_ID]);
GO

-- Creating foreign key on [Role_ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserRole]
    FOREIGN KEY ([Role_ID])
    REFERENCES [dbo].[Roles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole'
CREATE INDEX [IX_FK_UserRole]
ON [dbo].[Users]
    ([Role_ID]);
GO

-- Creating foreign key on [Problem_ID] in table 'SolutionRevisions'
ALTER TABLE [dbo].[SolutionRevisions]
ADD CONSTRAINT [FK_ProblemSolutionRevision]
    FOREIGN KEY ([Problem_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProblemSolutionRevision'
CREATE INDEX [IX_FK_ProblemSolutionRevision]
ON [dbo].[SolutionRevisions]
    ([Problem_ID]);
GO

-- Creating foreign key on [CreatedBy_ID] in table 'SolutionRevisions'
ALTER TABLE [dbo].[SolutionRevisions]
ADD CONSTRAINT [FK_UserCreateSolutionRevision]
    FOREIGN KEY ([CreatedBy_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCreateSolutionRevision'
CREATE INDEX [IX_FK_UserCreateSolutionRevision]
ON [dbo].[SolutionRevisions]
    ([CreatedBy_ID]);
GO

-- Creating foreign key on [CreatedBy_ID] in table 'PostItems'
ALTER TABLE [dbo].[PostItems]
ADD CONSTRAINT [FK_UserCreatePostItem]
    FOREIGN KEY ([CreatedBy_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCreatePostItem'
CREATE INDEX [IX_FK_UserCreatePostItem]
ON [dbo].[PostItems]
    ([CreatedBy_ID]);
GO

-- Creating foreign key on [Post_ID] in table 'PostItems'
ALTER TABLE [dbo].[PostItems]
ADD CONSTRAINT [FK_PostItemPost]
    FOREIGN KEY ([Post_ID])
    REFERENCES [dbo].[Posts]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PostItemPost'
CREATE INDEX [IX_FK_PostItemPost]
ON [dbo].[PostItems]
    ([Post_ID]);
GO

-- Creating foreign key on [Problem_ID] in table 'Posts'
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [FK_ProblemPost]
    FOREIGN KEY ([Problem_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProblemPost'
CREATE INDEX [IX_FK_ProblemPost]
ON [dbo].[Posts]
    ([Problem_ID]);
GO

-- Creating foreign key on [Record_ID] in table 'JudgeInfos'
ALTER TABLE [dbo].[JudgeInfos]
ADD CONSTRAINT [FK_RecordJudgeInfo]
    FOREIGN KEY ([Record_ID])
    REFERENCES [dbo].[Records]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RecordJudgeInfo'
CREATE INDEX [IX_FK_RecordJudgeInfo]
ON [dbo].[JudgeInfos]
    ([Record_ID]);
GO

-- Creating foreign key on [From_ID] in table 'Mails'
ALTER TABLE [dbo].[Mails]
ADD CONSTRAINT [FK_MailFrom]
    FOREIGN KEY ([From_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MailFrom'
CREATE INDEX [IX_FK_MailFrom]
ON [dbo].[Mails]
    ([From_ID]);
GO

-- Creating foreign key on [To_ID] in table 'Mails'
ALTER TABLE [dbo].[Mails]
ADD CONSTRAINT [FK_MailTo]
    FOREIGN KEY ([To_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MailTo'
CREATE INDEX [IX_FK_MailTo]
ON [dbo].[Mails]
    ([To_ID]);
GO

-- Creating foreign key on [LatestRevisionOf_ID] in table 'ProblemRevisions'
ALTER TABLE [dbo].[ProblemRevisions]
ADD CONSTRAINT [FK_LastestRevisionOfProblem]
    FOREIGN KEY ([LatestRevisionOf_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LastestRevisionOfProblem'
CREATE INDEX [IX_FK_LastestRevisionOfProblem]
ON [dbo].[ProblemRevisions]
    ([LatestRevisionOf_ID]);
GO

-- Creating foreign key on [LatestSolutionOf_ID] in table 'SolutionRevisions'
ALTER TABLE [dbo].[SolutionRevisions]
ADD CONSTRAINT [FK_LatestSolutionOfProblem]
    FOREIGN KEY ([LatestSolutionOf_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LatestSolutionOfProblem'
CREATE INDEX [IX_FK_LatestSolutionOfProblem]
ON [dbo].[SolutionRevisions]
    ([LatestSolutionOf_ID]);
GO

-- Creating foreign key on [User_ID] in table 'UserAttendContest'
ALTER TABLE [dbo].[UserAttendContest]
ADD CONSTRAINT [FK_UserAttendContest_User]
    FOREIGN KEY ([User_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserAttendContest_User_ID] in table 'UserAttendContest'
ALTER TABLE [dbo].[UserAttendContest]
ADD CONSTRAINT [FK_UserAttendContest_Contest]
    FOREIGN KEY ([UserAttendContest_User_ID])
    REFERENCES [dbo].[Contests]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAttendContest_Contest'
CREATE INDEX [IX_FK_UserAttendContest_Contest]
ON [dbo].[UserAttendContest]
    ([UserAttendContest_User_ID]);
GO

-- Creating foreign key on [User_ID] in table 'Records'
ALTER TABLE [dbo].[Records]
ADD CONSTRAINT [FK_UserRecord]
    FOREIGN KEY ([User_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRecord'
CREATE INDEX [IX_FK_UserRecord]
ON [dbo].[Records]
    ([User_ID]);
GO

-- Creating foreign key on [Contest_ID] in table 'ContestProblem'
ALTER TABLE [dbo].[ContestProblem]
ADD CONSTRAINT [FK_ContestProblem_Contest]
    FOREIGN KEY ([Contest_ID])
    REFERENCES [dbo].[Contests]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Problem_ID] in table 'ContestProblem'
ALTER TABLE [dbo].[ContestProblem]
ADD CONSTRAINT [FK_ContestProblem_Problem]
    FOREIGN KEY ([Problem_ID])
    REFERENCES [dbo].[Problems]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContestProblem_Problem'
CREATE INDEX [IX_FK_ContestProblem_Problem]
ON [dbo].[ContestProblem]
    ([Problem_ID]);
GO

-- Creating foreign key on [Judger_ID] in table 'TestCases_SpecialJudgedTestCase'
ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase]
ADD CONSTRAINT [FK_SpecialJudgedTestCaseUploadedFile]
    FOREIGN KEY ([Judger_ID])
    REFERENCES [dbo].[UploadedFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SpecialJudgedTestCaseUploadedFile'
CREATE INDEX [IX_FK_SpecialJudgedTestCaseUploadedFile]
ON [dbo].[TestCases_SpecialJudgedTestCase]
    ([Judger_ID]);
GO

-- Creating foreign key on [CreatedBy_ID] in table 'HomepageRevisions'
ALTER TABLE [dbo].[HomepageRevisions]
ADD CONSTRAINT [FK_UserCreateHomepageRevision]
    FOREIGN KEY ([CreatedBy_ID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCreateHomepageRevision'
CREATE INDEX [IX_FK_UserCreateHomepageRevision]
ON [dbo].[HomepageRevisions]
    ([CreatedBy_ID]);
GO

-- Creating foreign key on [ID] in table 'TestCases_SpecialJudgedTestCase'
ALTER TABLE [dbo].[TestCases_SpecialJudgedTestCase]
ADD CONSTRAINT [FK_SpecialJudgedTestCase_inherits_TestCase]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[TestCases]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ID] in table 'TestCases_TranditionalTestCase'
ALTER TABLE [dbo].[TestCases_TranditionalTestCase]
ADD CONSTRAINT [FK_TranditionalTestCase_inherits_TestCase]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[TestCases]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------