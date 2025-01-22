<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false"
    CodeFile="pg_InfenionBNMCodes.aspx.vb" Inherits="MaxPayroll.WebForms_pg_InfenionBNMCodes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>

    <!--title and error msg table starts here-->
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">BNM Codes</asp:Label></td>
        </tr>
    </table>
    <!--main table starts here-->
    <asp:Table ID="Table1" CellSpacing="0" CellPadding="8" Width="100%" runat="server"
        BorderWidth="0">
        <asp:TableRow>
            <asp:TableCell VerticalAlign="Top">
                <!--tblError starts here-->
                <asp:Table Width="100%" ID="Table3" runat="server" CellPadding="2" CellSpacing="0"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" CssClass="MSG" ID="lblErrorMessage"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--tblError ends here-->
                <!--table1 starts here-->
                <asp:Table Width="100%" ID="Table2" runat="server" CellPadding="2" CellSpacing="0"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell Width="10%">
                            <asp:Label ID="Label1" runat="server" Text="Bank Name" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankName" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                 Width="200" TabIndex="1"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtBankName" Display="None"
                                            ErrorMessage="Bank Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label2" runat="server" Text="Rentas Bank Code" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtRentasBankCode" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="60" Width="200" TabIndex="2"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtRentasBankCode" Display="None"
                                            ErrorMessage="Rentas Bank Code Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^([a-zA-Z0-9_-])$"></asp:RegularExpressionValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label3" runat="server" Text="Rentas BIC" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtRentasBIC" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="60" Width="200" TabIndex="2"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtRentasBIC" Display="None"
                                            ErrorMessage="Rentas BIC Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblGiro" runat="server" Text="GIRO Routing Code" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtGIRORoutingCode" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="60" Width="200" TabIndex="2"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtGIRORoutingCode" Display="None"
                                            ErrorMessage="Routing Code Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblBNM" runat="server" Text="BNM Code" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBNMCode" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="60" Width="200" TabIndex="2"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtBNMCode" Display="None"
                                            ErrorMessage="BNM Code Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <br />
                            <br />
                            <asp:Button ID="btnSearch" runat="server" Text="Submit" Width="100px"></asp:Button>&nbsp;&nbsp;<input
                                type="button" onclick="window.location='pg_InfenionBNMCodes.aspx';" value="Clear"
                                runat="server" class="BUTTON" id="Reset1" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--table1 ends here-->
                <!--tblError starts here-->
                <br />
                <br />
                <asp:Table Width="100%" ID="Table4" runat="server" CellPadding="2" CellSpacing="0"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell>
                            <!--datagrid starts here-->
                            <asp:Panel ID="pnlGrid" CssClass="GridDivNoScroll" runat="server">
                                <asp:DataGrid runat="Server" ID="dgBNMCodes" AllowPaging="True" PageSize="15" PagerStyle-HorizontalAlign="Center"
                                 BorderWidth="0" GridLines="none"  HeaderStyle-CssClass="GridHeaderStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"
                                  ItemStyle-CssClass="GridItemStyle" HeaderStyle-HorizontalAlign="Left" PageIndex="5"
							            AutoGenerateColumns="False" CssClass ="Grid" PagerStyle-Mode="NumericPages">
                                    <Columns>
                                        <asp:BoundColumn DataField="Rec Id" HeaderText="Rec Id" Visible="false"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Bank Name" HeaderText="Bank Code"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Rentas Bank Code" HeaderText="Rentas Bank Code"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Rentas BIC" HeaderText="Rentas BIC"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="GIRO Routing Code" HeaderText="GIRO Routing Code"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="BNM Code" HeaderText="BNM Code"></asp:BoundColumn>
                                        <asp:TemplateColumn HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="hlLink" runat="server" Text="Edit"></asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>                                    
                                </asp:DataGrid>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--datagrid ends here-->
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <!--Main table ends here-->
    <asp:HiddenField ID="hfRecId" runat="server" />
</asp:Content>
