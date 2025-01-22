<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_NetMenu" CodeFile="pg_NetMenu.aspx.vb" %>

<% Response.CacheControl = "no-cache" %>
<% Response.AddHeader("Pragma", "no-cache") %>
<% Response.Expires = -1 %> 

<html>
  <head runat=server>
    <title>Menu</title>
    <link href="Styles.Css" rel="stylesheet" type="text/css"/>
  </head>
  <body>
    <form id="Form1" method="post" runat="server">
        <table width="175px" cellpadding=2 cellspacing=0 bgcolor="gray">
            <tr>
                <td>
                   <%--asp:TreeView ID="trvMenu" runat="server" DataSourceID="StartMenu" ImageSet="Simple">
            <ParentNodeStyle Font-Bold="False" />
            <HoverNodeStyle BackColor="Gray" />
            <SelectedNodeStyle
                Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" ForeColor="#5555DD" />
            <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="0px"
                NodeSpacing="0px" VerticalPadding="0px" />
            <DataBindings>
                <asp:TreeNodeBinding DataMember="siteMapNode"  PopulateOnDemand="True" 
                    TargetField="target" ValueField="name" NavigateUrlField="url" />
            </DataBindings> </asp:TreeView> --%>
                    <asp:Menu ID="Menu1" runat="server" BackColor="#B5C7DE" DataSourceID="StartMenu" 
                        DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#284E98"
                        StaticSubMenuIndent="10px" Orientation="Horizontal">
                        
                        <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <DynamicHoverStyle BackColor="#284E98" ForeColor="White" />
                        <DynamicMenuStyle BackColor="#B5C7DE" />
                        <StaticSelectedStyle BackColor="#507CD1" />
                        <DynamicSelectedStyle BackColor="#507CD1" />
                        <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                        <StaticHoverStyle BackColor="#284E98" ForeColor="White" />
                       
                        <DataBindings>
                            <asp:MenuItemBinding DataMember="siteMapNode" TextField="name" />
                            <asp:MenuItemBinding DataMember="siteMapNode" TextField="name" />
                        </DataBindings>
                       
                    </asp:Menu>
                    &nbsp;
                    <asp:XmlDataSource ID="StartMenu" runat="server" DataFile="~/App_Data/StartMenu.xml">
                    </asp:XmlDataSource>
                </td>
            </tr>
        </table>
    </form>
  </body>
</html>
