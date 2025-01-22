<%@ Page Language="vb" Inherits="MaxPayroll.PG_CutoffTime" AutoEventWireup="false"
    CodeFile="PG_CutoffTime.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master"
    EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>

    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">Cut Off Time</asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label CssClass="MSG" ID="lblMessage" runat="Server"></asp:Label></td>
        </tr>
    </table>
    <table cellspacing="0" cellpadding="8" border="0">
        <tr valign="top">
            <td>
                <!-- Main Table Starts Here -->
                <table>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <table cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap>
                                        <asp:Label ID="lblBank" runat="Server" Text="Select Bank:&nbsp;" CssClass="LABEL"></asp:Label>
                                        <asp:DropDownList ID="cmbBank" CssClass="MEDIUMTEXT" runat="Server" AutoPostBack="True">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <!-- Form Table Starts Here -->
                                        <table cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label runat="server" CssClass="BLABEL" Text="Payroll Submission" ID="Label38"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                        &nbsp;</td>
                                        </tr>
                                        </table>
                                     </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Normal Cut off Time" ID="Label2"
                                            Font-Underline="True"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label3"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblTime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label4"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="server" CssClass="LABEL" ID="Label5"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="server" CssClass="LABEL" ID="Label6"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="server" CssClass="LABEL" ID="Label7"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" CssClass="LABEL" Text="Privilege Cut off Time" ID="Label37"
                                            Font-Underline="True"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label36"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblPTime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label35"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="server" CssClass="LABEL" ID="Label34"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbPHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label8"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbPMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label9"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbPType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnSave" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" CssClass="BLABEL" Text="EPF Submission" ID="Label33"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label11"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblETime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label13"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="Server" CssClass="LABEL" ID="Label14"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbEHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label15"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbEMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label16"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbEType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnEPF" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnEClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="Server" CssClass="BLABEL" Text="SOCSO Submission" ID="Label10"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label12"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblSTime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label17"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="Server" CssClass="LABEL" ID="Label18"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbSHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label19"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbSMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label20"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbSType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnSocso" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnSClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <!----------------LHDN Cutoff Time Start-------------------------------->
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="Server" CssClass="BLABEL" Text="LHDN Submission" ID="Label21"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label22"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblLTime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label24"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="Server" CssClass="LABEL" ID="Label25"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbLHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label26"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbLMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label27"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbLType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnLHDN" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnLClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <!----------------LHDN Cutoff Time Stop-------------------------------->
                                <!----------------Direct Debit Cutoff Time Start-------------------------------->
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="Server" CssClass="BLABEL" Text="Direct Debit Submission" ID="lblDirectDebit"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label23"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblDTime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label29"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="Server" CssClass="LABEL" ID="Label30"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbDHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label31"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbDMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label32"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbDType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnDirectDebit" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnDClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <!----------------Direct Debit Time Stop-------------------------------->
                                <!----------------ZAKAT Cutoff Time Start-------------------------------->
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="Server" CssClass="BLABEL" Text="ZAKAT Submission" ID="lblZakat"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label28"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblZTime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label40"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="Server" CssClass="LABEL" ID="Label41"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbZHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label42"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbZMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label43"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbZType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnZAKAT" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnZClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <!----------------Zakat Time Stop-------------------------------->
                                <!----------------CPS Cutoff Time Start-------------------------------->
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="Server" CssClass="BLABEL" Text="CPS Submission" ID="lblCPS"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label39"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblCTime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label45"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="Server" CssClass="LABEL" ID="Label46"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbCHour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label47"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbCMin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label48"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbCType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnCPS" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnCClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>                        
                                 
                                 <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <!----------------CPS Time Stop-------------------------------->
                                
                                
                                
                                
                                 <!----------------PAYLINK  Cutoff Time Start-------------------------------->
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="Server" CssClass="BLABEL" Text="Paylink Submission" ID="Label1"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Current Cut Off Time:" ID="Label44"></asp:Label></td>
                                    <td>
                                        <asp:Label runat="Server" CssClass="BLABEL" ID="lblITime"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="Server" CssClass="LABEL" Text="Set Cut Off Time:" ID="Label50"></asp:Label></td>
                                    <td>
                                        <asp:Label Text="HH" runat="Server" CssClass="LABEL" ID="Label51"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbIhour" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="MM" runat="Server" CssClass="LABEL" ID="Label52"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbImin" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp;
                                        <asp:Label Text="Mode" runat="Server" CssClass="LABEL" ID="Label53"></asp:Label>&nbsp;
                                        <asp:DropDownList ID="cmbIType" AutoPostBack="False" CssClass="MINITEXT" runat="Server">
                                        </asp:DropDownList>&nbsp; </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        &nbsp;
                                        <asp:Button ID="btnInfenion" runat="Server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
                                        <asp:Button ID="btnInfClear" runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button>
                                    </td>
                                </tr>
                                <!----------------Paylink Time Stop-------------------------------->
                            </table>
                            <!-- Form Table Ends Here -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
   
</asp:Content>
