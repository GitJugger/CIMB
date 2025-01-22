Imports System.IO
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsUpload
Imports System.Data.SqlClient


Namespace MaxPayroll

    Partial Class pg_MandateUpload
        Inherits clsBasePage


#Region "Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

           
            'Create Instance of Upload Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strAuthLock As String
            Dim intReviewers As Int16, intAuthorisers As Int16

            Try
                txtValidationCode.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSave.ClientID + "').click();return false;}} else {return true}; ")

                If Not ss_strUserType = gc_UT_Uploader Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                'Get Authorization Lock Status - Start
                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    btnSave.Enabled = False
                    lblMessage.Text = "Your Validation Code has been locked due to invalid 3 attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                    Exit Try
                End If
                'Get Authorization Lock Status - Stop

                'Check for Active Reviewers/Authorisers - START
                intReviewers = clsCommon.fncBusinessRules("REVIEW", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, 0)
                intAuthorisers = clsCommon.fncBusinessRules("AUTHORIZE", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, 0)

                If intReviewers = 0 Or intAuthorisers = 0 Then
                    btnSave.Enabled = False
                    lblMessage.Text = "Required Number(s) of " & gc_UT_ReviewerDesc & "s/" & gc_UT_AuthDesc & "s are not Created or Active. Please contact your " & gc_UT_SysAdminDesc & "."
                    Exit Try
                End If
                'Check for Active Reviewers/Authorisers - STOP

                If Not Page.IsPostBack Then
                    
                    hAlert.Value = "N"
                    'Get File Types - STOP
                    btnSave.Text = enmButton.Submit.ToString

                    pnlAuthorize.Visible = False
                End If



                Call clsCommon.fncUploadBtnDisable(btnSave, False)

            Catch

                'Log Error
                Dim oGeneric As New Generic
                oGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "PG_MandateUpload - Page_Load", Err.Number, Err.Description)

            Finally
             
            End Try

        End Sub

#End Region

        
        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Select Case btnSave.Text
                    Case enmButton.Submit.ToString
                        Dim sPath As String = ""
                        Dim sFileName As String = flUpload.PostedFile.FileName
                        Dim oCommon As New clsCommon
                        If Len(sFileName.Trim) > 0 Then
                            sFileName = sFileName.Substring(sFileName.LastIndexOf("\") + 1)
                            If oCommon.fncBuildContent("File Check", "", ss_lngOrgID, ss_lngUserID, sFileName) = "Y" Then
                                lblMessage.Text = "The Mandate File [" & sFileName & "] is uploaded previously. Please rename the file."
                                Exit Try
                            End If

                            lblMessage.Text = "Please Enter your Validation Code & Confirm File Upload."
                            Me.lblFileName.Text = flUpload.PostedFile.FileName
                            pnlUpload.Visible = False
                            Me.pnlAuthorize.Visible = True
                            btnSave.Text = enmButton.Confirm.ToString

                            sFileName = oCommon.fncFileName(lblFileName.Text, False)
                            sPath = oCommon.fncFolder("Mandate File", "UPLOAD", ss_lngOrgID, ss_lngUserID, False)
                            sFileName = sPath & "\" & String.Format("{0:yyyyMMdd_hhmmss_}", DateTime.Now) & DateTime.Now.Millisecond.ToString & "_" & sFileName
                            ViewState("FileName") = sFileName

                            'flUpload.Value = sFileName
                            Me.flUpload.PostedFile.SaveAs(sFileName)
                        End If

                    Case enmButton.Confirm.ToString
                        Dim sMsg As String = ""
                        If MyBase.fncValidationCodeProcess(txtValidationCode.Text, sMsg) Then
                            Dim oItem As New clsMandateUpload

                            If oItem.Upload(ss_lngOrgID, ss_lngGroupID, ss_lngUserID, lblFileName.Text, ViewState("FileName"), sMsg) Then
                                lblMessage.Text = "Mandate file uploaded successfully."
                            Else
                                lblMessage.Text = sMsg
                            End If
                            prcReInit()
                        Else
                            lblMessage.Text = sMsg
                            'btnSave.Enabled = False
                        End If
                End Select
            Catch ex As Exception

            End Try


        End Sub
        Private Sub prcReInit()
            pnlAuthorize.Visible = False
            pnlUpload.Visible = True
            lblFileName.Text = ""
            btnSave.Text = enmButton.Submit.ToString
        End Sub
    End Class

End Namespace
