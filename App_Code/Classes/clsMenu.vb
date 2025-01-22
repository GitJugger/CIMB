#Region "Name Space "

Imports MaxGeneric
Imports MaxMiddleware
Imports Microsoft.VisualBasic
Imports System.Linq

#End Region


Namespace MaxPayroll

    Public Class clsMenu

#Region "Global Declarations "

        Private _sPath As String = ""
        Private _sText As String = ""
        Private _Helper As New Helper()
        Private _bIsHeader As Boolean = True

#End Region

        Public Property sPath() As String
            Get
                Return _sPath
            End Get
            Set(ByVal value As String)
                _sPath = value
            End Set
        End Property
        Public Property sText() As String
            Get
                Return _sText
            End Get
            Set(ByVal value As String)
                _sText = "<font color=#790008>" & value & "</font>"

            End Set
        End Property
        Public Property bIsHeader() As Boolean
            Get
                Return _bIsHeader
            End Get
            Set(ByVal value As Boolean)
                _bIsHeader = value
            End Set
        End Property

        Dim objResxMgr As New MaxPayroll.SatelliteResx.ResourceManagerEx
        Public Function fncGenerateMenu(Optional ByRef sSubMenuList As String = "") As String
            Dim sMenu As String = ""
            Dim stblFront As String = ""
            Dim stbllEnd As String = ""
            Dim sColAction As String = "  onMouseOver=""javascript:MOver(this.id);"" onMouseOut=""javascript:MOut(this.id);"""
            Dim sColFront As String = ""
            Dim sColEnd As String = ""
            Dim URLParam As String = ""


            Dim arrMenu As New ArrayList

            Dim sTDItem As String = ""
            Dim iTDNo As Integer = 100
            Dim bShowDottedLine As Boolean = False
            Dim lbl As New Label
            'Generate Array Item for menu base on the Type
            arrMenu = fncGenerateMenuArray()
            Dim sUserType As String = HttpContext.Current.Session(gc_Ses_UserType)
            If sUserType = Nothing Then
                sUserType = ""
            End If

            stblFront = "<table border=""0"" cellpadding=""2"" cellspacing=""0"" width=""110px"" class=""menutitle"">"

            stbllEnd = "</table>"

            Dim arrMenuItem As clsMenu
            Dim iCount As Integer
            Dim iCountHd As Integer = 0
            For iCount = 0 To arrMenu.Count - 1
                arrMenuItem = CType(arrMenu(iCount), clsMenu)
                If arrMenuItem.bIsHeader Then
                    sTDItem += "<div onclick=""SwitchMenu('sub" & iCount.ToString & "')"">" & stblFront &
                        "<tr height='25px'><td id=""Td" & iCount.ToString & """" & sColAction
                    If arrMenuItem.sPath.Trim <> "" Then
                        If iCount + 1 < arrMenu.Count - 1 Then
                            If CType(arrMenu(iCount + 1), clsMenu).bIsHeader = True Then
                                sTDItem += "onclick=""javascript:Direct('" & arrMenuItem.sPath & "');"""
                            End If
                        Else
                            sTDItem += "onclick=""javascript:Direct('" & arrMenuItem.sPath & "');"""
                        End If

                    End If

                    'sTDItem += "><asp:label ID='lbl" & iCount.ToString & "' runat=""server"">" &  & "</asp:label></td></tr></table></div>"
                    sTDItem += " align=""left""><table width=""100px"" cellpadding=0 cellspacing=0 border=0 class=""menutitle""><tr><td style=""padding-left:8"">" & fncRenderLabel(lbl, arrMenuItem.sText) & "" & "</td></tr></table></td></tr>"
                Else
                    sSubMenuList += "sub" & CStr(iCount - 1) & ","
                    sTDItem += "<div class=""submenu"" id=""sub" & CStr(iCount - 1) & """>" & "<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100px"">"
                    sTDItem += fncGenerateMenuItem(arrMenu, iCount, iTDNo)

                End If
                sTDItem += "</table></div>"
                iTDNo += 1
            Next
            sMenu = sTDItem
            If Len(sSubMenuList) > 0 AndAlso sSubMenuList.LastIndexOf(",") = Len(sSubMenuList) - 1 Then
                sSubMenuList = sSubMenuList.Substring(0, Len(sSubMenuList) - 1)
            End If

            URLParam = HttpContext.Current.Request.Url.AbsolutePath

            Dim URLSperator = URLParam.Replace("/WebForms/", "")

            Dim StronglyTypedList = arrMenu.Cast(Of clsMenu)().ToList()

            Dim arr = StronglyTypedList.Where(Function(i As clsMenu) i.sPath.Contains(URLSperator)).FirstOrDefault()


            Dim ExceptionMenuList = New List(Of String)({"PG_ViewApprMatrix.aspx", "PG_ViewLogs.aspx", "PG_BankField.aspx", "PG_ListFile.aspx", "PG_BankAccount3.aspx", "pg_MandateList.aspx", "pg_MandatesDetails.aspx", "pg_SetCutOffTime.aspx",
                                                        "PG_Mail.aspx", "PG_BankAccount.aspx", "pg_BankCodeMapping.aspx", "pg_BankList.aspx", "pg_BankMaster.aspx", "pg_BankOrgCode.aspx", "pg_Banner.aspx", "PG_BlockFile.aspx", "pg_CPS_Cheque_Maintainence.aspx",
                                                        "PG_CPSAssignChequeNo.aspx", "pg_CPSEditCharges.aspx", "PG_CPSFileSubmission.aspx", "PG_CutoffTime.aspx", "PG_DeleteFile.aspx", "PG_CustomerFormat.aspx", "PG_FileAuth.aspx", "pg_fileListing.aspx", "PG_FileReview.aspx",
                                                        "Pg_FileSettings.aspx", "pg_FileType.aspx", "pg_Group.aspx", "pg_Group3.aspx", "pg_InfenionBNMCodes.aspx", "PG_Lang.aspx", "pg_MAcknowledgeDetails.aspx", "PG_MandateFileDetails.aspx", "pg_MerchantChargeDefinition.aspx",
                                                        "PG_Message.aspx", "PG_ModifyGroup.aspx", "pg_ModifyOrganisation.aspx", "pg_ModifyOrganisation.aspx", "pg_NetMenu.aspx", "pg_PaymentService.aspx", "pg_PaymentWinMaster.aspx", "pg_Product.aspx", "PG_QueryReport.aspx",
                                                        "PG_ReportDownloader.aspx", "pg_SearchBank.aspx", "pg_SearchPaymentService.aspx", "pg_SearchPaymentWin.aspx", "pg_SearchProduct.aspx", "PG_ServiceAccount.aspx", "PG_SessionCheck.aspx", "PG_StopPayment.aspx", "pg_TrasnsCode.aspx", "pg_TransGrid.aspx",
                                                        "PG_ViewCustomer.aspx", "pg_viewOrganizationRoles.aspx", "progress.aspx", "PG_ResetPassword.aspx", "pg_ReportSearch.aspx", "PG_ReportSearch.aspx"})
            If sUserType <> "" AndAlso sUserType = "BU" Then
                ExceptionMenuList.Add("PG_CreateRole.aspx")
            End If
            If sUserType = clsCommon.fncGetPrefix(enmUserType.U_Uploader) AndAlso URLSperator = "PG_ViewReportServices.aspx" Then
                Dim URLParama = HttpContext.Current.Request.QueryString("ReportName")
                If URLParama <> MaxPayroll.mdConstant.gc_RptFileStatus Then
                    HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                End If
            End If
            If sUserType = clsCommon.fncGetPrefix(enmUserType.CA_SysAdmin) AndAlso URLSperator = "PG_ViewReportServices.aspx" Then
                Dim URLParama = HttpContext.Current.Request.QueryString("ReportName")
                If URLParama = MaxPayroll.mdConstant.gc_RptUserRole OrElse URLParama = MaxPayroll.mdConstant.gc_RptUserGroup OrElse URLParama = MaxPayroll.mdConstant.gc_RptUserLog Then
                    Dim s = ""
                Else
                    HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                End If
            End If
            If sUserType = clsCommon.fncGetPrefix(enmUserType.CA_SysAdmin) AndAlso (URLSperator = "pg_ReportSearch.aspx" OrElse URLSperator = "PG_ReportSearch.aspx") Then

                HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")

            End If
            'If arr Is Nothing Then
            '    Dim exist = ExceptionMenuList.Contains(URLSperator)
            '    If exist = False Then
            '        HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
            '    End If
            'End If
            Return sMenu
        End Function



#Region " Tryed Sub Menu"


        ''Public Function fncGenerateMenu(Optional ByRef sSubMenuList As String = "") As String
        ''    Dim sMenu As String = ""
        ''    Dim stblFront As String = ""
        ''    Dim stbllEnd As String = ""
        ''    Dim sColAction As String = "  onMouseOver=""javascript:MOver(this.id);"" onMouseOut=""javascript:MOut(this.id);"""
        ''    Dim sColFront As String = ""
        ''    Dim sColEnd As String = ""

        ''    Dim arrMenu As New ArrayList

        ''    Dim sTDItem As String = ""
        ''    Dim iTDNo As Integer = 100
        ''    Dim bShowDottedLine As Boolean = False
        ''    Dim lbl As New Label
        ''    'Generate Array Item for menu base on the Type
        ''    arrMenu = fncGenerateMenuArray()


        ''    stblFront = "<table border=""0"" cellpadding=""2"" cellspacing=""0"" width=""110px"" class=""menutitle"">"

        ''    stbllEnd = "</table>"

        ''    Dim arrMenuItem As clsMenu
        ''    Dim iCount As Integer
        ''    Dim iCountHd As Integer = 0
        ''    For iCount = 0 To arrMenu.Count - 1
        ''        arrMenuItem = CType(arrMenu(iCount), clsMenu)
        ''        If arrMenuItem.bIsHeader Then
        ''            sTDItem += "<div onclick=""SwitchMenu('sub" & iCount.ToString & "')"">" & stblFront & _
        ''                "<tr height='25px'><td id=""Td" & iCount.ToString & """" & sColAction
        ''            If arrMenuItem.sPath.Trim <> "" Then
        ''                If iCount + 1 < arrMenu.Count - 1 Then
        ''                    If CType(arrMenu(iCount + 1), clsMenu).bIsHeader = True Then
        ''                        sTDItem += "onclick=""javascript:Direct('" & arrMenuItem.sPath & "');"""
        ''                    End If
        ''                Else
        ''                    sTDItem += "onclick=""javascript:Direct('" & arrMenuItem.sPath & "');"""
        ''                End If

        ''            End If

        ''            'sTDItem += "><asp:label ID='lbl" & iCount.ToString & "' runat=""server"">" &  & "</asp:label></td></tr></table></div>"
        ''            sTDItem += " align=""left""><table width=""110px"" cellpadding=0 cellspacing=0 border=0 class=""menutitle""><tr><td style=""padding-left:8"">" & fncRenderLabel(lbl, arrMenuItem.sText) & "" & "</td></tr></table></td></tr>"
        ''        Else

        ''            If arrMenuItem.bIsSubHeader Then

        ''                sSubMenuList += "sub" & CStr(iCount - 1) & ","
        ''                sTDItem += "<div class=""submenu"" id=""sub" & CStr(iCount - 1) & """>" & "<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""110px"">"
        ''                sTDItem += fncGenerateMenuItem(arrMenu, iCount, iTDNo)
        ''            Else
        ''                sSubMenuList += "sub" & CStr(iCount - 2) & ","
        ''                sTDItem += "<div class=""submenu"" id=""sub" & CStr(iCount - 2) & """>" & "<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""110px"">"
        ''                sTDItem += fncGenerateMenuItem(arrMenu, iCount, iTDNo)
        ''            End If
        ''        End If
        ''        sTDItem += "</table></div>"
        ''        iTDNo += 1
        ''    Next
        ''    sMenu = sTDItem
        ''    If Len(sSubMenuList) > 0 AndAlso sSubMenuList.LastIndexOf(",") = Len(sSubMenuList) - 1 Then
        ''        sSubMenuList = sSubMenuList.Substring(0, Len(sSubMenuList) - 1)
        ''    End If
        ''    Return sMenu
        ''End Function
