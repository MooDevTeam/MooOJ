using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Moo.Text;
public partial class Controls_WikiEditor : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        divPreviewWrapper.Visible = true;
        divPreview.InnerHtml = WikiParser.Parse(txtWiki.Text);
    }
}