<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Create.aspx.cs" Inherits="Problem_Create" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>创建题目</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="题目">
        <Moo:LinkBarItem URL="~/Problem/List.aspx" Text="列表" />
        <Moo:LinkBarItem URL="~/Problem/Create.aspx" Selected="true" Text="创建" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!Permission.Check("problem.create",false,false) %>'>
        <asp:Literal runat="server" Text="<%$Resources:Moo,NoEnoughPermission%>" />
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr>
            <th>
                名称
            </th>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="100%"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtName" ValidationExpression=".{1,40}"
                    Display="Dynamic" CssClass="validator">长度需在1~40位之间</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                类型
            </th>
            <td>
                <asp:DropDownList ID="ddlType" runat="server">
                    <asp:ListItem Selected="True" Value="Tranditional">传统</asp:ListItem>
                    <asp:ListItem Value="SpecialJudged">自定义测评</asp:ListItem>
                    <asp:ListItem Value="Interactive">交互式</asp:ListItem>
                    <asp:ListItem Value="AnswerOnly">提交答案</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                内容
            </th>
            <td>
                <Moo:WikiEditor ID="txtContent" runat="server" />
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