#End Region

        Private Function fncRenderLabel(ByVal lbl As Label, ByVal sValue As String) As String
            lbl.Text = ""
            If lbl.Text = "" Then
                lbl.Text = sValue
            End If
            Return lbl.Text
        End Function

        Public Function fncGenerateMenuItem(ByVal arrMenu As ArrayList, ByRef iCurrentItemNo As Integer, ByRef iTDNo As Integer) As String
            Dim sMenu As String = ""
            Dim sColFront As String = ""
            Dim sColEnd As String = ""
            Dim iSubNo As Integer = iCurrentItemNo - 1
            Dim iCount As Integer
            Dim arrMenuItem As clsMenu
            Dim lbl As New Label

            For iCount = iCurrentItemNo To arrMenu.Count - 1

                arrMenuItem = CType(arrMenu(iCount), clsMenu)
                If arrMenuItem.bIsHeader = True Then
                    Exit For
                End If
                If arrMenuItem.sPath.Trim = "" Then
                    sColFront = "<tr><td  id=""Td" & iCount.ToString & """  onMouseOver=""javascript:MOver(this.id);"" onMouseOut=""javascript:MOut(this.id);"">"
                Else
                    sColFront = "<tr><td id=""Td" & iCount.ToString & """  onMouseOver=""javascript:MOver(this.id);"" onMouseOut=""javascript:MOut(this.id);"" onclick=""javascript:Direct('" & arrMenuItem.sPath & "');"" height=""23px"">"
                End If
                sMenu += sColFront & "<table width=""100px"" class=""menuitem"" cellpadding=2 cellspacing=0 border=0><tr><td align=""right"" valign=""top"" width=""20px""><img src=../Include/images/menu_arrow.png border=0></td><td width=""130px"">" & fncRenderLabel(lbl, arrMenuItem.sText) & "</td></tr></table>" & "</td></tr>"
                iTDNo += 1
            Next


            iCurrentItemNo = iCount - 1
            Return sMenu
        End Function

        Private Function fncGenerateMenuArray_old() As ArrayList
            Dim oitem As New clsCommon

            Dim arrMenu As New ArrayList
            Dim arrMenuItem As New clsMenu
            Dim sUserType As String = HttpContext.Current.Session(gc_Ses_UserType)
            If sUserType = Nothing Then
                sUserType = ""
            End If
            Dim sUserID As String = HttpContext.Current.Session(gc_Ses_UserID)
            If sUserID = Nothing Then
                sUserID = ""
            End If
            Dim sGroup As String = HttpContext.Current.Session(gc_Ses_GroupID)
            If sGroup = Nothing Then
                sGroup = "0"
            End If
            Dim sNoMenu As String = HttpContext.Current.Session("NoMenu")
            If sNoMenu = Nothing Then
                sNoMenu = "False"
            End If
            If sNoMenu = "True" Then
                sUserType = ""
            End If

            Const sMailbox As String = "Mail Box"
            Const sGroupChg As String = "Group Change"

            If sNoMenu = "False" AndAlso ConfigurationManager.AppSettings("USER") = "CUSTOMER" AndAlso (IsNumeric(sUserID) AndAlso CInt(sUserID) > 0) AndAlso (sUserType <> gc_UT_SysAdmin AndAlso sUserType <> gc_UT_SysAuth) AndAlso CInt(sGroup) = 0 Then
                arrMenuItem = New clsMenu
                arrMenuItem.sPath = ""
                arrMenuItem.sText = "User Account"
                arrMenuItem.bIsHeader = True
                arrMenu.Add(arrMenuItem)

                arrMenuItem = New clsMenu
                arrMenuItem.sPath = "PG_ChangePassword.aspx"
                arrMenuItem.sText = "Change Password"
                arrMenuItem.bIsHeader = False
                arrMenu.Add(arrMenuItem)

                If sUserType <> clsCommon.fncGetPrefix(enmUserType.IU_InquiryUser) AndAlso sUserType <> clsCommon.fncGetPrefix(enmUserType.BD_BankDownloader) Then
                    arrMenuItem = New clsMenu
                    arrMenuItem.sPath = "PG_ChangeAuthCode.aspx"
                    arrMenuItem.sText = "Change Validation Code"
                    arrMenuItem.bIsHeader = False
                    arrMenu.Add(arrMenuItem)
                End If

                arrMenuItem = New clsMenu
                arrMenuItem.sPath = "PG_GroupChange.aspx?Mode=True"
                arrMenuItem.sText = sGroupChg
                arrMenuItem.bIsHeader = True
                arrMenu.Add(arrMenuItem)

                arrMenuItem = New clsMenu
                arrMenuItem.sPath = "PG_Logout.aspx"
                arrMenuItem.sText = "Logout"
                arrMenuItem.bIsHeader = True
                arrMenu.Add(arrMenuItem)

                Return arrMenu
            End If

            If sUserType <> clsCommon.fncGetPrefix(enmUserType.BO_BankOperator) And sUserType <> "" Then

                arrMenuItem = New clsMenu
                arrMenuItem.sPath = "PG_Inbox.aspx"
                arrMenuItem.sText = sMailbox
                arrMenuItem.bIsHeader = True
                arrMenu.Add(arrMenuItem)

                arrMenuItem = New clsMenu
                arrMenuItem.sPath = ""
                arrMenuItem.sText = "User Account"
                arrMenuItem.bIsHeader = True
                arrMenu.Add(arrMenuItem)

                arrMenuItem = New clsMenu
                arrMenuItem.sPath = "PG_ChangePassword.aspx"
                arrMenuItem.sText = "Change Password"
                arrMenuItem.bIsHeader = False
                arrMenu.Add(arrMenuItem)

                If sUserType <> clsCommon.fncGetPrefix(enmUserType.IU_InquiryUser) AndAlso sUserType <> clsCommon.fncGetPrefix(enmUserType.BD_BankDownloader) Then
                    arrMenuItem = New clsMenu
                    arrMenuItem.sPath = "PG_ChangeAuthCode.aspx"
                    arrMenuItem.sText = "Change Validation Code"
                    arrMenuItem.bIsHeader = False
                    arrMenu.Add(arrMenuItem)
                End If

            End If



            Select Case sUserType


                Case ""
                    arrMenuItem = New clsMenu
                    arrMenuItem.sPath = "PG_Index.aspx"
                    arrMenuItem.sText = "Login"
                    arrMenuItem.bIsHeader = True
                    arrMenu.Add(arrMenuItem)

                    'arrMenuItem = New clsMenu
                    'arrMenuItem.sPath = ""
                    'arrMenuItem.sText = "FAQ"
                    'arrMenuItem.bIsHeader = True
                    'arrMenu.Add(arrMenuItem)


                Case clsCommon.fncGetPrefix(enmUserType.BA_BankAdmin)

                    If True = True Then



                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Setup"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)



                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_CreateRole.aspx"
                        arrMenuItem.sText = "Create Bank User"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewRoles.aspx"
                        arrMenuItem.sText = "Modify / Delete Bank User(s)"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        'commented by farid 06032017
                        'arrMenuItem = New clsMenu
                        'arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.H2HUser.ToString
                        'arrMenuItem.sText = "Organization User Maintenance"
                        'arrMenuItem.bIsHeader = False
                        'arrMenu.Add(arrMenuItem)

                        '** Added by Naresh on -23-02-11

                        'arrMenuItem = New clsMenu
                        'arrMenuItem.sPath = "pg_InfenionBNMCodes.aspx"
                        'arrMenuItem.sText = "Infenion BNMCodes"
                        'arrMenuItem.bIsHeader = False
                        'arrMenu.Add(arrMenuItem)
                        'end commented

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_RoleAccessRights.aspx"
                        arrMenuItem.sText = "Role Access Rights"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "fd"
                        arrMenuItem.sText = "Authorization"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=View"
                        arrMenuItem.sText = "Pending"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Reject"
                        arrMenuItem.sText = "Rejected"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Done"
                        arrMenuItem.sText = "Accepted"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Audit Trails"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BU&Log=Acc"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankUserDesc & " Access Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BU&Log=Fail"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankUserDesc & " Unsuccessful Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BU&Log=Mod"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankUserDesc & " Modification Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=IU&Log=Acc"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_InquiryUserDesc & " Access Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=IU&Log=Fail"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_InquiryUserDesc & " Unsuccessful Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AdministrativeAuditTrail.aspx"
                        arrMenuItem.sText = "Administrative Audit Trail Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViolationReport.aspx"
                        arrMenuItem.sText = "Violation Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_UserListingReport.aspx"
                        arrMenuItem.sText = "User Listing Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessMatrix.aspx"
                        arrMenuItem.sText = "Access Matrix Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)




                    End If

                Case clsCommon.fncGetPrefix(enmUserType.BU_BankUser)

                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Setup"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        'START - Add CPS Phase III
                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_CPS_Cheque_Allocation.aspx?Mode=Allocation"
                        arrMenuItem.sText = "CPS Cheque Allocation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_CPS_Cheque_Allocation.aspx?Mode=Submission"
                        arrMenuItem.sText = "CPS File For Submission"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)
                        'STOP


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_BankFormat.aspx"
                        arrMenuItem.sText = "Bank File Settings"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        '**Commented by Naresh-07-04-11 
                        'arrMenuItem = New clsMenu
                        'arrMenuItem.sPath = "PG_CutoffTime.aspx"
                        'arrMenuItem.sText = "Cut Off Time"
                        'arrMenuItem.bIsHeader = False
                        'arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SetCutoffTime.aspx"
                        arrMenuItem.sText = "Cut Off Time"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_PinMailerSerial.aspx"
                        arrMenuItem.sText = "Pin Mailer Serial"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_PinRequisition.aspx"
                        arrMenuItem.sText = "Pin Mailer Requisition"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_PinAuthorize.aspx"
                        arrMenuItem.sText = "Pin Mailer Authorization"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_PinGenerate.aspx"
                        arrMenuItem.sText = "Pin Mailer Generation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_Column_Field_Relation.aspx"
                        arrMenuItem.sText = "Column Field Relation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_FileTypeSettings.aspx"
                        arrMenuItem.sText = "File Type Settings"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Organization"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_CPSOrgList.aspx"
                        arrMenuItem.sText = "CPS Transaction Charges"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_Organization.aspx"
                        arrMenuItem.sText = "Create Organization"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.Modify.ToString
                        arrMenuItem.sText = "Modify Organization"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.File.ToString
                        arrMenuItem.sText = "Organization File Settings"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.Bank.ToString
                        arrMenuItem.sText = "Organization Bank Accounts"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.Epf.ToString
                        arrMenuItem.sText = "Organization EPF Employer No."
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.Socso.ToString
                        arrMenuItem.sText = "Organization SOCSO Employer No."
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.LHDN.ToString
                        arrMenuItem.sText = "Organization LHDN Employer No."
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.ZAKAT.ToString
                        arrMenuItem.sText = "Organization ZAKAT Employer Reference"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewPassword.aspx"
                        arrMenuItem.sText = "Modify " & gc_UT_SysAdminDesc & " / " & gc_UT_SysAuthDesc
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)



                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_vieworganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.Mandates.ToString
                        arrMenuItem.sText = "Mandates Details"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)
                        ''CPS Phase III START
                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.CPS_Cheque_Details.ToString()
                        arrMenuItem.sText = "CPS Cheque Details Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.CPS_Monthly_Cheque.ToString()
                        arrMenuItem.sText = "CPS Monthly Cheque Status"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.CPS_Stale_Cheque.ToString()
                        arrMenuItem.sText = "CPS Stale Cheque"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque.ToString()
                        arrMenuItem.sText = "CPS Unclaimed Cheque"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.CPS_Damage_Cheque.ToString()
                        arrMenuItem.sText = "CPS Damage Cheque"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_ShowReport.aspx?ReportName=" & ReportHelper.MandateSearchType.CPS_Charges_Report.ToString()
                        arrMenuItem.sText = "CPS Charges Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.CPS_Complience_Report.ToString()
                        arrMenuItem.sText = "CPS Compliance Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)



                        ''CPS Phase III STOP
                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Manual_Regis.ToString()
                        arrMenuItem.sText = "Mandate Manual Registation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Auto_Regis.ToString()
                        arrMenuItem.sText = "Mandate Auto Registation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Auto.ToString()
                        arrMenuItem.sText = "Mandate Movement Auto"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Manual.ToString()
                        arrMenuItem.sText = "Mandate Movement Manual"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Upload.ToString()
                        arrMenuItem.sText = "Daily Billing File - Upload"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Reject.ToString()
                        arrMenuItem.sText = "Daily Billing File - Rejected"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Summary.ToString()
                        arrMenuItem.sText = "Direct Debit Summary Report"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUploadList
                        arrMenuItem.sText = "UploadList"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)



                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptFileStatus
                        arrMenuItem.sText = "File Status"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptOrganizationList
                        arrMenuItem.sText = "Organization List"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserRole
                        arrMenuItem.sText = "User Role"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserGroup
                        arrMenuItem.sText = "User Group"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserLock
                        arrMenuItem.sText = "User Lock"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria2.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserExpiry
                        arrMenuItem.sText = "User Expiry"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptPinGeneration
                        arrMenuItem.sText = "Pin Generation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria2.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptDormant
                        arrMenuItem.sText = "Dormant"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptCancel
                        arrMenuItem.sText = "Cancellation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptRegistration
                        arrMenuItem.sText = "Registration"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria2.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptFileSubmission
                        arrMenuItem.sText = "File Upload"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If

                Case clsCommon.fncGetPrefix(enmUserType.BS_BankAuthorizer)

                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Authorization"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Edit"
                        arrMenuItem.sText = "Pending"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Reject"
                        arrMenuItem.sText = "Rejected"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Done"
                        arrMenuItem.sText = "Accepted"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)



                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_PinAuthorize.aspx"
                        arrMenuItem.sText = "Pin Mailer"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Audit Trails"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BS&Log=Acc"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankAuthDesc & "Access Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BS&Log=Fail"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankAuthDesc & " Unsuccessful Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BA&Log=Acc"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankAdminDesc & " Access Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BA&Log=Fail"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankAdminDesc & " Unsuccessful Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BA&Log=Mod"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankAdminDesc & " Modification Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BA&Log=Del"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankAdminDesc & " Deletion Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BU&Log=Acc"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankUserDesc & " Access Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BU&Log=Fail"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankUserDesc & " Unsuccessful Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=BU&Log=Mod"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_BankUserDesc & " Modification Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=IU&Log=Acc"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_InquiryUserDesc & " Access Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_AccessLogs.aspx?User=IU&Log=Fail"
                        arrMenuItem.sText = MaxPayroll.mdConstant.gc_UT_InquiryUserDesc & " Unsuccessful Logs"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If


                Case clsCommon.fncGetPrefix(enmUserType.BD_BankDownloader)

                    If True = True Then


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "File Download(WEB)"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadApprovedFile.aspx?FileType=Autopay File"
                        arrMenuItem.sText = "Autopay File"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadApprovedFile.aspx?FileType=CPS File"
                        arrMenuItem.sText = "CPS File"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadApprovedFile.aspx?FileType=Direct Debit"
                        arrMenuItem.sText = "Direct Debit"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "File Download(H2H)"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadApprovedFile.aspx?FileType=Autopay File&Mod=H2H"
                        arrMenuItem.sText = "Autopay File"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadApprovedFile.aspx?FileType=AutopaySNA File&Mod=H2H"
                        arrMenuItem.sText = "AutopaySNA File"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadApprovedFile.aspx?FileType=CPS File&Mod=H2H"
                        arrMenuItem.sText = "CPS File"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadApprovedFile.aspx?FileType=Direct Debit&Mod=H2H"
                        arrMenuItem.sText = "Direct Debit"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If

                Case clsCommon.fncGetPrefix(enmUserType.RD_ReportDownloader)

                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_GroupChange.aspx?Mode=True"
                        arrMenuItem.sText = sGroupChg
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_DownloadCIMBReport.aspx"
                        arrMenuItem.sText = "Report Download"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        'Added by Naresh On-23-02-11
                        If oitem.CheckFileType(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID), "PayLinkPayroll") Then
                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptInfenionDetails & "&LoadOrgIDSession=1"
                            arrMenuItem.sText = "PayLink Report"
                            arrMenuItem.bIsHeader = True
                            arrMenu.Add(arrMenuItem)
                        End If

                    End If

                Case clsCommon.fncGetPrefix(enmUserType.IU_InquiryUser)


                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Organization"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=Modify"
                        arrMenuItem.sText = "View Organization"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=Bank"
                        arrMenuItem.sText = "Organization Bank Accounts"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=File"
                        arrMenuItem.sText = "Organization File Settings"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=Epf"
                        arrMenuItem.sText = "Organization EPF Employer No."
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=Socso"
                        arrMenuItem.sText = "Organization SOCSO Employer No."
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=LHDN"
                        arrMenuItem.sText = "Organization LHDN Employer No."
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewOrganisation.aspx?Req=ZAKAT"
                        arrMenuItem.sText = "Organization ZAKAT Employer Reference"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewPassword.aspx"
                        arrMenuItem.sText = "View " & gc_UT_SysAdminDesc & " / " & gc_UT_SysAuthDesc
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUploadList
                        arrMenuItem.sText = "Upload List"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptFileStatus
                        arrMenuItem.sText = "File Status"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptOrganizationList
                        arrMenuItem.sText = "Organization List"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserRole
                        arrMenuItem.sText = "User Role"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserGroup
                        arrMenuItem.sText = "User Group"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserLock
                        arrMenuItem.sText = "User Lock"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria2.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserExpiry
                        arrMenuItem.sText = "User Expiry"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptPinGeneration
                        arrMenuItem.sText = "Pin Generation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria2.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptDormant
                        arrMenuItem.sText = "Dormant"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptCancel
                        arrMenuItem.sText = "Cancellation"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptRegistration
                        arrMenuItem.sText = "Registration"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "pg_SearchReportServicesByGlobalCriteria2.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptFileSubmission
                        arrMenuItem.sText = "File Upload"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If


                Case clsCommon.fncGetPrefix(enmUserType.CA_SysAdmin)

                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Setup"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_Group2.aspx?Mod=New"
                        arrMenuItem.sText = "Create Groups"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_CreateRole.aspx"
                        arrMenuItem.sText = "Create Users"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Maintenance"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ListGroup.aspx"
                        arrMenuItem.sText = "Modify Groups"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewRoles.aspx"
                        arrMenuItem.sText = "Modify Users"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)



                        If HttpContext.Current.Session(MaxPayroll.mdConstant.gc_Ses_VerificationType) = "DUAL" Then

                            If True = True Then

                                arrMenuItem = New clsMenu
                                arrMenuItem.sPath = ""
                                arrMenuItem.sText = "Authorization"
                                arrMenuItem.bIsHeader = True
                                arrMenu.Add(arrMenuItem)

                                arrMenuItem = New clsMenu
                                arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=View"
                                arrMenuItem.sText = "Pending"
                                arrMenuItem.bIsHeader = False
                                arrMenu.Add(arrMenuItem)

                                arrMenuItem = New clsMenu
                                arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Reject"
                                arrMenuItem.sText = "Rejected"
                                arrMenuItem.bIsHeader = False
                                arrMenu.Add(arrMenuItem)

                                arrMenuItem = New clsMenu
                                arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Done"
                                arrMenuItem.sText = "Accepted"
                                arrMenuItem.bIsHeader = False
                                arrMenu.Add(arrMenuItem)

                            End If

                        End If


                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserGroup & "&LoadOrgIDSession=1"
                        arrMenuItem.sText = "Group List"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserRole & "&LoadOrgIDSession=1"
                        arrMenuItem.sText = "User List"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserLog & "&LoadOrgIDSession=1"
                        arrMenuItem.sText = "User Log"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If


                Case clsCommon.fncGetPrefix(enmUserType.SA_SysAuthorizer)

                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Authorization"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Edit"
                        arrMenuItem.sText = "Pending"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Reject"
                        arrMenuItem.sText = "Rejected"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ApprMatrix.aspx?Mode=Done"
                        arrMenuItem.sText = "Accepted"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserGroup & "&LoadOrgIDSession=1"
                        arrMenuItem.sText = "Group List"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserRole & "&LoadOrgIDSession=1"
                        arrMenuItem.sText = "User List"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptUserLog & "&LoadOrgIDSession=1"
                        arrMenuItem.sText = "User Log"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If

                Case clsCommon.fncGetPrefix(enmUserType.U_Uploader)


                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_GroupChange.aspx?Mode=True"
                        arrMenuItem.sText = sGroupChg
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_FileUpload.aspx"
                        arrMenuItem.sText = "File Upload"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        'Added by Naresh On-08-04-11
                        If oitem.CheckFileType(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID),
                                               "Hybrid Mandate", "Hybrid AutopaySNA", "Hybrid Direct Debit") Then
                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_PTPTNUpload.aspx"
                            arrMenuItem.sText = "Bulk Upload"
                            arrMenuItem.bIsHeader = True
                            arrMenu.Add(arrMenuItem)

                        End If

                        'Added by Teja On-18-06-12
                        If oitem.CheckFileType(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID),
                                               "Direct Debit") Then
                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "PG_UploadFile.aspx?" & WebHelper.ServiceType & MaxGeneric.clsGeneric.AddEqual() & _Helper.DirectDebit_Name()
                            arrMenuItem.sText = "Direct Debit Upload"
                            arrMenuItem.bIsHeader = True
                            arrMenu.Add(arrMenuItem)

                        End If

                        If oitem.fncCheckDirectDebit(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID)) Then
                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "PG_MandateUpload.aspx"
                            arrMenuItem.sText = "Mandate Upload"
                            arrMenuItem.bIsHeader = True
                            arrMenu.Add(arrMenuItem)
                        End If

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        If oitem.fncCheckDirectDebit(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID)) Then

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Manual_Regis.ToString()
                            arrMenuItem.sText = "Mandate Manual Registation"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Auto_Regis.ToString()
                            arrMenuItem.sText = "Mandate Auto Registation"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Auto.ToString()
                            arrMenuItem.sText = "Mandate Movement Auto"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Manual.ToString()
                            arrMenuItem.sText = "Mandate Movement Manual"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)


                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Upload.ToString()
                            arrMenuItem.sText = "Daily Billing File - Upload"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Reject.ToString()
                            arrMenuItem.sText = "Daily Billing File - Rejected"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Summary.ToString()
                            arrMenuItem.sText = "Direct Debit Summary Report"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)
                        End If

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptFileStatus
                        arrMenuItem.sText = "File Status"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If

                Case clsCommon.fncGetPrefix(enmUserType.R_Reviewer)


                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_GroupChange.aspx?Mode=True"
                        arrMenuItem.sText = sGroupChg
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_FileList.aspx"
                        arrMenuItem.sText = "File Review"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        If oitem.fncCheckDirectDebit(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID)) Then


                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Manual_Regis.ToString()
                            arrMenuItem.sText = "Mandate Manual Registation"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Auto_Regis.ToString()
                            arrMenuItem.sText = "Mandate Auto Registation"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Auto.ToString()
                            arrMenuItem.sText = "Mandate Movement Auto"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Manual.ToString()
                            arrMenuItem.sText = "Mandate Movement Manual"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)


                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Upload.ToString()
                            arrMenuItem.sText = "Daily Billing File - Upload"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Reject.ToString()
                            arrMenuItem.sText = "Daily Billing File - Rejected"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Summary.ToString()
                            arrMenuItem.sText = "Direct Debit Summary Report"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)
                        End If

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptFileStatus
                        arrMenuItem.sText = "File Status"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If

                Case clsCommon.fncGetPrefix(enmUserType.A_Authorizer)

                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_GroupChange.aspx?Mode=True"
                        arrMenuItem.sText = sGroupChg
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_FileListAuth.aspx"
                        arrMenuItem.sText = "File Approve & Submission"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Reports"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)
                        If oitem.fncCheckDirectDebit(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID)) Then

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Manual_Regis.ToString()
                            arrMenuItem.sText = "Mandate Manual Registation"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Auto_Regis.ToString()
                            arrMenuItem.sText = "Mandate Auto Registation"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Auto.ToString()
                            arrMenuItem.sText = "Mandate Movement Auto"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Movement_Manual.ToString()
                            arrMenuItem.sText = "Mandate Movement Manual"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)


                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Upload.ToString()
                            arrMenuItem.sText = "Daily Billing File - Upload"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Billing_Reject.ToString()
                            arrMenuItem.sText = "Daily Billing File - Rejected"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)

                            arrMenuItem = New clsMenu
                            arrMenuItem.sPath = "pg_ReportSearch.aspx?ReportName=" & ReportHelper.MandateSearchType.Mandate_Summary.ToString()
                            arrMenuItem.sText = "Direct Debit Summary Report"
                            arrMenuItem.bIsHeader = False
                            arrMenu.Add(arrMenuItem)
                        End If
                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_ViewReportServices.aspx?ReportName=" & MaxPayroll.mdConstant.gc_RptFileStatus
                        arrMenuItem.sText = "File Status"
                        arrMenuItem.bIsHeader = False
                        arrMenu.Add(arrMenuItem)

                    End If

                Case clsCommon.fncGetPrefix(enmUserType.I_Interceptor)

                    If True = True Then

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = "PG_GroupChange.aspx?Mode=True"
                        arrMenuItem.sText = sGroupChg
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                        arrMenuItem = New clsMenu
                        arrMenuItem.sPath = ""
                        arrMenuItem.sText = "Report"
                        arrMenuItem.bIsHeader = True
                        arrMenu.Add(arrMenuItem)

                    End If



            End Select



            If sUserType <> "" Then
                arrMenuItem = New clsMenu
                arrMenuItem.sPath = "PG_Logout.aspx"
                arrMenuItem.sText = "Logout"
                arrMenuItem.bIsHeader = True
                arrMenu.Add(arrMenuItem)
            End If



            Return arrMenu

        End Function

        Private Function fncGenerateMenuArray() As ArrayList
            Dim oitem As New clsCommon

            Dim arrMenu As New ArrayList
            Dim arrMenuItem As New clsMenu
            Dim sUserType As String = HttpContext.Current.Session(gc_Ses_UserType)
            If sUserType = Nothing Then
                sUserType = ""
            End If
            Dim sUserID As String = HttpContext.Current.Session(gc_Ses_UserID)
            If sUserID = Nothing Then
                sUserID = ""
            End If
            Dim sGroup As String = HttpContext.Current.Session(gc_Ses_GroupID)
            If sGroup = Nothing Then
                sGroup = "0"
            End If
            Dim sNoMenu As String = HttpContext.Current.Session("NoMenu")
            If sNoMenu = Nothing Then
                sNoMenu = "False"
            End If
            If sNoMenu = "True" Then
                sUserType = ""
            End If

            Dim newArraList As ArrayList = GetRoleArrayList(sUserType, arrMenu, oitem)

            Return newArraList

        End Function


        Private Function GetRoleRights(roleName As String) As DataSet


            Dim clsBank As New MaxPayroll.clsBank
            Dim dsLogs As New DataSet

            Try

                dsLogs = clsBank.GetRoleRights(roleName)

            Catch ex As Exception

            End Try


            Return dsLogs

        End Function

        Private Function GetRoleArrayList(sUserType As String, arrMenu As ArrayList, oitem As clsCommon) As ArrayList

            Dim arrMenu1 As New ArrayList
            Dim arrMenuItem1 As New clsMenu

            Try

                If sUserType = "" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "PG_Index.aspx"
                    arrMenuItem1.sText = "Login"
                    arrMenuItem1.bIsHeader = True
                    arrMenu1.Add(arrMenuItem1)

                    Return arrMenu1
                End If

                Dim ds As DataSet = GetRoleRights(sUserType)



                If ds.Tables("LOGS").Rows.Count > 0 Then

                    Dim rights = From data In ds.Tables("LOGS").AsEnumerable()
                                 Where data.Field(Of Boolean)("Rights") = True
                                 Select data


                    Dim isHeader As Boolean = False
                    Dim menu As String = ""

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "PG_Inbox.aspx"
                    arrMenuItem1.sText = "Mail Box"
                    arrMenuItem1.bIsHeader = True
                    arrMenu1.Add(arrMenuItem1)

                    For Each item In rights


                        Dim menuName = item("MenuName")
                        Dim pageName = item("PageName")
                        Dim right = item("Rights")
                        Dim sPath As String = ""
                        Dim urlPath As String = item("UrlPath")


                        If (menu = "") Then
                            isHeader = True
                            menu = menuName
                            sPath = ""
                        Else
                            isHeader = False

                            If menu <> menuName Then
                                isHeader = True
                                sPath = ""
                                menu = menuName
                            End If

                        End If

                        If isHeader Then

                            AssignHeader(sUserType, menuName, pageName, urlPath, arrMenu1, oitem)

                            AssignSubMenu(sUserType, menuName, pageName, urlPath, arrMenu1, oitem)

                        Else

                            AssignSubMenu(sUserType, menuName, pageName, urlPath, arrMenu1, oitem)

                        End If



                    Next



                End If

                If sUserType <> "" Then
                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "PG_Logout.aspx"
                    arrMenuItem1.sText = "Logout"
                    arrMenuItem1.bIsHeader = True
                    arrMenu1.Add(arrMenuItem1)

                End If

                'Dim missingInList1 As ArrayList = GetDifference(arrMenu, arrMenu1)
                'Dim missingInList2 As ArrayList = GetDifference(arrMenu1, arrMenu)

            Catch ex As Exception
                Dim msg As String = ex.Message
            End Try

            Return arrMenu1

        End Function

        Function GetDifference(ByVal list1 As ArrayList, ByVal list2 As ArrayList) As ArrayList
            Dim difference As New ArrayList()

            list1.Sort()
            list2.Sort()

            For Each item In list2
                If Not list1.Contains(item) Then
                    difference.Add(item)
                End If
            Next

            Return difference
        End Function

        Sub AssignSubMenu(ByVal sUserType As String, ByVal menuName As String, ByVal pageName As String, ByVal urlPath As String, ByRef arrMenu1 As ArrayList, oitem As clsCommon)
            Dim arrMenuItem1 As New clsMenu
            Try

                If menuName = "Group Change" _
                Or menuName = "Report Download" _
                Or menuName = "PayLink Report" _
                Or menuName = "File Upload" _
                Or menuName = "Bulk Upload" _
                Or menuName = "Direct Debit Upload" _
                Or menuName = "Mandate Upload" _
                Or menuName = "File Review" _
                Or menuName = "File Approve & Submission" _
                Or menuName = "Report" _
                    Then
                    Exit Sub
                End If

                If menuName = "Authorization" And pageName = "Pending" And sUserType = "BS" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "PG_ApprMatrix.aspx?Mode=Edit"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Authorization" And sUserType = "CA" Then

                    If (HttpContext.Current.Session(MaxPayroll.mdConstant.gc_Ses_VerificationType) = "DUAL") Then

                        arrMenuItem1 = New clsMenu
                        arrMenuItem1.sPath = urlPath
                        arrMenuItem1.sText = pageName
                        arrMenuItem1.bIsHeader = False
                        arrMenu1.Add(arrMenuItem1)


                    End If

                ElseIf menuName = "Authorization" And pageName = "Pending" And sUserType = "SA" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "PG_ApprMatrix.aspx?Mode=Edit"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)



                ElseIf menuName = "Reports" And pageName <> "File Status" And (sUserType = "U" Or sUserType = "R" Or sUserType = "A") Then

                    If oitem.fncCheckDirectDebit(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID)) Then

                        arrMenuItem1 = New clsMenu
                        arrMenuItem1.sPath = urlPath
                        arrMenuItem1.sText = pageName
                        arrMenuItem1.bIsHeader = False
                        arrMenu1.Add(arrMenuItem1)

                    End If

                ElseIf menuName = "Reports" And pageName = "Mandate Manual Registation" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Manual_Regis"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "Mandate Auto Registation" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Auto_Regis"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "Mandate Movement Auto" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Movement_Auto"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "Mandate Movement Manual" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Movement_Manual"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "Mandate Movement Manual" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Movement_Manual"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "Daily Billing File - Upload" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Billing_Upload"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "Daily Billing File - Rejected" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Billing_Reject"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "Direct Debit Summary Report" And sUserType = "BU" Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=Mandate_Summary"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                ElseIf menuName = "Reports" And pageName = "File Status" And (sUserType = "BU" Or sUserType = "IU") Then

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = "pg_SearchReportServicesByOrganization.aspx?ReportName=FileStatus"
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)



                ElseIf menuName = "Report" And pageName = "Report" And sUserType = "I" Then


                Else

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = urlPath
                    arrMenuItem1.sText = pageName
                    arrMenuItem1.bIsHeader = False
                    arrMenu1.Add(arrMenuItem1)

                End If

            Catch ex As Exception

            End Try

        End Sub


        Sub AssignHeader(ByVal sUserType As String, ByVal menuName As String, ByVal pageName As String, ByVal urlPath As String, ByRef arrMenu1 As ArrayList, oitem As clsCommon)
            Dim arrMenuItem1 As New clsMenu
            Try

                If menuName = "Authorization" And sUserType = "CA" Then

                    If (HttpContext.Current.Session(MaxPayroll.mdConstant.gc_Ses_VerificationType) = "DUAL") Then

                        arrMenuItem1 = New clsMenu
                        arrMenuItem1.sPath = ""
                        arrMenuItem1.sText = menuName
                        arrMenuItem1.bIsHeader = True
                        arrMenu1.Add(arrMenuItem1)


                    End If

                ElseIf menuName = "Group Change" _
                    Or menuName = "Report Download" _
                    Or menuName = "File Upload" _
                    Or menuName = "File Review" _
                    Or menuName = "File Approve & Submission" _
                    Or menuName = "Report" _
                    Then


                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = urlPath
                    arrMenuItem1.sText = menuName
                    arrMenuItem1.bIsHeader = True
                    arrMenu1.Add(arrMenuItem1)


                ElseIf menuName = "PayLink Report" And pageName = "PayLink Report" And sUserType = "RD" Then

                    If oitem.CheckFileType(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID), "PayLinkPayroll") Then

                        arrMenuItem1 = New clsMenu
                        arrMenuItem1.sPath = urlPath
                        arrMenuItem1.sText = menuName
                        arrMenuItem1.bIsHeader = True
                        arrMenu1.Add(arrMenuItem1)

                    End If

                ElseIf menuName = "Bulk Upload" And pageName = "Bulk Upload" And sUserType = "U" Then

                    If oitem.CheckFileType(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID),
                                               "Hybrid Mandate", "Hybrid AutopaySNA", "Hybrid Direct Debit") Then

                        arrMenuItem1 = New clsMenu
                        arrMenuItem1.sPath = urlPath
                        arrMenuItem1.sText = menuName
                        arrMenuItem1.bIsHeader = True
                        arrMenu1.Add(arrMenuItem1)

                    End If

                ElseIf menuName = "Direct Debit Upload" And pageName = "Direct Debit Upload" And sUserType = "U" Then

                    If oitem.CheckFileType(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID),
                                               "Direct Debit") Then

                        arrMenuItem1 = New clsMenu
                        arrMenuItem1.sPath = urlPath
                        arrMenuItem1.sText = menuName
                        arrMenuItem1.bIsHeader = True
                        arrMenu1.Add(arrMenuItem1)

                    End If

                ElseIf menuName = "Mandate Upload" And pageName = "Mandate Upload" And sUserType = "U" Then

                    If oitem.fncCheckDirectDebit(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_GroupID)) Then

                        arrMenuItem1 = New clsMenu
                        arrMenuItem1.sPath = urlPath
                        arrMenuItem1.sText = menuName
                        arrMenuItem1.bIsHeader = True
                        arrMenu1.Add(arrMenuItem1)

                    End If

                Else

                    arrMenuItem1 = New clsMenu
                    arrMenuItem1.sPath = ""
                    arrMenuItem1.sText = menuName
                    arrMenuItem1.bIsHeader = True
                    arrMenu1.Add(arrMenuItem1)

                End If

            Catch ex As Exception

            End Try

        End Sub

    End Class
End Namespace