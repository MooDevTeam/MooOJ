<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Create.aspx.cs" Inherits="Mail_Create" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>创建邮件</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="邮件">
        <Moo:LinkBarItem URL='<%#"~/Mail/List.aspx?otherID="+receiver.ID %>' Text="列表" />
        <Moo:LinkBarItem URL='<%#"~/Mail/?id="+(Request["replyTo"]==null?0:int.Parse(Request["replyTo"])) %>'
            Hidden='<%#Request["replyTo"]==null %>' Text="邮件" />
        <Moo:LinkBarItem URL='<%#"~/Mail/Create.aspx?to="+receiver.ID %>' Selected="true"
            Text="创建" />
    </Moo:LinkBar>
    
    <table class="detailTable">
        <tr>
            <th>
                收信人
            </th>
            <td>
                <Moo:UserSign runat="server" User='<%#receiver %>' />
            </td>
        </tr>
        <tr>
            <th>
                标题
            </th>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="100%" Text='<%#initialTitle %>'></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtTitle" ValidationExpression=".{1,40}"
                    Display="Dynamic" CssClass="validator">长度需在1~40位之间</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr runat="server" id="trPreview" visible="false">
            <th>
                预览
            </th>
            <td>
                <div runat="server" id="divPreview">
                </div>
            </td>
        </tr>
        <tr>
            <th>
                内容
            </th>
            <td>
                <asp:TextBox ID="txtContent" runat="server" Style="width: 100%" Rows="20" TextMode="MultiLine"
                    Text='<%#initialContent %>'></asp:TextBox>
                <Moo:WikiSupported runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                发信人
            </th>
            <td style="text-align: right;">
                <Moo:UserSign runat="server" User='<%#theSender %>' />
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
                <asp:Button ID="btnPreview" runat="server" CausesValidation="false" Text="预览" OnClick="btnPreview_Click" />
                <asp:Button ID="btnSubmit" runat="server" Enabled="false" Text="发送" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
    
</asp:Content>
