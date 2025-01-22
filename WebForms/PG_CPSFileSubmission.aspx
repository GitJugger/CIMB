<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="PG_CPSFileSubmission.aspx.vb" Inherits="Maxpayroll.WebForms_PG_CPSFileSubmission"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:Table ID="tblCPSChequeInfo" runat ="server" Height="162px" Width="490px" >
       	<asp:TableRow ID="trDesc" runat="server">
       	<asp:TableCell ID="TableCell1" Wrap="False" runat="server">
       	 <asp:Label ID="lblHeader" runat="server" Text="Cheque Number Replacement" Font-Bold></asp:Label>
           <br /><br />
           <asp:Label runat="server" ID="lblError" Visible="false" ForeColor="red" Text=""></asp:Label>
           <br /><br />
           </asp:TableCell>
        </asp:TableRow>
        	
      <asp:TableRow ID="trOption" runat="server" >
      <asp:TableCell runat="server" >
      <asp:RadioButtonList runat="server" ID="rblOption" AutoPostBack="true"  >
      <asp:ListItem Text="File Submission for processing" Selected="True" ></asp:ListItem>
      <asp:ListItem Text="Cheque No Replacement" ></asp:ListItem>
      </asp:RadioButtonList>
      </asp:TableCell>
      </asp:TableRow>
       <%--	<asp:TableRow ID="trRecordWithNo" runat="server" Visible="false"  >
         <asp:TableCell ID="TableCell4" Wrap="False" runat="server" ColumnSpan="2">
                Number of Records With Cheque No Assign by Customer&nbsp;&nbsp;: &nbsp;&nbsp;
                 <asp:Label ID="lblNoRecWithNo" runat="server" Text=""></asp:Label>&nbsp;&nbsp;
                  <asp:LinkButton ID="lbDetailsRecords" runat="server">Details</asp:LinkButton>
                </asp:TableCell>
                
   
    
         
         
        </asp:TableRow>--%>
                     <asp:TableRow ID="trGridDetailRecord" Visible="false"  >
            <asp:TableCell ID="TableCell5" Wrap="False" runat="server" >
                
             
                
                <asp:DataGrid ID="dgRecordwithNo" runat="server" AutoGenerateColumns="False" >
                
    <Columns>
     <asp:BoundColumn DataField ="ROW" HeaderText="Row" HeaderStyle-Font-Bold="true" HeaderStyle-Height="10px" HeaderStyle-Width = "30px" />
    <asp:BoundColumn DataField ="ACCOUNT_NO" HeaderText="Account Number" HeaderStyle-Font-Bold="true" HeaderStyle-Height="10px" />
    <asp:BoundColumn DataField ="CHEQUE_NO" HeaderText="Cheque No Used" HeaderStyle-Font-Bold="true" HeaderStyle-Height="10px" />
  
		<asp:TemplateColumn HeaderText="Assign New Cheque No." HeaderStyle-Font-Bold="true" HeaderStyle-Height="10px"> 

                <ItemTemplate> <center>
                    <asp:TextBox ID="txtNewChequeNo" runat="server"/></center>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Cheque Number must be numeric only." 
             ValidationExpression="^\d+$" ControlToValidate="txtNewChequeNo">             
            </asp:RegularExpressionValidator>
           
                </ItemTemplate> 
                
                </asp:TemplateColumn> 

	</Columns>
</asp:DataGrid>
<%--<br /><br />
Next Available Cheque No: 
<asp:Label runat="server" id="lblNextNo"></asp:Label> <br />
 <asp:CheckBox ID="cbChequeNewBatch" runat="server"  AutoPostBack="true"/>Replace Cheque Number with New Batch<br />
 Enter Cheque number<asp:TextBox ID="txtNewBatchNo" runat="server" Enabled="false" AutoPostBack="true" ></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Cheque Number must be numeric only." 
             ValidationExpression="^\d+$" ControlToValidate="txtNewBatchNo">             
            </asp:RegularExpressionValidator>--%>
                </asp:TableCell>
                
      
       </asp:TableRow>
        
             <%--Grid Record END--%>
        
        
          	
       
       
      
        
       
        	<asp:TableRow ID="trAuth" Visible="false">
			<asp:TableCell Width="20%" ColumnSpan="2">
				<asp:Label ID="lblUpload" Runat="Server" CssClass="LABEL" Text="Enter Valiation Code :"></asp:Label>
			
			
				<asp:TextBox ID="txtAuthCode" TextMode="Password" Runat="Server"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
       
        <asp:TableRow ID="trbutton">
        <asp:TableCell ID="TableCell7" runat ="server" ColumnSpan ="2">
        <asp:Button ID="btnSubmit" Text="Submit" runat="server" />
        <asp:Button ID="btnReset" Text="Cancel" runat="server"/>
        <asp:Button ID="btnBack" Text="  Back  " runat="server"/>
        </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    
    
    <asp:RequiredFieldValidator ID="rfvAuthCode" Runat="Server" ErrorMessage="Validation Code Cannot be blank" Display="None" ControlToValidate="txtAuthCode"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{6,24}" ErrorMessage="Validation Code must be 6 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
    	<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields,"
					ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
</asp:Content>