<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateTime.ascx.cs" Inherits="Controls_DateTime" %>
<div style="background: lightblue; margin: 10px; display: inline-block; padding: 10px;
    border-radius: 10px; text-align: center;">
    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="display: inline-block; margin: auto;">
                <asp:Calendar ID="dateTimeCalendar" runat="server" NextMonthText="下月" PrevMonthText="上月"
                    NextPrevFormat="CustomText" UseAccessibleHeader="true">
                    <NextPrevStyle BorderStyle="None" />
                    <TitleStyle BorderStyle="None" BackColor="WhiteSmoke" />
                    <DayStyle BorderStyle="None" />
                    <TodayDayStyle BackColor="lightblue" />
                    <SelectedDayStyle BackColor="blue" />
                    <DayStyle BackColor="White" BorderStyle="None" />
                    <OtherMonthDayStyle ForeColor="gray" />
                </asp:Calendar>
                <asp:CustomValidator runat="server" CssClass="validator" Display="Dynamic" OnServerValidate="ValidateCalendarSelected">必须选择</asp:CustomValidator>
            </div>
            <div style="margin-top: 10px;">
                <div style="display: inline-block;">
                    <asp:TextBox ID="txtHour" runat="server" MaxLength="2" Columns="2" ViewStateMode="Disabled"></asp:TextBox>
                    <span style="font-weight: bold;">时</span>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtHour" CssClass="validator"
                        Display="Dynamic">不能为空</asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ControlToValidate="txtHour" Operator="DataTypeCheck"
                        Type="Integer" CssClass="validator" Display="Dynamic">须为整数</asp:CompareValidator>
                    <asp:RangeValidator runat="server" ControlToValidate="txtHour" Type="Integer" MinimumValue="0"
                        MaximumValue="23" Display="Dynamic" CssClass="validator">应在0~23之间</asp:RangeValidator>
                </div>
                <div style="display: inline-block;">
                    <asp:TextBox ID="txtMinute" runat="server" MaxLength="2" Columns="2" ViewStateMode="Disabled"></asp:TextBox>
                    <span style="font-weight: bold;">分</span>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMinute" CssClass="validator"
                        Display="Dynamic">不能为空</asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ControlToValidate="txtMinute" Operator="DataTypeCheck"
                        Type="Integer" CssClass="validator" Display="Dynamic">须为整数</asp:CompareValidator>
                    <asp:RangeValidator runat="server" ControlToValidate="txtMinute" Type="Integer" MinimumValue="0"
                        MaximumValue="59" Display="Dynamic" CssClass="validator">应在0~59之间</asp:RangeValidator>
                </div>
                <div style="display: inline-block;">
                    <asp:TextBox ID="txtSecond" runat="server" MaxLength="2" Columns="2" ViewStateMode="Disabled"></asp:TextBox>
                    <span style="font-weight: bold;">秒</span>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSecond" CssClass="validator"
                        Display="Dynamic">不能为空</asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ControlToValidate="txtSecond" Operator="DataTypeCheck"
                        Type="Integer" CssClass="validator" Display="Dynamic">须为整数</asp:CompareValidator>
                    <asp:RangeValidator runat="server" ControlToValidate="txtSecond" Type="Integer" MinimumValue="0"
                        MaximumValue="59" Display="Dynamic" CssClass="validator">应在0~59之间</asp:RangeValidator>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
