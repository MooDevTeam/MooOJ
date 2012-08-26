<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Mail_Default" %>

<%@ Import Namespace="Moo.Text" %>
<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%# HttpUtility.HtmlEncode(mail.Title)%></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="邮件">
        <Moo:LinkBarItem URL='<%#"~/Mail/?id="+mail.ID %>' Selected="true" Text="邮件" />
        <Moo:LinkBarItem URL='<%#"~/Mail/Create.aspx?to="+mail.From.ID+"&replyTo="+mail.ID %>'
            Shortcut="true" Hidden='<%#((SiteUser)User.Identity).ID==mail.From.ID %>' Text="回复" />
    </Moo:LinkBar>
    <h1>
        <%#HttpUtility.HtmlEncode(mail.Title) %>
    </h1>
    <div>
        <span style="font-weight: bold; font-size: 14pt;">To:</span>
        <div>
            <Moo:UserSign runat="server" User='<%#mail.To %>' />
        </div>
    </div>
    <div>
        <%# WikiParser.Parse(mail.Content)%>
    </div>
    <div style="text-align: right;">
        <div style="display: inline-block; text-align: left;">
            <span style="font-weight: bold; font-size: 14pt;">From:</span>
            <div>
                <Moo:UserSign runat="server" User='<%#mail.From %>' />
            </div>
        </div>
    </div>
</asp:Content>
