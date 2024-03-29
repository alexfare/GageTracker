Module GlobalVars
    Private _userActive As Boolean = False

    Public Property UserActive As Boolean
        Get
            Return _userActive
        End Get
        Set(value As Boolean)
            _userActive = value
        End Set
    End Property

End Module
