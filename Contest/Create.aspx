<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Create.aspx.cs" Inherits="Contest_Create" %>

<%@ Import Namespace="Moo.Authorization" %>
<%@ Import Namespace="Moo.DB" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>创建比赛</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="比赛">
        <Moo:LinkBarItem URL="~/Contest/List.aspx" Text="列表" />
        <Moo:LinkBarItem URL="~/Contest/Create.aspx" Selected="true" Text="创建" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!Permission.Check("contest.create",false,false) %>'>
        <asp:Literal runat="server" Text="<%$Resources:Moo,NoEnoughPermission%>" />
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr>
            <th>
                名称
            </th>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="100%"></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitle" CssClass="validator"
                    Display="Dynamic">不能为空</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtTitle" ValidationExpression=".{1,40}"
                    Display="Dynamic" CssClass="validator">长度需在1~40位</asp:RegularExpressionValidator>
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
                描述
            </th>
            <td>
                <asp:TextBox ID="txtDescription" runat="server" Width="100%" Rows="20" TextMode="MultiLine"></asp:TextBox>
                <Moo:WikiSupported runat="server" />
            </td>
        </tr>
        <tr>
            <th>
                时间
            </th>
            <td>
                <Moo:DateTime ID="timeStart" runat="server" />
                <span style="vertical-align: middle; font-weight: bold;">至</span>
                <Moo:DateTime ID="timeEnd" runat="server" />
                <asp:CustomValidator runat="server" CssClass="validator" Display="Dynamic" OnServerValidate="ValidatePositiveTimeSpan"><br />结束时间不能早于开始时间</asp:CustomValidator>
            </td>
        </tr>
        <tr>
            <th>
                开始后
            </th>
            <td>
                <asp:CheckBox ID="chkAllowTestingOnStart" runat="server" Text="允许评测" Checked="true" />
                <asp:CheckBox ID="chkHideProblemOnStart" runat="server" Text="隐藏题目" />
                <asp:CheckBox ID="chkHideTestCaseOnStart" runat="server" Text="隐藏测试数据" Checked="true" />
                <asp:CheckBox ID="chkLockProblemOnStart" runat="server" Text="锁定题目" Checked="true" />
                <asp:CheckBox ID="chkLockTestCaseOnStart" runat="server" Text="锁定测试数据" Checked="true" />
                <asp:CheckBox ID="chkLockSolutionOnStart" runat="server" Text="锁定题解" Checked="true" />
                <asp:CheckBox ID="chkLockPostOnStart" runat="server" Text="锁定帖子" Checked="true" />
                <asp:CheckBox ID="chkLockRecordOnStart" runat="server" Text="锁定记录" />
            </td>
        </tr>
        <tr>
            <th>
                结束后
            </th>
            <td>
                <asp:CheckBox ID="chkAllowTestingOnEnd" runat="server" Text="允许评测" Checked="true" />
                <asp:CheckBox ID="chkHideProblemOnEnd" runat="server" Text="隐藏题目" />
                <asp:CheckBox ID="chkHideTestCaseOnEnd" runat="server" Text="隐藏测试数据" />
                <asp:CheckBox ID="chkLockProblemOnEnd" runat="server" Text="锁定题目" />
                <asp:CheckBox ID="chkLockTestCaseOnEnd" runat="server" Text="锁定测试数据" />
                <asp:CheckBox ID="chkLockSolutionOnEnd" runat="server" Text="锁定题解" />
                <asp:CheckBox ID="chkLockPostOnEnd" runat="server" Text="锁定帖子" />
                <asp:CheckBox ID="chkLockRecordOnEnd" runat="server" Text="锁定记录" />
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
                <asp:Button ID="btnSubmit" runat="server" Enabled="false" Text="创建" OnClick="btnSubmit_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
