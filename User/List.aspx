<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="User_List" %>

<%@ Import Namespace="Moo.DB" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>用户列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="用户">
        <Moo:LinkBarItem URL="~/User/List.aspx" Selected="true" Text="列表" />
        <Moo:LinkBarItem URL="~/User/Register.aspx" Text="注册" />
    </Moo:LinkBar>
    
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="Users" OrderBy="it.[Score] DESC"
        Include="Role">
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        CssClass="listTable" DataKeyNames="ID" DataSourceID="dataSource" CellSpacing="-1">
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <%#grid.PageIndex*grid.PageSize+Container.DataItemIndex+1%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ID" HeaderText="用户编号" SortExpression="ID" />
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/User/?id="+Eval("ID") %>'>
                        <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="分数" SortExpression="Score">
                <ItemTemplate>
                    <span style="color: Red;">
                        <%#Eval("Score") %>
                    </span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="角色" SortExpression="Role.ID">
                <ItemTemplate>
                    <%#((Role)Eval("Role")).DisplayName %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="BriefDescription" HeaderText="简述" />
        </Columns>
    </asp:GridView>
    
</asp:Content>
