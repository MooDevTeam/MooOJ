<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Modify.aspx.cs" Inherits="Problem_Modify" %>

<%@ Import Namespace="Moo.Authorization" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>修改
        <%#HttpUtility.HtmlEncode(problem.Name) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="题目">
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID %>' Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/Problem/Modify.aspx?id="+problem.ID %>' Selected="true"
            Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Problem/Update.aspx?id="+problem.ID %>' Text="更新" />
        <Moo:LinkBarItem URL='<%#"~/Problem/History.aspx?id="+problem.ID %>' Text="历史" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx?problemID="+problem.ID %>' Shortcut="true"
            Text="提交" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Special="true"
            Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="记录" />
    </Moo:LinkBar>
    <Moo:InfoBlock runat="server" Type="Alert" Visible='<%#!Permission.Check("problem.modify",false,false) %>'>
        您可能不具备完成此操作所必须的权限。
    </Moo:InfoBlock>
    <table class="detailTable">
        <tr>
            <th>
                名称
            </th>
            <td>
                <asp:TextBox ID="txtName" runat="server" Width="100%" Text='<%#problem.Name %>'></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" Display="Dynamic"
                    CssClass="validator">不能为空</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtName" ValidationExpression=".{1,20}"
                    Display="Dynamic" CssClass="validator">长度需在1~20位</asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <th>
                类型
            </th>
            <td>
                <asp:DropDownList ID="ddlType" runat="server">
                    <asp:ListItem Value="Tranditional">传统</asp:ListItem>
                    <asp:ListItem Value="SpecialJudged">自定义测评</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <th>
                选项
            </th>
            <td>
                <asp:CheckBox ID="chkHidden" runat="server" Checked='<%#problem.Hidden %>' Text="隐藏题目" />
                <asp:CheckBox ID="chkTestCaseHidden" runat="server" Checked='<%#problem.TestCaseHidden %>'
                    Text="隐藏测试数据" />
                <asp:CheckBox ID="chkAllowTesting" runat="server" Checked='<%#problem.AllowTesting %>'
                    Text="允许评测" />
                <asp:CheckBox ID="chkLock" runat="server" Checked='<%#problem.Lock %>' Text="锁定题目" />
                <asp:CheckBox ID="chkLockTestCase" runat="server" Checked='<%#problem.LockTestCase %>'
                    Text="锁定测试数据" />
                <asp:CheckBox ID="chkLockSolution" runat="server" Checked='<%#problem.LockSolution %>'
                    Text="锁定题解" />
                <asp:CheckBox ID="chkLockRecord" runat="server" Checked='<%#problem.LockRecord %>'
                    Text="锁定记录" />
                <asp:CheckBox ID="chkLockPost" runat="server" Checked='<%#problem.LockPost %>' Text="锁定帖子" />
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
