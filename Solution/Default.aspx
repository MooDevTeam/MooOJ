<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Solution_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        的题解</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="题解">
        <Moo:LinkBarItem URL='<%#"~/Solution/?revision="+revision.ID %>' Selected="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Solution/Update.aspx?revision="+revision.ID %>' Text="更新" />
        <Moo:LinkBarItem URL='<%#"~/Solution/History.aspx?id="+problem.ID %>' Text="历史" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx?problemID="+problem.ID %>' Shortcut="true" Text="提交" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID %>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Special="true" Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true" Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true" Text="记录" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#problem.LatestSolution.ID!=revision.ID %>'>
        您阅读的是
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        的题解的历史版本，如需阅读最新版本请<a runat="server" href='<%#"~/Solution/?revision="+problem.ID%>'>单击这里</a>。
    </Moo:InfoBlock>
    <h1>
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        的题解
    </h1>
    <div>
        <%#WikiParser.Parse(revision.Content) %>
    </div>
</asp:Content>
