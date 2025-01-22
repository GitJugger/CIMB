Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data


Namespace MaxPayroll


    Partial Class pg_DownloadCIMBReport
        Inherits clsBasePage

        Private _Customer As New clsCustomer
        Private _Helper As New Helper

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer


            Try
                'if not Bank user or Inquiry User
                If Not ss_strUserType = gc_UT_ReportDownloader Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                If Page.IsPostBack = False Then
                    'prcBindDGNewFile()
                    prcBindDGDownloadedFile()
                End If


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

        Sub prcBindDGDownloadedFile()
            Dim AutopaySNA() As String, Autopay() As String, DDebit() As String, oFile() As String
            Dim HybridSNA() As String = Nothing, HybridDDebit() As String = Nothing, HybridMandate() As String = Nothing
            Dim MsgDirNotExist As String = Nothing, SqlStatement As String = Nothing

            ''Get Files from respective directory - START
            If System.IO.Directory.Exists(_Helper.ReportAutopaySNAConfig) Then
                AutopaySNA = System.IO.Directory.GetFiles(_Helper.ReportAutopaySNAConfig)
            End If
            If System.IO.Directory.Exists(_Helper.ReportAutopayConfig) Then
                Autopay = System.IO.Directory.GetFiles(_Helper.ReportAutopayConfig)
            End If
            If System.IO.Directory.Exists(_Helper.ReportDDebitConfig) Then
                DDebit = System.IO.Directory.GetFiles(_Helper.ReportDDebitConfig)
            End If
            If System.IO.Directory.Exists(_Helper.ReportHDDebitConfig) Then
                HybridDDebit = System.IO.Directory.GetFiles(_Helper.ReportHDDebitConfig)
            End If
            If System.IO.Directory.Exists(_Helper.ReportHAPSConfig) Then
                HybridSNA = System.IO.Directory.GetFiles(_Helper.ReportHAPSConfig)
            End If
            If System.IO.Directory.Exists(_Helper.ReportHMNDConfig) Then
                HybridMandate = System.IO.Directory.GetFiles(_Helper.ReportHMNDConfig)
            End If
            ''Get Files from respective directory - STOP

            Dim DTFiletype As New DataTable
            Dim DSFiletype As New DataSet

            'DSFiletype = _Customer.fncGetGroupPaymentService(ss_lngOrgID, ss_lngUserID, ss_lngGroupID)

            'Build sql statement 
            SqlStatement = _Helper.GetSQLReportDownload_FileTypes & ss_lngGroupID
            DSFiletype = MaxMiddleware.PPS.GetDataSet(SqlStatement, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)
            DTFiletype = DSFiletype.Tables(0)
            Dim i As Integer
            Dim dv As New DataView
            Dim sTemp As String = ""
            Dim arrFile As New clsCIMBReports
            Dim arrFileItem As clsCIMBReport
            Dim dtable As New DataTable
            Dim drow As DataRow
            Dim dc As DataColumn
            Dim ReadStartPos As Integer = 0



            For i = 0 To DTFiletype.Rows.Count - 1
                lblHeading.Text = DTFiletype.Rows(i)("FTYPE")
                Dim UseOrgCode As Boolean = False
                Dim OrgCodeTable As DataTable = Nothing
                Dim dtOrgcode As DataRow = Nothing
                Dim RdChar As Integer = 0
                Dim RdLength As Integer = 0
                Dim RDOption As Integer = 0

                If lblHeading.Text = "Autopay File" Then
                    oFile = Autopay
                    RdChar = System.Configuration.ConfigurationManager.AppSettings("RDAutopayChar")


                    If System.Configuration.ConfigurationManager.AppSettings("RDAutopayCode") = 1 Then
                        OrgCodeTable = _Helper.GetOrgCodeForOrgId(ss_lngOrgID)
                        UseOrgCode = True
                    Else
                        RDOption = System.Configuration.ConfigurationManager.AppSettings("RDAutopayCode")

                    End If

                ElseIf lblHeading.Text = "Direct Debit" Then
                    oFile = DDebit

                    'RdChar = System.Configuration.ConfigurationManager.AppSettings("RESWEB")
                    RdChar = System.Configuration.ConfigurationManager.AppSettings("RDDDChar")

                    'If System.Configuration.ConfigurationManager.AppSettings("RESCode") = 1 Then
                    If System.Configuration.ConfigurationManager.AppSettings("RDDDCode") = 1 Then
                        OrgCodeTable = _Helper.GetOrgCodeForOrgId(ss_lngOrgID)
                        UseOrgCode = True
                    Else
                        RDOption = System.Configuration.ConfigurationManager.AppSettings("RDDDCode")
                    End If

                ElseIf lblHeading.Text = "AutopaySNA File" Then
                    oFile = AutopaySNA
                    RdChar = System.Configuration.ConfigurationManager.AppSettings("RDSNAChar")


                    If System.Configuration.ConfigurationManager.AppSettings("RDSNACode") = 1 Then
                        OrgCodeTable = _Helper.GetOrgCodeForOrgId(ss_lngOrgID)
                        UseOrgCode = True
                    Else
                        RDOption = System.Configuration.ConfigurationManager.AppSettings("RDSNACode")
                    End If
                ElseIf lblHeading.Text = _Helper.HybridDirectDebit_Name Then
                    oFile = HybridDDebit
                    RdChar = System.Configuration.ConfigurationManager.AppSettings("RDHDDChar")


                    If System.Configuration.ConfigurationManager.AppSettings("RDHDDCode") = 1 Then
                        OrgCodeTable = _Helper.GetOrgCodeForOrgId(ss_lngOrgID)
                        UseOrgCode = True
                    Else
                        RDOption = System.Configuration.ConfigurationManager.AppSettings("RDHDDCode")
                    End If
                ElseIf lblHeading.Text = _Helper.HybridAutoPaySNA_Name Then
                    oFile = HybridSNA
                    RdChar = System.Configuration.ConfigurationManager.AppSettings("RDHAPSChar")


                    If System.Configuration.ConfigurationManager.AppSettings("RDHPSCode") = 1 Then
                        OrgCodeTable = _Helper.GetOrgCodeForOrgId(ss_lngOrgID)
                        UseOrgCode = True
                    Else
                        RDOption = System.Configuration.ConfigurationManager.AppSettings("RDHPSCode")
                    End If

                ElseIf lblHeading.Text = _Helper.HybridMandate_Name Then
                    oFile = HybridMandate
                    RdChar = System.Configuration.ConfigurationManager.AppSettings("RDHMNDChar")


                    If System.Configuration.ConfigurationManager.AppSettings("RDHMNDCode") = 1 Then
                        OrgCodeTable = _Helper.GetOrgCodeForOrgId(ss_lngOrgID)
                        UseOrgCode = True
                    Else
                        RDOption = System.Configuration.ConfigurationManager.AppSettings("RDHMNDCode")
                    End If
                Else

                    Continue For

                End If

                For Each sTemp In oFile
                    Dim FileName As String
                    Try
                        If UseOrgCode = True Then

                            For Each dtOrgcode In OrgCodeTable.Rows

                                RdLength = Len(dtOrgcode("BankOrgCode"))

                                If sTemp.Length > 9 Then
                                    If sTemp.Substring(sTemp.LastIndexOf("\") + 1 + RdChar, RdLength) = dtOrgcode("BankOrgCode") Then
                                        arrFileItem = New clsCIMBReport
                                        arrFileItem.FileName = sTemp
                                        arrFileItem.FileType = lblHeading.Text
                                        arrFileItem.FileDateTime = System.IO.File.GetCreationTime(sTemp)
                                        arrFileItem.OriFileName = _Helper.fncGetFileName(sTemp.Substring(sTemp.LastIndexOf("\") + 1), lblHeading.Text)
                                        'arrFileItem.FileLastAccess = System.IO.File.GetLastAccessTime(sTemp)
                                        arrFile.Add(arrFileItem)
                                        arrFileItem = Nothing


                                    End If
                                End If
                            Next
                        ElseIf RDOption = 2 Then
                            If sTemp.Length > 9 Then
                                If sTemp.Substring(sTemp.LastIndexOf("\") + 1 + RdChar, 6) = ss_lngOrgID Then
                                    arrFileItem = New clsCIMBReport
                                    arrFileItem.FileName = sTemp
                                    arrFileItem.FileType = lblHeading.Text
                                    arrFileItem.FileDateTime = System.IO.File.GetCreationTime(sTemp)
                                    arrFileItem.OriFileName = _Helper.fncGetFileName(sTemp.Substring(sTemp.LastIndexOf("\") + 1), lblHeading.Text)
                                    'arrFileItem.FileLastAccess = System.IO.File.GetLastAccessTime(sTemp)
                                    arrFile.Add(arrFileItem)
                                    arrFileItem = Nothing


                                End If
                            End If
                        End If
                    Catch ex As Exception
                        Dim clsGeneric As New Generic
                        Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page_Load - pg_DownloadCIMBReport", Err.Number, Err.Description)
                        clsGeneric = Nothing
                    End Try


                Next
                If arrFile.Count > 0 Then

                    arrFile.Sort(clsCIMBReports.ReportFields.FileDateTime, False)

                    dgDownloadedFile.DataSource = arrFile

                End If

                dgDownloadedFile.CurrentPageIndex = 0
                dgDownloadedFile.DataBind()
            Next




        End Sub

        Protected Sub dgDownloadedFile_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgDownloadedFile.ItemCommand



            Select Case e.CommandName
                Case "Download"

                    Dim AutopaySNA As String, Autopay As String, DDebit As String, sFilePath As String

                    AutopaySNA = _Helper.ReportAutopaySNAConfig
                    Autopay = _Helper.ReportAutopayConfig
                    DDebit = _Helper.ReportDDebitConfig
                    Dim sFileName As String = e.Item.Cells(0).Text



                    'If System.IO.File.Exists(sFileName) Then
                    If System.IO.File.Exists(sFileName) Then
                        Dim oDownloader As New clsBankDownloader

                        prcBindDGDownloadedFile()
                        Response.ContentType = "text"
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" & e.Item.Cells(0).Text.Substring(e.Item.Cells(0).Text.LastIndexOf("\") + 1))

                        Response.TransmitFile(sFileName)
                        Response.End()
                    Else
                        Me.lblMessage.Text = "File [" & e.Item.Cells(0).Text & "] not exist."
                    End If
                    'Else
                    'Me.lblMessage.Text = "Folder [" & sFilePath & "] not exist."
                    'End If
            End Select
        End Sub

        Protected Sub dgDownloadedFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDownloadedFile.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim lnkBtn As New LinkButton
                    lnkBtn = CType(e.Item.FindControl("lnkBtnNewFile"), LinkButton)
                    lnkBtn.Text = e.Item.Cells(0).Text.Substring(e.Item.Cells(0).Text.LastIndexOf("\") + 1)
                    lnkBtn.Attributes.Add("onclick", "setReloadTime(3)")
            End Select
        End Sub

        Protected Sub dgDownloadedFile_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDownloadedFile.PageIndexChanged
            Me.prcBindDGDownloadedFile()
            dgDownloadedFile.CurrentPageIndex = e.NewPageIndex
            dgDownloadedFile.DataBind()
        End Sub

    End Class
    Public Class MyObject
        Implements IComparable
        Public SSN As String
        Public EmpID As Integer
        Public Name As String

        Public Function CompareTo(ByVal obj As Object) As Integer _
           Implements System.IComparable.CompareTo
            If Not TypeOf obj Is MyObject Then
                Throw New Exception("Object is not MyObject")
            End If
            Dim Compare As MyObject = CType(obj, MyObject)
            Dim result As Integer = Me.SSN.CompareTo(Compare.SSN)

            If result = 0 Then
                result = Me.SSN.CompareTo(Compare.SSN)
            End If
            Return result
        End Function
    End Class



End Namespace