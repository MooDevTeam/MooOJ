<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserSign.ascx.cs" Inherits="UserSign" %>
<%@ Import Namespace="Moo.Authorization" %>
<div runat="server" id="signWrap" class='<%#Vertical?"signWrapVertical":"signWrap" %>'>
    <asp:Literal ID="signRememberEmail" runat="server" Text='<%#user.Email %>' Visible="false"></asp:Literal>
    <ajax:Gravatar ID="signImage" CssClass="signImage" runat="server" DefaultImageBehavior="MysteryMan" Email='<%#signRememberEmail.Text %>' Size='<%#Vertical?120:80 %>'/>
    <div class="signRight">
        <div class="signName">
            <a runat="server" class="signNameLink" href='<%#"~/User/?id="+user.ID %>' title='<%#user.Role.DisplayName %>'>
                <%#HttpUtility.HtmlEncode(user.Name) %>
            </a>
        </div>
        <div class="signDescription">
            <%#HttpUtility.HtmlEncode(user.BriefDescription) %>
        </div>
        <div class="signBottomWrap">
            <div runat="server" class="signControl" id="signControlSelf">
                <a runat="server" href='<%#"~/Special/Logout.ashx?token="+(HttpContext.Current.User.Identity.IsAuthenticated?((SiteUser)HttpContext.Current.User.Identity).Token:0)%>'>
                    登出</a>
            </div>
            <div runat="server" class="signControl" id="signControlOther">
                <a runat="server" href='<%#"~/Mail/Create.aspx?to="+user.ID %>'>发送消息</a>
            </div>
            <div class="signScore">
                <%#user.Score %>
            </div>
        </div>
    </div>
</div>
