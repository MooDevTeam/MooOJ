<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InfoBlock.ascx.cs" Inherits="InfoBlock" %>
<div id="infoBlockWrap" runat="server" class="infoBlockWrap">
    <img ID="infoBlockImage" src="" runat="server" alt="" class="infoBlockImage" />
    <div class="infoBlockContent">
        <asp:PlaceHolder ID="childrenHolder" runat="server"></asp:PlaceHolder>
    </div>
</div>
<asp:PlaceHolder ID="endMarker" runat="server"></asp:PlaceHolder>
