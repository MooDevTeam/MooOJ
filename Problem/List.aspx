<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="Problem_List" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>题目列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="题目">
        <Moo:LinkBarItem URL="~/Problem/List.aspx" Selected="true" Text="列表" />
        <Moo:LinkBarItem URL="~/Problem/Create.aspx" Text="创建" />
    </Moo:LinkBar>
    <fieldset>
        <legend>查询</legend>
        <asp:Label runat="server">题目名称</asp:Label>
        <asp:TextBox ID="txtName" runat="server" ValidationGroup="grpQuery"></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ValidationGroup="grpQuery" ControlToValidate="txtName"
            Display="Dynamic" CssClass="validator">不能为空</asp:RequiredFieldValidator>
        <asp:Button ID="btnQuery" runat="server" ValidationGroup="grpQuery" Text="查询" OnClick="btnQuery_Click" />
    </fieldset>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        CssClass="listTable" CellSpacing="-1" OnRowDeleting="grid_RowDeleting" DataKeyNames="ID"
        OnPageIndexChanging="grid_PageIndexChanging" EmptyDataText='<%$ Resources:Moo,EmptyDataText %>'>
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="题目编号" SortExpression="ID" />
            <asp:BoundField DataField="Score" HeaderText="我的得分" SortExpression="Score" />
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <a href='<%#"~/Problem/?id="+Eval("ID") %>' runat="server">
                        <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SubmissionCount" HeaderText="提交次数" SortExpression="SubmissionCount" />
            <asp:BoundField DataField="Type" HeaderText="类型" />
            <asp:CommandField ShowDeleteButton="True" HeaderText="操作"></asp:CommandField>
        </Columns>
    </asp:GridView>
</asp:Content>
