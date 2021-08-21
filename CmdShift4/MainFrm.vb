Public Class MainFrm
    Dim WithEvents keyhook As New KeyboardHook()
    Dim altPressed As Boolean = False
    Private Sub kbHook_KeyDown(ByVal Key As System.Windows.Forms.Keys) Handles keyhook.KeyDown
        If Key = Keys.LMenu Or Key = Keys.RMenu Then
            altPressed = True
        End If
    End Sub
    Dim f As New Form1
    Private Sub kbHook_KeyUp(ByVal Key As System.Windows.Forms.Keys) Handles keyhook.KeyUp
        If Key = Keys.LMenu Or Key = Keys.RMenu Then
            altPressed = False
        End If
        If f Is Nothing Or f.IsDisposed Then
            f = New Form1
        End If
        If Key = Keys.PrintScreen And Not altPressed And Not f.Visible Then
                f.Show()
        End If
    End Sub
    Private Sub MainFrm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Visible = False
        Me.Hide()
    End Sub

    Private Sub MainFrm_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
        Me.Visible = False
        Me.Hide()
    End Sub
End Class