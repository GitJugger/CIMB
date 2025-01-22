<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pg_CPSEditCharges.aspx.vb" Inherits="MaxPayroll.pg_CPSEditCharges" 
MasterPageFile="~/WebForms/mp_Master.master"  %> 


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server"><table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Maintainence Charges</asp:Label></td>
      </tr>
      <tr>
         <td><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
  </table>
  <script type="text/javascript" src="../include/common.js"> </script>
  <script type="text/javascript">

function NumericDecimalOnly()
{
var key = window.event.keyCode; 

if (key <48 || key >57)
{
if (key != 46) 
window.event.returnValue = false; 
}
}

function NumericOnly()
{
var key = window.event.keyCode; 

if (key <48 || key >57)
{
window.event.returnValue = false; 
}
}
function showConfirm(){
var bConfirm = confirm('Are you sure?');
return bConfirm;
}
function getMessage() 
{ 
var ans; 
var ans=confirm('Is it your confirmation.....?'); 
if (ans==true) 
{ 
Hidden1.value='Yes'; 
} 
else 
{ 
Hidden1.value='No';} 
} 

</script>

<asp:Table ID ="tblMain" runat="server" Width= "300px">
	<asp:TableRow>
	<asp:TableHeaderCell Text="Select Type of Charges" ></asp:TableHeaderCell>	
	</asp:TableRow>
	<asp:TableRow ID ="tr_RadioList">
	<asp:TableHeaderCell>
    <asp:RadioButtonList ID="rbtlCharges" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
    <asp:ListItem Text="Fixed" Value="1"></asp:ListItem>
    <asp:ListItem Text="Tier" Value="2"></asp:ListItem>
    </asp:RadioButtonList>
    </asp:TableHeaderCell>
	</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
	<asp:Table ID="tblCharges" runat ="server" Width = "100%">

	<asp:TableRow></asp:TableRow>

	<asp:TableRow ID="tr_Fixed" HorizontalAlign="Left" >
	<asp:TableCell Width="50%" HorizontalAlign="Right" >
	<asp:Label ID="lblSubscription" runat="server" Text="Subscription Fees (RM):" CssClass="LABEL"></asp:Label>
	</asp:TableCell>
	<asp:TableCell HorizontalAlign="Left" Width="50%">
	<asp:TextBox ID="__FixedCharge" runat="server" CssClass="MEDIUMTEXT" MaxLength="30" OnKeyPress="NumericDecimalOnly()" ></asp:TextBox>
	</asp:TableCell>
	<asp:TableCell>
		<asp:RequiredFieldValidator ID="reqfFixedFees" runat="server" ErrorMessage="Subscripton fee cannot be blank" 
	ControlToValidate="__FixedCharge"  Display="None"></asp:RequiredFieldValidator>
	</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="tr_Tier" HorizontalAlign= "Left">
	<asp:TableCell>
	<asp:GridView ID="grdTire" runat="server" AutoGenerateColumns="False" DataKeyNames="CHARGE_ID" ShowFooter="True" >
    <Columns>
		 <asp:TemplateField HeaderText="Transactions From"> 
                <EditItemTemplate> 
                    <asp:TextBox ID="txtFrom" runat="server" Text='<%# Bind("TRANS_FROM") %>'></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="txtNewFrom" Text='<%# GetStartVal(Eval("Trans_From")) %>' runat="server"  ReadOnly="true"></asp:TextBox> 
                </FooterTemplate> 
                <ItemTemplate> 
                    <asp:Label ID="lblFrom" runat="server" Text='<%# Bind("TRANS_FROM") %>'></asp:Label> 
                </ItemTemplate> 
             <HeaderStyle HorizontalAlign="Left" />
          </asp:TemplateField> 
          <asp:TemplateField HeaderText="Transactions To" > 
                <EditItemTemplate> 
                    <asp:TextBox ID="txtTo" runat="server" Text='<%# Bind("TRANS_TO") %>'></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate> 
                    <asp:TextBox ID="txtNewTo" runat="server" OnKeyPress="NumericOnly()"></asp:TextBox> 
          <%--          <asp:RequiredFieldValidator ID="reqfTireTo" runat="server" ErrorMessage="* Cannot be Empty" 
                    ControlToValidate="txtNewTo" Display="None" ></asp:RequiredFieldValidator>--%>
                </FooterTemplate> 
                <ItemTemplate> 
                    <asp:Label ID="lblTo" runat="server" Text='<%# Bind("TRANS_TO") %>'></asp:Label> 
                </ItemTemplate> 
              <HeaderStyle HorizontalAlign="Left" />
          </asp:TemplateField> 
          <asp:TemplateField HeaderText="Transaction Charges"> 
                <EditItemTemplate> 
                    <asp:TextBox ID="txtCharge" runat="server" Text='<%# Bind("TRANS_CHARGE") %>'></asp:TextBox> 
                </EditItemTemplate> 
                <FooterTemplate>       
                    <asp:TextBox ID="txtNewCharge" runat="server" OnKeyPress="NumericDecimalOnly()"></asp:TextBox> 
                <%--    <asp:RequiredFieldValidator ID="reqfTireCharges" runat="server" ErrorMessage="* Cannot be Empty" 
                    ControlToValidate="txtNewCharge" Display="None" ></asp:RequiredFieldValidator>--%>
                </FooterTemplate> 
                <ItemTemplate> 
                    <asp:Label ID="lblCharge" runat="server" Text='<%# Bind("TRANS_CHARGE") %>'></asp:Label> 
                </ItemTemplate> 
              <HeaderStyle HorizontalAlign="Left" />
          </asp:TemplateField> 
          <asp:TemplateField  ShowHeader="False"> 
                <FooterTemplate> 
                    <asp:LinkButton ID="lnkAdd" runat="server" CausesValidation="True"  
                    CommandName="Insert" Text="Insert" CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton> 
                </FooterTemplate> 
              <HeaderStyle HorizontalAlign="Left" />
          </asp:TemplateField> 
	    </Columns>
    </asp:GridView>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell ID= "tc_PreviousCharges" Visible="false" Text= "Previous Charges ">
        <asp:GridView ID="grdPreviousCharge" runat="server" >
        </asp:GridView>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow></asp:TableRow>
    <asp:TableRow></asp:TableRow>
	    <asp:TableRow ID="tr_FixedButtons">
	    <asp:TableCell Width="100%" HorizontalAlign="Left">
	    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="SHORTBUTTON" />&nbsp;
	    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="SHORTBUTTON" />
	    </asp:TableCell>
	    </asp:TableRow>
	    <asp:TableRow></asp:TableRow>
    <asp:TableRow >
    <asp:TableCell>
        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="SHORTBUTTON"
        OnClientClick="return confirm('Are you sure you want to delete?');" />
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow></asp:TableRow>
    <asp:TableRow></asp:TableRow>

    </asp:Table>
    
  </asp:TableCell>  
 </asp:TableRow>
<asp:TableRow ID="tr_Note">
<asp:TableCell>
    <asp:Label ID="lblNote" runat="server" Text=" Note : Please put '0' for unlimited TransactionsTo " 
    ForeColor="red"></asp:Label>
</asp:TableCell>
</asp:TableRow>
</asp:Table>

 	<input type="hidden" runat="server" id="__OrgID" name="__OrgID" value="0"/>
 	<input type="hidden" runat="server" id="__FixedChargeID" name="__FixedChargeID" value="0"/>
 	<input type="hidden" runat="server" id="Hidden1" name="TestHidden" value="0"/>
</asp:Content>