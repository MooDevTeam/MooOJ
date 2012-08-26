<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Create.aspx.cs" Inherits="Record_Create" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>为
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        创建记录</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="记录">
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Text="列表" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx?problemID="+problem.ID%>' Selected="true"
            Text="创建" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID%>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID%>' Special="true"
            Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="帖子" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!canCreate %>'>
        您可能不具备完成此操作所必须的权限。
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr>
            <th>
                代码
            </th>
            <td>
                <asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine" Rows="20" Width="100%" />
            </td>
        </tr>
        <tr>
            <th>
                语言
            </th>
            <td>
                <asp:DropDownList ID="ddlLanguage" runat="server">
                    <asp:ListItem Selected="true" Value="cxx">CXX</asp:ListItem>
                    <asp:ListItem Value="c">C</asp:ListItem>
                    <asp:ListItem Value="pascal">Pascal</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                选项
            </th>
            <td>
                <asp:CheckBox ID="chkPublicCode" runat="server" Checked="true" Text="以XXX许可协议公开我的代码" />
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
                <asp:Button ID="btnSubmit" runat="server" Text="创建" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
