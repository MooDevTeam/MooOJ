<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WikiEditor.ascx.cs" Inherits="Controls_WikiEditor" %>
<asp:UpdatePanel runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <fieldset id="fieldPreview" runat="server" visible="false">
            <legend>预览</legend>
            <asp:Literal ID="litOnlyPreview" runat="server" Text='<%$Resources:Moo,ItsOnlyPreview %>' />
            <div id="divPreview" runat="server">
            </div>
        </fieldset>
        <div class="wikiEditorToolBar">
            <div>
                <a href="javascript:asBold('<%=txtWiki.ClientID %>');" style="font-weight: bold;">B</a>
            </div>
            <div>
                <a href="javascript:asItalic('<%=txtWiki.ClientID %>');" style="font-style: italic;">
                    I</a>
            </div>
            <div>
                <a href="javascript:asUnderline('<%=txtWiki.ClientID %>');" style="text-decoration: underline;">
                    U</a>
            </div>
            <div>
                <a href="javascript:asStrick('<%=txtWiki.ClientID %>');" style="text-decoration: line-through;">
                    S</a>
            </div>
            <div>
                <a href="javascript:asSup('<%=txtWiki.ClientID %>');">x<sup>2</sup></a>
            </div>
            <div>
                <a href="javascript:asSub('<%=txtWiki.ClientID %>');">x<sub>2</sub></a>
            </div>
            <div>
                <a href="javascript:asMath('<%=txtWiki.ClientID %>');">公式</a>
            </div>
            <div>
                <a href="javascript:asNoWiki('<%=txtWiki.ClientID %>');">NoWiki</a>
            </div>
            <div>
                <a href="javascript:asLink('<%=txtWiki.ClientID %>');">链接</a>
            </div>
            <div>
                <select id="selectColor" onchange="changeColor('<%=txtWiki.ClientID %>',this)">
                    <option selected="selected" value="">颜色</option>
                    <option value="red">红色</option>
                    <option value="green">绿色</option>
                    <option value="blue">蓝色</option>
                    <option value="yellow">黄色</option>
                    <option value="cyan">青色</option>
                    <option value="magenta">品红</option>
                    <option value="white">白色</option>
                    <option value="black">黑色</option>
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
                    <option selected="selected" value="">语言高亮</option>
                    <option value="c++">C++</option>
                    <option value="c++">C</option>
                    <option value="pascal">Pascal</option>
                </select>
            </div>
            <div style="float: right;">
                <asp:Button ID="btnPreview" runat="server" CausesValidation="false" Text="预览" OnClick="btnPreview_Click" />
            </div>
            <div class="clear" style="display: block; float: none;">
            </div>
        </div>
        <asp:TextBox ID="txtWiki" runat="server" Width="100%" Rows="20" ViewStateMode="Disabled"
            TextMode="MultiLine"></asp:TextBox>
        <div>
            <img id="wikiSupportedImg" runat="server" src="~/image/OK.png" alt="" />
            <span style="color: Green; font-weight: bold;">WikiSupported</span> <span>此处内容支持<a
                id="wikiSupportedLink" runat="server" href="~/Help/?id=6">Wiki格式</a>。</span>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
