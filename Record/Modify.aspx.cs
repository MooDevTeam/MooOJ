using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Utility;
using Moo.DB;
using Moo.Authorization;
public partial class Record_Modify : System.Web.UI.Page
{
    protected bool canModify;

    protected Record record;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    int recordID = int.Parse(Request["id"]);
                    record = (from r in db.Records
                              where r.ID == recordID
                              select r).SingleOrDefault<Record>();
                }

                if (record == null)
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }

                canModify = Permission.Check("record.modify", false, false)
                    || User.Identity.IsAuthenticated && record.User.ID == ((SiteUser)User.Identity).ID;

                ViewState["recordID"] = record.ID;
                Page.DataBind();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        int recordID = (int)ViewState["recordID"];
        using (MooDB db = new MooDB())
        {
            record = (from r in db.Records
                      where r.ID == recordID
                      select r).Single<Record>();
            canModify = Permission.Check("record.modify", false, false)
                    || User.Identity.IsAuthenticated && record.User.ID == ((SiteUser)User.Identity).ID;
            if (!canModify)
            {
                Permission.Check("i'm super man.", false);
                return;
            }
            record.PublicCode = chkPublicCode.Checked;
            db.SaveChanges();
        }

        PageUtil.Redirect("修改成功", "~/Record/?id=" + recordID);
    }
}