<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Contest_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(contest.Title) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="比赛">
        <Moo:LinkBarItem URL='<%#"~/Contest/?id="+contest.ID %>' Selected="true" Text="比赛" />
        <Moo:LinkBarItem URL='<%#"~/Contest/Modify.aspx?id="+contest.ID %>' Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Contest/Result.aspx?id="+contest.ID %>' Text="结果" />
    </Moo:LinkBar>
    <h1>
        <%#HttpUtility.HtmlEncode(contest.Title) %></h1>
    <table class="detailTable">
        <tr>
            <th>
                开始时间
            </th>
            <td>
                <%#string.Format("{0:yyyy-MM-dd HH:mm:ss}",contest.StartTime) %>
            </td>
        </tr>
        <tr>
            <th>
                结束时间
            </th>
            <td>
                <%#string.Format("{0:yyyy-MM-dd HH:mm:ss}", contest.EndTime)%>
            </td>
        </tr>
        <tr>
            <th>
                状态
            </th>
            <td>
                <%#contest.Status %>
            </td>
        </tr>
        <tr id="trBeforeStart" runat="server" visible='<%#DateTimeOffset.Now<contest.StartTime %>'>
            <th>
                距离比赛开始
            </th>
            <td>
                <%#string.Format("{0:%d}天 {0:%h}时 {0:%m}分 {0:%s}秒",contest.StartTime-DateTimeOffset.Now) %>
            </td>
        </tr>
        <tr id="trBeforeEnd" runat="server" visible='<%#DateTimeOffset.Now > contest.StartTime && DateTimeOffset.Now < contest.EndTime %>'>
            <th>
                距离比赛结束
            </th>
            <td>
                <%#string.Format("{0:%d}天 {0:%h}时 {0:%m}分 {0:%s}秒", contest.EndTime - DateTimeOffset.Now)%>
            </td>
        </tr>
        <tr id="trViewResult" runat="server" visible='<%#DateTimeOffset.Now > contest.EndTime %>'>
            <th>
                比赛结果
            </th>
            <td>
                <a runat="server" href='<%#"~/Contest/Result.aspx?id="+contest.ID %>'>查看结果</a>
            </td>
        </tr>
        <tr id="trAttend" runat="server" visible='<%#User.Identity.IsAuthenticated && DateTimeOffset.Now < contest.EndTime %>'>
            <th>
                报名
            </th>
            <td>
                <asp:Button ID="btnAttend" runat="server" CausesValidation="false" Text="我要报名" Visible='<%#!attended %>'
                    OnClick="btnAttend_Click" />
                <asp:Literal runat="server" Visible='<%#attended %>'>您已完成报名</asp:Literal>
            </td>
        </tr>
        <tr>
            <th>
                描述
            </th>
            <td>
                <div>
                    <%#WikiParser.Parse(contest.Description) %>
                </div>
            </td>
        </tr>
        <tr>
            <th>
                题目
            </th>
            <td>
                <fieldset>
                    <legend>添加</legend>输入题目编号：
                    <asp:TextBox ID="txtProblemID" runat="server" ValidationGroup="grpAdd"></asp:TextBox>
                    <asp:RequiredFieldValidator runat="server" ValidationGroup="grpAdd" ControlToValidate="txtProblemID"
                        CssClass="validator" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ValidationGroup="grpAdd" ControlToValidate="txtProblemID"
                        Operator="DataTypeCheck" Type="Integer" CssClass="validator" Display="Dynamic">需为整数</asp:CompareValidator>
                    <asp:CustomValidator ID="validateAdd" runat="server" ValidationGroup="grpAdd" ControlToValidate="txtProblemID"
                        CssClass="validator" Display="Dynamic" OnServerValidate="validateAdd_ServerValidate"></asp:CustomValidator>
                    <asp:Button ID="btnAdd" runat="server" ValidationGroup="grpAdd" Text="添加" OnClick="btnAdd_Click" />
                </fieldset>
                <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False" CssClass="listTable"
                    CellSpacing="-1" OnRowDeleting="grid_RowDeleting" DataKeyNames="ID" EmptyDataText='<%$ Resources:Moo,EmptyDataText %>'>
                    <AlternatingRowStyle BackColor="LightBlue" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="题目编号" SortExpression="ID" />
                        <asp:TemplateField HeaderText="名称">
                            <ItemTemplate>
                                <a runat="server" href='<%#"~/Problem/?id="+Eval("ID") %>'>
                                    <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Type" HeaderText="类型" SortExpression="Type" />
                        <asp:CommandField HeaderText="操作" ShowDeleteButton="True" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
