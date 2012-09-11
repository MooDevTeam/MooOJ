<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Create.aspx.cs" Inherits="Post_Create" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>创建帖子</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="帖子">
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"~/Post/List.aspx":"~/Post/List.aspx?problemID="+int.Parse(Request["problemID"]) %>'
            Text="列表" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"~/Post/Create.aspx":"~/Post/Create.aspx?problemID="+int.Parse(Request["problemID"]) %>'
            Selected="true" Text="创建" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/Problem/?id="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="题目" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/TestCase/List.aspx?id="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="测试数据" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/Solution/?id="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="题解" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/Record/List.aspx?problemID="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="记录" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!canCreate %>'>
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
        <tr id="trPreview" runat="server" visible="false">
            <th>
                预览
            </th>
            <td>
            <asp:Literal ID="litOnlyPreview" runat="server" Text='<%$Resources:Moo,ItsOnlyPreview %>' />
                <div id="divPreview" runat="server">
                </div>
            </td>
        </tr>
        <tr>
            <th>
                内容
            </th>
            <td>
                <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="20" Width="100%"></asp:TextBox>
                <Moo:WikiSupported runat="server" />
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
                <asp:Button ID="btnPreview" runat="server" Text="先预览" CausesValidation="false" OnClick="btnPreview_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="后创建" Enabled="false" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
