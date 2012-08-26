using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Authorization;
using Moo.DB;
using Moo.Utility;
public partial class File_Default : System.Web.UI.Page
{
    protected UploadedFile file;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("file.read", true)) return;

        if (!Page.IsPostBack)
        {
            using (MooDB db = new MooDB())
            {
                if (Request["id"] != null)
                {
                    int fileID = int.Parse(Request["id"]);
                    file = (from f in db.UploadedFiles
                            where f.ID == fileID
                            select f).SingleOrDefault<UploadedFile>();
                }

                if (file == null)
                {
                    PageUtil.Redirect("找不到相关内容", "~/");
                    return;
                }

                Page.DataBind();
            }
        }
    }
}