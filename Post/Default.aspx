<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="Post_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <link href="../css/Post_Default.css" rel="Stylesheet" type="text/css" />
    <title>
        <%#HttpUtility.HtmlEncode(post.Name) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="帖子">
        <Moo:LinkBarItem URL='<%#"~/Post/?id="+post.ID %>' Selected="true" Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Post/Modify.aspx?id="+post.ID %>' Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Post/Reply.aspx?id="+post.ID %>' Text="回复" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+(post.Problem==null?0:post.Problem.ID) %>'
            Special="true" Hidden='<%#post.Problem==null %>' Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+(post.Problem==null?0:post.Problem.ID) %>'
            Special="true" Hidden='<%#post.Problem==null %>' Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+(post.Problem==null?0:post.Problem.ID) %>'
            Special="true" Hidden='<%#post.Problem==null %>' Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+(post.Problem==null?0:post.Problem.ID) %>'
            Special="true" Hidden='<%#post.Problem==null %>' Text="记录" />
    </Moo:LinkBar>
    <h1>
        <%#HttpUtility.HtmlEncode(post.Name) %>
    </h1>
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="PostItems" OrderBy="it.[ID]" Where="it.[Post].ID=@postID"
        Include="CreatedBy,CreatedBy.Role,Post,Post.Problem" EnableDelete="True">
        <WhereParameters>
            <asp:QueryStringParameter Name="postID" QueryStringField="id" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="ID" DataSourceID="dataSource" ShowHeader="False" CssClass="postTable"
        PageSize='20' OnRowDeleting="grid_RowDeleting" EmptyDataText="人去楼空" OnRowDeleted="grid_RowDeleted">
        <Columns>
            <asp:TemplateField HeaderText="内容" ItemStyle-CssClass="postCell">
                <ItemTemplate>
                    <div class="postContent">
                        <div>
                            <%# grid.PageSize * grid.PageIndex + Container.DataItemIndex + 1 %>楼 <a runat="server"
                                href='<%#"~/Post/Reply.aspx?replyTo="+Eval("ID") %>'>回复</a>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="删除"></asp:LinkButton>
                            <Moo:UserSign runat="server" User='<%#Eval("CreatedBy") %>' Style="float: right;" />
                            <div class="clear">
                            </div>
                        </div>
                        <%# WikiParser.Parse((string)Eval("Content"))%>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
