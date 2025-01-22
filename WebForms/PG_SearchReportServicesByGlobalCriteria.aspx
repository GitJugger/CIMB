<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_SearchReportServicesByGlobalCriteria" CodeFile="PG_SearchReportServicesByGlobalCriteria.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>
   
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %> 

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="PG_Calendar.js"></script>
    <!-- Main Table Starts Here -->
	<!-- Report Viewer Starts Here -->
				
				<!-- Report Viewer Ends Here -->
				 <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Create Group" ID="lblHeading"></asp:Label>
            </td>
         </tr>
         <tr>
            <td>
              	<asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label>
            </td>
         </tr>
      </table> 
       <asp:panel ID="pnlReport" runat="server" Height="500px" Width="600px" Visible="false">
   <rsweb:ReportViewer  ID="rvReport" runat="server" Width="100%" ProcessingMode="Remote" ShowParameterPrompts="false" EnableTheming="true">
        </rsweb:ReportViewer>
   
       </asp:panel>
      
      
     
	<asp:Table ID="tblMain" CellPadding="8" CellSpacing="0" Width="100%" Runat="Server">

		<asp:TableRow>
			<asp:TableCell Width="100%">
				
				<!-- Form Table Starts Here -->
				<asp:Table Width="90%" CellPadding="2" CellSpacing="1" ID="tblForm" Runat="Server">

					<asp:TableRow>
						<asp:TableCell Width="90%" ColumnSpan="2">
							<asp:RadioButton ID="rdAll" Runat="Server" CssClass="LABEL" GroupName="SearchOptions" Text="All Records" Checked="True"></asp:RadioButton>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdOrgId" Runat="Server" Text="Organization Id" CssClass="LABEL" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox ID="txtOrgId" CssClass="SMALLTEXT" Runat="Server" MaxLength="7"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator8" runat="server" ControlToValidate="txtOrgId" Display="None"
                                            ErrorMessage="Org Id Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdOrgName" Runat="Server" Text="Organization Name" CssClass="LABEL" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox Runat="Server" ID="txtOrgName" CssClass="MEDIUMTEXT" MaxLength="40"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator7" runat="server" ControlToValidate="txtOrgName" Display="None"
                                            ErrorMessage="Org Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trAccNo" Visible="False">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdAccNo" Runat="Server" CssClass="LABEL" Text="Acccount No" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox ID="txtAccNo" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="14"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtAccNo" Display="None"
                                            ErrorMessage="Acc No Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trFileName">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdFileName" Runat="Server" Text="File Name" CssClass="LABEL" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox ID="txtFileName" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="50"></asp:TextBox>
                             <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtFileName" Display="None"
                                            ErrorMessage="File Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trCancel">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdCancel" Runat="Server" Text="Cancelled By" GroupName="SearchOptions" CssClass="LABEL"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox ID="txtCancelBy" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="20"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtCancelBy" Display="None"
                                            ErrorMessage="Cancel by Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trStopDate">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdDate" Runat="Server" CssClass="LABEL" Text="Stop Payment Date (DD/MM/YYYY)" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox ID="txtStopDate" Runat="Server" CssClass="SMALLTEXT" MaxLength="10" onfocus='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtStopDate, "dd/mm/yyyy");'></asp:TextBox>
							<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtStopDate, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"></a>
							<asp:RegularExpressionValidator ID="revStop" Runat="server" ControlToValidate="txtStopDate" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Stop Payment Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trCreateDt">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdCreateDt" Runat="Server" CssClass="LABEL" Text="Created Date (DD/MM/YYYY)" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox ID="txtCreateDt" Runat="Server" CssClass="SMALLTEXT" MaxLength="10" onfocus='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtCreateDt, "dd/mm/yyyy");'></asp:TextBox>&nbsp;
							<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtCreateDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"></a>
							<asp:RegularExpressionValidator ID="revCreateDate" Runat="server" ControlToValidate="txtCreateDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Create Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trCreateBy">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdCreateBy" Runat="Server" CssClass="LABEL" Text="Created By" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:TextBox ID="txtCreateBy" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="20"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCreateBy" Display="None"
                                            ErrorMessage="Created by Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdMonth" Runat="Server" CssClass="LABEL" Text="For the Month/Year" GroupName="SearchOptions"></asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%">
							<asp:DropDownList ID="ddlMonth" Runat="Server" CssClass="SMALLTEXT">
								<asp:ListItem Value="">Select</asp:ListItem>
								<asp:ListItem Value="01">January</asp:ListItem>
								<asp:ListItem Value="02">February</asp:ListItem>
								<asp:ListItem Value="03">March</asp:ListItem>
								<asp:ListItem Value="04">April</asp:ListItem>
								<asp:ListItem Value="05">May</asp:ListItem>
								<asp:ListItem Value="06">June</asp:ListItem>
								<asp:ListItem Value="07">July</asp:ListItem>
								<asp:ListItem Value="08">August</asp:ListItem>
								<asp:ListItem Value="09">September</asp:ListItem>
								<asp:ListItem Value="10">October</asp:ListItem>
								<asp:ListItem Value="11">November</asp:ListItem>
								<asp:ListItem Value="12">December</asp:ListItem>
							</asp:DropDownList>
							&nbsp;
							<asp:DropDownList ID="ddlYear" Runat="Server" CssClass="SMALLTEXT">						
							</asp:DropDownList>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdFrom" Runat="Server" Text="From (DD/MM/YYYY)" CssClass="LABEL" GroupName="SearchOptions">
							</asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%" Wrap="False">
							<asp:TextBox ID="txtFromDt" Runat="Server" MaxLength="10" CssClass="SMALLTEXT" onfocus='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtFromDt, "dd/mm/yyyy");'></asp:TextBox>&nbsp;
							<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtFromDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"></a>
							<asp:Label Runat="Server" Text="To" CssClass="LABEL"></asp:Label>&nbsp;
							<asp:TextBox ID="txtToDt" Runat="Server" CssClass="SMALLTEXT" MaxLength="10" onfocus='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtToDt, "dd/mm/yyyy");'></asp:TextBox>&nbsp;
							<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtToDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"></a>
							<asp:RegularExpressionValidator ID="revFromDt" Runat="server" ControlToValidate="txtFromDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="From Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
							<asp:RegularExpressionValidator ID="revToDt" Runat="server" ControlToValidate="txtToDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="To Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trRequest" Visible="False">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdRequestBy" Runat="Server" Text="Request By" CssClass="LABEL" GroupName="SearchOptions">
							</asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%" Wrap="False">
							<asp:TextBox ID="txtRequestBy" CssClass="MEDIUMTEXT" Runat="server" MaxLength="50"></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtRequestBy" Display="None"
                                            ErrorMessage="Request by Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trApprove" Visible="False">
						<asp:TableCell Width="20%" Wrap="False">
							<asp:RadioButton ID="rdApproveBy" Runat="Server" Text="Approve By" CssClass="LABEL" GroupName="SearchOptions">
							</asp:RadioButton>
						</asp:TableCell>
						<asp:TableCell Width="70%" Wrap="False">
							<asp:TextBox ID="txtApproveBy" CssClass="MEDIUMTEXT" Runat="server" MaxLength="50"></asp:TextBox>
                             <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtApproveBy" Display="None"
                                            ErrorMessage="Approve by Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Width="20%" Wrap="False">
							<asp:Button ID="btnSearch" Runat="Server" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
							<input type="reset" id="btnClear" name="btnClear" runat="Server" value="Clear" class="BUTTON">
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
				<!-- Form Table Ends Here -->
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Main Table Ends Here -->
    <asp:ValidationSummary DisplayMode="BulletList" ID="vsSearchReportServicesCriteria" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>
</asp:Content>