#Region "Name Spaces "

Imports MaxGeneric
Imports MaxModule
Imports System.IO
Imports System.Text
Imports System.Reflection.MethodBase

#End Region

Namespace MaxPayroll

    Partial Class PG_ReportDownloader
        Inherits clsBasePage

#Region "Global Declaration "

        'Create Instances - Start
        Private _Message As New Message
        Private _DataBase As New DataBase
        Private _WebHelper As New WebHelper
        Private _WebCommon As New WebCommon
        Private _WebReport As New WebReport
        Private _MaxCommon As New MaxReadWrite.Common
        'Create Instances - Stop

#End Region

#Region "Get Service Type "

        Private Function GetServiceType() As String
            Return clsGeneric.NullToString(ddlServiceType.SelectedItem.Text)
        End Function

#End Region

#Region "Page Load "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Not Page.IsPostBack Then

                    'Set Report Initialisations
                    Call PageInit()

                    'Set File Type
                    Call BindDataGrid()

                End If

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            End Try

        End Sub

#End Region

#Region "Page Init "

        Private Sub PageInit()

            'Create Instances
            Dim ServiceTypes As DataTable = Nothing

            Try

                'Get the Service Types
                ServiceTypes = _WebCommon.GetGroupServiceTypes(ss_lngGroupID, String.Empty)

                'Populate Service Type DropdownList - Start
                Call FormHelp.PopulateDropDownList(ServiceTypes, ddlServiceType, _
                    _WebHelper.ColServiceType, _WebHelper.ColServiceId)
                'Populate Service Type DropdownList - Stop

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            End Try

        End Sub

#End Region

#Region "Selected Index Changed - File type "

        Protected Sub FileTypeChange(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlServiceType.SelectedIndexChanged

            Call BindDataGrid()

        End Sub

#End Region

#Region "Bind Data grid "

        Private Sub BindDataGrid()

            'Create Instances - Start
            Dim _FileInfo As FileInfo = Nothing
            Dim _ReportParam As ReportParam = Nothing
            Dim _DirectoryInfo As DirectoryInfo = Nothing
            Dim _ReportParamList As New List(Of ReportParam)
            'Create Instances - Stop

            'Variable Declarations - Start
            Dim ReportOrgIdStart As Short = 0, ReportOrgIdEnd As Short = 0
            Dim ReportsInFolder As String() = Nothing, ReportFolder As String = Nothing
            Dim ReportFile As String = Nothing, ReportOrgId As Integer = 0, FileNameNoPath As String = Nothing
            'Variable Declarations - Stop

            Try

                'Get Report Folder
                ReportFolder = WebReport.ReportFolder(GetServiceType())

                'Set Directory Info
                _DirectoryInfo = New DirectoryInfo(ReportFolder)

                'Loop thro the Reports in the folder - Start
                For Each _FileInfo In _DirectoryInfo.GetFiles()

                    'Get File Name
                    FileNameNoPath = Path.GetFileName(_FileInfo.FullName)

                    'Get Org id Start & End Positions - Start
                    ReportOrgIdEnd = WebReport.ReportOrgIdEnd(GetServiceType())
                    ReportOrgIdStart = WebReport.ReportOrgIdStart(GetServiceType())
                    'Get Org id Start & End Positions - Stop

                    'Get Report Org Id from file name - Start
                    ReportOrgId = FileNameNoPath.Substring(ReportOrgIdStart,
                        (ReportOrgIdEnd - ReportOrgIdStart) + 1)
                    'Get Report Org Id from file name - Stop

                    'Add Report Details - Start
                    _ReportParam = New ReportParam
                    _ReportParam.File_Name = _FileInfo.FullName
                    _ReportParam.File_Type = GetServiceType()
                    _ReportParam.File_DateTime = _FileInfo.CreationTime
                    _ReportParam.Created_FileName = _WebReport.GetUploadFileName(FileNameNoPath)
                    _ReportParamList.Add(_ReportParam)
                    'Add Report Details - Stop

                Next
                'Loop thro the Reports in the folder - Stop

                'Bind Data Grid - Start
                dgDownloadedFile.DataSource = _ReportParamList
                dgDownloadedFile.CurrentPageIndex = 0
                dgDownloadedFile.DataBind()
                'Bind Data Grid - Stop

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            End Try

        End Sub

#End Region

#Region "Download Reports "

        Private Sub Page_Submit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click

            'Create Instances
            Dim DownloadFiles As DataTable = Nothing

            'Variable Declarations
            Dim ReportZipFile As String = Nothing, ErrorDetails As String = Nothing

            Try

                'Get Selected Files to be downloaded - Start
                DownloadFiles = DataHelp.SelectInputGridToTable(dgDownloadedFile, _
                    WebReport.ColMessageSelect, 0, WebReport.ColDownloadFile)
                'Get Selected Files to be downloaded - Stop

                'Zip Report Files - Start
                ReportZipFile = _WebReport.ZipReportFiles(GetServiceType(), _
                   ss_lngOrgID, DownloadFiles, ErrorDetails)
                'Zip Report Files - Stop

                'if no error during Zipping - Start
                If FormHelp.IsBlank(ErrorDetails) Then

                    'Prepare download file - Start
                    Response.Clear()
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" & Path.GetFileName(ReportZipFile))
                    Response.ContentType = "application/x-zip-compressed"
                    Response.TransmitFile(ReportZipFile)
                    Response.End()
                    'Prepare download file - Stop

                End If
                'if no error during Zipping - Stop

            Catch ex As Exception

                'Log Error - Start
                If Not ex.Message = WebHelper.ThreadAborted Then
                    Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                End If
                'Log Error - Stop

            End Try

        End Sub

#End Region

    End Class

End Namespace
