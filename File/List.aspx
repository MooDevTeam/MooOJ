<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="File_List" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>文件列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="文件">
        <Moo:LinkBarItem URL="~/File/List.aspx" Selected="true" Text="列表" />
        <Moo:LinkBarItem URL="~/File/Create.aspx" Text="创建" />
    </Moo:LinkBar>
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EnableFlattening="False" EntitySetName="UploadedFiles"
        EntityTypeFilter="" OrderBy="it.[ID] DESC" Select="">
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" DataSourceID="dataSource" AutoGenerateColumns="False"
        CssClass="listTable" CellSpacing="-1" AllowPaging="True" AllowSorting="True"
        DataKeyNames="ID" OnRowDeleting="grid_RowDeleting" EmptyDataText='<%$ Resources:Moo,EmptyDataText %>'>
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="文件编号" SortExpression="ID" />
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/File/?id="+Eval("ID") %>'>
                        <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField HeaderText="操作" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
