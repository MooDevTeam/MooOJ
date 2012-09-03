<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="TestCase_List" %>

<%@ Import Namespace="Moo.DB" %>
<%@ Import Namespace="Moo.Utility" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        的测试数据列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="测试数据">
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Selected="true"
            Text="列表" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/Create.aspx?id="+problem.ID %>' Text="创建" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID %>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="记录" />
    </Moo:LinkBar>
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="TestCases" OrderBy="it.[ID] DESC"
        Include="Problem,CreatedBy" EntityTypeFilter="" Select="" Where="it.[Problem].ID=@problemID">
        <WhereParameters>
            <asp:QueryStringParameter Name="problemID" QueryStringField="id" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        CssClass="listTable" DataSourceID="dataSource" OnRowDeleting="grid_RowDeleting"
        PageSize='<%$ Resources:Moo,GridViewPageSize %>' DataKeyNames="ID" CellSpacing="-1"
        EmptyDataText='<%$ Resources:Moo,EmptyDataText %>'>
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="测试数据编号" SortExpression="ID" />
            <asp:TemplateField HeaderText="类型">
                <ItemTemplate>
                    <%#PageUtil.GetEntity<TestCase>(Container.DataItem).GetType().Name %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="创建者">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/User/?id="+Eval("CreatedBy.ID") %>'>
                        <%#HttpUtility.HtmlEncode(Eval("CreatedBy.Name")) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="~/TestCase/?id={0}"
                DataTextField="ID" DataTextFormatString="查看" HeaderText="查看" />
            <asp:TemplateField HeaderText="操作" ShowHeader="False">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CausesValidation="False" CommandName="Delete" Text="删除"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
