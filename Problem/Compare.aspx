<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Compare.aspx.cs" Inherits="Problem_Compare" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>比较
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        的不同版本</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="题目">
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID %>' Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/Problem/Modify.aspx?id="+problem.ID %>' Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Problem/Update.aspx?id="+problem.ID %>' Text="更新" />
        <Moo:LinkBarItem URL='<%#"~/Problem/History.aspx?id="+problem.ID %>' Text="历史" />
        <Moo:LinkBarItem URL='<%#"~/Problem/Compare.aspx?revisionOld="+revisionOld.ID+"&revisionNew="+revisionNew.ID %>'
            Selected="true" Text="比较" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx?problemID="+problem.ID %>' Shortcut="true"
            Text="提交" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Special="true"
            Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="记录" />
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
