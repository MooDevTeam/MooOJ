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
        <asp:Literal runat="server" Text="<%$Resources:Moo,NoEnoughPermission%>" />
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr runat="server" visible='<%#problem.Type=="Tranditional" || problem.Type=="SpecialJudged" || problem.Type=="Interactive" %>'>
            <th>
                代码
            </th>
            <td>
                <asp:TextBox ID="txtCode" runat="server" TextMode="MultiLine" Rows="20" Width="100%" />
            </td>
        </tr>
        <tr runat="server" visible='<%#problem.Type=="Tranditional" || problem.Type=="SpecialJudged" || problem.Type=="Interactive" %>'>
            <th>
                语言
            </th>
            <td>
                <asp:DropDownList ID="ddlLanguage" runat="server">
                    <asp:ListItem Selected="true" Value="c++">C++</asp:ListItem>
                    <asp:ListItem Value="c">C</asp:ListItem>
                    <asp:ListItem Value="pascal">Pascal</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr runat="server" visible='<%#problem.Type=="AnswerOnly" %>'>
            <th>
                答案
            </th>
            <td>
                <asp:PlaceHolder ID="answerArea" runat="server"></asp:PlaceHolder>
            </td>
        </tr>
        <tr>
            <th>
                选项
            </th>
            <td>
                <asp:CheckBox ID="chkPublicCode" runat="server" Checked="true" Text="公开我的代码" />
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
