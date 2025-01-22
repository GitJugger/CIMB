<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_GroupChange" CodeFile="PG_GroupChange.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
        
    		<script type="text/javascript" src="../include/common.js"></script>
   		
         <table cellpadding="5" cellspacing="0" width="100%" border="0">
            <tr>
               <td id="cHeader">
                  <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Group Change" ID="lblHeading"></asp:Label>
               </td>
            </tr>
            <tr>
               <td>
                  <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
                  <br /><asp:Button ID="btnSignOut" runat="server" CssClass="BUTTON" Text="Logout" Visible="false" />
               </td>
            </tr>
         </table> 
				 <!-- Main Table Starts Here -->
				
				 <asp:Table ID="tblMain" Runat="Server" Width="100%" CellPadding="2" CellSpacing="1" BorderWidth="0">				
						<asp:TableRow>
							<asp:TableCell Width="100%">
								<!-- Form Table Starts Here -->
								<asp:Table Width="100%" CellPadding="2" CellSpacing="1" Runat="Server" ID="tblForm1" BorderWidth="0">
									<asp:TableRow>
										<asp:TableCell>
											<asp:Label ID="lblUserName" CssClass="BLABEL" Runat="Server"></asp:Label>
										</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell>
											<asp:Label Runat="Server" CssClass="LABEL" Text="You belong to these groups. Please select the group you would like to access."></asp:Label>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<!-- Form Table Ends Here -->
								
								<!-- 2nd Form Table Starts Here -->
								<asp:Table Width="100%" CellPadding="2" CellSpacing="1" Runat="Server" ID="tblForm2" BorderWidth="0">
									<asp:TableRow>
										<asp:TableCell>&nbsp;</asp:TableCell>
									</asp:TableRow>
									<asp:TableRow>
										<asp:TableCell VerticalAlign="Middle">
											<asp:Label CssClass="LABEL" Runat="Server" Text="Select Group" ID="Label2" NAME="Label2"></asp:Label>&nbsp;
											<asp:DropDownList Runat="Server" CssClass="LABEL" ID="ddlGroupList"></asp:DropDownList>&nbsp;
											<asp:Button Runat="Server" Text="Proceed" CssClass="BUTTON" ID="btnProceed"></asp:Button>
										</asp:TableCell>
									</asp:TableRow>
								</asp:Table>
								<!-- 2nd Form Table Ends Here -->
								
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
					<!-- Main Table Ends Here -->

</asp:Content>