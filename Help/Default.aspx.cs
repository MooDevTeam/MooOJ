using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Moo.Authorization;
using Moo.Utility;
public partial class Help_Default : System.Web.UI.Page
{
    protected string content;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Permission.Check("help.read", true)) return;

        if (!Page.IsPostBack)
        {
            if (Request["id"] != null)
            {
                int helpID = int.Parse(Request["id"]);
                string filePath = Server.MapPath("~/Help/" + helpID + ".txt");
                if (File.Exists(filePath))
                {
                    content = File.ReadAllText(filePath);
                }
                else
                {
                    PageUtil.Redirect(Resources.Moo.FoundNothing, "~/");
                    return;
                }
            }

            if (content == null)
            {
                Response.Redirect("~/Help/?id=1", true);
                return;
            }
            Page.DataBind();
        }
    }
}