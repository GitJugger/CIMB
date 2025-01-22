<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pg_BankAccount3.aspx.vb" Inherits="MaxPayroll.PG_BankAccount3" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
     <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			window.location.href = "PG_ViewOrganisation.aspx?Req=Bank";
		}
		//function fncShowAccOrgCode(sId, sAccId, sAccNo)
		//{
		//  sPath = 'pg_BankOrgCode.aspx?OrgId=' + sId + '&AccId=' + sAccId + '&AccNo=' + sAccNo
  //          //retval = window.showModalDialog(sPath);
  //        retval = window.open(sPath, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=200, left=500, width=800, height=600");
		//  if (retval == true)
		//     window.location='pg_logout.aspx'

  //      }
        function fncShowAccOrgCode(sId, sAccId, sAccNo)
        {
            sPath = 'pg_BankOrgCode.aspx?OrgId=' + sId + '&AccId=' + sAccId + '&AccNo=' + sAccNo

            var w = 800;
            var h = 500;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var targetWin = window.open(sPath, '_blank', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

        }
		
		function ddPaymentTypeChanged()
		{
		    //
		    if (document.forms[0].ctl00$cphContent$ddlPaymentType.options[document.forms[0].ctl00$cphContent$ddlPaymentType.selectedIndex].value==6){
		        
		        document.all('ctl00$cphContent$ddlAccType').selectedIndex = 0;
		        document.all('ctl00$cphContent$lblAccType').style.visibility = 'visible';
		        document.all('ctl00$cphContent$ddlAccType').style.visibility = 'visible';
		        }
		    else{
		        document.all('ctl00$cphContent$ddlAccType').selectedIndex = 0;
		        document.all('ctl00$cphContent$lblAccType').style.visibility = 'hidden';
		        document.all('ctl00$cphContent$ddlAccType').style.visibility = 'hidden';
		        }
		}
    </script>
    
 	
    
	<!-- Main Table Starts Here -->
	 <table id="tblMain" runat="server" cellpadding="8" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label ID="lblHeading" CssClass="FORMHEAD" Runat="Server" Text=""></asp:Label></td>
      </tr>
      <tr>
         <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
   </table>
	 <table cellpadding="0" cellspacing="8" border="0" style="width:100%" runat="server" id="tblSearch">
	 <tr>
	     <td width="20%"></td>
	     <td width="25%"></td>
	     <td width="20%"></td>
	     <td width="25%"></td>
	     <td width="10%"></td>
	 </tr>
      <tr>
         
          <td><asp:label ID="lblBank" Text="Bank" runat="server"></asp:label></td>
         <td>
           <asp:DropDownList ID="ddlBankName" Width="150" Runat="Server" CssClass="MEDIUMTEXT" DataValueField="BankId"  DataTextField="BankName" ></asp:DropDownList>
         </td>
          <td></td>
         <td></td>
         <td></td>
      </tr>
      <tr>
         <td><asp:label ID="lblAccName" Text="Account Name" runat="server"></asp:label></td>
         <td><asp:TextBox ID="txtAccName" Width="150" runat="server" CssClass="LARGETEXT"></asp:TextBox><asp:RequiredFieldValidator ID="rfvAccName" Runat="Server" Display="None" ErrorMessage="Account Name Cannot be blank" ControlToValidate="txtAccName"></asp:RequiredFieldValidator></td>
         <td></td>
         <td></td>
         <td></td>
      </tr>
      <tr>
         <td><asp:label ID="lblAccNo" Text="Account Number" Width="150" runat="server"></asp:label></td>
         <td><asp:TextBox ID="txtAccNo" runat="server" CssClass="LARGETEXT" MaxLength="16"></asp:TextBox><asp:RequiredFieldValidator ID="rfvAccNo" Runat="Server" Display="None" ErrorMessage="Account No. Cannot be blank" ControlToValidate="txtAccNo"></asp:RequiredFieldValidator></td>
            <td></td>
            <td></td>
            <td></td>
      </tr>
      <tr>
         <td></td>
         <td><asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="BUTTON" /></td>
         <td></td>
         <td></td>
         <td></td>
      </tr>
      </table>
	    <table width="100%" id="tblForm" cellpadding="0" cellspacing="8" border="0">
			   <tr>
			   <td colspan="3">
			      <asp:panel ID="pnlGrid" runat="server" CssClass="GridDivNoScroll" BorderWidth="1" Width="98%">
               <asp:DataGrid Width="98%" cssclass="Grid" ID="dgBankAccts" Runat="Server" AllowPaging="true" AllowSorting="False"
                AutoGenerateColumns="False" DataKeyField="ACCID" CellPadding="3" PageSize="15" GridLines="none"
                 PagerStyle-HorizontalAlign="Center" BorderWidth="0" HeaderStyle-CssClass="GridHeaderStyle" 
                 AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle"  HeaderStyle-HorizontalAlign="left"
                  PagerStyle-Mode="NumericPages" OnDeleteCommand="dgBankAccts_Delete"  OnPageIndexChanged="dgPageChanged">
                  <Columns>
                  <asp:BoundColumn Datafield="ACCID" HeaderText="From"  ReadOnly="True" Visible="False"></asp:BoundColumn>
                  <asp:BoundColumn Datafield="ACCNO"   ReadOnly="True" Visible="False"></asp:BoundColumn>
                  <asp:TemplateColumn ItemStyle-Height="30" HeaderText="No.">
                     <HeaderStyle Width="2%"></HeaderStyle>
                        <ItemTemplate>
                           <%#(dgBankAccts.CurrentPageIndex) * (dgBankAccts.PageSize) + (Container.ItemIndex + 1)%>
                        </ItemTemplate>
                  </asp:TemplateColumn>
                  <asp:BoundColumn HeaderText="Bank"  DataField="BankDesc"></asp:BoundColumn>
                  <asp:BoundColumn HeaderText="Account Name"  DataField="ACCNAME"></asp:BoundColumn>
                  <asp:BoundColumn HeaderText="Account Number" DataField="ACCNO"></asp:BoundColumn>
                  <asp:BoundColumn HeaderText="Payment Type" DataField="PaySerDesc" Visible="false"></asp:BoundColumn>
                  
                  <asp:TemplateColumn HeaderText="Bank Org. Code" >
                     <ItemTemplate>
                        <a href="javascript:fncShowAccOrgCode('<%# Request.QueryString("Id") %>','<%# Eval("ACCID") %>','<%# Eval("ACCNO") %>','<%# Eval("Account_Type") %>')"><asp:label ID="lblAddOrgCode" runat="server">Add/Modify</asp:label></a>
                     </ItemTemplate>
                  </asp:TemplateColumn>
                  <asp:BoundColumn Visible="false" HeaderText="Account&nbsp;Type" ></asp:BoundColumn>
                  <asp:TemplateColumn HeaderText="Action" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" CommandName="Delete" Text="Delete" runat="server" />
                    </ItemTemplate>
                  </asp:TemplateColumn>
                  <asp:BoundColumn HeaderText="" DataField="ACCID" Visible="false"></asp:BoundColumn>
                  <asp:ButtonColumn Text="Delete" ButtonType="PushButton" CommandName="Delete" HeaderText="Action" HeaderStyle-Width="8%" visible="false"></asp:ButtonColumn>
                  <asp:BoundColumn DataField="Account_Type" Visible="false"></asp:BoundColumn>
               </Columns>
            </asp:DataGrid>
         </asp:panel>
			</td>
		</tr>
		<tr>
			<td style="width:20%">	
				<asp:Label ID="Label1" CssClass="LABEL" Runat="Server" Text="Subscription Fees Debit A/C"></asp:Label>
			</td>
			<td colspan="2" align="left" >
				<asp:DropDownList ID="ddlPrimary" Width="300px" Runat="server"></asp:DropDownList>&nbsp;<asp:Button ID="btnPrimary" Runat="server" CssClass="BUTTON" Text="Update" CausesValidation="False"></asp:Button>
			</td>
		</tr>
		<tr><td colspan="3">&nbsp;</td> </tr>
		<tr>
			<td colspan="3">
				<input id="Button1" type="button" runat="server" value="Back To View" class="BUTTON" onclick="fncBack();"/><input type="hidden" runat="server" id="hAction" name="hAction"/>
	<input type="hidden" runat="server" id="hStatus" name="hStatus"/><asp:ValidationSummary Runat="Server" ShowMessageBox="True" ShowSummary="False" ID="valSummary" EnableClientScript="True" HeaderText="Please Incorporate The Below Validations,"></asp:ValidationSummary>
			</td>
		</tr>
		</table>

</asp:Content>

