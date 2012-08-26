using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
[PersistChildren(true), ParseChildren(false)]
public partial class InfoBlock : System.Web.UI.UserControl
{
    public string Style
    {
        set
        {
            infoBlockWrap.Style.Value += value;
        }
    }

    public enum BlockType
    {
        Error,Alert,Info
    }

    public BlockType Type
    {
        set
        {
            switch (value)
            {
                case BlockType.Error:
                    infoBlockWrap.Style.Add(HtmlTextWriterStyle.BackgroundColor, "pink");
                    infoBlockWrap.Style.Add(HtmlTextWriterStyle.BorderColor, "red");
                    infoBlockImage.Src = "~/image/error.png";
                    break;
                case BlockType.Alert:
                    infoBlockWrap.Style.Add(HtmlTextWriterStyle.BackgroundColor, "gold");
                    infoBlockWrap.Style.Add(HtmlTextWriterStyle.BorderColor, "orange");
                    infoBlockImage.Src = "~/image/alert.png";
                    break;
                case BlockType.Info:
                    infoBlockWrap.Style.Add(HtmlTextWriterStyle.BackgroundColor, "lightblue");
                    infoBlockWrap.Style.Add(HtmlTextWriterStyle.BorderColor, "blue");
                    infoBlockImage.Src = "~/image/info.png";
                    break;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void RenderChildren(HtmlTextWriter writer)
    {
        int cIndex = childrenHolder.Controls.Count;
        int eIndex = Controls.IndexOf(endMarker);
        int maxIndex = Controls.Count - 1;
        for (int i = maxIndex; i > eIndex; i--)
        {
            Control control = Controls[i];
            Controls.RemoveAt(i);
            childrenHolder.Controls.AddAt(cIndex, control);
        }
        base.RenderChildren(writer);
    }
}