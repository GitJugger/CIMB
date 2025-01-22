
<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_CustomerFormat" CodeFile="PG_CustomerFormat.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript">
    
		function fncView()
		{
			var lngOrgId;
			var strOrgName;
			lngOrgId = document.forms[0].ctl00$cphContent$hOrgId.value;
			strOrgName = document.forms[0].ctl00$cphContent$hOrgN.value;
			window.location.href = "PG_ListFile.aspx?Id="+lngOrgId+"&Name="+strOrgName;
		}
    </script>

      
    <!-- Main Table Starts Here -->
    <asp:Table CellPadding="1" CellSpacing="2" Runat="server" Width="100%"  ID="tblMain" >
		<asp:TableRow>
			<asp:TableCell Width="100%">
				<asp:Label Runat="Server" CssClass="FORMHEAD" Text="" ID="lblHead"></asp:Label></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Main Table Ends Here -->
		
	<!-- Border Table Starts Here -->	
	<asp:Table Width="100%" Runat="Server" CellPadding="0" CellSpacing="0" ID="Table1" >
	<asp:TableRow><asp:TableCell>
	<!-- File Parameter Table Starts Here -->
	<asp:Table Width="100%" Runat="Server" CellPadding="2" CellSpacing="3" ID="tblParam">
		<asp:TableRow ID="trOrgId" Visible="False">
		<asp:TableCell Width="20%" Wrap="False">
			<asp:Label Runat="server" Text="Organisation Id" CssClass="LABEL" ID="Label11"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:Label ID="lblOrgId" Runat="Server" CssClass="BLABEL"></asp:Label>
		</asp:TableCell>
	    </asp:TableRow>
	    <asp:TableRow ID="trOrgName" Visible="False">
		    <asp:TableCell Width="20%" Wrap="False">
			    <asp:Label Runat="server" CssClass="LABEL" Text="Organisation Name" ID="Label15"></asp:Label>
		    </asp:TableCell>
		    <asp:TableCell Width="80%">
			    <asp:Label ID="lblOrgName" CssClass="BLABEL" Runat="server"></asp:Label>
		    </asp:TableCell>
	    </asp:TableRow>
	    <asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="Server" ID="lblBankSelect" CssClass="LABEL" Text="Select Bank"></asp:Label></asp:TableCell>			
			<asp:TableCell>
				<asp:DropDownList CssClass="MEDIUMTEXT" Runat="Server" ID="ddlBank" AutoPostBack="True">
				</asp:DropDownList><input id="hidBank" type="hidden" runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="Server" ID="lblSelect" CssClass="LABEL" Text="Select File Type"></asp:Label></asp:TableCell>			
			<asp:TableCell>
				<asp:DropDownList CssClass="MEDIUMTEXT" Runat="Server" ID="cmbFile" AutoPostBack="True">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<%--ADDED DUPLICATE BANK SETTINGS START--%>
		<asp:TableRow  ID="trduplicate" Visible="false">
			<asp:TableCell>
				<asp:Label ID="Label5" Runat="Server" CssClass="LABEL" Text="Duplicate Bank Settings"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
			<asp:CheckBox ID="cbduplicate" runat="server" />
			</asp:TableCell>
		</asp:TableRow>
		<%--DUPLICATE BANK SETTINGS END--%>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="Server" Text="Select File Format" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList Runat="Server" ID="cmbFormat" CssClass="BIGTEXT" AutoPostBack="True">
					<asp:ListItem Value=""></asp:ListItem>
					<asp:ListItem Value="COL">Excel File</asp:ListItem>
					<asp:ListItem Value="DELIM">Delimiter Separated</asp:ListItem> 
					<asp:ListItem Value="POS">Position Separated</asp:ListItem>
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow  ID="trName">
			<asp:TableCell>
				<asp:Label Runat="Server" CssClass="LABEL" Text="Format Name"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:TextBox ID="txtFormatName" Runat="Server" CssClass="LARGETEXT" MaxLength="50"></asp:TextBox>
				<asp:RequiredFieldValidator ID="rfvFormatName" ControlToValidate="txtFormatName" Runat="Server" ErrorMessage="Format Name cannot be blank" Display="None"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow ID="trHeader">
			<asp:TableCell>
				<asp:Label Runat="Server" CssClass="LABEL" Text="File Header Exist"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:CheckBox Runat="Server" ID="chkHeader"></asp:CheckBox>&nbsp;
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Header Lines" ID="Label4"></asp:Label>&nbsp;&nbsp;
				<asp:TextBox ID="txtHLines" CssClass="MINITEXT" Runat="Server" Text="0"></asp:TextBox>
                <asp:RegularExpressionValidator ID="rgetxtHLines" runat="server" Display="None"
        ErrorMessage="Header Lines is invalid" ControlToValidate="txtHLines" ValidationExpression="^\d{1,1000}$"></asp:RegularExpressionValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trFooter">
			<asp:TableCell>
				<asp:Label Runat="Server" CssClass="LABEL" Text="File Footer Exist" ID="Label2"></asp:Label>
			</asp:TableCell>
				<asp:TableCell><asp:CheckBox Runat="Server" ID="chkFooter"></asp:CheckBox>&nbsp;
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Footer Lines" ID="Label3"></asp:Label>&nbsp;&nbsp;&nbsp;
				<asp:TextBox ID="txtFLines" CssClass="MINITEXT" Runat="Server" Text="0"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regtxtFLines" runat="server" Display="None"
        ErrorMessage="Header Lines is invalid" ControlToValidate="txtFLines" ValidationExpression="^\d{1,1000}$"></asp:RegularExpressionValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trExtn">
			<asp:TableCell Runat="Server" ID="Tablecell1"><asp:Label Runat="Server" Text="Select File Extension" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell Runat="Server" ID="Tablecell2">
				<asp:DropDownList CssClass="SMALLTEXT" Runat="Server" ID="cmbExtn"></asp:DropDownList>
				<asp:RequiredFieldValidator ID="rfvExtn" Runat="Server" ErrorMessage="Select File Extension" ControlToValidate="cmbExtn" Display="None"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>

        <asp:TableRow ID="trEncry">
			<asp:TableCell Runat="Server" ID="Tablecell3"><asp:Label ID="Label6" Runat="Server" Text="Select Encryption Type" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell Runat="Server" ID="Tablecell4">
				<asp:DropDownList CssClass="MEDIUMTEXT" Runat="Server" ID="ddlEncryptionType"></asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		
			
		<asp:TableRow ID="trDelim">
			<asp:TableCell>
				<asp:Label Runat="Server" Text="Select Delimiter" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList ID="cmbDelim" CssClass="SMALLTEXT" Runat="Server"></asp:DropDownList>
				<asp:RequiredFieldValidator ID="rfvDelim" Runat="Server" ErrorMessage="Select Delimiter" ControlToValidate="cmbDelim" Display="None"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- File Parameter Table Ends Here -->
	</asp:TableCell></asp:TableRow></asp:Table>
	<!-- Border Table Ends Here -->	
			
	<!-- Data Grid Table Starts Here -->
	<asp:Table Width="100%" CellPadding="1" CellSpacing="2" Runat="Server" ID="tblGrid">
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<!-- Datagrid For Position Separated Starts Here -->		
				<asp:DataGrid Runat="Server" ID="dgPosition" AllowPaging="false" PageSize="5" PagerStyle-HorizontalAlign="Center" 
				GridLines="Both" CellPadding="3" CellSpacing="0" Font-Name="Verdana" 
				Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" 
				width="70%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False" EnableViewState="True">
				<Columns>
					<asp:TemplateColumn HeaderText="Bank Field Name" HeaderStyle-Width="25%">
						<ItemTemplate>
							<asp:Label Runat="Server" ID="lblField" Text='<%# DataBinder.Eval(Container.DataItem,"FDESC")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False">
						<ItemTemplate>
							<asp:TextBox Runat="Server" Visible="False" Text='<%# DataBinder.Eval(Container.DataItem,"FID")%>'></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Start Position" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center" >
						<ItemTemplate>
							<asp:TextBox CssClass="MINITEXT" Runat="Server" MaxLength="5" ID="txtStartPos" Text='<%# fnGetPosition("START",Container.DataItem("FId"))%>'></asp:TextBox>
							<!-- Modify Start position column for LHDN -->	
							<!--<asp:TextBox CssClass="MINITEXT" Runat="Server" Text=<%# DataBinder.Eval(Container.DataItem,"SPOS")%> ID="txtStartPosLHDN"></asp:TextBox>-->
							<asp:RequiredFieldValidator ID="rfvStartPos" ControlToValidate="txtStartPos" Runat="Server" Display="None" ErrorMessage=<%# fnMessage(Container.DataItem("FDESC"),"Start Position Cannot Be Blank")%>></asp:RequiredFieldValidator>
							<asp:RegularExpressionValidator ID="revStartPos" ControlToValidate="txtStartPos" Runat="Server" Display="None" ErrorMessage=<%# fnMessage(Container.DataItem("FDESC"),"Start Position Should Be Numeric")%> ValidationExpression="[0-9]{1,5}"></asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="End Position" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center" >
						<ItemTemplate>
							<asp:TextBox CssClass="MINITEXT" Runat="Server" ID="txtEndPos" MaxLength="5" Text='<%# fnGetPosition("END",Container.DataItem("FId"))%>'></asp:TextBox>
							<!-- Modify End position column for LHDN -->	
							<!--<asp:TextBox CssClass="MINITEXT" Runat="Server" Text=<%# DataBinder.Eval(Container.DataItem,"EPOS")%> ID="txtEndPosLHDN"></asp:TextBox>-->
							<asp:RequiredFieldValidator ID="rfvEndPos" Runat="Server" ControlToValidate="txtEndPos" Display="None" ErrorMessage=<%# fnMessage(Container.DataItem("FDESC"),"End Position Cannot Be Blank")%>></asp:RequiredFieldValidator>
							<asp:RegularExpressionValidator ID="revEndPos" Runat="Server" ControlToValidate="txtEndPos" Display="None" ErrorMessage=<%# fnMessage(Container.DataItem("FDESC"),"End Position Should Be Numeric")%> ValidationExpression="[0-9]{1,5}"></asp:RegularExpressionValidator>
							<asp:RangeValidator ControlToValidate="txtEndPos" ></asp:RangeValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Content Type" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center" >
						<ItemTemplate>
                            <asp:Label ID="lblcontenttype" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ContentType")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					
				</Columns>
				</asp:DataGrid>
				<!-- Datagrid For Position Separated Ends Here -->
				</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2">
				<!-- Datagrid For Column Separated Starts Here -->
				<asp:DataGrid Runat="Server" ID="dgColumn" AllowPaging="false" PageSize="5" PagerStyle-HorizontalAlign="Center" 
				BorderWidth="1" GridLines="Both" CellPadding="3" CellSpacing="0" Font-Name="Verdana" 
				Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" 
				width="50%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
				<Columns>
					<asp:TemplateColumn HeaderText="Bank Field Name" HeaderStyle-Width="25%">
						<ItemTemplate>
							<asp:Label Runat="Server" ID="Label1" Text=<%# DataBinder.Eval(Container.DataItem,"FDESC")%>></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn Visible="False">
						<ItemTemplate>
							<asp:TextBox Runat="Server" Visible="False" Text=<%# DataBinder.Eval(Container.DataItem,"FID")%> ID="Textbox1"></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Column Position" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<asp:TextBox CssClass="MINITEXT" Runat="Server" MaxLength="3" ID="txtColPos" Text=<%# fnGetPosition("COLUMN",Container.DataItem("FId"))%>></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvColPos" ControlToValidate="txtColPos" Runat="Server" Display="None" ErrorMessage=<%# fnMessage(Container.DataItem("FDESC"),"Column Position Cannot Be Blank")%>></asp:RequiredFieldValidator>
							<asp:RegularExpressionValidator ID="revColPos" ControlToValidate="txtColPos" Runat="Server" Display="None" ErrorMessage=<%# fnMessage(Container.DataItem("FDESC"),"Column Position Should Be Numeric")%> ValidationExpression="[0-9]{1,3}"></asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>					
					<asp:TemplateColumn HeaderText="Content Type" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Center" >
						<ItemTemplate>
                            <asp:Label ID="lblcontenttype" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ContentType")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				</asp:DataGrid>
				<!-- Datagrid For Column Separated Ends Here -->
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trSubmit">
			<asp:TableCell Width="100%" ColumnSpan="2"> 
				<asp:Button ID="btnSave" Runat="Server" Text="Submit" CssClass="BUTTON"></asp:Button>&nbsp;
				<input type="button" runat="Server" value="Back To View" class="BUTTON" onclick="fncView();" />
			</asp:TableCell>	
		</asp:TableRow>
		<asp:TableRow Visible="False" ID="trBack">
			<asp:TableCell Width="100%" ColumnSpan="2"> 
				<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncView();" />
			</asp:TableCell>	
		</asp:TableRow>
	</asp:Table>
	<!-- Data Grid Table Ends Here -->
	<input type="hidden" runat="Server" id="hFormat" name="hFormat" />
    <input type="hidden" runat="server" id="hHeader" name="hHeader" />
    <input type="hidden" runat="server" id="hFooter" name="hFooter" />
    <input type="hidden" runat="server" id="hFileId" name="hFileId" />
    <input type="hidden" runat="server" id="hHLine" name="hHLine" />
    <input type="hidden" runat="server" id="hFLine" name="hFLine" />
    <input type="hidden" runat="server" id="hType" name="hType" />
    <input type="hidden" runat="server" id="hExtn" name="hExtn" />
    <input type="hidden" runat="server" id="hDelim" name="hDelim" />
    <input type="hidden" runat="server" id="hOrgId" name="hOrgId" />
    <input type="hidden" runat="server" id="hOrgN" name="hOrgN" />
    <input type="hidden" runat="server" id="hFileType" name="hFileType" />
     <input type="hidden" runat="server" id="hfEncType" name="hfEncType" />
    
    <!-- Validation Starts here -->
    <asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations," ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
    <!-- Validation Ends here -->
</asp:Content>
