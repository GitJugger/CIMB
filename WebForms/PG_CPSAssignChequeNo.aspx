<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PG_CPSAssignChequeNo.aspx.vb" Inherits="Maxpayroll.PG_CPSAssignChequeNo" 
 MasterPageFile="~/WebForms/mp_Master.master"%>

    

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<script type="text/javascript" src="PG_Calendar.js"></script>
    <asp:Table ID="tblCPSChequeInfo" runat ="server" Height="162px" Width="490px" >
       	<asp:TableRow ID="trDesc" runat="server">
       	<asp:TableCell ID="TableCell1" Wrap="False" runat="server">
       	 <asp:Label ID="lblHeader" runat="server" Text="Cheque Number Allocation" Font-Bold></asp:Label>
           <br /><br />
           <asp:Label runat="server" ID="lblError" Visible="false" ForeColor="red" Text=""></asp:Label>
           <br /><br />
           </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="trerror" runat="server" >
        <asp:TableCell >
         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="ERROR : Cheque Number must be numeric only and greater then '0'." 
             ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}?$" ControlToValidate="txtChequeNo">    </asp:RegularExpressionValidator> <br />
            <asp:RequiredFieldValidator ID="reqfChequeNo" runat="server" ErrorMessage="ERROR : Cheque No Cannot be Blank" ControlToValidate="txtChequeNo"></asp:RequiredFieldValidator>
                <br /><br />
        </asp:TableCell>
        </asp:TableRow>
        
        	<asp:TableRow ID="trNoRecord" runat="server">
         <asp:TableCell ID="TableCell3" Wrap="False" runat="server" ColumnSpan="2">
                Number of Records In File&nbsp;&nbsp;: &nbsp;&nbsp;
                 <asp:Label ID="lblNoofRecords" runat="server" Text=""></asp:Label>
                </asp:TableCell>
         
        </asp:TableRow>
      
     
             <%--Grid Record END--%>
        
        
          	<asp:TableRow ID="trReusable" runat="server">
           <asp:TableCell Wrap="False" runat="server" ColumnSpan="2">
                Number of Reusable Cheque&nbsp;:&nbsp;&nbsp;
                <asp:Label ID="lblReusable" runat="server" Text=""></asp:Label>&nbsp;&nbsp;
                <asp:LinkButton ID="lbDetails" runat="server" Visible="false">Details</asp:LinkButton>
                </asp:TableCell>               
           
       </asp:TableRow>
       
       <asp:TableRow ID="trGrid" Visible="false" >
            <asp:TableCell ID="tblcellGrid" Wrap="False" runat="server" >
                
                <%--Put Reusable cheque Grid Here--%>
                
                <asp:DataGrid ID="dgRejectedCheque" runat="server" AutoGenerateColumns="False" >
                
    <Columns>
    
    <asp:BoundColumn DataField ="CHEQUE_NO" HeaderText="Cheque No Available" HeaderStyle-Font-Bold="true" HeaderStyle-Height="10px" />
     
		<asp:TemplateColumn HeaderText="Select" HeaderStyle-Font-Bold="true" HeaderStyle-Height="10px"> 

                <ItemTemplate> <center>
                    <asp:CheckBox ID="cbChequeNo" runat="server" /></center>
                </ItemTemplate> </asp:TemplateColumn> 

	</Columns>
</asp:DataGrid>
                </asp:TableCell>
       </asp:TableRow>
       <asp:TableRow id="trRejectCheque" runat="server" Visible="false">
        
        <asp:TableCell ID="TableCell2" runat="server" ColumnSpan="2" >
        <asp:CheckBox ID="cbRejectNo" runat="server"  AutoPostBack="true" />
       
        
         Select All (Skip/Rejected/Damage Cheque Number)
      </asp:TableCell>
        </asp:TableRow>
           <asp:TableRow ID="trDate">
                                <asp:TableCell Width="20%" Wrap="False" ColumnSpan="2">
                                    <asp:Label CssClass="LABEL" runat="Server" Text="Payment Date (DD/MM/YYYY)" ID="Label3"
                                        NAME="Label3"></asp:Label>&nbsp;&nbsp;
                                
                                
                                    <input type="text" id="txtPayDate" name="txtPayDate" class="SMALLTEXT" runat="server"
                                        onfocus="popUpCalendar(this, document.all('ctl00$cphContent$txtPayDate'), 'dd/mm/yyyy');" />&nbsp;
                                    <a href="#" onclick="popUpCalendar(this, document.all('ctl00$cphContent$txtPayDate'), 'dd/mm/yyyy');">
                                        <img src="../Include/Images/date.gif" border="0"></a>
                                </asp:TableCell>
                            </asp:TableRow>
        
        <asp:TableRow ID="trCustomCheque" runat="server">
        <asp:TableCell runat="server" ColumnSpan="2" >
        <p><br />
         <asp:CheckBox ID="cbUseCustomCheque" runat="server" OnCheckedChanged="checked_clicked" AutoPostBack="true"/>Use Custom Cheque Number ?
         </p></asp:TableCell>
        </asp:TableRow>
        
        
        <asp:TableRow ID="trInputChequeNo" runat="server">
         
         <asp:TableCell Wrap="False" runat="server" ColumnSpan="2">
         
         
                Cheque Start Number : 
                <asp:TextBox ID="txtChequeNo" runat="server" Enabled="false" MaxLength="6"></asp:TextBox>
             <asp:RegularExpressionValidator
                                            ID="revChequeNo" runat="server" ControlToValidate="txtChequeNo" Display="None"
                                            ErrorMessage="Keyword Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
           </asp:TableCell>

      
        </asp:TableRow>
        	<asp:TableRow ID="trAuth" Visible="false">
			<asp:TableCell Width="20%" ColumnSpan="2">
				<asp:Label ID="lblUpload" Runat="Server" CssClass="LABEL" Text="Enter Valiation Code :"></asp:Label>
			
			
				<asp:TextBox ID="txtAuthCode" TextMode="Password" Runat="Server"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
       
        <asp:TableRow ID="trbutton">
        <asp:TableCell runat ="server" ColumnSpan ="2">
        <asp:Button ID="btnSubmit" Text="Submit" runat="server" />
        <asp:Button ID="btnReset" Text="Cancel" runat="server"/>
        <asp:Button ID="btnBack" Text="  Back  " runat="server"/>
        </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    
    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please enter date in the correct format (dd/mm/yyyy)" 
             ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" ControlToValidate="txtPayDate" Display="None" >
   </asp:RegularExpressionValidator>
      <asp:RequiredFieldValidator ID="rfvAuthCode" Runat="Server" ErrorMessage="Validation Code Cannot be blank" Display="None" ControlToValidate="txtAuthCode"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{6,24}" ErrorMessage="Validation Code must be 6 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
    <asp:ValidationSummary runat="Server" ID="vsUpload" EnableClientScript="true" ShowMessageBox="true"
        ShowSummary="false" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
    

</asp:Content>