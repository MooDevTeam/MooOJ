<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LinkBar.ascx.cs" Inherits="Controls_LinkBar" %>
<div style="text-align: center;">
    <div class="linkbarTitle">
        <%#Title %>
    </div>
    <div class="linkbarWrap">
        <asp:Repeater runat="server" DataSource='<%#Items %>'>
            <ItemTemplate>
                <div runat="server" class='<%#(bool)Eval("Selected")?"linkbarItemWrapSelected":
                                          (bool)Eval("Special")?"linkbarItemWrapSpecial":
                                          (bool)Eval("Shortcut")?"linkbarItemWrapShortcut":
                                          "linkbarItemWrap" %>' visible='<%#!(bool)Eval("Hidden") %>'>
                    <a href='<%#Eval("URL") %>' runat="server" class="linkbarLink">
                        <%#Eval("Text") %>
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div class="clear">
        </div>
    </div>
</div>
