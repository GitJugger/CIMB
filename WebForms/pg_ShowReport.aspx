<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_ShowReport.aspx.vb" Inherits="WebForms_pg_ShowReport" title="Report Page" %>

<%@ Register Assembly="ReportViewer" Namespace="Microsoft.Samples.ReportingServices"
    TagPrefix="cc1" %>
    
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<asp:panel ID="pnlGrid" runat="server" Height="500px" Width="640px">
<rsweb:ReportViewer  ID="rvReport" runat="server" Width="100%" ProcessingMode="Remote" 
ShowParameterPrompts="false" EnableTheming="true"></rsweb:ReportViewer>
</asp:panel>
</asp:Content>

