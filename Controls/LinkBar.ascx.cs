using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

[ParseChildren(true, "Items")]
public partial class Controls_LinkBar : System.Web.UI.UserControl
{
    public List<Moo.Controls.LinkBarItem> Items { get; set; }

    public string Title { get; set; }
    public override void DataBind()
    {
        foreach (Moo.Controls.LinkBarItem item in Items)
        {
            item.DataBind();
        }
        base.DataBind();
    }
}
