<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Security.aspx.cs" Inherits="Special_Security" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>安全问题</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <h3 style="color: Red;">
        由于安全问题，您被迫登出。
        这<span style="font-weight:bold;">有可能</span>是由于您的账号被其他人登录所致。
        请定期更改您的密码，养成良好的安全习惯。
        您可以选择<a runat="server" href="~/Special/Login.aspx">重新登录</a>。
    </h3>
</asp:Content>
