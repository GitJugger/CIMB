<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_FileList"
    CodeFile="PG_FileList.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>

    <script language="JavaScript">
           	function fncBack()
		{
			window.location.href = "PG_FileList.aspx";
		}
		function fncClear()
		{
			window.location.href = "PG_FileList.aspx";
		}
    </script>

    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader" width="45%" height="18">
                <asp:Label Text="File Review" CssClass="FORMHEAD" runat="server" ID="lblHeading"></asp:Label></td>
                <td align="left">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Include/Images/mozilla_blu.gif" />&nbsp;<font color="gray">Loading</font>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="Server" CssClass="MSG"></asp:Label></td>
        </tr>
    </table>
    <!-- Main Table Starts Here -->
    <asp:Table Width="100%" ID="tblMain" CellPadding="2" CellSpacing="0" runat="Server">
        <asp:TableRow>
            <asp:TableCell Width="100%">
                <!--Form Table Starts Here -->
                <asp:Table Width="100%" ID="tblForm" CellPadding="2" CellSpacing="5" runat="Server">
                    <asp:TableRow>
                        <asp:TableCell Width="100%">
                            <!-- Data Grid Starts here -->
                            <asp:Panel runat="server" ID="dvGridScroll" CssClass="GridDiv" Width="580px">
                                <asp:UpdatePanel ID="updpnl_Form" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:DataGrid ID="dgFile" runat="Server" AllowSorting="False" AutoGenerateColumns="False"
                                            PagerStyle-Mode="NumericPages" GridLines="none" PagerStyle-HorizontalAlign="Center"
                                            BorderColor="black" BorderWidth="0" Font-Names="Verdana" Font-Size="8pt" CssClass="Grid"
                                            HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"
                                            ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" CellPadding="4">
                                            <Columns>
                                                <asp:TemplateColumn HeaderText="" HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="Server" AutoPostBack="true" ></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:TemplateColumn HeaderText="File Type" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:Label Visible="false" ID="lblFileType" runat="server"><%#DataBinder.Eval(Container.DataItem,"FType")%></asp:Label>
                                                        <asp:Label ID="lblaFileType" runat="server"><%#DataBinder.Eval(Container.DataItem,"FType")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                <asp:BoundColumn DataField="FId" HeaderText="File Id" HeaderStyle-Width="15%" Visible="false">
                                                </asp:BoundColumn>                                  
                                                <asp:BoundColumn DataField="FType" HeaderText="File Type" HeaderStyle-Width="15%"
                                                    Visible="False"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="FName" HeaderText="File Name" HeaderStyle-Width="15%"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="VDate" HeaderText="Payment Date" HeaderStyle-Width="15%">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="WfId" HeaderText="WorkFlow Id" HeaderStyle-Width="15%"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="False">
                                                </asp:BoundColumn>
                                                <asp:BoundColumn DataField="Date" HeaderText="Upload Date" HeaderStyle-Width="15%"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="FTrans" HeaderText="Transaction" HeaderStyle-Width="10%"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                <asp:BoundColumn DataField="FAmount" HeaderText="Total Amount" HeaderStyle-Width="10%"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                                                <asp:TemplateColumn HeaderText="Remarks" HeaderStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtRemarks" runat="Server" CssClass="MEDIUMTEXT" TextMode="MultiLine"
                                                            Rows="3" MaxLength="255"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateColumn>
                                                 <asp:BoundColumn DataField="FileType_Id" HeaderText="FileType_Id" HeaderStyle-Width="15%" Visible="false">
                                                </asp:BoundColumn> 
                                                 <asp:BoundColumn DataField="PaySer_Id" HeaderText="PaySer_Id" HeaderStyle-Width="15%" Visible="false">
                                                </asp:BoundColumn>  
                                            </Columns>
                                        </asp:DataGrid>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnCheckAll" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnUnCheck" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </asp:Panel>
                            <!-- Data Grid Ends Here -->
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="100%">
                            <asp:TextBox BorderWidth="0" Width="100%" Height="100" BorderStyle="None" ID="txtalert"
                                runat="server" CssClass="MSG" ReadOnly="True" TextMode="MultiLine"></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                            <asp:Button ID="btnCheckAll" runat="server" Text="Select All" CssClass="BUTTON"></asp:Button>&nbsp;
                            <asp:Button ID="btnUnCheck" runat="server" Text="Unselect All" CssClass="BUTTON"></asp:Button>&nbsp;
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trAuthCode">
                        <asp:TableCell Width="90%">
                            <asp:Label runat="Server" CssClass="LABEL" Text="Enter Validation Code"></asp:Label>&nbsp;
                            <asp:TextBox ID="txtAuthCode" CssClass="BIGTEXT" runat="Server" TextMode="Password"
                                MaxLength="24"></asp:TextBox>&nbsp;
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trSubmit">
                        <asp:TableCell Width="90%" ColumnSpan="2">
                            <asp:Button ID="btnAccept" CssClass="BUTTON" runat="Server" Text="Accept"></asp:Button>&nbsp;
                            <asp:Button ID="btnReject" CssClass="BUTTON" runat="Server" Text="Reject"></asp:Button>&nbsp;
                            <input type="button" id="btnReset" name="btnReset" runat="Server" value="Clear" class="BUTTON"
                                onclick="fncClear();">
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trConfirm">
                        <asp:TableCell Width="90%" ColumnSpan="2">
                            <asp:Button ID="btnConfirm" runat="Server" CssClass="BUTTON" Text="Confirm"></asp:Button>&nbsp;
                            <input type="button" id="btnBack" name="btnBack" runat="Server" value="Back" class="BUTTON"
                                onclick="fncBack();">
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!-- Form Table Ends Here -->
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <input id="hdncommand" type="hidden" name="hdndcommand" runat="Server">
    <input id="hdnpymtdt" type="hidden" name="hdnpymtdt" runat="Server">
    <input id="hdnTAmount" type="hidden" name="hdnTAmount" runat="Server">
    <asp:RequiredFieldValidator ID="rfvAuth" runat="Server" ControlToValidate="txtAuthCode"
        ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgeAuthCode" runat="server" ControlToValidate="txtAuthCode"
        ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters"
        Display="None"></asp:RegularExpressionValidator>
    <asp:ValidationSummary runat="Server" ID="vsFileReview" EnableClientScript="True"
        ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations,">
    </asp:ValidationSummary>
    <!-- Main Table Ends Here -->
</asp:Content>
