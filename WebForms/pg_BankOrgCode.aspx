<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pg_BankOrgCode.aspx.vb" Inherits="MaxPayroll.pg_BankOrgCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Expires" content="0">
    <meta http-equiv="Cache-Control" content="no-cache">
    <meta http-equiv="Pragma" content="no-cache">

    <base target="_self" />
    <title>Bank Organization Code Maintenance</title>
    <script type="text/javascript" src="../include/common.js"></script>
    <link href="../Include/Styles.css" rel="stylesheet" type="text/css" />
</head>
<body id="body" runat="server" >
    <form id="form1" runat="server">
      <table id="tblMain" runat="server" cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader"><asp:Label ID="lblHeading" CssClass="FORMHEAD" Runat="Server" Text="Bank Organization Code Maintenance"></asp:Label></td>
         </tr>
         <tr>
            <td><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
         </tr>
      </table>
      <table cellpadding="8" cellspacing="0" width="100%">
         <tr>
            <td style="width:40%"><asp:label ID="lblTAccNo" runat="server" Text="Account No."></asp:label></td>
            <td style="width:60%"><asp:Label ID="lblAccNo" runat="server"></asp:Label></td>
         </tr>
         <tr>
            <td><asp:label ID="lblTBnkOrgCode" runat="server" Text="New Bank Organization Code"></asp:label>&nbsp;<span style="color:red">*</span></td>
            <td><asp:TextBox ID="txtBnkOrgCode" runat="server" CssClass="LARGETEXT" MaxLength="5"></asp:TextBox></td>
         </tr>
         <tr>
          
            <td colspan="2" align="center"><asp:Button ID="btnAdd" CssClass="BUTTON" runat="server" Text="Add" />&nbsp;
                <asp:Button ID="btnDelete" CssClass="BUTTON" runat="server" Text="Delete" />&nbsp;
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="BUTTON" /></td>
         </tr>
         <tr>
            <td colspan="2">
            <div style="height: 300px; overflow:auto;">
               <asp:panel id="pnlGrid" BorderWidth="1" Height="300px" runat="server"> 

                  <asp:DataGrid Runat="Server" ID="dgBankOrgCode" AllowSorting="false" AllowPaging="false" PageSize="15" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" GridLines="none" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" width="100%" HeaderStyle-Font-Bold="True"  AutoGenerateColumns="false" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                     <Columns>
                        <asp:TemplateColumn>
                           <ItemTemplate>
                              <asp:CheckBox ID="chkSelect" runat="server" />
                           </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="Org_ID" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="Account_No" Visible="false"></asp:BoundColumn>
                        <asp:BoundColumn DataField="BankOrgCode" HeaderText="Bank Organization Code"></asp:BoundColumn>
                     </Columns>
                   </asp:DataGrid>
                </asp:panel>
               </div>
            </td>
         </tr>
      </table>
        
    </form>
</body>
</html>
