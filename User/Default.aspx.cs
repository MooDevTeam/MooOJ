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
        if (!Permission.Check("user.read",true)) return;

        using (MooDB db = new MooDB())
        {
            int id=int.Parse(Request["id"]);
            user = (from u in db.Users
                    where u.ID == id
                    select u).SingleOrDefault<User>();
            if (user == null)
            {
                PageUtil.Redirect("找不到相关内容", "~/");
                return;
            }

            Page.DataBind();
        }
    }
}