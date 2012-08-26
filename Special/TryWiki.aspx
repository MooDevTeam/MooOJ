<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="TryWiki.aspx.cs" Inherits="Special_TryWiki" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>Wiki演练场</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <h1>
        Wiki演练场
    </h1>
    
    <table class="detailTable">
        <tr>
            <th>
                效果
            </th>
            <td>
                <div id="divShow" runat="server" enableviewstate="false">
                    <%= Page.IsPostBack?Moo.Text.WikiParser.Parse(txtWiki.Text):"请在下方输入Wiki源码，单击预览按钮，这里就会显示最终效果。"%>
                </div>
            </td>
        </tr>
        <tr>
            <th>
                源码
            </th>
            <td>
                <asp:TextBox ID="txtWiki" runat="server" TextMode="MultiLine" Rows="25" Width="100%"
                    EnableViewState="false"></asp:TextBox>
                <Moo:WikiSupported runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center;">
                <asp:Button ID="btnSubmit" runat="server" Text="预览" />
            </td>
        </tr>
    </table>
    
</asp:Content>
