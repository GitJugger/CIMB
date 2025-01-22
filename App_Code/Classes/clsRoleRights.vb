Option Strict Off
Option Explicit On

Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.Encryption
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data


Namespace MaxPayroll

    Public Class clsRoleRights

#Region "RoleRights Maintenance"

        '*******************************************************************************************************************
        'Procedure Name     :   fnDB_User
        'Purpose            :   To Handle User Insert/Update
        'Arguments          :   Add/Update
        'Return Values      :   Ok/Error Msg
        'Author             :   Sujith Sharatchandran - 
        'Created            :   10/10/2003
        '********************************************************************************************************************
        Public Function fnDB_Menu(ByVal strAction As String, ByVal strMenuNames As String, ByVal strPageNames As String,
                                  ByVal strRoleNames As String, ByVal bRightsAction As Boolean, Optional ByVal lngRoleId As Long = 0) As Long

            'Create Instance of SQL Command Object
            Dim cmdMenuDetails As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASP .Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngRoleRightsId As Long
            Dim strMenuName As String
            Dim strPageName As String
            Dim bRights As Boolean
            Dim strRoleName As String

            Try
                'Assign Values to Declared Variables
                strRoleName = ASPNetContext.Request.Form("ctl00$cphContent$txtRoleName") 'Role Name
                strMenuName = ASPNetContext.Request.Form("ctl00$cphContent$txtMenuName") 'Menu Name
                strPageName = ASPNetContext.Request.Form("ctl00$cphContent$txtPageName") 'Page Name
                bRights = (ASPNetContext.Request.Form("ctl00$cphContent$chkRights") = "on") 'Rights Checkbox

                'Retrieve Organisation Id
                If lngRoleId > 0 Then
                    lngRoleRightsId = lngRoleId
                Else
                    lngRoleRightsId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0) 'Organisation Id
                End If

                'Initialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdMenuDetails
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_RoleRights" 'Stored Procedure Name
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_MenuName", strMenuNames))
                    .Parameters.Add(New SqlParameter("@in_PageName", strPageNames))
                    .Parameters.Add(New SqlParameter("@in_Rights", bRightsAction))
                    .Parameters.Add(New SqlParameter("@in_RoleName", strRoleNames))
                    .Parameters.Add(New SqlParameter("@in_RoleRightsId", lngRoleId))
                    .Parameters.Add(New SqlParameter("@out_RoleRightsId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_RoleRightsId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                lngRoleRightsId = cmdMenuDetails.Parameters("@out_RoleRightsId").Value

                Return lngRoleRightsId

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngRoleRightsId, 0, "fnDB_Menu - Error", Err.Number, Err.Description)

                Return 0

            Finally

                'Terminate SQL Connection 
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdMenuDetails = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region


    End Class

End Namespace