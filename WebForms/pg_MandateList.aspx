<%@ Page Language="vb" Inherits="MaxPayroll.pg_MandateList" AutoEventWireup="false" CodeFile="pg_MandateList.aspx.vb"     MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

		<script type="text/javascript" language="JavaScript">
		function fncNew()
		{
			window.location.href = "pg_MandatesDetails.aspx";
		}
		</script>

      <script type="text/javascript" src="../include/common.js"></script>
		
			<!--Main Table Starts Here -->
			<input type="hidden" id="hReq" name="hReq" runat="server" />
			
				
				<table cellpadding="5" cellspacing="0" width="100%" border="0">
                    <tr>
                        <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
                    </tr>
                </table>
				<table border="0" cellpadding="8" cellspacing="0" width="100%">
				<tr>
					<td>
						<!--Form Table Starts Here -->
						<table border="0" cellpadding="2" cellspacing="0" width="100%">
							<tr>
								<td style="width:20%" class="LABEL">&nbsp;Reference No.</td>
								<td style="width:80%">&nbsp;<asp:TextBox ID="txtRefNo" runat="server" CssClass="MEDIUMTEXT" MaxLength="30"></asp:TextBox></td>
							</tr>
							<tr>
								<td class="LABEL">&nbsp;Bank Org. Code</td>
								<td>&nbsp;<asp:TextBox ID="txtBankOrgCode" runat="server" CssClass="MEDIUMTEXT" MaxLength="4"></asp:TextBox></td>
							</tr>
							<tr>
								<td class="LABEL">&nbsp;Account No.</td>
								<td>&nbsp;<asp:TextBox ID="txtAccountNo" runat="server" CssClass="MEDIUMTEXT" MaxLength="14"></asp:TextBox></td>
							</tr>
							<tr>
								<td class="LABEL">&nbsp;Customer Name</td>
								<td>&nbsp;<asp:TextBox ID="txtCustName" runat="server" CssClass="MEDIUMTEXT" MaxLength="20"></asp:TextBox></td>
							</tr>				
						</table>
				<!--Form Table Ends Here -->
				    </td>
				</tr>
				<tr>
					<td><asp:Button Runat="Server" ID="btnSearch" Text="Search" CssClass="BUTTON"></asp:Button></td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="lblMessage" Runat="server" CssClass="MSG" />
						<asp:panel ID="pnlGrid" CssClass ="GridDiv" runat = "server">
						<asp:DataGrid Runat="Server" ID="dgMandateList" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
							PagerStyle-HorizontalAlign="Center" BorderWidth="0" OnPageIndexChanged="prPageChange"
							GridLines="none"  HeaderStyle-CssClass="GridHeaderStyle"
							AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle" HeaderStyle-HorizontalAlign="Left"
							AutoGenerateColumns="False" CssClass ="Grid" DataKeyField="ID" >
							<Columns>	
							    <asp:BoundColumn DataField="OrgId" Visible="false"></asp:BoundColumn>					    
							    <asp:BoundColumn DataField="RefNo" HeaderText="Ref. No."></asp:BoundColumn>						
		                        <asp:BoundColumn DataField="BankOrgCode" HeaderText="Bank Org. Code"></asp:BoundColumn>
		                        <asp:BoundColumn DataField="CustomerName" HeaderText="Customer Name"></asp:BoundColumn>
		                        <asp:BoundColumn DataField="AccNo" HeaderText="Account No."></asp:BoundColumn>      
		                        <asp:BoundColumn DataField="LimitAmount" HeaderText="Limit Amt." DataFormatString="{0:N2}"></asp:BoundColumn>
		                        <asp:BoundColumn DataField="Frequency" HeaderText="Freq."></asp:BoundColumn>      
		                        <asp:BoundColumn DataField="FrequencyLimit" HeaderText="Freq. Limit"></asp:BoundColumn>      
								<asp:TemplateColumn HeaderText="Select">
									<ItemTemplate>
										<a href="pg_MandatesDetails.aspx?MId=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Id=<%#DataBinder.Eval(Container.DataItem,"OrgId")%>&RefNo=<%#DataBinder.Eval(Container.DataItem,"RefNo")%>&BankOrgCode=<%#DataBinder.Eval(Container.DataItem, "BankOrgCode") %>&AccNo=<%#DataBinder.Eval(Container.DataItem, "AccNo") %>&Name=<%#DataBinder.Eval(Container.DataItem,"CustomerName")%>&PageMode=<%=MaxPayroll.mdConstant.enmPageMode.EditMode%>">Mandates Maintenance</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								</Columns>
								
						</asp:DataGrid></asp:panel>
					</td>
				</tr>
				<tr>
					<td>
						<!--<input type="button" runat="Server" value="Create New" class="BUTTON" id="btnNew" visible="false" onclick="fncNew();">  -->
						<input type="button" runat="server" value="Show All" class="BUTTON" onclick="fncShow();" />
					</td>
				</tr>
			</table>
			<!-- Main Table Ends Here-->
			
			<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields," ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>

</asp:Content>