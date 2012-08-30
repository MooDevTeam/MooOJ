<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="TestCase_Default" %>

<%@ Import Namespace="Moo.Utility" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(testCase.Problem.Name) %>
        的测试数据</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar ID="linkbar" runat="server" Title="测试数据">
        <Moo:LinkBarItem URL='<%#"~/TestCase/List.aspx?id="+problem.ID %>' Text="列表" />
        <Moo:LinkBarItem URL='<%#"~/TestCase/?id="+testCase.ID %>' Selected="true" Text="测试数据" />
        <Moo:LinkBarItem URL='<%#"~/Problem/?id="+problem.ID %>' Special="true" Text="题目" />
        <Moo:LinkBarItem URL='<%#"~/Solution/?id="+problem.ID %>' Special="true" Text="题解" />
        <Moo:LinkBarItem URL='<%#"~/Post/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="帖子" />
        <Moo:LinkBarItem URL='<%#"~/Record/List.aspx?problemID="+problem.ID %>' Special="true"
            Text="记录" />
    </Moo:LinkBar>
    <asp:MultiView ID="multiView" runat="server">
        <asp:View ID="viewTranditional" runat="server">
            <table class="detailTable">
                <tr>
                    <th>
                        题目
                    </th>
                    <td>
                        <a runat="server" href='<%#"~/Problem/?id="+problem.ID %>'>
                            <%#HttpUtility.HtmlEncode(problem.Name) %>
                        </a>
                    </td>
                </tr>
                <tr>
                    <th>
                        类型
                    </th>
                    <td>
                        Tranditional
                    </td>
                </tr>
                <tr>
                    <th>
                        分数
                    </th>
                    <td>
                        <%#asTranditional.Score %>
                    </td>
                </tr>
                <tr>
                    <th>
                        时间限制
                    </th>
                    <td>
                        <%#asTranditional.TimeLimit%>
                        ms
                    </td>
                </tr>
                <tr>
                    <th>
                        内存限制
                    </th>
                    <td>
                        <%#asTranditional.MemoryLimit %>
                        byte(s)
                    </td>
                </tr>
                <tr>
                    <th>
                        输入
                    </th>
                    <td>
                        <pre><%#HttpUtility.HtmlEncode(Encoding.UTF8.GetString(asTranditional.Input,0,Math.Min(DISPLAY_LENGTH,asTranditional.Input.Length)))%></pre>
                        <a runat="server" href='<%#"~/TestCase/Download.ashx?id="+testCase.ID+"&field=Input" %>'>
                            下载完整输入</a>
                    </td>
                </tr>
                <tr>
                    <th>
                        答案
                    </th>
                    <td>
                        <pre><%#HttpUtility.HtmlEncode(Encoding.UTF8.GetString(asTranditional.Answer,0,Math.Min(DISPLAY_LENGTH,asTranditional.Answer.Length)))%></pre>
                        <a runat="server" href='<%#"~/TestCase/Download.ashx?id="+testCase.ID+"&field=Answer" %>'>
                            下载完整答案</a>
                    </td>
                </tr>
            </table>
        </asp:View>
        <asp:View ID="viewSpecialJudged" runat="server">
            <table class="detailTable">
                <tr>
                    <th>
                        题目
                    </th>
                    <td>
                        <a runat="server" href='<%#"~/Problem/?id="+problem.ID %>'>
                            <%#HttpUtility.HtmlEncode(problem.Name) %>
                        </a>
                    </td>
                </tr>
                <tr>
                    <th>
                        类型
                    </th>
                    <td>
                        SpecialJudged
                    </td>
                </tr>
                <tr>
                    <th>
                        分数
                    </th>
                    <td>
                        <%#asSpecialJudged.Score %>
                    </td>
                </tr>
                <tr>
                    <th>
                        时间限制
                    </th>
                    <td>
                        <%#asSpecialJudged.TimeLimit%>
                        ms
                    </td>
                </tr>
                <tr>
                    <th>
                        内存限制
                    </th>
                    <td>
                        <%#asSpecialJudged.MemoryLimit %>
                        byte(s)
                    </td>
                </tr>
                <tr>
                    <th>
                        测评程序
                    </th>
                    <td>
                        <a runat="server" href='<%#"~/File/?id="+asSpecialJudged.Judger.ID %>'>
                            <%#HttpUtility.HtmlEncode(asSpecialJudged.Judger.Name) %>
                        </a>
                    </td>
                </tr>
                <tr>
                    <th>
                        输入
                    </th>
                    <td>
                        <pre><%#HttpUtility.HtmlEncode(Encoding.UTF8.GetString(asSpecialJudged.Input,0,Math.Min(DISPLAY_LENGTH,asSpecialJudged.Input.Length)))%></pre>
                        <a runat="server" href='<%#"~/TestCase/Download.ashx?id="+testCase.ID+"&field=Input" %>'>
                            下载完整输入</a>
                    </td>
                </tr>
                <tr>
                    <th>
                        答案
                    </th>
                    <td>
                        <pre><%#HttpUtility.HtmlEncode(Encoding.UTF8.GetString(asSpecialJudged.Answer,0,Math.Min(DISPLAY_LENGTH,asSpecialJudged.Answer.Length)))%></pre>
                        <a runat="server" href='<%#"~/TestCase/Download.ashx?id="+testCase.ID+"&field=Answer" %>'>
                            下载完整答案</a>
                    </td>
                </tr>
            </table>
        </asp:View>
    </asp:MultiView>
</asp:Content>
