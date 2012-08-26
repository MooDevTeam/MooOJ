using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.DB;
using Moo.Authorization;
using Moo.Utility;
public partial class Record_Default : System.Web.UI.Page
{
    protected JudgeInfo info;
    protected Record record;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("record.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    CollectEntityByID(db, int.Parse(Request["id"]));
                    
                }

                if (record == null)
                {
                    PageUtil.Redirect("找不到相关内容", "~/");
                    return;
                }

                trCode.Visible = User.Identity.IsAuthenticated && ((SiteUser)User.Identity).ID == record.User.ID
                    || Permission.Check("record.code.read", false, false)
                    || record.PublicCode;
                Page.DataBind();
            }
        }
    }

    void CollectEntityByID(MooDB db, int id)
    {
        record = (from r in db.Records
                where r.ID == id
                select r).SingleOrDefault<Record>();
        info = record == null ? null : record.JudgeInfo;
    }
}