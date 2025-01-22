#Region "Name Spaces "

Option Strict On
Option Explicit On
Imports MaxGeneric
Imports MaxModule
Imports System.IO
Imports System.Configuration.ConfigurationManager

#End Region

#Region "Report Parameters "

Public Class ReportParam

    'Variable Declarations - Start
    Private _FileType As String
    Private _FileName As String
    Private _CreatedFileName As String
    Private _FileDateTime As DateTime
    Private _FileLastAccess As DateTime
    'Variable Declarations - Stop

    Public Property File_Name() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property
    Public Property File_Type() As String
        Get
            Return _FileType
        End Get
        Set(ByVal value As String)
            _FileType = value
        End Set
    End Property
    Public Property Created_FileName() As String
        Get
            Return _CreatedFileName
        End Get
        Set(ByVal value As String)
            _CreatedFileName = value
        End Set
    End Property
    Public Property File_DateTime() As DateTime
        Get
            Return _FileDateTime
        End Get
        Set(ByVal value As DateTime)
            _FileDateTime = value
        End Set
    End Property
    Public Property File_LastAccess() As DateTime
        Get
            Return _FileLastAccess
        End Get
        Set(ByVal value As DateTime)
            _FileLastAccess = value
        End Set
    End Property

End Class

#End Region

Public Class WebReport

#Region "Global Variables "

    'Create Instances
    Private _DataBase As New MaxModule.DataBase()

#End Region

#Region "Config Properties "

    Public Shared Function ReportFolder(ByVal ServiceType As String) As String

        Return clsGeneric.NullToString(ConfigurationManager.AppSettings( _
           ServiceType.Replace(" ", clsGeneric.AddUnderScore()) & "_REPORT_FOLDER"))

    End Function

    Public Shared Function ReportDownloadFolder(ByVal ServiceType As String) As String

        Return clsGeneric.NullToString(ConfigurationManager.AppSettings( _
           ServiceType.Replace(" ", clsGeneric.AddUnderScore()) & "_REPORT_DOWNLOAD_FOLDER"))

    End Function

    Public Shared Function CompressFolder(ByVal ServiceType As String) As String

        Return clsGeneric.NullToString(ConfigurationManager.AppSettings( _
           ServiceType.Replace(" ", clsGeneric.AddUnderScore()) & "_COMPRESS_DOWNLOAD_FOLDER"))

    End Function

    Public Shared Function ReportOrgIdStart(ByVal ServiceType As String) As Short

        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings( _
           ServiceType.Replace(" ", clsGeneric.AddUnderScore()) & "_REPORT_ORGID_START"))

    End Function

    Public Shared Function ReportOrgIdEnd(ByVal ServiceType As String) As Short

        Return clsGeneric.NullToShort(ConfigurationManager.AppSettings( _
           ServiceType.Replace(" ", clsGeneric.AddUnderScore()) & "_REPORT_ORGID_END"))

    End Function

    Public Shared Function ZipFileExtension() As String

        Return clsGeneric.NullToString(ConfigurationManager.AppSettings("Zip_File_Extn"))

    End Function

#End Region

#Region "SQL Properties "

    Public ReadOnly Property SqlGetUploadFileName() As String
        Get
            Return "MPG_Get_File_Name "
        End Get
    End Property

#End Region

#Region "Column Properties "

    Public Shared ReadOnly Property ColDownloadFile() As String
        Get
            Return "Download_File"
        End Get
    End Property

    Public Shared ReadOnly Property ColMessageSelect() As String
        Get
            Return "Message_Select"
        End Get
    End Property

#End Region

#Region "Get Service Type Name "

    Public Shared ReadOnly Property GetServiceTypeName(ByVal ServiceType As String) As String
        Get
            Return ServiceType.Replace(" ", clsGeneric.AddUnderScore())
        End Get
    End Property

#End Region

#Region "Get Upload File Name "

    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Purpose        : Get the Upload file Name
    'Created        : 15/07/2012
    Public Function GetUploadFileName(ByVal ReportFileName As String) As String

        'Build SqlStatement - Start
        Dim SqlStatement As String = SqlGetUploadFileName & clsGeneric.AddQuotes(ReportFileName)
        'Build SqlStatement - Stop

        Return clsGeneric.NullToString(_DataBase.GetValue(SqlStatement))

    End Function

#End Region

#Region "Zip Report Files "

    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Purpose        : Zip Report Files
    'Created        : 15/07/2012
    Public Function ZipReportFiles(ByVal ServiceType As String, ByVal OrganizationId As Integer, _
       ByVal DownloadFiles As DataTable, ByRef ErrorDetails As String) As String

        'Create Instances - Start
        Dim _DataRow As DataRow = Nothing
        Dim _Service As New MaxCams.Service()
        'Create Instances - Stop

        'Variable Declarations - Start
        Dim DownloadFolder As String = Nothing
        Dim ReportFileName As String = Nothing, ZipFileName As String = Nothing
        'Variable Declarations - Stop

        Try

            'Build Folder Name
            DownloadFolder = ReportDownloadFolder(ServiceType) & OrganizationId & "\"

            'if folder does not exists - Start
            If Not Directory.Exists(DownloadFolder) Then

                'Create Folder
                Call Directory.CreateDirectory(DownloadFolder)

            End If
            'if folder does not exists - Stop

            'Delete Previous files if any available in the Directory - Start
            'Call File.Delete(DownloadFolder)
            For Each files As String In Directory.GetFiles(DownloadFolder)
                File.Delete(files)
            Next
            'Delete Previous files if any available in the Directory - Stop

            'Loop thro the Download Files - Start
            For Each _DataRow In DownloadFiles.Rows

                'Get Repor File Name
                ReportFileName = clsGeneric.NullToString(_DataRow(ColDownloadFile))

                'Copy File to Download Folder - Start
                Call File.Copy(ReportFileName, DownloadFolder & _
                    Path.GetFileName(ReportFileName), True)
                'Copy File to Download Folder - Stop

            Next
            'Loop thro the Download Files - Stop

            'Build Zip File Name - Start
            ZipFileName = OrganizationId & clsGeneric.AddUnderScore()
            ZipFileName &= GetServiceTypeName(ServiceType) & clsGeneric.AddUnderScore()
            ZipFileName &= clsGeneric.GetDateTimeStamp() & ZipFileExtension()
            'Build Zip File Name - Start

            'Zip Download Folder - Start
            If _Service.ZipDirectory(DownloadFolder, ZipFileName, _
                CompressFolder(ServiceType), False, False, ErrorDetails) Then

                'if error details not blank - Start
                If Not FormHelp.IsBlank(ErrorDetails) Then
                    Return String.Empty
                End If
                'if error details not blank - Start

            Else

                Return String.Empty

            End If
            'Zip Download Folder - Stop

            Return CompressFolder(ServiceType) & ZipFileName

        Catch ex As Exception

            ErrorDetails = ex.Message
            Return String.Empty

        End Try

    End Function

#End Region

End Class
