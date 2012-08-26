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
    
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="Problems" EnableDelete="True">
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        CssClass="listTable" DataSourceID="dataSource" AllowSorting="True" 
        CellSpacing="-1" onrowdeleting="grid_RowDeleting">
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="题目编号" SortExpression="ID" />
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <a href='<%#"~/Problem/?id="+Eval("ID") %>' runat="server">
                        <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Type" HeaderText="类型" />
            <asp:CommandField ShowDeleteButton="True" HeaderText="操作"></asp:CommandField>
        </Columns>
    </asp:GridView>
    
</asp:Content>
