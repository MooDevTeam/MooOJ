<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Problem_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(problem.Name) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="题目">
        <Moo:LinkBarItem URL='<%#"~/Problem/?revision="+revision.ID %>' Selected="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/Problem/Modify.aspx?id="+problem.ID %>' Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Problem/Update.aspx?revision="+revision.ID %>' Text="更新" />
        <Moo:LinkBarItem URL='<%#"~/Problem/History.aspx?id="+problem.ID %>' Text="历史" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx?problemID="+problem.ID %>' Shortcut="true" Text="提交" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Special="true" Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true" Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true" Text="记录" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible="<%#revision.ID!=problem.LatestRevision.ID %>">
        您阅读的是
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        的历史版本，如需阅读最新版本请<a href='<%#"~/Problem/?id="+problem.ID %>' runat="server">单击这里</a>。
    </Moo:InfoBlock>
    <h1>
        <%#HttpUtility.HtmlEncode(problem.Name) %>
    </h1>
    <div>
        <%#canRead?WikiParser.Parse(revision.Content):"<span style='color:red; font-weight:bold;'>题目隐藏，内容不可见。</span>" %>
    </div>
</asp:Content>
