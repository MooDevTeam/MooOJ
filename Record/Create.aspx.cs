using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Record_Create : System.Web.UI.Page
{
    protected bool canCreate;

    protected Problem problem;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["problemID"] != null)
                {
                    CollectEntityByProblemID(db, int.Parse(Request["problemID"]));
                }

                if (problem == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                canCreate = problem.LockRecord ? Permission.Check("record.locked.create", false, false) : Permission.Check("record.create", false, false);

                ViewState["problemID"] = problem.ID;
                Page.DataBind();
            }
        }
    }

    void CollectEntityByProblemID(MooDB db, int id)
    {
        problem = (from p in db.Problems
                   where p.ID == id
                   select p).SingleOrDefault<Problem>();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        int problemID = (int)ViewState["problemID"];
        using (MooDB db = new MooDB())
        {
            Problem problem = (from p in db.Problems
                               where p.ID == problemID
                               select p).Single<Problem>();
            if (problem.LockRecord)
            {
                if (!Permission.Check("record.locked.create", false)) return;
            }
            else
            {
                if (!Permission.Check("record.create", false)) return;
            }

            User currentUser = ((SiteUser)User.Identity).GetDBUser(db);
            Record record = new Record()
             {
                 Problem = problem,
                 User = currentUser,
                 Code = txtCode.Text,
                 Language = ddlLanguage.SelectedValue,
                 PublicCode = chkPublicCode.Checked,
                 CreateTime = DateTimeOffset.Now
             };
            db.Records.AddObject(record);

            problem.SubmissionCount++;
            if (!(from r in db.Records
                  where r.ID != record.ID && r.User.ID == currentUser.ID && r.Problem.ID == problem.ID
                  select r).Any())
            {
                problem.SubmissionUser++;
            }

            db.SaveChanges();
        }

        PageUtil.Redirect("操作成功", "~/Record/List.aspx?userID=" + ((SiteUser)User.Identity).ID);
    }
}