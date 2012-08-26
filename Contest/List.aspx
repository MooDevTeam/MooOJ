<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="Contest_List" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>比赛列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="比赛">
        <Moo:LinkBarItem URL="~/Contest/List.aspx" Selected="true" Text="列表" />
        <Moo:LinkBarItem URL="~/Contest/Create.aspx" Text="创建" />
    </Moo:LinkBar>
    
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EnableFlattening="False" 
        EntitySetName="Contests" EntityTypeFilter="" OrderBy="it.[ID] DESC" Select="">
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        CssClass="listTable" DataKeyNames="ID" DataSourceID="dataSource" CellSpacing="-1"
        OnRowDeleting="grid_RowDeleting">
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="比赛编号" ReadOnly="True" SortExpression="ID" />
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/Contest/?id="+Eval("ID") %>'>
                        <%# HttpUtility.HtmlEncode(Eval("Title"))%>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="StartTime" HeaderText="开始时间" SortExpression="StartTime" />
            <asp:BoundField DataField="EndTime" HeaderText="结束时间" SortExpression="EndTime" />
            <asp:CommandField HeaderText="操作" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
    
</asp:Content>
