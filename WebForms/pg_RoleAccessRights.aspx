<%@ Page Language="vb" Inherits="MaxPayroll.PG_RoleAccessRights" CodeFile="PG_RoleAccessRights.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <link type="text/css" href="../Include/Styles.css" rel="stylesheet">
    <script type="text/javascript" src="../Include/js/Plugins/datatables.min.js"></script>
    <link type="text/css" href="../Include/js/Plugins/datatables.min.css" rel="stylesheet">
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" src="../include/js/Forms/role_access_rights.js"></script>

    <script type="text/javascript" language="JavaScript">
        function fncBack() {
            var strRequest = document.getElementById('<%= hRequest.ClientID %>').value;
            if (strRequest === "View") {
                window.location.href = "PG_ApprMatrix.aspx?Mode=View";
            } else if (strRequest === "Reject") {
                window.location.href = "PG_ApprMatrix.aspx?Mode=Reject";
            } else if (strRequest === "Done") {
                window.location.href = "PG_ApprMatrix.aspx?Mode=Done";
            } else if (strRequest === "Edit") {
                window.location.href = "PG_ApprMatrix.aspx?Mode=Edit";
            }
        }

        function fncNew() {
            window.location.href = "PG_RoleAccessRights.aspx";
        }

        function fncView() {
            var strRequest = document.getElementById('<%= hRequest.ClientID %>').value;
            var hOrgId = document.getElementById('<%= hOrgId.ClientID %>').value;

            if (strRequest === "BU") {
                window.location.href = "PG_ViewPassword.aspx";
            } else {
                if (hOrgId > 0) {
                    window.location.href = "PG_ViewOrganizationRoles.aspx?Id=" + hOrgId;
                } else {
                    window.location.href = "PG_ViewRoles.aspx";
                }
            }
        }

        function fncShow() {
            var lngUserId = document.getElementById('<%= hUserId.ClientID %>').value;
            if (lngUserId === "") {
                window.location.href = "PG_RoleAccessRights.aspx";
            } else {
                window.location.href = "PG_RoleAccessRights.aspx?Id=" + lngUserId;
            }
        }
    </script>

    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td>
                <asp:Label runat="Server" CssClass="FORMHEAD" Text="CIMB Gateway Role Rights" ID="lblHeading"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="Server" ID="lblMessage" CssClass="MSG" Text="Role rights saved successfully." Visible="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="font-weight: bold; color: red; font-size: 8pt;font-weight:400;font-size:14px">
                <label id="myLabel"></label><br />
            </td>
        </tr>
    </table>

    <asp:Table Width="100%" CellPadding="8" CellSpacing="0" runat="Server" BorderWidth="0" ID="tblMainForm">
        <asp:TableRow ID="trRoles">
            <asp:TableCell Width="15%">
                <asp:Label runat="Server" Text="Role Name" CssClass="LABEL" ID="lblRole"></asp:Label>&nbsp;
                <asp:Label Text="*" runat="server" CssClass="MAND" ID="Label1"></asp:Label>
            </asp:TableCell>
            <asp:TableCell Width="20%">
                <asp:DropDownList ID="cmbRoles" runat="Server" CssClass="MEDIUMTEXT"
                    AutoPostBack="true" OnSelectedIndexChanged="roleName_SelectedIndexChanged">
                </asp:DropDownList>
               
            </asp:TableCell>

             <asp:TableCell Width="70%" ColumnSpan="2">
                <asp:Button ID="btnSubmit" Text="Submit" CssClass="BUTTON" runat="Server" Visible="false"></asp:Button>
                <button style="width: 100px" type="button" id="submitBtn">Submit</button>
                <asp:Button ID="btnClears" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
               
            </asp:TableCell>

        </asp:TableRow>

        <%--<asp:TableRow ID="trSubmit">
           
        </asp:TableRow>--%>
    </asp:Table>

    <div id="tableContainer" runat="server">
        <table id="dgUsers" class="display" style="width: 100%; border-color: Black; border-width: 1px; border-style: solid; font-size: 8pt; border-collapse: collapse;">
            <thead style="background: #CCCCCC;">
                <tr>
                    <th>Menu Name</th>
                    <th>Page Name</th>
                    <th>Rights</th>
                    <th style="visibility: hidden">Id</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblMenuName" runat="server" Text='<%# Eval("MenuName") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPageName" runat="server" Text='<%# Eval("PageName") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Rights") %>' />
                            </td>
                            <td style="visibility: hidden">
                                <asp:Label ID="id" runat="server" Text='<%# Eval("RightsId") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>

    <div id="tableContainer1" runat="server">
        <table id="dgUsersBind" class="display" style="width: 100%; border-color: Black; border-width: 1px; border-style: solid; font-size: 8pt; border-collapse: collapse;">
            <thead style="background: #CCCCCC;">
                <tr>
                    <th>Menu Name</th>
                    <th>Page Name</th>
                    <th>Rights</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="Repeater2" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblMenuName" runat="server" Text='<%# Eval("Menu_Name") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblPageName" runat="server" Text='<%# Eval("Page_Name") %>'></asp:Label>
                            </td>
                            <td>
                                <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("Rights") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>


    <asp:HiddenField ID="hRequest" runat="server" />
    <asp:HiddenField ID="hOrgId" runat="server" />
    <asp:HiddenField ID="hUserId" runat="server" />

</asp:Content>
