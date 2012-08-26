using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class User_Default : System.Web.UI.Page
{
    protected User user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("user.read", true)) return;

        using (MooDB db = new MooDB())
        {
            if (Request["id"] != null)
            {
                int id = int.Parse(Request["id"]);
                user = (from u in db.Users
                        where u.ID == id
                        select u).SingleOrDefault<User>();
            }
            if (user == null)
            {
                PageUtil.Redirect("找不到相关内容", "~/");
                return;
            }

            ViewState["userID"] = user.ID;

            grid.DataSource = GetDataToBind(db);
            Page.DataBind();
        }
    }

    IQueryable GetDataToBind(MooDB db)
    {
        return from p in db.Problems
               let records = from r in db.Records
                             where r.User.ID == user.ID && r.Problem.ID == p.ID
                                   && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                             select r
               where records.FirstOrDefault<Record>() != null
               let score = records.Max(r => r.JudgeInfo.Score)
               select new
               {
                   ID = p.ID,
                   Name = p.Name,
                   Score = score
               };
    }

    protected void grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        using (MooDB db = new MooDB())
        {
            grid.PageIndex = e.NewPageIndex;
            grid.DataSource = GetDataToBind(db);
            grid.DataBind();
        }
    }
}