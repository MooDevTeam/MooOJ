<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="Mail_List" %>

<%@ Import Namespace="Moo.Authorization" %>
<%@ Import Namespace="Moo.DB" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>邮件列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="邮件">
        <Moo:LinkBarItem URL='<%#"~/Mail/List.aspx"+(Request["otherID"]==null?"":"?otherID="+int.Parse(Request["otherID"])) %>'
            Selected="true" Text="列表" />
        <Moo:LinkBarItem URL='<%#"~/Mail/Create.aspx?to="+(Request["otherID"]==null?0:int.Parse(Request["otherID"])) %>'
            Hidden='<%#Request["otherID"]==null %>' Text="创建" />
    </Moo:LinkBar>
    <fieldset>
        <legend>查询</legend>
        <asp:Label runat="server">对方名称</asp:Label>
        <asp:TextBox ID="txtOtherName" runat="server" ValidationGroup="grpQuery" Text='<%#otherName %>'></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ValidationGroup="grpQuery" ControlToValidate="txtOtherName"
            Display="Dynamic" CssClass="validator">不能为空</asp:RequiredFieldValidator>
        <asp:CustomValidator ID="validateOtherName" runat="server" ValidationGroup="grpQuery"
            ControlToValidate="txtOtherName" Display="Dynamic" CssClass="validator" 
            onservervalidate="validateOtherName_ServerValidate">无此用户</asp:CustomValidator>
        <asp:Button ID="btnQuery" runat="server" ValidationGroup="grpQuery" Text="查询" 
            onclick="btnQuery_Click" />
    </fieldset>
    <Moo:InfoBlock ID="deletingFailure" runat="server" Type="Error" ViewStateMode="Disabled"
        Visible="false">
        您不是收信人且收信人已读
    </Moo:InfoBlock>
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="Mails" OrderBy="it.[ID] DESC" Include="From,To"
        EnableDelete="True" Where="(it.[To].[ID]=@currentUserID or it.[From].[ID]=@currentUserID)
and
(@otherID is null or it.[To].[ID]=@otherID or it.[From].[ID]=@otherID)">
        <WhereParameters>
            <asp:QueryStringParameter Name="otherID" QueryStringField="otherID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        CssClass="listTable" DataSourceID="dataSource" CellSpacing="-1" DataKeyNames="ID"
        PageSize='<%$Resources:Moo,GridViewPageSize %>' OnRowDeleting="grid_RowDeleting"
        EmptyDataText='<%$ Resources:Moo,EmptyDataText %>'>
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="邮件编号" SortExpression="ID" />
            <asp:TemplateField HeaderText="对方">
                <ItemTemplate>
                    <a runat="server" visible='<%#(int)Eval("From.ID")==((SiteUser)User.Identity).ID%>'
                        href='<%#"~/User/?id="+Eval("To.ID") %>'>
                        <%#((User)Eval("To")).Name %>
                    </a><a id="A1" runat="server" visible='<%#(int)Eval("From.ID")!=((SiteUser)User.Identity).ID%>'
                        href='<%#"~/User/?id="+Eval("From.ID") %>'>
                        <%#((User)Eval("From")).Name %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="方向">
                <ItemStyle Font-Bold="true" />
                <ItemTemplate>
                    <asp:Literal runat="server" Visible='<%#(int)Eval("From.ID")==((SiteUser)User.Identity).ID %>'><span style="color:red;">&lt;---</span></asp:Literal>
                    <asp:Literal runat="server" Visible='<%#(int)Eval("To.ID")==((SiteUser)User.Identity).ID %>'><span style="color:Green;">---&gt;</span></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="标题">
                <ItemTemplate>
                    <a href='<%#"~/Mail/?id="+Eval("ID") %>' runat="server" style='<%#!(bool)Eval("IsRead") && (int)Eval("To.ID")==((SiteUser)User.Identity).ID?"font-weight:bold; font-size:larger;": ""%>'>
                        <%#HttpUtility.HtmlEncode(Eval("Title")) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="收信人已读" SortExpression="IsRead">
                <ItemTemplate>
                    <asp:Literal runat="server" Visible='<%#Eval("IsRead") %>'>是的</asp:Literal>
                    <asp:Literal runat="server" Visible='<%#!(bool)Eval("IsRead") %>'>还没呢</asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField HeaderText="操作" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
