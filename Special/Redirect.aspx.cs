using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Special_Redirect : System.Web.UI.Page
{
    protected string redirectURL;
    protected string info;
    protected void Page_Load(object sender, EventArgs e)
    {
        redirectURL=HttpUtility.HtmlEncode(Request["url"]);
        info = HttpUtility.HtmlEncode(Request["info"]);
        Page.DataBind();
    }
}