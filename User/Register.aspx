<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Register.aspx.cs" Inherits="User_Register" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>注册用户</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="用户">
        <Moo:LinkBarItem URL="~/User/List.aspx" Text="列表" />
        <Moo:LinkBarItem URL="~/User/Register.aspx" Selected="true" Text="注册" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!Permission.Check("user.create",true,false) %>'>
        <asp:Literal runat="server" Text="<%$Resources:Moo,NoEnoughPermission%>" />
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr>
            <th>
                用户名
            </th>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" Width="100%"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserName" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtUserName" ValidationExpression=".{1,12}"
                    Display="Dynamic" CssClass="validator">长度需在1~12位之间</asp:RegularExpressionValidator>
                <asp:CustomValidator runat="server" ControlToValidate="txtUserName" OnServerValidate="ValidateUserName"
                    Display="Dynamic" CssClass="validator">用户名已被抢注</asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>
                密码
            </th>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPassword" ValidationExpression=".{6,20}"
                    Display="Dynamic" CssClass="validator">长度需在6~20位之间</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                再输一遍
            </th>
            <td>
                <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
                <asp:CompareValidator runat="server" ControlToValidate="txtPassword2" ControlToCompare="txtPassword"
                    Display="Dynamic" CssClass="validator">两次输入的密码不匹配</asp:CompareValidator>
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
                <asp:Button ID="btnSubmit" runat="server" Text="注册" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
