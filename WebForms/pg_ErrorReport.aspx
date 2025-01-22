<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_ErrorReport.aspx.vb" Inherits="WebForms_pg_ErrorReport" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
    <asp:SqlDataSource ID="dsErrorLog" runat="server" ConnectionString="<%$ ConnectionStrings:CIMBGatewayDB27TestedConnectionString %>"
        SelectCommand="SELECT [Error_Id], [Org_Id], [User_Id], [Operation], [Error_Number], [Error_Description], [Error_Dt], [H2H_File_Name] FROM [tCor_ErrorLog] ORDER BY [Error_Dt]">
    </asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True"
        AutoGenerateColumns="False" DataKeyNames="Error_Id" DataSourceID="dsErrorLog"
         PagerStyle-HorizontalAlign="Center"  BorderWidth="0" GridLines="Both"  HeaderStyle-CssClass="GridHeaderStyle" 
				width="100%" HeaderStyle-HorizontalAlign="Left" >
        <Columns>
            <asp:BoundField DataField="Error_Id" HeaderText="Error Id" InsertVisible="False"
                ReadOnly="True" SortExpression="Error_Id">
                <HeaderStyle Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Org_Id" HeaderText="Org Id" SortExpression="Org_Id" />
            <asp:BoundField DataField="User_Id" HeaderText="User Id" SortExpression="User_Id" />
            <asp:BoundField DataField="Operation" HeaderText="Operation" SortExpression="Operation" />
            <asp:BoundField DataField="Error_Description" HeaderText="Error_Description" SortExpression="Error_Description" />
            <asp:BoundField DataField="Error_Dt" HeaderText="Error_Dt" SortExpression="Error_Dt" />
            <asp:BoundField DataField="H2H_File_Name" HeaderText="H2H_File_Name" SortExpression="H2H_File_Name" />
        </Columns>
    </asp:GridView>
</asp:Content>

