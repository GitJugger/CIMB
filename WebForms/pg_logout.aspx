<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_Logout" CodeFile="PG_Logout.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<script type="text/javascript" >
	function createCookie(name,value,days) 
	{
	    if (days) 
	    {
		    var date = new Date();
		    date.setTime(date.getTime()+(days*24*60*60*1000));
		    var expires = "; expires="+date.toGMTString();
	    }
	    else var expires = "";
	    document.cookie = name+"="+value+expires+"; path=/";
	    
    }
    createCookie("subMenu","",-1);
     function logout()
     {
        
        window.location = 'PG_Index.aspx';
     }
    </script>
			<table width="100%" cellpadding="0" cellspacing="0">
				<tr>
					<td style="width:100%"></td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				
				<% If Request.QueryString("Timed") = "" Then %>
				<tr>
					<td align="center" class="LABEL"> 
						<asp:Label ID="lbl01" Runat="server">You have successfully logged out.</asp:Label>
						<asp:Label ID="lblLogout" Runat="server">
						Thank you for using our payment service.<br/><br/>
						To Login again, please click <a href="#javascript:void(0);" onclick="javascript:logout();">here</a>.
						<br/><br/>
						Please remember to clear your browser's cache before you close your Internet browser.
						</asp:Label>
						</td>
				</tr>
				
				<% Else %>
				<tr>
					<td align="center" class="LABEL"> 
						<asp:Label ID="lbl02" Runat="server">Your login session has timed out.</asp:Label>
						<asp:Label ID="lblLogout02" Runat="server">
						Thank you for using our payment service<br/><br/>
						To Login again, please click <a  href="#javascript:void(0);" onclick="javascript:logout();">here</a>.
						<br/><br/>
						Please remember to clear your browser's cache before you close your Internet browser.
						</asp:Label>
						</td>
				</tr>
				<% End If %>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
				</tr>
				
				
			</table>

</asp:Content>
