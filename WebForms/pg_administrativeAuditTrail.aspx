<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_AdministrativeAuditTrail" CodeFile="PG_AdministrativeAuditTrail.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>

    <link type="text/css" href="../Include/Styles.css" rel="stylesheet">
    <script type="text/javascript" src="../Include/js/Plugins/datatables.min.js"></script>
    <link type="text/css" href="../Include/js/Plugins/datatables.min.css" rel="stylesheet">
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
    <script type="text/javascript" src="../include/js/Forms/administrativetrail.js"></script>


    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading"></asp:Label></td>
        </tr>
    </table>

    <asp:ValidationSummary DisplayMode="BulletList" ID="lblValidator" runat="server"
        ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:"></asp:ValidationSummary>

    <asp:Table Width="100%" runat="Server" ID="Table1" CellPadding="8" CellSpacing="0" Visible="true">

        <asp:TableRow>

            <asp:TableCell Width="100%">
                <!-- Search Table Starts Here -->
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="conditional">
                    <ContentTemplate>
                        <asp:Table Width="90%" CellPadding="2" CellSpacing="0" ID="Table2" runat="Server">
                            <asp:TableRow Visible="false">
                                <asp:TableCell Width="90%" ColumnSpan="2">
                                    <asp:RadioButton ID="radAll" runat="Server" CssClass="LABEL" GroupName="SearchOptions" Text="All Records" Checked="True"></asp:RadioButton>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="radFrom" runat="Server" CssClass="LABEL" Text="Audit From Date"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="70%">
                                    <asp:TextBox ID="textFromDt" AutoPostBack="true" runat="Server" CssClass="SMALLTEXT" MaxLength="10"></asp:TextBox>&nbsp;
				<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$textFromDt, "dd/mm/yyyy");'>
                    <img src="../Include/Images/date.gif" border="0" /></a>
                                    <asp:Label runat="Server" CssClass="LABEL" Text="Audit To Date" ID="Label2"></asp:Label>&nbsp;
				<asp:TextBox ID="textToDt" runat="Server" CssClass="SMALLTEXT" MaxLength="10"></asp:TextBox>&nbsp;
				<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$textToDt, "dd/mm/yyyy");'>
                    <img src="../Include/Images/date.gif" border="0" /></a>
                                    <asp:RegularExpressionValidator ID="reveFromDate" runat="server" ControlToValidate="textFromDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
                                    <asp:RegularExpressionValidator ID="reveToDate" runat="server" ControlToValidate="textToDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="radUserId" runat="Server" CssClass="LABEL" Text="User Id"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="70%">
                                    <asp:TextBox ID="textUserId" runat="Server" CssClass="MEDIUMTEXT" MaxLength="14"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="textUserId" ValidationExpression="^[\w\-\s]+$" ErrorMessage="UserId Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="radUserName" runat="Server" CssClass="LABEL" Text="User Name"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="70%">
                                    <asp:TextBox ID="textUserName" runat="Server" CssClass="MEDIUMTEXT" MaxLength="20"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="textUserName" ValidationExpression="^[\w\-\s]+$" ErrorMessage="UserName Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
                <table cellpadding="2" border="0" width="100%" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSearchs" runat="Server" CssClass="BUTTON" Text="Search"></asp:Button>&nbsp;
				<asp:Button ID="btnClears" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button></td>
                    </tr>
                </table>
                <!-- Search Table Ends Here -->
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="100%">
		
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="100%">&nbsp;</asp:TableCell>
        </asp:TableRow>
    </asp:Table>



    <table id="example" class="display" style="width: 100%; border-color: Black; border-width: 1px; border-style: solid; font-size: 8pt; border-collapse: collapse;">
        <thead style="background: #CCCCCC;">
            <tr class="hidden-row">
                <th colspan="6"></th>
            </tr>

            <tr class="hidden-row">
                <th colspan="6">Report Date : <p id="dateTime"></p>

                </th>
            </tr>

            <tr class="hidden-row">
                <th colspan="6">Report Generated By : <%=Names%> </th>
            </tr>

            <tr class="hidden-row">
                <th colspan="6"></th>
            </tr>
            <tr>
                <th>Date Time</th>
                <th>Performed By</th>
                <th>User Id</th>
                <th>Action</th>
                <th>Before Value</th>
                <th>After Value</th>

            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Date_Time") %></td>
                        <td><%# Eval("Performed_By") %></td>
                        <td><%# Eval("User_Id") %></td>
                        <td><%# Eval("Trans_Field") %></td>
                        <td><%# Eval("Old_Data") %></td>
                        <td><%# Eval("New_Data") %></td>

                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

</asp:Content>
