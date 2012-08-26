<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserSign.ascx.cs" Inherits="UserSign" %>
<div runat="server" id="signWrap" class='<%#Vertical?"signWrapVertical":"signWrap" %>'>
    <img class="signImage" runat="server" src="<%#HttpUtility.HtmlAttributeEncode(user.ImageURL)%>"
        alt="" />
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
                <a runat="server" href="~/Special/Logout.ashx">登出</a>
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
