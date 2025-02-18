<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_AccessMatrix" CodeFile="PG_AccessMatrix.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>

    <link type="text/css" href="../Include/Styles.css" rel="stylesheet">
    <script type="text/javascript" src="../Include/js/Plugins/datatables.min.js"></script>
    <link type="text/css" href="../Include/js/Plugins/datatables.min.css" rel="stylesheet">
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
    <script type="text/javascript" src="../include/js/Forms/accessMatrix.js"></script>


    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="Server" ID="lblMessage" CssClass="MSG" Text="Role rights saved successfully." Visible="false"></asp:Label>
            </td>
        </tr>
    </table>

    <asp:ValidationSummary DisplayMode="BulletList" ID="lblValidator" runat="server"
        ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:"></asp:ValidationSummary>

    <asp:Table Width="100%" runat="Server" ID="Table1" CellPadding="8" CellSpacing="0" Visible="true">

        <asp:TableRow ID="trRoles">
            <asp:TableCell Width="15%">
                <asp:Label runat="Server" Text="Menu Name" CssClass="LABEL" ID="lblRole"></asp:Label>&nbsp;
                <asp:Label Text="*" runat="server" CssClass="MAND" ID="Label1"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Width="20%">
                <asp:DropDownList ID="cmbRoles" runat="Server" CssClass="MEDIUMTEXT"
                    >
                   <%-- <asp:ListItem Text="All" Value="1" Selected="True"></asp:ListItem>--%>
                </asp:DropDownList>
            </asp:TableCell>

            <asp:TableCell Width="60%" ColumnSpan="2">
                <asp:Button ID="btnSearchs" runat="Server" CssClass="BUTTON" Text="Search"></asp:Button>&nbsp;
                <asp:Button ID="btnClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
            </asp:TableCell>

        </asp:TableRow>

       <%-- <asp:TableRow ID="trSubmit">
            <asp:TableCell Width="100%" ColumnSpan="2">
                <asp:Button ID="btnSearchs" runat="Server" CssClass="BUTTON" Text="Refresh"></asp:Button>
            </asp:TableCell>
        </asp:TableRow>--%>
    </asp:Table>



    <table id="example" class="display" style="width: 100%; border-color: Black; border-width: 1px; border-style: solid; font-size: 8pt; border-collapse: collapse;">
        <thead style="background: #CCCCCC;">
            <tr class="hidden-row">
                <th colspan="15"></th>
            </tr>

            <tr class="hidden-row">
                <th colspan="15">Report Date : <p id="dateTime"></p>

                </th>
            </tr>

            <tr class="hidden-row">
                <th colspan="15">Report Generated By : <%=Names%> </th>
            </tr>

            <tr class="hidden-row">
                <th colspan="15"></th>
            </tr>
            <tr>
                <th>Menu Name</th>
                <th>Page Name</th>
                <th title ="Bank User">BU</th>
                <th title ="Bank Admin">BA</th>
                <th title ="Bank Authorizer">BS</th>
                <th title ="Bank Downloader">BD</th>
                <th title ="Inquiry User">IU</th>
                <th title ="Report Downloader">RD</th>
                <th title ="Uploader">U</th>
                <th title ="Reviewer">R</th>
                <th title ="Authorizer">A</th>
                <th title ="System Authorizer">SA</th>
                <th title ="System Admin">CA</th>
                <th title ="Interceptor">I</th>
                <th title ="Bank Operator">BO</th>
               

            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Menu_Name") %></td>
                        <td><%# Eval("Page_Name") %></td>
                        <td >
                            <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("BU") %>' Enabled="false" /></td>
                         <td >
                            <asp:CheckBox ID="CheckBox2" runat="server" Checked='<%# Eval("BA") %>' Enabled="false" /></td>
                        <td>
                            <asp:CheckBox ID="CheckBox3" runat="server" Checked='<%# Eval("BS") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox4" runat="server" Checked='<%# Eval("BD") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox5" runat="server" Checked='<%# Eval("IU") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox6" runat="server" Checked='<%# Eval("RD") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox7" runat="server" Checked='<%# Eval("U") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox8" runat="server" Checked='<%# Eval("R") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox9" runat="server" Checked='<%# Eval("A") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox10" runat="server" Checked='<%# Eval("SA") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox11" runat="server" Checked='<%# Eval("CA") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox12" runat="server" Checked='<%# Eval("I") %>' Enabled="false" /></td>
                        <td >
                            <asp:CheckBox ID="CheckBox13" runat="server" Checked='<%# Eval("BO") %>' Enabled="false" /></td>

                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>

</asp:Content>
