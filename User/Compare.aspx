<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Compare.aspx.cs" Inherits="User_Compare" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>比较
        <%#HttpUtility.HtmlEncode(userA.Name) %>
        与
        <%#HttpUtility.HtmlEncode(userB.Name) %></title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:LinkBar runat="server" Title="用户">
        <Moo:LinkBarItem URL="~/User/List.aspx" Text="列表" />
        <Moo:LinkBarItem URL="~/User/Register.aspx" Text="注册" />
        <Moo:LinkBarItem URL='<%#"~/User/Compare.aspx?userA="+userA.ID+"&userB="+userB.ID %>'
            Selected="true" Text="比较" />
    </Moo:LinkBar>
    <fieldset id="fldQuery" runat="server">
        <legend>查询</legend>
        <asp:Label runat="server">用户A编号</asp:Label>
        <asp:TextBox ID="txtUserA" runat="server" Text='<%#userA.ID %>'></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserA" Display="Dynamic"
            CssClass="validator">不能为空</asp:RequiredFieldValidator>
        <asp:CompareValidator runat="server" ControlToValidate="txtUserA" Operator="DataTypeCheck"
            Type="Integer" Display="Dynamic" CssClass="validator">需为整数</asp:CompareValidator>
        <asp:CustomValidator runat="server" ControlToValidate="txtUserA" Display="Dynamic"
            CssClass="validator" OnServerValidate="ValidateUserID">无此用户</asp:CustomValidator>
        <asp:Label runat="server">用户B编号</asp:Label>
        <asp:TextBox ID="txtUserB" runat="server" Text='<%#userB.ID %>'></asp:TextBox>
        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUserB" Display="Dynamic"
            CssClass="validator">不能为空</asp:RequiredFieldValidator>
        <asp:CompareValidator runat="server" ControlToValidate="txtUserB" Operator="DataTypeCheck"
            Type="Integer" Display="Dynamic" CssClass="validator">需为整数</asp:CompareValidator>
        <asp:CustomValidator runat="server" ControlToValidate="txtUserB" Display="Dynamic"
            CssClass="validator" OnServerValidate="ValidateUserID">无此用户</asp:CustomValidator>
        <asp:Button ID="btnQuery" runat="server" Text="查询" OnClick="btnQuery_Click" />
    </fieldset>
    <div style="font-size: 20pt; text-align: center;">
        <Moo:UserSign runat="server" User='<%#userA %>' Style="float: left;" />
        Versus
        <Moo:UserSign runat="server" User='<%#userB %>' Style="float: right;" />
        <div class="clear">
        </div>
    </div>
    <style type="text/css">
        fieldset
        {
            margin: 10px;
        }
    </style>
    <fieldset>
        <legend>仅
            <%#HttpUtility.HtmlEncode(userA.Name) %>
            提交</legend>
        <asp:PlaceHolder ID="holderOnlyA" runat="server"></asp:PlaceHolder>
    </fieldset>
    <fieldset>
        <legend>仅
            <%#HttpUtility.HtmlEncode(userB.Name) %>
            提交</legend>
        <asp:PlaceHolder ID="holderOnlyB" runat="server"></asp:PlaceHolder>
    </fieldset>
    <fieldset>
        <legend>
            <%#HttpUtility.HtmlEncode(userA.Name) %>
            得分比
            <%#HttpUtility.HtmlEncode(userB.Name) %>
            高</legend>
        <asp:PlaceHolder ID="holderAGreaterThanB" runat="server"></asp:PlaceHolder>
    </fieldset>
    <fieldset>
        <legend>
            <%#HttpUtility.HtmlEncode(userB.Name) %>
            得分比
            <%#HttpUtility.HtmlEncode(userA.Name) %>
            高</legend>
        <asp:PlaceHolder ID="holderBGreaterThanA" runat="server"></asp:PlaceHolder>
    </fieldset>
    <fieldset>
        <legend>
            <%#HttpUtility.HtmlEncode(userB.Name) %>
            与
            <%#HttpUtility.HtmlEncode(userA.Name) %>
            得分相同</legend>
        <asp:PlaceHolder ID="holderSame" runat="server"></asp:PlaceHolder>
    </fieldset>
</asp:Content>
