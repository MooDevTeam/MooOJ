<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="File_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(file.Name) %></title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="文件">
        <Moo:LinkBarItem URL='<%#"~/File/?id="+file.ID %>' Selected="true" Text="文件" />
        <Moo:LinkBarItem URL='<%#"~/File/Download.ashx?id="+file.ID %>' Text="下载" />
    </Moo:LinkBar>
    <h1>
        <%#HttpUtility.HtmlEncode(file.Name) %></h1>
    <a runat="server" href='<%#"~/File/Download.ashx?id="+file.ID %>'>下载</a>
</asp:Content>
