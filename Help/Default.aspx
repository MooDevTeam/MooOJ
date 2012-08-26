<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Help_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>帮助</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <div>
        <%#WikiParser.Parse(content) %>
    </div>
</asp:Content>
