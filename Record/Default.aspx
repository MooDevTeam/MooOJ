<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Record_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(record.User.Name) %>
        为
        <%#HttpUtility.HtmlEncode(record.Problem.Name) %>
        创建的记录</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="记录">
        <Moo:LinkBarItem URL='<%#"~/Record/?id="+record.ID %>' Selected="true" Text="记录" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+record.Problem.ID %>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+record.Problem.ID %>' Special="true"
            Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+record.Problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+record.Problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/User/?id="+record.User.ID %>' Special="true" Text="用户" />
    </Moo:LinkBar>
    <table class="detailTable">
        <tr>
            <th>
                题目
            </th>
            <td>
                <a runat="server" href='<%#"~/Problem/?id="+record.Problem.ID %>'>
                    <%#HttpUtility.HtmlEncode(record.Problem.Name) %>
                </a>
            </td>
        </tr>
        <tr>
            <th>
                用户
            </th>
            <td>
                <Moo:UserSign runat="server" User='<%#record.User %>' />
            </td>
        </tr>
        <tr runat="server" visible='<%#info!=null %>'>
            <th>
                分数
            </th>
            <td>
                <%#info==null?0:info.Score %>
            </td>
        </tr>
        <tr>
            <th>
                语言
            </th>
            <td>
                <%#HttpUtility.HtmlEncode(record.Language) %>
            </td>
        </tr>
        <tr id="trCode" runat="server">
            <th>
                代码
            </th>
            <td>
                <pre><%#HttpUtility.HtmlEncode(record.Code) %></pre>
            </td>
        </tr>
        <tr runat="server" visible='<%#info!=null %>'>
            <th>
                测评信息
            </th>
            <td>
                <%#info==null?"":WikiParser.Parse(info.Info) %>
            </td>
        </tr>
    </table>
</asp:Content>
