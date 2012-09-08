<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Log.aspx.cs" Inherits="Log" %>

<asp:Content ContentPlaceHolderID="head" runat="Server">
    <title>日志</title>
</asp:Content>
<asp:Content ContentPlaceHolderID="main" runat="Server">
    <asp:EntityDataSource ID="dataSource" runat="server" ConnectionString="name=MooDB"
        DefaultContainerName="MooDB" EnableDelete="True" EntitySetName="Logs" Include="User">
    </asp:EntityDataSource>
    <asp:GridView ID="grid" runat="server" DataSourceID="dataSource" AutoGenerateColumns="False"
        AllowPaging="True" AllowSorting="True" CellSpacing="-1" CssClass="listTable"
        DataKeyNames="ID">
        <AlternatingRowStyle BackColor="LightBlue" />
        <Columns>
            <asp:BoundField DataField="ID" HeaderText="日志编号" ReadOnly="True" SortExpression="ID" />
            <asp:BoundField DataField="CreateTime" HeaderText="日期" SortExpression="CreateTime"
                 DataFormatString="{0:yyyy-MM-dd hh:mm:ss}" />
            <asp:TemplateField HeaderText="级别" SortExpression="Level">
                <ItemStyle Font-Bold="true" />
                <ItemTemplate>
                    <asp:Literal runat="server" Visible='<%#(byte)Eval("Level")==0 %>'><span style="color:Gray;">调试</span></asp:Literal>
                    <asp:Literal runat="server" Visible='<%#(byte)Eval("Level")==1 %>'><span style="color:Black;">信息</span></asp:Literal>
                    <asp:Literal runat="server" Visible='<%#(byte)Eval("Level")==2 %>'><span style="color:Orange;">警告</span></asp:Literal>
                    <asp:Literal runat="server" Visible='<%#(byte)Eval("Level")==3 %>'><span style="color:Red;">错误</span></asp:Literal>
                    <asp:Literal runat="server" Visible='<%#(byte)Eval("Level")==4 %>'><span style="color:DarkRed;">致命</span></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="信息" SortExpression="Info">
                <ItemTemplate>
                    <div style="text-align: left;">
                        <%#HttpUtility.HtmlEncode(Eval("Info")).Replace("\r\n","\n").Replace("\n","<br/>") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RemoteAddress" HeaderText="客户端地址" SortExpression="RemoteAddress" />
            <asp:TemplateField HeaderText="用户" SortExpression="User.ID">
                <ItemTemplate>
                    <a runat="server" href='<%#"~/User/?id="+Eval("User.ID") %>' visible='<%#Eval("User")!=null %>'>
                        <%#HttpUtility.HtmlEncode(Eval("User.Name")) %>
                    </a>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
