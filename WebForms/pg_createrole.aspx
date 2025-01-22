<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_CreateRole" CodeFile="PG_CreateRole.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <script type="text/javascript" src="../include/common.js"></script>
    
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			var strRequest;
			strRequest = document.all('ctl00$cphContent$hRequest').value;
			
			if(strRequest == "View")
			{
				window.location.href = "PG_ApprMatrix.aspx?Mode=View";
			}
			else if(strRequest == "Reject")
			{
				window.location.href = "PG_ApprMatrix.aspx?Mode=Reject";
			}
			else if(strRequest == "Done")
			{
				window.location.href = "PG_ApprMatrix.aspx?Mode=Done";
			}
			else if(strRequest == "Edit")
			{
				window.location.href = "PG_ApprMatrix.aspx?Mode=Edit";
			}
		}
		
		function fncNew()
		{
			window.location.href = "PG_CreateRole.aspx";
		}
		
		function fncView()
		{
			var strRequest = document.all('ctl00$cphContent$hRequest').value;
			if(strRequest == "BU")
			{
				window.location.href = "PG_ViewPassword.aspx";
			}
			else
			{
			    if (document.forms[0].ctl00$cphContent$hOrgId.value > 0)
			    {
			        window.location.href = "PG_ViewOrganizationRoles.aspx?Id=" + document.forms[0].ctl00$cphContent$hOrgId.value
			        
			    }
			    else
			    {
			        window.location.href = "PG_ViewRoles.aspx";
			    }
				//window.location.href = "PG_ViewRoles.aspx";
			}
		}
		function fncShow()
		{
			var lngUserId;
			lngUserId = document.all('ctl00$cphContent$hUserId').value;
			if(lngUserId == "")
			{
				window.location.href = "PG_CreateRole.aspx";
			}
			else
            {
                
				window.location.href = "PG_CreateRole.aspx?Id=" + lngUserId;
			}
			
		}
	   </script>
    <script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
        
     <!-- Main Table Starts Here -->
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Create User" ID="lblHeading"></asp:Label>
            </td>
         </tr>
         <tr>
            <td>
               <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
            </td>
         </tr>
      </table> 
    <!-- Main Table Ends Here -->

    
		<!-- Form Table Starts here -->
		<asp:Table Width="100%" CellPadding="8" CellSpacing="0" Runat="Server" BorderWidth="0" ID="tblMainForm">
			<asp:TableRow>
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="User Id" ID="lblUserId"></asp:Label>
				&nbsp;
					<asp:Label Text="*" Runat="server" CssClass="MAND"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox Runat="Server" ID="txtUserLogin" MaxLength="14" CssClass="MEDIUMTEXT"></asp:TextBox>
                    <asp:RegularExpressionValidator
                                            ID="revtxtUserLogin" runat="server" ControlToValidate="txtUserLogin" Display="None"
                                            ErrorMessage="User Login Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="User Name" ID="lblUserName"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="server" CssClass="MAND"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox Runat="Server" ID="txtUserName" MaxLength="20" CssClass="MEDIUMTEXT"></asp:TextBox>
                    <asp:RegularExpressionValidator
                                            ID="revUserName" runat="server" ControlToValidate="txtUserName" Display="None"
                                            ErrorMessage="User Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trRoles">
				<asp:TableCell Width="30%">
						<asp:Label Runat="Server" Text="Select User Role" CssClass="LABEL" ID="lblRole"></asp:Label>&nbsp;
						<asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label1" NAME="Label1"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:DropDownList ID="cmbRoles" Runat="Server" CssClass="MEDIUMTEXT">
					</asp:DropDownList>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trPassword">
				<asp:TableCell Width="30%">
						<asp:Label Runat="Server" CssClass="LABEL" Text="Password" ID="lblPassword"></asp:Label>&nbsp;
						<asp:Label Text="*" Runat="Server" CssClass="MAND"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
						<asp:TextBox Runat="Server" ID="txtPassword" CssClass="MEDIUMTEXT" TextMode="Password" MaxLength="24">
						</asp:TextBox></asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%">
						<asp:Label Runat="Server" CssClass="LABEL" Text="Expiry Date (DD/MM/YYYY)" ID="lblExpiry"></asp:Label>&nbsp;
						<asp:Label Text="*" Runat="Server" CssClass="MAND"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<input type="text" id="txtExpDate" name="txtExpDate" runat="server" class="SMALLTEXT" onfocus='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtExpDate, "dd/mm/yyyy");'/>&nbsp;
					<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtExpDate, "dd/mm/yyyy");'><img alt="Calendar" id="imgCalExpDate" runat="server" src="../Include/Images/date.gif" border="0"/></a>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%">
						<asp:Label Runat="Server" CssClass="LABEL" Text="Password Change Period" ID="lblChange"></asp:Label>&nbsp;
						<asp:Label Text="*" Runat="Server" CssClass="MAND" ID="Label5"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtPassChangePeriod" Runat="server" CssClass="MINITEXT" MaxLength="3"></asp:TextBox>&nbsp;
					<asp:DropDownList ID="cbPassChangePeriod" runat="server" CssClass="SMALLTEXT">
						<asp:ListItem Value="">Period</asp:ListItem>
						<asp:ListItem Value="D">Day(s)</asp:ListItem>
						<asp:ListItem Value="M">Month(s)</asp:ListItem>
						<asp:ListItem Value="Y">Year(s)</asp:ListItem>
					</asp:DropDownList>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trAuthCode">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Validation Code" ID="lblAuthCode"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="Server" CssClass="MAND"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtAuthCode" Runat="server" CssClass="MEDIUMTEXT" MaxLength="14" TextMode="Password"></asp:TextBox></asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trDisplay" Visible="False">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Display File Content"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:RadioButtonList ID="rdDisplay" Runat="Server" CssClass="LABEL" RepeatDirection="Horizontal">
						<asp:ListItem Value="1" Selected="True">Detail</asp:ListItem>
						<asp:ListItem Value="2">Summary</asp:ListItem>
					</asp:RadioButtonList>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trLimit" Visible="False">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Validation Limit"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					&nbsp;<asp:RadioButton ID="rdNLimit" Runat="Server" CssClass="LABEL" Text="No Limit" GroupName="Limit" Checked="True"></asp:RadioButton>
					<asp:RadioButton ID="rdLimit" Runat="Server" CssClass="LABEL" Text="RM " GroupName="Limit"></asp:RadioButton>
					<asp:TextBox ID="txtLimit" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="15" Text="0.00"></asp:TextBox>&nbsp;
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trGroups">
				<asp:TableCell Width="30%" VerticalAlign="Top">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Select Group(s)"></asp:Label>
					&nbsp;<asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label22" NAME="Label22"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:CheckBoxList ID="chkGroups" Runat="Server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trEmailAddress" Visible="false">
				<asp:TableCell Width="30%" VerticalAlign="Top">
					<asp:Label ID="Label27" Runat="Server" CssClass="LABEL" Text="Email Address"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtEmailAddress" runat="server" CssClass="MEDIUMTEXT" MaxLength="50"></asp:TextBox>
                     <asp:RegularExpressionValidator ID="revEmail" Runat="Server" ControlToValidate="txtEmailAddress" ValidationExpression=".+@.+\..+" ErrorMessage="Invalid Email Account" Display="None"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trEmailAlert" Visible="false">
				<asp:TableCell Width="30%" VerticalAlign="Top">
					<asp:Label ID="Label25" Runat="Server" CssClass="LABEL" Text="Receive Email Alert"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:CheckBox ID="chkEmailAlert" Runat="Server"></asp:CheckBox>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%">
						<asp:Label CssClass="LABEL" ID="lblActive" Text="Status" Runat="Server"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:RadioButtonList ID="rdStatus" Runat="Server" CssClass="LABEL" RepeatDirection="Horizontal">
					</asp:RadioButtonList>
				</asp:TableCell>
			</asp:TableRow>

			<asp:TableRow ID="trStaffNo">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Staff Number" ID="Label29"></asp:Label>&nbsp;
					<asp:Label Text="" Runat="Server" CssClass="MAND"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtStaffNumber" Runat="server" CssClass="MEDIUMTEXT" MaxLength="20" ></asp:TextBox></asp:TableCell>
			</asp:TableRow>

			<asp:TableRow ID="trReset" Visible="False">
				<asp:TableCell Width="30%" >
					<asp:Label Runat="server" Text="Reset Id" CssClass="LABEL"></asp:Label>
				</asp:TableCell>
				<asp:TableCell>
					<asp:CheckBox ID="chkReset" Runat="server" CssClass="LABEL"></asp:CheckBox>
				</asp:TableCell> 
			</asp:TableRow>
			<asp:TableRow ID="trReason" Visible="False">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Modification Reason"></asp:Label>
					&nbsp;<asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label23"></asp:Label>
				</asp:TableCell>
				<asp:TableCell>
					<asp:TextBox ID="txtReason" Runat="Server" CssClass="BIGTEXT" TextMode="MultiLine" Rows="3" MaxLength="255"></asp:TextBox>
                    <asp:RegularExpressionValidator
                                            ID="revReason" runat="server" ControlToValidate="txtReason" Display="None"
                                            ErrorMessage="Reason Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trSubmit">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<asp:Button ID="btnSubmit" Text="Submit" CssClass="BUTTON" Runat="Server"></asp:Button>&nbsp;
					<input type="reset" value="Clear" id="btnReset" runat="Server" class="BUTTON"/>&nbsp;
					<input id='inptBackToView' type="button" value="Back To View" runat="Server" onclick="fncView();"/>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trBack" Visible="False">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<input type="button" runat="Server" value="Back" onclick="fncBack();"/>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<!-- Form Table Ends here -->
		
		<!-- Confirm Table Starts here -->
		<asp:Table Width="100%" CellPadding="8" CellSpacing="0" Runat="Server" BorderWidth="0" ID="tblConfirm">
			<asp:TableRow>
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="User Id" ID="Label3"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label4"></asp:Label></asp:TableCell>
			<asp:TableCell Width="70%">
					<asp:TextBox Runat="Server" ID="txtCUserLogin" MaxLength="14" CssClass="MEDIUMTEXT" ReadOnly="True">
					</asp:TextBox>
                 <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCUserLogin" Display="None"
                                            ErrorMessage="User Login Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                
			</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="User Name" ID="Label6"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label7"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox Runat="Server" ID="txtCUserName" MaxLength="20" CssClass="MEDIUMTEXT" ReadOnly="True"></asp:TextBox>
                     <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCUserName" Display="None"
                                            ErrorMessage="Username Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCPassword">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" Text="Password" CssClass="LABEL" ID="Label2" NAME="Label2"></asp:Label> </asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:Label Runat="Server" Text="**********" CssClass="LABEL" ID="Label8" NAME="Label8"></asp:Label> </asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%" Wrap="False">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Expiry Date (DD/MM/YYYY)" ID="Label10"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="Server" CssClass="MAND" ID="Label11"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox Runat="Server" ID="txtCExpDate" MaxLength="10" CssClass="MEDIUMTEXT" ReadOnly="True"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator8" Runat="server" ControlToValidate="txtCExpDate" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>

				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Password Change Period" ID="Label12"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="Server" CssClass="MAND" ID="Label13"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtCPassChangePeriod" Runat="server" CssClass="MINITEXT" MaxLength="3" ReadOnly="True"></asp:TextBox>&nbsp;
					<asp:TextBox ID="txtCChangeUnit" Runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
                     <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCPassChangePeriod" Display="None"
                                            ErrorMessage="ChangePeriod Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                    
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCRoles">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" Text="User Role" CssClass="LABEL" ID="Label16"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label17"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtRole" Runat="Server" CssClass="MEDIUMTEXT" ReadOnly="True"></asp:TextBox>
                     <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtRole" Display="None"
                                            ErrorMessage="Role Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>

				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCAuthCode">
				<asp:TableCell Width="30%">
						<asp:Label Runat="Server" Text="Validation Code" CssClass="LABEL" ID="Label9" NAME="Label9"></asp:Label> </asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:Label Runat="Server" Text="**********" CssClass="LABEL" ID="Label14" NAME="Label14"></asp:Label> </asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCDisplay" runat='server'>
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Display File Content" ID="Label18"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtCDisplay" CssClass="MEDIUMTEXT" Runat="Server" ReadOnly="True"></asp:TextBox>
                     <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtCDisplay" Display="None"
                                            ErrorMessage="Display Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCLimit">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Validation Limit" ID="Label19"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:Label ID="lblLimit" Runat="Server" CssClass="LABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCGroups">
				<asp:TableCell Width="30%" VerticalAlign="Top">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Group(s) Selected" ID="Label20"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<select id="lbxGroups" name="lbxGroups" runat="Server" size="6"></select>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCEmailAddress" Visible="false">
				<asp:TableCell Width="30%" VerticalAlign="Top">
					<asp:Label ID="Label28" Runat="Server" CssClass="LABEL" Text="Email Address"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox id ="txtCEmailAddress" runat="server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>	
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator7" Runat="Server" ControlToValidate="txtCEmailAddress" ValidationExpression=".+@.+\..+" ErrorMessage="Invalid Email Account" Display="None"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCEmailAlert" Visible = "False">
				<asp:TableCell Width="30%" VerticalAlign="Top">
					<asp:Label ID="Label26" Runat="Server" CssClass="LABEL" Text="Receive Email Alert"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:Label ID="lblReceiveEmail" Runat="Server" CssClass="LABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="30%">
					<asp:Label CssClass="LABEL" ID="Label21" Text="Status" Runat="Server"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtStatus" Runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
				</asp:TableCell>
			</asp:TableRow>

			<asp:TableRow ID="trCStaffNo">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Staff Number" ID="Label30"></asp:Label>&nbsp;
					<asp:Label Text="" Runat="Server" CssClass="MAND"></asp:Label></asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtCStaffNumber" Runat="server" CssClass="MEDIUMTEXT" MaxLength="20" ></asp:TextBox></asp:TableCell>
			</asp:TableRow>

			<asp:TableRow ID="trCReason">
				<asp:TableCell Width="30%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Modification Reason" ID="Label24"></asp:Label>
				</asp:TableCell>
				<asp:TableCell>
					<asp:TextBox ID="txtCReason" Runat="Server" CssClass="BIGTEXT" TextMode="MultiLine" Rows="3" MaxLength="255">
					</asp:TextBox>
                    <asp:RegularExpressionValidator
                                            ID="revReason1" runat="server" ControlToValidate="txtCReason" Display="None"
                                            ErrorMessage="Reason Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trAuth">
				<asp:TableCell Width="30%" Wrap="False">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Validation Code"></asp:Label>&nbsp;
					<asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label15"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtCAuthCode" Runat="Server" CssClass="MEDIUMTEXT" TextMode="Password" MaxLength="24"></asp:TextBox>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" Runat="server" ControlToValidate="txtCAuthCode" ErrorMessage="Validation Code Cannot Be Blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="RegularExpressionValidator9" Runat="server" ControlToValidate="txtCAuthCode" ValidationExpression="^([a-zA-Z])[a-zA-Z0-9]*$" ErrorMessage="Validation Code  must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
                </asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trConfirm">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<asp:Button ID="btnConfirm" Runat="Server" CssClass="BUTTON" Text="Confirm"></asp:Button>&nbsp;
					<asp:Button ID="btnUpdate" Text="Confirm" Runat="Server" CssClass="BUTTON" Visible="False"></asp:Button>&nbsp;
                    <asp:Button ID="btnBackToView" Runat="Server" CssClass="BUTTON" Text="Back" CausesValidation="false"></asp:Button>
				<%--<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncShow();"/>--%>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCreate" Visible="False">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<input type="button" runat="server" value="Create New Role" onclick="fncNew();" causesvalidation="false"/>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trCBack" Visible="False">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<input type="button" runat="Server" value="Back To View"  onclick="fncView();"/>
                    <%--<asp:Button ID="btnBackview" Runat="Server" CssClass="BUTTON" Text="Back To View"></asp:Button>--%>&nbsp;
                    <%--<input type="button" runat="Server" value="Back To View" onc ="Redirect_ServerClick"/>--%>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<!-- Confirm Table Ends here -->
		
		<!-- Hidden Box Starts Here -->
		<input type="hidden" name="hLimit" id="hLimit" runat="Server"/>
		<input type="hidden" name="hUserId" id="hUserId" runat="Server"/>
		<input type="hidden" name="hVerify" id="hVerify" runat="Server"/>
		<input type="hidden" name="hStatus" id="hStatus" runat="Server"/>
		<input type="hidden" name="hDisplay" id="hDisplay" runat="Server"/>
		<input type="hidden" name="hPassword" id="hPassword" runat="Server"/>
		<input type="hidden" name="hAuthCode" id="hAuthCode" runat="Server"/>
		<input type="hidden" name="hApproved" id="hApproved" runat="Server"/>
		<input type="hidden" name="hDelete" id="hDelete" runat="Server"/>
		<input type="hidden" name="hRequest" id="hRequest" runat="Server"/>
		<input type="hidden" name="hReset" id="hReset" runat="server"/>
		<input type="hidden" name="hIsReceiveEmail" id="hIsReceiveEmail" runat="server" value="0"/>
	    <input type="hidden" name="hEmail" id="hEmail" runat="server" />
	    <input type="hidden" name="hOrgId" id="hOrgId" runat="server" />	
		<!-- Hidden Box Ends Here -->
		
		<!-- Audit Trail Hidden Boxes -->
		<input type="hidden" runat="server" name="AhUName" id="AhUName"/>
		<input type="hidden" runat="server" name="AhURole" id="AhURole"/>
		<input type="hidden" runat="server" name="AhExpry" id="AhExpry"/>
		<input type="hidden" runat="server" name="AhPerid" id="AhPerid"/>
		<input type="hidden" runat="server" name="AhPUnit" id="AhPUnit"/>
		<input type="hidden" runat="server" name="AhStats" id="AhStats"/>
		<!-- Audit Trail Hidden Boxes -->
		
		<!-- Validations Starts Here -->
		<asp:RequiredFieldValidator ID="rfvTxtUserId" Runat="server" ControlToValidate="txtUserLogin" ErrorMessage="User ID Cannot Be Blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator ID="rfvTxtUserName" Runat="server" ControlToValidate="txtUserName" ErrorMessage="User Name Cannot Be Blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator ID="rfvPassword" Runat="Server" ControlToValidate="txtPassword" ErrorMessage="Password Cannot Be Blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="rgePassword" Runat="server" ControlToValidate="txtPassword" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Password must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
		<asp:RequiredFieldValidator ID="rfvExpiryDate" Runat="server" ControlToValidate="txtExpDate" ErrorMessage="Expiry Date Cannot Be Blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator ID="rfvReason" Runat="server" ControlToValidate="txtReason" ErrorMessage="Modification Reason cannot be blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="revExpiryDate" Runat="server" ControlToValidate="txtExpDate" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="rfvCombo" Runat="server" ControlToValidate="cbPassChangePeriod" ErrorMessage="Select Password Change Period" Display="None"></asp:RequiredFieldValidator>
		<asp:RequiredFieldValidator ID="rfvPassChangePeriod" Runat="server" ControlToValidate="txtPassChangePeriod" ErrorMessage="Password Change Period Cannot Be Blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="rgePassChangePeriod" Runat="server" ControlToValidate="txtPassChangePeriod" ValidationExpression="^\d{1,3}$" ErrorMessage="Password Change period must be 1 to 3 digits" display="None"></asp:RegularExpressionValidator>
		<asp:RequiredFieldValidator ID="rfvAuthCode" Runat="server" ControlToValidate="txtAuthCode" ErrorMessage="Validation Code Cannot Be Blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code  must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
		<asp:RequiredFieldValidator ID="rfvRole" Runat="Server" ControlToValidate="cmbRoles" ErrorMessage="Select Role Type" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="revLimit" Runat="Server" ControlToValidate="txtLimit" ValidationExpression="[0-9.,]{1,14}" ErrorMessage="Validation Limit should be in currency value." Display="None"></asp:RegularExpressionValidator>&nbsp;
		<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations," ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
		<!-- Validations Ends Here -->
			
</asp:Content>