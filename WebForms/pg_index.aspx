<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_Index" CodeFile="pg_index.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
  

    <script type="text/javascript" language="JavaScript">
			function popUp(url) 
			{
				sealWin = window.open(url,"win",'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=500,height=450');
				self.name = "mainWin";
				window.parent.location.reload();
			}
			function Page_Init()
			{
				document.forms[0].ctl00$cphContent$txtOrgId.focus();
			}
    </script>

    <table id="tblMenu" border="0" cellpadding="2" cellspacing="2" style="width: 100%"
        class="menutitle">
        <tr>
            <td colspan="2" style="height: 20px" id="cHeader">&nbsp;&nbsp;&nbsp;</td>
        </tr>
        <tr>
            <td colspan="3">
                &nbsp;&nbsp;&nbsp;<asp:Label runat="Server" CssClass="MSG" ID="lblMsg"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 130px">
                &nbsp;&nbsp;&nbsp;<asp:Label runat="Server" ID="lblOrgID" Text="Organization ID"
                    CssClass="LABEL"></asp:Label>
            </td>
            <td style="width: 220px">
                <asp:TextBox runat="Server" ID="txtOrgId" AutoPostBack="False" CssClass="MEDIUMTEXT"
                    MaxLength="11" TabIndex="1" Width="170" BorderStyle="Solid" BorderWidth="1px"
                    BorderColor="Gray"></asp:TextBox>
            </td>
           
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label4" runat="server" Text="User ID" CssClass="LABEL"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="Server" ID="txtUserId" AutoPostBack="False" CssClass="MEDIUMTEXT"
                    MaxLength="16" TabIndex="2" Width="170" BorderStyle="Solid" BorderWidth="1px"
                    BorderColor="Gray"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label3" runat="Server" Text="Password" CssClass="LABEL"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="Server" ID="txtPassword" AutoPostBack="False" CssClass="MEDIUMTEXT"
                    MaxLength="24" TabIndex="3" Width="170" BorderStyle="Solid" BorderWidth="1px"
                    BorderColor="Gray" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left" style="height: 30px;">
                <asp:Button Text="Login" ID="btnSave" runat="Server" TabIndex="4" Width="82px"></asp:Button>&nbsp;<input
                    type="reset" value="Clear" style="width: 82px" />
            </td>
        </tr>
    </table>

    <table cellspacing="0" cellpadding="0" border="0" style="border-width: 0px; width: 99%;
        border-collapse: collapse;">
        <tr>
            <td>
                <br />
                &nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" Text="Reminder: " CssClass="NOTE" runat="Server"></asp:Label><asp:Label
                    ID="Label2" CssClass="LABEL" Text="Please do not reveal your Password to anyone. "
                    runat="Server"></asp:Label>
              
            </td>
        </tr>
        <tr>
            <td>&nbsp;
            </td>
        </tr>
         <tr>
            <td>&nbsp;
            </td>
        </tr>
    
    </table>
    <asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please incorporate the following validations;"
        runat="Server" EnableClientScript="True" ID="frmValidator"></asp:ValidationSummary>
    <asp:RequiredFieldValidator ErrorMessage="Organization ID" ID="valOrgId" runat="Server"
        Text="" ControlToValidate="txtOrgId" Display="none"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ErrorMessage="User ID" ID="valUserId" runat="Server"
        Text="" ControlToValidate="txtUserId" Display="none"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ErrorMessage="Password" ID="valPwd" runat="Server" Text=""
        ControlToValidate="txtPassword" Display="none"></asp:RequiredFieldValidator>
    <!-- Main Table Starts Here -->

    <script type="text/javascript">
//document.all('cHeader').style.filter='progid:DXImageTransform.Microsoft.Gradient(gradientType=1,startColorStr=#FDD55C,endColorStr=#DE9B63)';
    </script>

</asp:Content>
