<%@ Page Title="重定向……" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Redirect.aspx.cs" Inherits="Special_Redirect" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <div style="text-align: center;">
        <div style="margin: auto; display: inline-block; padding: 10px; min-width: 400px;">
            <Moo:InfoBlock runat="server" Type="Info" Style="margin: auto;">
                <%#info %>
            </Moo:InfoBlock>
            <span style="font-size: 10pt;">等待三秒或<a id="redirectLink" runat="server" href='<%#redirectURL %>'>点击这里</a>将会重定向。
            </span>
            <script type="text/javascript">
                setTimeout(function () { window.location = main_redirectLink.href; }, 3000);
            </script>
        </div>
    </div>
</asp:Content>
