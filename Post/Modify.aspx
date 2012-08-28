<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Modify.aspx.cs" Inherits="Post_Modify" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>修改
        <%#post.Name %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="帖子">
        <Moo:LinkBarItem URL='<%#"~/Post/?id="+post.ID %>' Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Post/Modify.aspx?id="+post.ID %>' Selected="true" Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Post/Reply.aspx?id="+post.ID %>' Text="回复" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!Permission.Check("post.modify",false,false) %>'>
        <asp:Literal runat="server" Text="<%$Resources:Moo,NoEnoughPermission%>" />
    </Moo:InfoBlock>
    
    <table class="detailTable">
        <tr>
            <th>
                名称
            </th>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="100%" Text='<%#post.Name %>'></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" CssClass="validator"
                    Display="Dynamic">不能为空</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtName" ValidationExpression=".{1,40}"
                    CssClass="validator" Display="Dynamic">长度需在1~40位</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                选项
            </th>
            <td>
                <asp:CheckBox ID="chkLock" runat="server" Text="锁定" Checked='<%#post.Lock %>' />
                <asp:CheckBox ID="chkOnTop" runat="server" Text="置顶" Checked='<%#post.OnTop %>' />
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
