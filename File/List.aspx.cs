using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class File_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("file.list", true)) return;
        if (!Page.IsPostBack)
        {
            Page.DataBind();
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        int fileID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            UploadedFile file = (from f in db.UploadedFiles
                                 where f.ID == fileID
                                 select f).Single<UploadedFile>();
            bool allowed = User.Identity.IsAuthenticated && ((SiteUser)User.Identity).ID == file.CreatedBy.ID
                    || Permission.Check("file.delete", false,false);
            if (!allowed)
            {
                Permission.Check("i'm super man.", false);
                return;
            }
            var spjTestCases = from t in db.TestCases.OfType<SpecialJudgedTestCase>()
                               where t.Judger.ID == file.ID
                               select t;
            var interactiveTestCases = from t in db.TestCases.OfType<InteractiveTestCase>()
                                       where t.Invoker.ID == file.ID
                                       select t;
            var answerOnlyTestCases = from t in db.TestCases.OfType<AnswerOnlyTestCase>()
                                      where t.Judger.ID == file.ID
                                      select t;
            if (spjTestCases.Any() || interactiveTestCases.Any() || answerOnlyTestCases.Any())
            {
                PageUtil.Redirect("尚有测试点使用此文件，无法删除。", "~/File/List.aspx");
                return;
            }

            File.Delete(file.Path);
            db.UploadedFiles.DeleteObject(file);
            db.SaveChanges();

            Logger.Warning(db, "删除文件#" + file.ID);
        }

        grid.Rows[e.RowIndex].Visible = false;
    }
}