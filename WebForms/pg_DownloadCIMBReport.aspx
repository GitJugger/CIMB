<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pg_DownloadCIMBReport.aspx.vb" Inherits="MaxPayroll.pg_DownloadCIMBReport" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

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
		//  retval = window.showModalDialog(sPath);
		//  if (retval == true)
		//     window.location='pg_logout.aspx'

  //      }
        function fncShowAccOrgCode(sId, sAccId, sAccNo) {
            sPath = 'pg_BankOrgCode.aspx?OrgId=' + sId + '&AccId=' + sAccId + '&AccNo=' + sAccNo

            var w = 800;
            var h = 500;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var targetWin = window.open(sPath, '_blank', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

        }
		var reloadTimer = null;
        var sURL = unescape(window.location.pathname);

		function setReloadTime(secs) 
        { //this function is used to refresh the broswer
            if (arguments.length == 1) 
            { //if some seconds are passed in then create and set the timer and 
              //have it call this function again with no seconds passed in
                if (reloadTimer)
               clearTimeout(reloadTimer);

                reloadTimer = setTimeout("setReloadTime()", 
                                 Math.ceil(parseFloat(secs) * 1000));
            }
            else 
            { //No seconds were passed in the timer must be up clear the timer 
             //and refresh the browser
                reloadTimer = null;
                //passing true causes the request to go back to the web server
                // false refreshs the page from history
                //This is javascript 1.2
                location.reload(unescape(window.location.pathname));
                 //This is javascript 1.1
                //window.location.replace( sURL );
            }
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
            <td id="cHeader"><asp:Label ID="lblHeading" CssClass="FORMHEAD" Runat="Server" Text="" Visible="false"></asp:Label></td>
        </tr>
        <tr>
            <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
        </tr>
    </table>
    <table cellpadding="8" cellspacing="0" width="100%" border="0">
        <tr>
            <td>
                <asp:Panel ID="pnlDownloadedFile" runat="server" CssClass="GridDiv" ScrollBars="Vertical">
				    <asp:DataGrid ID="dgDownloadedFile" Runat="Server" AllowPaging="True" AllowSorting="False" AutoGenerateColumns="False" PagerStyle-Mode="NumericPages"  CellPadding="3" CellSpacing="0" PageSize="15" GridLines="none" PagerStyle-HorizontalAlign="Center" Font-Names="Verdana" Font-Size="8pt" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" Width="98%" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" DataKeyField="FileType">
		                <Columns>
		                     <asp:BoundColumn DataField="FileName" Visible="false"></asp:BoundColumn>
		                    <asp:TemplateColumn HeaderText="Report">
		                        <ItemTemplate>
		                            <a id="lnkNewFile" runat="server"></a>
		                            <asp:LinkButton CommandName="Download" runat="server" ID="lnkBtnNewFile" ></asp:LinkButton>		                            
		                        </ItemTemplate>
		                    </asp:TemplateColumn>
		                    <asp:BoundColumn DataField="FileType" HeaderText="File Type"></asp:BoundColumn>
		                    <asp:BoundColumn DataField="FileDateTime" HeaderText="Create Date"></asp:BoundColumn>
		                    <asp:BoundColumn DataField="OriFileName" HeaderText="File Name"></asp:BoundColumn>      
		                </Columns>		    
	                </asp:DataGrid>
	            </asp:Panel>
            </td>
        </tr>
    </table>
	 

</asp:Content>

