<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(revision.Title) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="主页">
        <Moo:LinkBarItem URL='<%#"~/?revision="+revision.ID %>' Selected="true" Text="主页" />
        <Moo:LinkBarItem URL='<%#"~/Update.aspx?revision="+revision.ID %>' Text="更新" />
        <Moo:LinkBarItem URL="~/History.aspx" Text="历史" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!isLatest %>'>
        您所阅读的主页是历史版本，如需阅读最新版本请<a runat="server" href="~/">单击这里</a>。
    </Moo:InfoBlock>
    <h1>
        <%#HttpUtility.HtmlEncode(revision.Title)%></h1>
    <div>
        <%#WikiParser.Parse(revision.Content) %>
    </div>
</asp:Content>
