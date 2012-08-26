<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Captcha.ascx.cs" Inherits="Controls_Captcha" %>
<%@ Import Namespace="Moo.Authorization" %>
<asp:UpdatePanel ID="updateCaptcha" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div style="color: Green;" runat="server" visible='<%#Permission.Check("captcha.skip",false,false) %>'>
            根据您的权限，验证码已略过。
        </div>
        <div runat="server" visible='<%#!Permission.Check("captcha.skip",false,false) %>'>
            <asp:Image ID="imgCaptcha" runat="server" Width="200px" Height="100px"/><br />
            <asp:TextBox ID="txtCaptcha" runat="server" MaxLength="5" ViewStateMode="Disabled"></asp:TextBox>
            <asp:LinkButton ID="btnChange" runat="server" CausesValidation="false" OnClick="btnChange_Click">换一个</asp:LinkButton>
            <asp:CustomValidator ID="validateCaptcha" runat="server" ControlToValidate="txtCaptcha"
                ValidateEmptyText="true" OnServerValidate="Validate" CssClass="validator" Display="Dynamic"><br/>验证码不对啊！</asp:CustomValidator>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<%-- 
<%@ Import Namespace="Moo.Authorization" %>
<div style="font-size: 10pt; border-radius: 10px; background: lightblue;
    padding: 10px;">
    请将下面文字还原为正确语序。<span style="font-size:8pt;">(由于长期使用所造成的一切后遗症，本插件概不负责)</span><br />
    例如：<span style="font-weight: bold; font-size: 12pt;">“今天气天好！真”</span>可以还原为<span style="font-weight: bold;
        font-size: 12pt;">“今天天气真好！”</span> <span style="font-weight: bold; color: Green;"
            runat="server" visible='<%#Permission.Check("captcha.answer.read",false,false) %>'>
            根据您的权限已自动填充正确答案(*^__^*) </span>
    <br />
    <asp:TextBox ID="txtCaptcha" runat="server" ViewStateMode="Disabled" Width="100%"></asp:TextBox>
    <div style="text-align: center;">
        <asp:Button ID="btnShowCaptchaAnswer" runat="server" Text="Show Answer Debug Only"
            CausesValidation="false" OnClick="btnShowCaptchaAnswer_Click" />
        <asp:Button ID="btnChangeCaptcha" runat="server" Text="换一个" CausesValidation="false"
            OnClick="btnChangeCaptcha_Click" />
        <asp:CustomValidator runat="server" ValidateEmptyText="true" OnServerValidate="Validate"
            Display="Dynamic" CssClass="validator" Style="font-size: 12pt;">语序不正确</asp:CustomValidator>
    </div>
</div>
--%>