<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="User_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(user.Name) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="用户">
        <Moo:LinkBarItem URL='<%#"~/User/?id="+user.ID %>' Selected="true" Text="用户" />
        <Moo:LinkBarItem URL='<%#"~/User/Modify.aspx?id="+user.ID %>' Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?userID="+user.ID %>' Special="true" Text="记录" />
    </Moo:LinkBar>
    <h1>
        <%#HttpUtility.HtmlEncode(user.Name) %></h1>
    <Moo:UserSign runat="server" User='<%#user %>' Style="float: right;" />
    <div>
        <%#WikiParser.Parse(user.Description) %>
    </div>
</asp:Content>
