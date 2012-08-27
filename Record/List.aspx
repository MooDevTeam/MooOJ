<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="List.aspx.cs" Inherits="Record_List" %>

<%@ Import Namespace="Moo.DB" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>记录列表</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="记录">
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?"+(problemID==null?"":"problemID="+problemID)
                                                      +(userID==null?"":"&userID="+userID)
                                                      +(contestID==null?"":"&contestID="+contestID) %>'
            Selected="true" Text="列表" />
        <Moo:LinkBarItem URL='<%#"~/Record/Create.aspx"+(problemID==null?"":"?problemID="+problemID)%>'
            Hidden='<%#problemID==null %>' Text="创建" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problemID %>' Special="true" Hidden='<%#problemID==null %>'
            Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problemID %>' Special="true"
            Hidden='<%#problemID==null %>' Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problemID %>' Special="true" Hidden='<%#problemID==null %>'
            Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problemID %>' Special="true"
            Hidden='<%#problemID==null %>' Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/User/?id="+problemID %>' Special="true" Hidden='<%#problemID==null %>'
            Text="用户" />
    </Moo:LinkBar>
    <div>
        题目编号
        <asp:TextBox ID="txtProblemID" runat="server" Text='<%#problemID %>'></asp:TextBox>
        <asp:CompareValidator runat="server" ControlToValidate="txtProblemID" Operator="DataTypeCheck"
            Type="Integer" Display="Dynamic" CssClass="validator">不是整数</asp:CompareValidator>
        用户编号
        <asp:TextBox ID="txtUserID" runat="server" Text='<%#userID %>'></asp:TextBox>
        <asp:CompareValidator runat="server" ControlToValidate="txtUserID" Operator="DataTypeCheck"
            Type="Integer" Display="Dynamic" CssClass="validator">不是整数</asp:CompareValidator>
        比赛编号
        <asp:TextBox ID="txtContestID" runat="server" Text='<%#contestID %>'></asp:TextBox>
        <asp:CompareValidator runat="server" ControlToValidate="txtContestID" Operator="DataTypeCheck"
            Type="Integer" Display="Dynamic" CssClass="validator">不是整数</asp:CompareValidator>
        <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />
    </div>
    <asp:GridView ID="grid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="ID" CssClass="listTable" CellSpacing="-1" OnPageIndexChanging="grid_PageIndexChanging" PageSize='<%$Resources:Moo,GridViewPageSize %>'
        OnRowDeleting="grid_RowDeleting" EmptyDataText='<%$ Resources:Moo,EmptyDataText %>'>
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="记录编号" ReadOnly="True" SortExpression="ID" />
            <asp:TemplateField HeaderText="题目" SortExpression="Problem.ID">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/Problem/?id="+((Problem)Eval("Problem")).ID %>'>
                        <%#HttpUtility.HtmlEncode(((Problem)Eval("Problem")).Name)%>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="用户" SortExpression="User.ID">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/User/?id="+((User)Eval("User")).ID %>'>
                        <%#HttpUtility.HtmlEncode(((User)Eval("User")).Name)%>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="语言" DataField="Language" SortExpression="Language" />
            <asp:BoundField HeaderText="创建时间" DataField="CreateTime" SortExpression="CreateTime" />
            <asp:TemplateField HeaderText="分数">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/Record/?id="+Eval("ID") %>'>
                        <asp:Literal runat="server" Visible='<%#Eval("JudgeInfo")==null %>'>
                            待评测
                        </asp:Literal>
                        <div runat="server" visible='<%#Eval("JudgeInfo")!=null %>'>
                            <%#Eval("JudgeInfo")==null?0:((JudgeInfo)Eval("JudgeInfo")).Score%>
                        </div>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField HeaderText="操作" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
