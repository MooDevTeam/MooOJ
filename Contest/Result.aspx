<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Result.aspx.cs" Inherits="Contest_Result" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(contest.Title) %>
        的结果</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="比赛">
        <Moo:LinkBarItem URL='<%#"~/Contest/?id="+contest.ID %>' Text="比赛" />
        <Moo:LinkBarItem URL='<%#"~/Contest/Modify.aspx?id="+contest.ID %>' Text="修改" />
        <Moo:LinkBarItem URL='<%#"~/Contest/Result.aspx?id="+contest.ID %>' Selected="true"
            Text="结果" />
    </Moo:LinkBar>
    
    <table class="detailTable">
        <tr>
            <th>
                报名人数
            </th>
            <td>
                <%#contest.User.Count %>
            </td>
        </tr>
        <tr>
            <th>
                平均分
            </th>
            <td>
                <%#averageScore %>
            </td>
        </tr>
        <tr>
            <th>
                排名
            </th>
            <td>
                <asp:GridView ID="grid" runat="server" AllowPaging="true" CssClass="listTable" AutoGenerateColumns="False"
                    CellSpacing="-1" OnPageIndexChanging="grid_PageIndexChanging" EmptyDataText='<%$ Resources:Moo,EmptyDataText %>' PageSize='<%$Resources:Moo,GridViewPageSize %>'>
                    <AlternatingRowStyle BackColor="LightBlue" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="用户编号" SortExpression="ID" />
                        <asp:TemplateField HeaderText="名称">
                            <ItemTemplate>
                                <a runat="server" href='<%#"~/User/?id="+Eval("ID") %>'>
                                    <%#HttpUtility.HtmlEncode(Eval("Name")) %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="总分" SortExpression="Score">
                            <ItemTemplate>
                                <a runat="server" href='<%#"~/Record/List.aspx?contestID="+contest.ID+"&userID="+Eval("ID") %>'>
                                    <%#Eval("Score") %>
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    
</asp:Content>
