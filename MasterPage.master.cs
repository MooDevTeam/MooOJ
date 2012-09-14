using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.Web.Security;
using Moo.Authorization;
using Moo.DB;
public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            IIdentity identity = HttpContext.Current.User.Identity;
            if (identity.IsAuthenticated)
            {
                using (MooDB db = new MooDB())
                {
                    int userID = ((SiteUser)identity).ID;
                    int haveNotReadCount = (from m in db.Mails
                                            where m.IsRead == false && m.To.ID == userID
                                            select m).Count<Mail>();
                    if (haveNotReadCount > 0)
                    {
                        ((HtmlGenericControl)viewMail.FindControl("haveNotRead")).InnerText = "(" + haveNotReadCount + ")";
                    }
                }
            }
            viewUserInfo.DataBind();
        }
    }
}
