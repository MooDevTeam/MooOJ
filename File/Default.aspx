<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="File_Default" %>

<%@ Import Namespace="Moo.Text" %>
<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>
        <%#HttpUtility.HtmlEncode(file.Name) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="文件">
        <Moo:LinkBarItem URL='<%#"~/File/?id="+file.ID %>' Selected="true" Text="文件" />
        <Moo:LinkBarItem URL='<%#"~/File/Download.ashx?id="+file.ID %>' Text="下载" />
    </Moo:LinkBar>
    <h1>
        <%#HttpUtility.HtmlEncode(file.Name) %></h1>
    <table class="detailTable">
        <tr>
            <th>
                创建者
            </th>
            <td>
                <a runat="server" href='<%#"~/User/?id="+file.CreatedBy.ID %>'>
                    <%#HttpUtility.HtmlEncode(file.CreatedBy.Name) %>
                </a>
            </td>
        </tr>
        <tr>
            <th>
                下载
            </th>
            <td>
                <a runat="server" href='<%#"~/File/Download.ashx?id="+file.ID %>'>下载文件</a>
            </td>
        </tr>
        <tr>
            <th>
                描述
            </th>
            <td>
                <div>
                    <%#WikiParser.Parse(file.Description)%>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
