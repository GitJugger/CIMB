Imports Microsoft.VisualBasic

Public Class clsCIMBReport
    Private _FileName As String
    Public Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Private _FileDateTime As DateTime
    Private _FileType As String

    Public Property FileType() As String
        Get
            Return _FileType
        End Get
        Set(ByVal value As String)
            _FileType = value
        End Set
    End Property
    Private _OriFileName As String
    Public Property OriFileName() As String
        Get
            Return _OriFileName
        End Get
        Set(ByVal value As String)
            _OriFileName = value
        End Set
    End Property
    Public Property FileDateTime() As DateTime
        Get
            Return _FileDateTime
        End Get
        Set(ByVal value As DateTime)
            _FileDateTime = value
        End Set
    End Property

    Private _FileLastAccess As DateTime
    Public Property FileLastAccess() As DateTime
        Get
            Return _FileLastAccess
        End Get
        Set(ByVal value As DateTime)
            _FileLastAccess = value
        End Set
    End Property
End Class
