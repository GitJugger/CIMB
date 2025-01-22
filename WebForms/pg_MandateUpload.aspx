<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_MandateUpload"
    CodeFile="pg_MandateUpload.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master"
    EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>

    <script type="text/javaScript">
      function fncBack()
      {
      
        window.history.back();
      }
      function fncNew()
      {
          
        window.location.href = "PG_MandateUpload.aspx";
      }
      function fncProgressBar()
      {
         
         Page_InvalidControlToBeFocused = null;
     
         if (typeof(Page_Validators) == "undefined") 
         {
            return true;
         }
         
         var i;
         for (i = 0; i < Page_Validators.length; i++) 
         {
            ValidatorValidate(Page_Validators[i], '', null);
         }
         ValidatorUpdateIsValid();
       //  if (Page_IsValid == true)
         //   window.showModelessDialog('progress.aspx','','dialogHeight: 100px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: yes; status: No;scroll:yes;');
       }
       
       
    </script>

    <script type="text/javascript" src="PG_Calendar.js"></script>

    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader" style="width:45%; height:18">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">&nbsp;File Upload</asp:Label></td>
            <td align="left">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Include/Images/mozilla_blu.gif" />&nbsp;<font color="gray">Loading</font>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
           
        </tr>
    </table>
    <%-- Main Table Starts Here --%>
    <table width="100%" cellpadding="8" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblMessage" runat="Server" CssClass="MSG"></asp:Label></td>
        </tr>
    </table>

         
            <table cellSpacing="0" cellPadding="8 "width="100%" border="0">
                <tr>
                    <td style="WIDTH: 20%">Mandate File</td>
                    <td style="WIDTH: 80%"><asp:Panel id="pnlUpload" runat="server"> <input id="flUpload" type="file" size="50" name="flUpload" runat="Server" /><asp:RequiredFieldValidator ID="rfvFile" runat="Server" ControlToValidate="flUpload"
        ErrorMessage="Select Upload File" Display="None"></asp:RequiredFieldValidator></asp:Panel><asp:Label id="lblFileName" runat="server"></asp:Label></td>
                </tr>
                <asp:Panel id="pnlAuthorize" runat="server">
                    <tr>
                        <td>Validation Code</td>
                        <td><asp:TextBox id="txtValidationCode" Runat="Server" TextMode="Password" MaxLength="24" CssClass="MEDIUMTEXT"></asp:TextBox></td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td colSpan="2"><asp:Button id="btnSave" runat="Server" CssClass="BUTTON" Text="Submit" OnClientClick="javascript:fncProgressBar();" OnClick="btnSave_Click"></asp:Button>&nbsp;<input id="Button1" class="BUTTON" onclick="fncNew();" type="button" value="Clear" runat="server" />
                    </td>
                </tr>
            </table>
        
    &nbsp;<%-- Main Table Ends Here --%><%-- Validation Controls Starts Here --%>
   
    &nbsp;
    <asp:RequiredFieldValidator ID="rfvAuth" runat="Server" ControlToValidate="txtValidationCode"
        ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
    &nbsp;
    <asp:RegularExpressionValidator ID="rgeAuthCode" runat="server" ControlToValidate="txtValidationCode"
        ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters"
        Display="None"></asp:RegularExpressionValidator>&nbsp;
    <asp:ValidationSummary runat="Server" ID="vsMandateUpload" EnableClientScript="true" ShowMessageBox="true"
        ShowSummary="false" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
    &nbsp;<%-- Validation Controls Starts Here --%><%--Hidden Boxes Starts here --%>
    &nbsp;
    <input type="hidden" id="hFileName" name="hFileName" runat="Server" />
    <input type="hidden" id="hAlert" name="hAlert" runat="Server" />
    &nbsp;
    <%--Hidden Boxes Ends Here --%>
</asp:Content>
