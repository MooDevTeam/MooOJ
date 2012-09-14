<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WikiEditor.ascx.cs" Inherits="Controls_WikiEditor" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div id="divPreviewWrapper" runat="server" visible="false">
            <asp:Literal ID="litOnlyPreview" runat="server" Text='<%$Resources:Moo,ItsOnlyPreview %>' />
            <div id="divPreview" runat="server">
            </div>
        </div>
        <div class="wikiEditorToolBar">
            <div>
                <a href="javascript:toBold('<%=txtWiki.ClientID %>');" style="display: inline-block;
                    font-weight: bold;">B</a>
            </div>
            <div>
                <a href="javascript:toItalic('<%=txtWiki.ClientID %>');" style="display: inline-block;
                    font-style: italic;">I</a>
            </div>
            <div>
                <a href="javascript:toUnderline('<%=txtWiki.ClientID %>');" style="display: inline-block;
                    text-decoration: underline;">U</a>
            </div>
            <div>
                <select id="selectColor" onchange="changeColor('<%=txtWiki.ClientID %>',this)">
                    <option selected="selected" value="">颜色</option>
                    <option value="red">红色</option>
                    <option value="green">绿色</option>
                    <option value="blue">蓝色</option>
                </select>
            </div>
            <div>
                <select id="selectTitle" onchange="asTitle('<%=txtWiki.ClientID %>',this)">
                    <option selected="selected" value="">标题</option>
                    <option value="!!">二级标题</option>
                    <option value="!!!">三级标题</option>
                    <option value="!!!!">四级标题</option>
                    <option value="!!!!!">五级标题</option>
                    <option value="!!!!!!">六级标题</option>
                </select>
            </div>
            <div>
                <select id="selectSourceCode" onchange="asSourceCode('<%=txtWiki.ClientID %>',this)">
                    <option selected="selected" value="">源码</option>
                    <option value="c++">C++</option>
                    <option value="c++">C</option>
                    <option value="pascal">Pascal</option>
                </select>
            </div>
            <div class="clear" style="display: block; float: none;">
            </div>
        </div>
        <asp:TextBox ID="txtWiki" runat="server" Width="100%" Rows="20" ViewStateMode="Disabled"
            TextMode="MultiLine">Test Text.</asp:TextBox>
        <div style="text-align: center;">
            <div style="float: left;">
                <img id="wikiSupportedImg" runat="server" src="~/image/OK.png" alt="" />
                <span style="color: Green; font-weight: bold;">WikiSupported</span> <span>此处内容支持<a
                    id="wikiSupportedLink" runat="server" href="~/Help/?id=6">Wiki格式</a>。</span>
            </div>
            <div style="float: right;">
                <asp:Button ID="btnPreview" runat="server" Text="预览" OnClick="btnPreview_Click" />
            </div>
            <div class="clear">
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
