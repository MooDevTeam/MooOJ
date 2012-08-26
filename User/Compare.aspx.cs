using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class User_Compare : System.Web.UI.Page
{
    protected User userA, userB;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("user.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                int userAID = int.Parse(Request["userA"]);
                int userBID = int.Parse(Request["userB"]);
                userA = (from u in db.Users
                         where u.ID == userAID
                         select u).SingleOrDefault<User>();
                userB = (from u in db.Users
                         where u.ID == userBID
                         select u).SingleOrDefault<User>();
                if (userA == null || userB == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                BindDatas(db);

                Page.DataBind();
            }
        }
    }

    void BindDatas(MooDB db)
    {
        var onlyA = from p in db.Problems
                    let recordA = from r in db.Records
                                  where r.User.ID == userA.ID && r.Problem.ID == p.ID
                                        && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                  select r
                    where recordA.Any()
                    let recordB = from r in db.Records
                                  where r.User.ID == userB.ID && r.Problem.ID == p.ID
                                        && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                  select r
                    where !recordB.Any()
                    select p;

        var onlyB = from p in db.Problems
                    let recordB = from r in db.Records
                                  where r.User.ID == userB.ID && r.Problem.ID == p.ID
                                        && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                  select r
                    where recordB.Any()
                    let recordA = from r in db.Records
                                  where r.User.ID == userA.ID && r.Problem.ID == p.ID
                                        && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                  select r
                    where !recordA.Any()
                    select p;

        var AGreaterThanB = from p in db.Problems
                            let recordA = from r in db.Records
                                          where r.User.ID == userA.ID && r.Problem.ID == p.ID
                                                && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                          select r
                            where recordA.Any()
                            let recordB = from r in db.Records
                                          where r.User.ID == userB.ID && r.Problem.ID == p.ID
                                                && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                          select r
                            where recordB.Any()
                            let scoreA = recordA.Max(r => r.JudgeInfo.Score)
                            let scoreB = recordB.Max(r => r.JudgeInfo.Score)
                            where scoreA > scoreB
                            select p;

        var BGreaterThanA = from p in db.Problems
                            let recordA = from r in db.Records
                                          where r.User.ID == userA.ID && r.Problem.ID == p.ID
                                                && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                          select r
                            where recordA.Any()
                            let recordB = from r in db.Records
                                          where r.User.ID == userB.ID && r.Problem.ID == p.ID
                                                && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                          select r
                            where recordB.Any()
                            let scoreA = recordA.Max(r => r.JudgeInfo.Score)
                            let scoreB = recordB.Max(r => r.JudgeInfo.Score)
                            where scoreB > scoreA
                            select p;

        var same = from p in db.Problems
                   let recordA = from r in db.Records
                                 where r.User.ID == userA.ID && r.Problem.ID == p.ID
                                       && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                 select r
                   where recordA.Any()
                   let recordB = from r in db.Records
                                 where r.User.ID == userB.ID && r.Problem.ID == p.ID
                                       && r.JudgeInfo != null && r.JudgeInfo.Score >= 0
                                 select r
                   where recordB.Any()
                   let scoreA = recordA.Max(r => r.JudgeInfo.Score)
                   let scoreB = recordB.Max(r => r.JudgeInfo.Score)
                   where scoreA == scoreB
                   select p;

        FillPlaceHolderWithProblems(holderOnlyA, onlyA);
        FillPlaceHolderWithProblems(holderOnlyB, onlyB);
        FillPlaceHolderWithProblems(holderAGreaterThanB, AGreaterThanB);
        FillPlaceHolderWithProblems(holderBGreaterThanA, BGreaterThanA);
        FillPlaceHolderWithProblems(holderSame, same);
    }
    void FillPlaceHolderWithProblems(PlaceHolder holder, IEnumerable<Problem> problems)
    {
        foreach (Problem p in problems)
        {
            holder.Controls.Add(new HyperLink()
            {
                Text = HttpUtility.HtmlEncode(p.Name),
                NavigateUrl = "~/Problem/?id=" + p.ID
            });
            holder.Controls.Add(new Literal() { Text = " " });
        }
    }
    protected void ValidateUserID(object source, ServerValidateEventArgs args)
    {
        TextBox toValidate = (TextBox)fldQuery.FindControl(((CustomValidator)source).ControlToValidate);
        using (MooDB db = new MooDB())
        {
            int id = int.Parse(toValidate.Text);
            args.IsValid = (from u in db.Users
                            where u.ID == id
                            select u).SingleOrDefault<User>() != null;
        }
    }
    protected void btnQuery_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;
        Response.Redirect("~/User/Compare.aspx?userA=" + txtUserA.Text + "&userB=" + txtUserB.Text, true);
    }
}