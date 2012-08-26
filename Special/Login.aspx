<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Special_Login"
    MasterPageFile="~/MasterPage.master" Title="" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>登录</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <Moo:InfoBlock runat="server" Type="Error" Visible='<%#Request["noPermission"]!=null %>'>
        您不具备完成此操作的权限，请更换更高权限的账户。
    </Moo:InfoBlock>
    <h1>登录</h1>
    <asp:LoginView ID="loginView" runat="server">
        <LoggedInTemplate>
            <Moo:InfoBlock runat="server" Type="Alert">
                对不起!您不能重复登录。是否需要<a href="Logout.ashx">登出</a>？
            </Moo:InfoBlock>
        </LoggedInTemplate>
        <AnonymousTemplate>
            
            <div>
                <table class="detailTable">
                    <tr>
                        <th>
                            用户名
                        </th>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" Width="100%"></asp:TextBox>
                            <asp:CustomValidator ID="validateUserName" runat="server" OnServerValidate="ValidateUserName"
                                Display="Dynamic" CssClass="validator">用户名不存在</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            密码
                        </th>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="100%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            选项
                        </th>
                        <td>
                            <asp:CheckBox ID="chkPersistent" runat="server" Text="记住我" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            验证码
                        </th>
                        <td>
                            <Moo:Captcha runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center;">
                            <asp:Button ID="btnSubmit" runat="server" Text="登录" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            
        </AnonymousTemplate>
    </asp:LoginView>
</asp:Content>
