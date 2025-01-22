<%@ Page Language="VB" AutoEventWireup="false" CodeFile="progress.aspx.vb" Inherits="MaxPayroll.progress" %>



<html xmlns="http://www.w3.org/1999/xhtml" >
<head><base target="_self" />
    <title>Progress</title>
   <link href="../WebForms/Styles.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
  
      function Close()
      {
           return 'Close this window will not affecting the uploading progress.'
      }

    </script>
</head>
<body>
   <form id="Form1" method="post" runat="server">
      <table width="100%" height="100%" cellpadding="2" cellspacing="2" border="0">
         <tr>
            <td class="BIGLABEL">&nbsp;&nbsp;&nbsp;<img alt="" src="../Include/images/ProgressCircle4.gif"> NOW LOADING...
            </td>
         </tr>
      </table>
   </form>
</body>
</html>
