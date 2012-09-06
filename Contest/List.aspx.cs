using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class Contest_List : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("contest.list", true)) return;
        if (!Page.IsPostBack)
        {
            Page.DataBind();
        }
    }
    protected void grid_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        e.Cancel = true;

        if (!Permission.Check("contest.delete", false)) return;
        int contestID = (int)e.Keys[0];
        using (MooDB db = new MooDB())
        {
            Contest contest = (from c in db.Contests
                               where c.ID == contestID
                               select c).Single<Contest>();

            contest.Problem.Clear();
            contest.User.Clear();
            db.Contests.DeleteObject(contest);
            db.SaveChanges();

            Logger.Warning(db, "删除比赛#" + contest.ID);
        }

        grid.Rows[e.RowIndex].Visible = false;
    }
}