<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pg_BankList.aspx.vb" Inherits="WebForms_pg_BankMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<% Response.CacheControl = "no-cache" %>
<% Response.AddHeader("Pragma", "no-cache") %>
<% Response.Expires = -1 %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Bank Master</title>
    <link href="Styles.css" rel="StyleSheet" type="text/css" />
    
</head>
<body onload="countDown()" onmousemove="resetCounter()" onclick="resetCounter()">
    <form id="form1" runat="server">
    <table width="100%" cellpadding="1" cellspacing="2" border="0">
        <tr>
            <td Class="FORMHEAD">Create/Modify Bank Information</td>
        </tr>
        <tr>
            <td>
                <table cellpadding=0 cellspacing=0 border=0 width=100%>
                    <tr>
                        <td width="20%"></td>
                        <td></td>
                    </tr>
                    <tr>
	                    <td>Bank</td>
	                    <td>
                            <asp:DropDownList ID="ddBank" runat="server" Width="150px">
                            </asp:DropDownList>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" /></td>
	                </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btnCreate" runat="server" Text="Create" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan=2>
                            <asp:GridView ID="gvBank" runat="server" AutoGenerateColumns="False" AllowPaging=true AllowSorting=true PageSize=20 PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" BorderWidth="1" GridLines="Both" CellPadding="3" CellSpacing="0" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" width="100%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center">
                            <Columns>
                                <asp:BoundField DataField="BankCode"  HeaderText="Bank Code"/>
                                <asp:BoundField DataField="BankDesc" HeaderText="Description" />
                                <asp:TemplateField HeaderText="Select">
                                    <ItemTemplate>
                                        <a href="PG_BankFormat.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"BankCode")%>">Modify/Delete</a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
	   
	</table> 
    </form>
</body>
</html>
