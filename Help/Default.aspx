<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Help_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>帮助</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="帮助">
        <Moo:LinkBarItem URL='<%#"~/Help/?id="+int.Parse(Request["id"]) %>' Selected="true"
            Text="帮助" />
        <Moo:LinkBarItem URL='<%#"~/Help/"+int.Parse(Request["id"])+".txt" %>' Text="源代码" />
    </Moo:LinkBar>
    <div>
        <%#WikiParser.Parse(content) %>
    </div>
</asp:Content>
