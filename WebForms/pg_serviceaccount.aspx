<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ServiceAccount" CodeFile="PG_ServiceAccount.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

      <script type="text/javascript" src="../include/common.js"></script>
    <script language="JavaScript">
		function fncBack()
		{
			//var strRequest = document.Form1.hRequest.value;
			//window.location.href = "PG_ViewOrganisation.aspx?Req=" + strRequest;
			window.history.back();
		}
    </script>
    
    
		<!-- Main Table Starts Here -->
		<table cellpadding="5" cellspacing="0" width="100%" border="0">
                <tr>
                    <td id="cHeader"><asp:Label ID="lblHeading" CssClass="FORMHEAD" Runat="Server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
                </tr>
                <tr>
                    <td id="Td2" height=5></td>
                </tr>
         </table>
		<!-- Main Table Ends Here -->
		<!-- Form Table Starts Here -->
		<asp:Table Width="100%" ID="tblForm" CellPadding="8" CellSpacing="0" Runat="Server">
			<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="3">
			<asp:panel ID="pnlGrid" runat=server CssClass ="GridDivNoScroll">
			<asp:DataGrid cssclass = grid ID="dgServiceAccount" Runat="Server" AllowPaging="False" AllowSorting="False" AutoGenerateColumns="False" DataKeyField="SRVID"
				PageSize="15" GridLines="Both" PagerStyle-HorizontalAlign="Center" BorderWidth="0"
				HeaderStyle-CssClass="GridHeaderStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle" Width="100%" HeaderStyle-HorizontalAlign="left" 
				OnItemDataBound="dgServiceAccts_ItemDataBound" OnDeleteCommand="dgServiceAccts_Delete">
				<Columns>
					<asp:BoundColumn Datafield="SRVID" HeaderText="" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="No."  >
						<ItemTemplate>
							<asp:Label ID="lblNo" Runat="Server" CssClass="LABEL"></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Description"  >
						<ItemTemplate>
							<asp:TextBox ID="txtServName" Runat="Server" CssClass="LARGETEXT" MaxLength="50" Text='<%# Container.DataItem("SRVNAME")%>'></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvServName" Runat="Server" Display="None" ControlToValidate="txtServName" ErrorMessage="Enter Description"></asp:RequiredFieldValidator>
						 <asp:RegularExpressionValidator
                                            ID="revServName" runat="server" ControlToValidate="txtServName" Display="None"
                                            ErrorMessage="Serv Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Employer No."  >
						<ItemTemplate>
							<asp:TextBox ID="txtServAcc" Runat="Server" CssClass="SMALLTEXT" MaxLength="10" Text='<%# Container.DataItem("SRVNO")%>'></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvServAcc" Runat="Server" Display="None" ControlToValidate="txtServAcc" ErrorMessage="Enter Employer No."></asp:RequiredFieldValidator>
						 <asp:RegularExpressionValidator
                                            ID="revServAcc" runat="server" ControlToValidate="txtServAcc" Display="None"
                                            ErrorMessage="ServAcc Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </ItemTemplate>
					</asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Employer Name"  >
						<ItemTemplate>
							<asp:TextBox ID="txtEmpName" Runat="Server" CssClass="SMALLTEXT" MaxLength="10" Text='<%# Container.DataItem("EMPNAME")%>'></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvEmpName" Runat="Server" Display="None" ControlToValidate="txtEmpName" ErrorMessage="Enter Employer Name"></asp:RequiredFieldValidator>
						 <asp:RegularExpressionValidator
                                            ID="revServEmpName" runat="server" ControlToValidate="txtEmpName" Display="None"
                                            ErrorMessage="Emp Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Select State"  >
						<ItemTemplate>
							<input type="hidden" id="hState" name="hState" runat="server" value='<%# Container.DataItem("SRVST") %>' />
							<asp:DropDownList ID="cmbState" CssClass="LABEL" Runat="server"></asp:DropDownList>
							<asp:RangeValidator ID="rgvState" MinimumValue="1" MaximumValue="30" Type="Integer" Runat="server" ErrorMessage="Select State" ControlToValidate="cmbState" Display="None"></asp:RangeValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Datafield="SRTEST" HeaderText="Status" ></asp:BoundColumn>
					<asp:ButtonColumn Text="Delete" ButtonType="PushButton" CommandName="Delete"  HeaderText="Action" ItemStyle-Wrap="false"></asp:ButtonColumn>
				</Columns>
			</asp:DataGrid>
			</asp:panel>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="3">
				<asp:button runat="server" CssClass="BUTTON" ID="btnBack" Text="Back to View" CausesValidation="false"></asp:button>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
		<input type="hidden" runat="server" id="hAction" name="hAction"/>
		<input type="hidden" runat="server" id="hOption" name="hOption"/>
		<input type="hidden" id="hStatus" name="hStatus" runat="server"/>
		<input type="hidden" id="hRequest" name="hRequest" runat="server"/>
			<asp:ValidationSummary Runat="Server" ShowMessageBox="True" ShowSummary="False" ID="vsServiceaccount" EnableClientScript="TRUE" HeaderText="Please Incorporate The Below Validations,"></asp:ValidationSummary>
	<!-- Form Table Ends Here -->
		
</asp:Content>