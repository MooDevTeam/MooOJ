<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="History.aspx.cs" Inherits="Solution_History" %>

<%@ Import Namespace="Moo.Utility" %>
<%@ Import Namespace="Moo.DB" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(problem.Name) %>
        的题解历史版本</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="题解">
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Solution/Update.aspx?id="+problem.ID %>' Text="更新" />
        <Moo:LinkBarItem URL='<%#"~/Solution/History.aspx?id="+problem.ID %>' Selected="true"
            Text="历史" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx?problemID="+problem.ID %>' Shortcut="true"
            Text="提交" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID %>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Special="true" Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="记录" />
    </Moo:LinkBar>
    <Moo:InfoBlock ID="infoDeletingLatest" runat="server" Type="Alert" ViewStateMode="Disabled" Visible="false">
        无法删除最新版本。
    </Moo:InfoBlock>
    
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="SolutionRevisions" OrderBy="it.[ID] DESC"
        Include="Problem,CreatedBy" Where="it.[Problem].[ID]=@problemID" 
        EnableDelete="True">
        <WhereParameters>
            <asp:QueryStringParameter Name="problemID" QueryStringField="id" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        CssClass="listTable" DataSourceID="dataSource" DataKeyNames="ID" 
        AllowSorting="True" PageSize='<%$Resources:Moo,GridViewPageSize %>'
        CellSpacing="-1" onrowdeleting="grid_RowDeleting" 
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
                    <a href='<%#"~/Solution/?revision="+Eval("ID") %>' runat="server">
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
