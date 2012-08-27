<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="User_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(user.Name) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="用户">
        <Moo:LinkBarItem URL='<%#"~/User/?id="+user.ID %>' Selected="true" Text="用户" />
        <Moo:LinkBarItem URL='<%#"~/User/Modify.aspx?id="+user.ID %>' Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?userID="+user.ID %>' Special="true"
            Text="记录" />
    </Moo:LinkBar>
    <h1>
        <%#HttpUtility.HtmlEncode(user.Name) %></h1>
    <Moo:UserSign runat="server" User='<%#user %>' Style="float: right;" />
    <div>
        <%#WikiParser.Parse(user.Description) %>
    </div>
    <div class="clear">
    </div>
    <fieldset>
        <legend>得分情况</legend>
        <asp:GridView ID="grid" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
            CssClass="listTable" CellSpacing="-1" EmptyDataText="此人甚懒，啥题没做。" OnPageIndexChanging="grid_PageIndexChanging" PageSize='<%$Resources:Moo,GridViewPageSize %>'>
            <AlternatingRowStyle BackColor="LightBlue"/>
            <Columns>
                <asp:BoundField HeaderText="题目编号" DataField="ID" SortExpression="ID" />
                <asp:TemplateField HeaderText="名称">
                    <ItemTemplate>
                        <a runat="server" href='<%#"~/Problem/?id="+Eval("ID") %>'>
                            <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="得分" SortExpression="Score">
                    <ItemTemplate>
                        <a runat="server" href='<%#"~/Record/List.aspx?problemID="+Eval("ID")+"&userID="+user.ID %>'>
                            <%#Eval("Score") %>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </fieldset>
</asp:Content>
