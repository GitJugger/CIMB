<%@ Page Title="" Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false"
 CodeFile="PG_ReportDownloader.aspx.vb" Inherits="MaxPayroll.PG_ReportDownloader" EnableEventValidation="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<script type="text/javascript" src="../include/common.js"></script>
<script type="text/javascript" language="JavaScript">
//Select Check box - Start
	function CheckAll(_CheckBox,_CheckBoxName)
    {
        var frmElements;
	    var IsChecked = _CheckBox.checked;
	    var frmInbox = document.forms[0];
	    for(Index = 0; Index < frmInbox.length; Index++)
	    {
		    frmElements = frmInbox.elements[Index];
		    if(frmElements.type == 'checkbox' && frmElements.name.indexOf(_CheckBoxName) != -1)
		    {
			    frmElements.checked = IsChecked;
		    }
	    }
    }
    //Select Check box - Stop

    //Select Check box - Start
    function btnSelectAll(IsChecked, _HeaderCheckBox, _CheckBoxName) {
        var frmElements;
        var frmInbox = document.forms[0];
        for (Index = 0; Index < frmInbox.length; Index++) {
            frmElements = frmInbox.elements[Index];
            if (frmElements.type == 'checkbox' && frmElements.name.indexOf(_CheckBoxName) != -1) {
                frmElements.checked = IsChecked;
            }
            else if (frmElements.type == 'checkbox' && frmElements.name.indexOf(_HeaderCheckBox) != -1) {
                frmElements.checked = IsChecked;
            }
        }
    }
    //Select Check box - Stop
</script>
<!-- Heading Table Starts Here -->
<table id="tblMain" runat="server" cellpadding="8" cellspacing="0" width="100%" border="0">
    <tr>
        <td id="cHeader"><asp:Label ID="lblHeading" CssClass="FORMHEAD" Runat="Server" Text="" Visible="false"></asp:Label></td>
    </tr>
    <tr>
        <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
    </tr>
</table>
<!-- Heading Table Ends Here -->

<!-- Form Table Starts Here -->
<asp:Table Width="100%" runat="Server" CellPadding="3" CellSpacing="2" ID="tblForm">
<asp:TableRow>
    <asp:TableCell Width="20%">
        <asp:Label ID="ServiceType" runat="Server" CssClass="LABEL" Text="File Type"></asp:Label>
    </asp:TableCell>
    <asp:TableCell Width="80%">
        <asp:DropDownList ID="ddlServiceType" runat="Server" CssClass="MEDIUMTEXT" AutoPostBack="True" OnSelectedIndexChanged="FileTypeChange">
        </asp:DropDownList>
    </asp:TableCell>
</asp:TableRow>
<asp:TableRow>
<asp:TableCell ColumnSpan="2">
    <asp:Panel ID="pnlDownloadedFile" runat="server" CssClass="GridDiv" ScrollBars="Vertical">
		<asp:DataGrid ID="dgDownloadedFile" Runat="Server" AllowPaging="True" AllowSorting="False" 
        AutoGenerateColumns="False" PagerStyle-Mode="NumericPages"  CellPadding="3" CellSpacing="0" 
        PageSize="15" GridLines="none" PagerStyle-HorizontalAlign="Center" Font-Names="Verdana" Font-Size="8pt" 
        CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" 
        AlternatingItemStyle-CssClass="GridAltItemStyle" Width="98%" ItemStyle-HorizontalAlign="left" 
        HeaderStyle-HorizontalAlign="left" DataKeyField="File_Type">
		    <Columns>
                <asp:TemplateColumn ItemStyle-Width="5%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                    <HeaderTemplate>
                        <input type="checkbox" runat="server" id="CheckAll" onclick="CheckAll(this,'Message_Select');"/>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" runat="server" id="Message_Select"/>
                        <input type="hidden" runat="server" id="Download_File" value='<%# DataBinder.Eval(Container.DataItem,"File_Name")%>'/>
                    </ItemTemplate>
                </asp:TemplateColumn>
		        <asp:BoundColumn DataField="File_Name" Visible="False"></asp:BoundColumn>
		        <asp:BoundColumn DataField="File_Type" HeaderText="File Type"></asp:BoundColumn>
		        <asp:BoundColumn DataField="File_DateTime" HeaderText="Create Date"></asp:BoundColumn>
		        <asp:BoundColumn DataField="Created_FileName" HeaderText="File Name"></asp:BoundColumn>      
		    </Columns>		    
	    </asp:DataGrid>
	</asp:Panel>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="trDelete">
        <asp:TableCell Width="100%" ColumnSpan="100">
        <input id="btnSelectAll" type="button" class="BUTTON" runat="server" value="Select All" onclick="btnSelectAll(true,'CheckAll','Message_Select');"/>&nbsp;
        <input id="btnUnSelectAll" type="button" class="BUTTON" runat="server" value="UnSelect All" onclick="btnSelectAll(false,'CheckAll','Message_Select');"/>&nbsp;
        <asp:Button ID="btnDownload" runat="server" Text="Download" CssClass="BUTTON"/>
        </asp:TableCell>
</asp:TableRow>
</asp:Table>
<!-- Form Table Ends Here -->
</asp:Content>