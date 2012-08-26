<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="Post_List" %>

<%@ Import Namespace="Moo.DB" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>帖子列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="帖子">
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"~/Post/List.aspx":"~/Post/List.aspx?problemID="+int.Parse(Request["problemID"]) %>'
            Selected="true" Text="列表" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"~/Post/Create.aspx":"~/Post/Create.aspx?problemID="+int.Parse(Request["problemID"]) %>'
            Text="创建" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/Problem/?id="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="题目" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/TestCase/List.aspx?id="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="测试数据" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/Solution/?id="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="题解" />
        <Moo:LinkBarItem URL='<%#Request["problemID"]==null?"":"~/Record/List.aspx?problemID="+int.Parse(Request["problemID"]) %>'
            Special="true" Hidden='<%#Request["problemID"]==null %>' Text="记录" />
    </Moo:LinkBar>
    
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EntitySetName="Posts" OrderBy="it.[OnTop] DESC,
it.[ID] DESC" Where="1=1"
        Include="Problem" EnableDelete="True" EntityTypeFilter="" Select="">
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        CssClass="listTable" DataKeyNames="ID" DataSourceID="dataSource" 
        CellSpacing="-1" onrowdeleting="grid_RowDeleting" EmptyDataText='<%$ Resources:Moo,EmptyDataText %>'>
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="帖子编号" ReadOnly="True" SortExpression="ID" />
            <asp:TemplateField HeaderText="名称">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/Post/?id="+Eval("ID") %>'>
                        <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                    </a>
                    <span style="color:red" runat="server" visible='<%#Eval("OnTop") %>'>[置顶]</span>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="题目" SortExpression="Problem.ID">
                <ItemTemplate>
                    <asp:Literal runat="server" Visible='<%#Eval("Problem")==null %>'>无</asp:Literal>
                    <a runat="server" href='<%#"~/Problem/?id="+(Eval("Problem")==null?0:((Problem)Eval("Problem")).ID) %>'
                        visible='<%#Eval("Problem")!=null %>'>
                        <%#Eval("Problem")==null?"":((Problem)Eval("Problem")).Name %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField HeaderText="操作" ShowDeleteButton="True" ShowHeader="True" />
        </Columns>
    </asp:GridView>
    
</asp:Content>
