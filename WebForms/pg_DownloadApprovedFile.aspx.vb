Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

Namespace MaxPayroll

    Partial Class pg_DownloadApprovedFile

        Inherits clsBasePage
        Private _Helper As New Helper


#Region "Declaration"
        Private ReadOnly Property sFileType() As String
            Get
                Return Request.QueryString("FileType") & ""
            End Get
        End Property

        Private ReadOnly Property bIsH2H() As Boolean
            Get
                If IsNothing(Request.QueryString("Mod")) Then
                    Return False
                ElseIf Request.QueryString("Mod") = "H2H" Then
                    Return True
                End If
            End Get
        End Property
#End Region

#Region "Page Event"
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            Try
                'if not Bank Downloader
                If Not ss_strUserType = gc_UT_BankDownloader Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                ' If Page.IsPostBack = False Then
                prcBindDGNewFile()
                prcBindDGDownloadedFile()
                'End If


            Catch

                'Error Log
                Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page_Load - pg_DownloadApprovedFile", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Instance of Customer Class Object
                clsCustomer = Nothing

            End Try
        End Sub
#End Region

#Region "Bind Data"
        Sub prcBindDGNewFile()
            Dim oDownloader As New clsBankDownloader
            dgNewFile.CurrentPageIndex = 0
            dgNewFile.DataSource = oDownloader.fncGetApprovedFileList(bIsH2H, True, sFileType)
            dgNewFile.DataBind()
        End Sub

        Sub prcBindDGDownloadedFile()
            Dim oDownloader As New clsBankDownloader
            dgDownloadedFile.CurrentPageIndex = 0
            dgDownloadedFile.DataSource = oDownloader.fncGetApprovedFileList(bIsH2H, False, sFileType)
            dgDownloadedFile.DataBind()
        End Sub
#End Region

#Region "Datagrid Event Handler"
#Region "dgNewFile"
        Protected Sub dgNewFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgNewFile.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim lnkBtn As New LinkButton
                    lnkBtn = CType(e.Item.FindControl("lnkBtnNewFile"), LinkButton)
                    lnkBtn.Text = e.Item.Cells(1).Text
                    lnkBtn.Attributes.Add("onclick", "setReloadTime(3)")
            End Select
        End Sub

        Protected Sub dgNewFile_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgNewFile.ItemCommand
            Select Case e.CommandName
                Case "Download"
                    Dim oCommon As New clsCommon
                    Dim sFilePath As String
                    sFilePath = fncGetFolder(e.Item.Cells(2).Text.Trim)

                    Dim sFileName As String = sFilePath & "\" & e.Item.Cells(1).Text

                    If System.IO.Directory.Exists(sFilePath) Then
                        If System.IO.File.Exists(sFileName) Then
                            Dim oDownloader As New clsBankDownloader
                            oDownloader.fncUpdateDownloadedFile(bIsH2H, CInt(e.Item.Cells(0).Text), e.Item.Cells(3).Text)
                            prcBindDGDownloadedFile()
                            prcBindDGNewFile()

                            Response.ContentType = "text"
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" & e.Item.Cells(1).Text)

                            Response.TransmitFile(sFileName)
                            Response.End()
                        Else
                            Me.lblMessage.Text = "File [" & e.Item.Cells(1).Text & "] not exist."
                        End If
                    Else
                        Me.lblMessage.Text = "Folder [" & sFilePath & "] not exist."
                    End If
            End Select

        End Sub

        Protected Sub dgNewFile_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgNewFile.PageIndexChanged
            Me.prcBindDGNewFile()

            If dgNewFile.PageCount - 1 >= e.NewPageIndex Then
                dgNewFile.CurrentPageIndex = e.NewPageIndex
            Else
                dgNewFile.CurrentPageIndex = e.NewPageIndex - 1
            End If
            dgNewFile.DataBind()
        End Sub
#End Region

#Region "dgDownloadedFile"
        Protected Sub dgDownloadedFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDownloadedFile.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim lnkBtn As New LinkButton
                    lnkBtn = CType(e.Item.FindControl("lnkBtnNewFile"), LinkButton)
                    lnkBtn.Text = e.Item.Cells(1).Text
                    lnkBtn.Attributes.Add("onclick", "setReloadTime(3)")
            End Select
        End Sub

        Protected Sub dgDownloadedFile_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDownloadedFile.ItemCommand
            Select Case e.CommandName
                Case "Download"
                    Dim oCommon As New clsCommon
                    Dim sFilePath As String
                    sFilePath = fncGetFolder(e.Item.Cells(2).Text.Trim)

                    Dim sFileName As String = sFilePath & "\" & e.Item.Cells(1).Text

                    If System.IO.Directory.Exists(sFilePath) Then
                        If System.IO.File.Exists(sFileName) Then
                            Response.ContentType = "text"
                            Response.AppendHeader("Content-Disposition", "attachment; filename=" & e.Item.Cells(1).Text)
                            Response.TransmitFile(sFileName)
                            Response.End()
                        Else
                            Me.lblMessage.Text = "File [" & e.Item.Cells(1).Text & "] not exist."
                        End If
                    Else
                        Me.lblMessage.Text = "Folder [" & sFilePath & "] not exist."
                    End If
            End Select
        End Sub

        Protected Sub dgDownloadedFile_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDownloadedFile.PageIndexChanged
            Me.prcBindDGDownloadedFile()
            If dgDownloadedFile.PageCount - 1 >= e.NewPageIndex Then
                dgDownloadedFile.CurrentPageIndex = e.NewPageIndex
            Else
                dgDownloadedFile.CurrentPageIndex = e.NewPageIndex - 1
            End If
            dgDownloadedFile.DataBind()
        End Sub
#End Region

        Private Function fncGetFolder(ByVal strFileType As String) As String
            Dim sRetVal As String = ""
            Select Case strFileType
                Case _Helper.Autopay_Name
                    If bIsH2H Then
                        sRetVal = ConfigurationManager.AppSettings("BD_H2HAutopayReportPath")
                    Else
                        sRetVal = ConfigurationManager.AppSettings("BD_WEBAutopayReportPath")
                    End If
                Case _Helper.AutopaySNA_Name
                    If bIsH2H Then
                        sRetVal = ConfigurationManager.AppSettings("BD_H2HAutopaySNAReportPath")
                    Else
                        sRetVal = ConfigurationManager.AppSettings("BD_WEBAutopaySNAReportPath")
                    End If
                Case _Helper.CPS_Name
                    If bIsH2H Then
                        sRetVal = ConfigurationManager.AppSettings("BD_H2HCPSIncomingPath")
                    Else
                        sRetVal = ConfigurationManager.AppSettings("BD_WEBCPSIncomingPath")
                    End If
                Case _Helper.DirectDebit_Name
                    If bIsH2H Then
                        sRetVal = ConfigurationManager.AppSettings("BD_H2HDDEBITIncomingPath")
                    Else
                        sRetVal = ConfigurationManager.AppSettings("BD_WEBDDEBITIncomingPath")
                    End If

            End Select
            Return sRetVal
        End Function
#End Region

    End Class
End Namespace