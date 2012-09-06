<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="History.aspx.cs" Inherits="History" %>

<%@ Import Namespace="Moo.Utility" %>
<%@ Import Namespace="Moo.DB" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>主页的历史版本</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="主页">
        <Moo:LinkBarItem URL="~/" Text="主页" />
        <Moo:LinkBarItem URL="~/Update.aspx" Text="更新" />
        <Moo:LinkBarItem URL="~/History.aspx" Selected="true" Text="历史" />
    </Moo:LinkBar>
    <Moo:InfoBlock ID="infoDeletingLast" runat="server" Type="Error" Visible="false"
        ViewStateMode="Disabled">
        不能删除仅有的版本。
    </Moo:InfoBlock>
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="HomepageRevisions" OrderBy="it.[ID] DESC"
        Include="CreatedBy" EnableDelete="True">
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="ID" CssClass="listTable" DataSourceID="dataSource" 
        AllowSorting="True" PageSize='<%$Resources:Moo,GridViewPageSize %>'
        CellSpacing="-1" OnRowDeleting="grid_RowDeleting" 
        EmptyDataText='<%$ Resources:Moo,EmptyDataText %>' 
        onrowdeleted="grid_RowDeleted">
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:TemplateField HeaderText="比较">
                <ItemTemplate>
                    <asp:CheckBox ID="chkCompare" runat="server" AutoPostBack="true" OnCheckedChanged="chkCompare_CheckedChanged" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ID" HeaderText="版本编号" SortExpression="ID" />
            <asp:TemplateField HeaderText="内容">
                <ItemTemplate>
                    <a href='<%#"~/?revision="+Eval("ID") %>' runat="server">
                        <%#HttpUtility.HtmlEncode(PageUtil.Truncate((string)Eval("Content"), 20))%>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="修改者" SortExpression="CreatedBy.ID">
                <ItemTemplate>
                    <a href='<%#"~/User/?id="+((User)Eval("CreatedBy")).ID%>' runat="server">
                        <%#HttpUtility.HtmlEncode(((User)Eval("CreatedBy")).Name) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Reason" HeaderText="修改原因" />
            <asp:CommandField HeaderText="操作" ShowDeleteButton="True" ShowHeader="True" />
        </Columns>
    </asp:GridView>
    <asp:Button ID="btnCompare" runat="server" Text="比较选定的版本" Enabled="false" OnClick="btnCompare_Click" />
</asp:Content>
