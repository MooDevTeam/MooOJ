<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Compare.aspx.cs" Inherits="Compare" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>比较主页的不同版本</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="主页">
        <Moo:LinkBarItem URL="~/" Text="主页" />
        <Moo:LinkBarItem URL="~/Update.aspx" Text="更新" />
        <Moo:LinkBarItem URL="~/History.aspx" Text="历史" />
        <Moo:LinkBarItem URL='<%#"~/Compare.aspx?revisionOld="+revisionOld.ID+"&revisionNew="+revisionNew.ID %>'
            Selected="true" Text="比较" />
    </Moo:LinkBar>
    <table style="width: 100%; border: none; table-layout: fixed;">
        <tr>
            <th style="border: none; width: 50%; background: none;">
                <a runat="server" href='<%#"~/Problem/?revision="+revisionOld.ID %>'>旧版本</a> #<%#revisionOld.ID %><br />
                <Moo:UserSign runat="server" UserID="<%#revisionOld.CreatedBy.ID %>" />
                <br />
                <%#HttpUtility.HtmlEncode(revisionOld.Reason) %>
            </th>
            <th style="padding: 10px; border: none; background: none;">
                <a runat="server" href='<%#"~/Problem/?revision="+revisionNew.ID %>'>新版本</a> #<%#revisionNew.ID %><br />
                <Moo:UserSign runat="server" UserID="<%#revisionNew.CreatedBy.ID %>" />
                <br />
                <%#HttpUtility.HtmlEncode(revisionNew.Reason) %>
            </th>
        </tr>
        <tr>
            <td style="padding: 10px; border: none; font-size: 14pt;" colspan="2">
                <pre><%#DiffGenerator.Generate(revisionOld.Content,revisionNew.Content)%></pre>
            </td>
        </tr>
    </table>
</asp:Content>
