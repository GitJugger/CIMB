<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false"
    CodeFile="pg_SetCutOffTime.aspx.vb" Inherits="MaxPayroll.WebForms_pg_SetCutOffTime" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript" src="../include/common.js"></script>
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">Cut Off Time</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label CssClass="MSG" ID="lblMessage" runat="Server"></asp:Label>
            </td>
        </tr>
    </table>
    <%-- Main table start --%>
    <asp:Table ID="tblForm" runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
        <asp:TableRow>
            <asp:TableCell Width="20%" Wrap="False">
                <asp:Label ID="PaySer_Id" runat="Server" CssClass="LABEL" Text="FileType"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Width="80%">
                <asp:DropDownList ID="__PaySer_Id" runat="server" CssClass="MEDIUMTEXT">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="20%">
                <asp:Label ID="Label1" runat="server" CssClass="LABEL" Text="Cutoff hour(HH):"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Width="80%">
                <asp:DropDownList ID="__Cutoff_Hour" runat="server" CssClass="MEDIUMTEXT">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="20%">
                <asp:Label ID="Label2" runat="server" CssClass="LABEL" Text="Cutoff Minutes(MM):"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Width="80%">
                <asp:DropDownList ID="__Cutoff_Min" runat="server" CssClass="MEDIUMTEXT">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="20%">
                <asp:Label ID="Label3" runat="server" CssClass="LABEL" Text="Cutoff Mode(AM/PM):"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Width="80%">
                <asp:DropDownList ID="__Cutoff_Type" runat="server" CssClass="MEDIUMTEXT">
                    <asp:ListItem Text="AM" Value="AM"></asp:ListItem>
                    <asp:ListItem Text="PM" Value="PM"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="20%">
                <asp:Label ID="lblBank" runat="server" CssClass="LABEL" Text="Select Bank:&nbsp;"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Width="80%">
                <asp:DropDownList ID="__BankId" runat="server" CssClass="MEDIUMTEXT">
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="100%" ColumnSpan="2">
                <asp:Button ID="btnSubmit" runat="server" CssClass="BUTTON" Text="Submit"></asp:Button>&nbsp;
               <%-- <asp:Button ID="btnClear" runat="server" CssClass="BUTTON" Text="Clear"></asp:Button>--%>
                <input id="Reset1" type="reset" runat="server" value="Clear" class="BUTTON"/>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="100%" ColumnSpan="2">
                <!-- Datagrid starts here -->
                <asp:DataGrid ID="dgCutoffTime" runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0"
                    HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" Width="100%"
                    CssClass="GridDivNoScroll" AlternatingItemStyle-CssClass="GridAltItemStyle" AutoGenerateColumns="False"
                    AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
                    BorderWidth="1">
                    <Columns>
                        <asp:BoundColumn DataField="PaySer_Id" Visible="false"></asp:BoundColumn>
                         <asp:BoundColumn DataField="Cutoff_Id" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="PaySer_Desc" HeaderText="Payment_Service"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CutOff_Hour" HeaderText="Cutoff_Hour"></asp:BoundColumn>
                        <asp:BoundColumn DataField="CutOff_Min" HeaderText="Cutoff_Min"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Cutoff_Type" HeaderText="Cutoff_Type"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Cutoff_Identity" HeaderText="Cutoff_Identity" Visible="false">
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="Submission_Type" HeaderText="Submission_Type"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="Modify_Url">Modify</asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <!-- Datagrid Ends here -->
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <%-- Main table stop --%>
   <%-- <asp:HiddenField ID="__Cutoff_Identity" runat="server" />--%>
</asp:Content>
