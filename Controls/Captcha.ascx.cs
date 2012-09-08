using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using Moo.Authorization;
using Moo.Utility;
public partial class Controls_Captcha : System.Web.UI.UserControl
{
    static char[] CHARACTERS = new char[]{'A','B','D','E','F','G','H','J','N','Q','R','T','Y',
                                          'a','b','d','e','f',    'h',    'n',    'r','t','y',
                                          '2','3','4','5','6','7','8'};
    static Dictionary<int, DateTimeOffset> lastSkip = new Dictionary<int, DateTimeOffset>();

    protected bool isSkip;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.DataBind();
            GenerateCaptcha();
        }
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        GenerateCaptcha();
    }

    void GenerateCaptcha()
    {
        string answer = GetRandomText();
        string message = answer + "," + this.ClientID + "," + Rand.RAND.Next();
        imgCaptcha.ImageUrl = "~/Special/Captcha.ashx?message=" + HttpUtility.UrlEncode(Convert.ToBase64String(Converter.Encrypt(Encoding.Unicode.GetBytes(message))));

        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            SiteUser siteUser = (SiteUser)HttpContext.Current.User.Identity;
            isSkip = Permission.Check("captcha.skip", false, false)
                || !lastSkip.ContainsKey(siteUser.ID)
                || lastSkip[siteUser.ID] < DateTimeOffset.Now.AddSeconds(-10);
            if (isSkip)
            {
                lastSkip[siteUser.ID] = DateTimeOffset.Now;
            }
        }
        else
        {
            isSkip = false;
        }

        if (isSkip)
        {
            txtCaptcha.Text = answer;
        }
        else
        {
            txtCaptcha.Text = "";
        }
        Session["MooCaptchaAnswer" + this.ClientID] = answer;
        Session["MooCaptchaGetImage" + this.ClientID] = "allowed";

        updateCaptcha.DataBind();
    }

    string GetRandomText()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < 5; i++)
        {
            sb.Append(CHARACTERS[Rand.RAND.Next(CHARACTERS.Length)]);
        }
        return sb.ToString();
    }

    protected void Validate(object source, ServerValidateEventArgs e)
    {
        e.IsValid = txtCaptcha.Text.ToLower() == ((string)Session["MooCaptchaAnswer" + this.ClientID]).ToLower();
        GenerateCaptcha();
    }
}