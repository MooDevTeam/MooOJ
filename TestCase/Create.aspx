<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Create.aspx.cs" Inherits="TestCase_Create" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>为
        <%#problem.Name %>
        创建测试数据</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="测试数据">
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Text="列表" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/Create.aspx?id="+problem.ID %>' Selected="true"
            Text="创建" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx?problemID="+problem.ID %>' Shortcut="true"
            Text="提交" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID %>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="记录" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!canCreate %>'>
        <asp:Literal runat="server" Text="<%$Resources:Moo,NoEnoughPermission%>" />
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr>
            <th>
                分数
            </th>
            <td>
                <asp:TextBox ID="txtScore" runat="server" Width="100%"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtScore" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:CompareValidator runat="server" ControlToValidate="txtScore" Operator="DataTypeCheck"
                    Type="Integer" Display="Dynamic" CssClass="validator">不是整数</asp:CompareValidator>
                <asp:RangeValidator runat="server" ControlToValidate="txtScore" Type="Integer" MinimumValue="0"
                    MaximumValue="1000" Display="Dynamic" CssClass="validator">需在0~1000之间</asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <th>
                时间限制
            </th>
            <td>
                <asp:TextBox ID="txtTimeLimit" runat="server" Width="100%"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTimeLimit" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:CompareValidator runat="server" ControlToValidate="txtTimeLimit" Operator="DataTypeCheck"
                    Type="Integer" Display="Dynamic" CssClass="validator">不是整数</asp:CompareValidator>
                <asp:RangeValidator runat="server" ControlToValidate="txtTimeLimit" Type="Integer"
                    MinimumValue="0" MaximumValue="60000" Display="Dynamic" CssClass="validator">需在0~60000之间</asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <th>
                内存限制
            </th>
            <td>
                <asp:TextBox ID="txtMemoryLimit" runat="server" Width="100%"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMemoryLimit" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:CompareValidator runat="server" ControlToValidate="txtMemoryLimit" Operator="DataTypeCheck"
                    Type="Integer" Display="Dynamic" CssClass="validator">不是整数</asp:CompareValidator>
                <asp:RangeValidator runat="server" ControlToValidate="txtMemoryLimit" Type="Integer"
                    MinimumValue="0" MaximumValue="1073741824" Display="Dynamic" CssClass="validator">需在0~1073741824之间</asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <th>
                输入文件
            </th>
            <td>
                <asp:FileUpload ID="fileInput" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="fileInput" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <th>
                答案文件
            </th>
            <td>
                <asp:FileUpload ID="fileAnswer" runat="server" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="fileAnswer" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr runat="server" visible='<%#problem.Type=="SpecialJudged" %>'>
            <th>
                测评程序文件编号
            </th>
            <td>
                <asp:TextBox ID="txtJudger" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtJudger" Display="Dynamic" CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:CompareValidator runat="server" ControlToValidate="txtJudger" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" CssClass="validator">需为整数</asp:CompareValidator>
                <asp:CustomValidator ID="validateJudger" runat="server" ControlToValidate="txtJudger" 
                    Display="Dynamic" CssClass="validator" 
                    onservervalidate="validateJudger_ServerValidate">无此文件</asp:CustomValidator>
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
