<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TryWiki.aspx.cs" Inherits="Special_TryWiki" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>Wiki演练场</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <h1>
        Wiki演练场
    </h1>
    <Moo:WikiEditor runat="server" />
</asp:Content>
