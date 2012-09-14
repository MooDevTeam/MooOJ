<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Create.aspx.cs" Inherits="File_Create" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>创建文件</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="文件">
        <Moo:LinkBarItem URL="~/File/List.aspx" Text="列表" />
        <Moo:LinkBarItem URL="~/File/Create.aspx" Selected="true" Text="创建" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!Permission.Check("file.create",false,false) %>'>
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
                上传
            </th>
            <td>
                <asp:FileUpload ID="fileUpload" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="fileUpload" Display="Dynamic"
                    CssClass="validator">必须选择</asp:RequiredFieldValidator>
                <asp:CustomValidator ID="validateFileLength" runat="server" ControlToValidate="fileUpload"
                    Display="Dynamic" CssClass="validator" OnServerValidate="validateFileLength_ServerValidate">须小于10MB</asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>
                描述
            </th>
            <td>
                <Moo:WikiEditor ID="txtDescription" runat="server" />
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
