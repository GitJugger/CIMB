<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_BankFormat"
    CodeFile="PG_BankFormat.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master"
    EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <!-- Main Table Starts Here -->
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">Bank File Settings</asp:Label></td>
        </tr>
    </table>
    <table width="100%" cellpadding="2" cellspacing="0">
        <tr>
            <td height="5px">
            </td>
        </tr>
        <tr>
            <td width="100%">
                <!-- Form Table Starts Here -->
                <table width="100%" cellpadding="2" cellspacing="0" border="0">
                    <tr>
                        <td nowrap width = "20%">
                            <asp:Label ID="lblBank" runat="Server" Text="Select Bank" CssClass="LABEL"></asp:Label></td>
                        <td width="40%">
                            <asp:DropDownList ID="cmbBank" CssClass="MEDIUMTEXT" runat="Server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td width="50%"></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFileType" runat="Server" Text="Select File Type" CssClass="LABEL"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="cmbType" CssClass="MEDIUMTEXT" runat="Server" AutoPostBack="true">
                            </asp:DropDownList></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="height: 28px">DB Table Name</td>
                        <td style="height: 28px">
                            <asp:TextBox ID="txtDBTableName" runat="server"></asp:TextBox>&nbsp;</td>
                        <td style="height: 28px"></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtInboundFolder" runat="server" Visible="False"></asp:TextBox></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtOutboundFolder" runat="server" Visible="False"></asp:TextBox></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtResponseFolder" runat="server" Visible="False"></asp:TextBox></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button runat="Server" Text="Update General Info" ID="btnUpdateGeneral" ValidationGroup="vgDBTableName" Visible="False">
                            </asp:Button>
                            &nbsp;<asp:Button runat="Server" CausesValidation="False" Text="Create New Field"
                                ID="btnNew"></asp:Button>
                            &nbsp;<asp:Button ID="btnCreate" runat="server" Text="Create DB Table" ValidationGroup="vgDBTableName">
                            </asp:Button>
                            &nbsp;<asp:Button runat="Server" ID="btnShow" Text="Display All Fields"></asp:Button>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br>
                            <asp:RequiredFieldValidator
                                ID="rfvDBTableName" runat="server" Display="Dynamic" ErrorMessage="Please key in DB Table Name"
                                ValidationGroup="vgDBTableName" ControlToValidate="txtDBTableName" SetFocusOnError="True"></asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <!-- Datagrid Starts Here -->
                            <div>
                            </div>
                            <asp:DataGrid runat="Server" ID="dgBank" AllowPaging="True" PageSize="20" PagerStyle-Mode="NumericPages"
                                PagerStyle-HorizontalAlign="Center" BorderWidth="0" GridLines="none" CellPadding="3"
                                CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" Width="100%" HeaderStyle-Font-Bold="True"
                                HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False" HeaderStyle-CssClass="GridHeaderStyle"
                                AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle"
                                OnPageIndexChanged="Page_Change">
                                <Columns>
                                    <asp:BoundColumn DataField="FId" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FLType" Visible="false"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FDesc" HeaderText="Field Description" HeaderStyle-Width="200"
                                        HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FStart" HeaderText="Start Position" HeaderStyle-Width="125"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FEnd" HeaderText="End Position" HeaderStyle-Width="125"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                      <asp:BoundColumn DataField="ColPos" HeaderText="Col Position" HeaderStyle-Width="125"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="FType" HeaderText="Content Type" HeaderStyle-Width="125"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <%-- <asp:BoundColumn DataField="MField" HeaderText="Matching Field" HeaderStyle-Width="125"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>--%>
                                        <asp:BoundColumn DataField="IsMandatory" HeaderText="Mandatory" HeaderStyle-Width="125"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="IsCustomerSetting" HeaderText="Customer Setting" HeaderStyle-Width="125"
                                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="Select" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hlLink" runat="server">Modify/Delete</asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                            <!-- Datagrid Ends Here -->
                        </td>
                    </tr>
                </table>
                <!-- Form Table Ends Here -->
            </td>
        </tr>
    </table>
    <!-- Main Table Ends Here -->
</asp:Content>
