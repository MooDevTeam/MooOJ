<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Modify.aspx.cs" Inherits="Record_Modify" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>修改
        <%#HttpUtility.HtmlEncode(record.User.Name) %>
        为
        <%#HttpUtility.HtmlEncode(record.Problem.Name) %>
        创建的记录</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="记录">
        <Moo:LinkBarItem URL='<%#"~/Record/?id="+record.ID %>' Text="记录" />
        <Moo:LinkBarItem URL='<%#"~/Record/Modify.aspx?id="+record.ID %>' Selected="true"
            Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+record.Problem.ID %>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+record.Problem.ID %>' Special="true"
            Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+record.Problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+record.Problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/User/?id="+record.User.ID %>' Special="true" Text="用户" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!canModify %>'>
        <asp:Literal runat="server" Text="<%$Resources:Moo,NoEnoughPermission%>" />
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr>
            <th>
                选项
            </th>
            <td>
                <asp:CheckBox ID="chkPublicCode" runat="server" Checked='<%#record.PublicCode %>'
                    Text="公开我的代码" />
            </td>
        </tr>
        <tr>
            <th>
                验证码
            </th>
            <td>
                <Moo:Captcha runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btnSubmit" runat="server" Text="修改" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
