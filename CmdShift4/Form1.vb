Public Class Form1
    Dim fg As Graphics
    Dim cr As Rectangle
    Dim sp As Point
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TopMost = True
        Me.Top = 0
        Me.Left = 0
        Me.Width = 0
        Me.Height = 0
        For Each scr As Screen In Screen.AllScreens
            If Me.Top > scr.Bounds.Top Then Me.Top = scr.Bounds.Top
            If Me.Left > scr.Bounds.Left Then Me.Left = scr.Bounds.Left
            Me.Width += scr.Bounds.Width
            Me.Height += scr.Bounds.Height
        Next
        
        fg = Me.CreateGraphics()
    End Sub
    Sub screenCap(ByVal Rect As Rectangle)
        fg.Clear(Me.BackColor)
        Dim i As Bitmap = New Bitmap(Rect.Width, Rect.Height)
        Dim g As Graphics = Graphics.FromImage(i)
        g.CopyFromScreen(Rect.X, Rect.Y, 0, 0, Rect.Size)
        My.Computer.Audio.Play(My.Resources.shutter, AudioPlayMode.Background)
        Clipboard.SetImage(i)
        Threading.Thread.Sleep(100)

        Me.Close()
        If autoPaste Then
            SendKeys.Send("^v")
        End If
    End Sub
    Dim autoPaste As Boolean = False
    Private Sub Form1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyData = Keys.Escape Then
            e.Handled = True
            Me.Close()
        ElseIf e.KeyData = Keys.Enter Then
            e.Handled = True
            cr = Me.DisplayRectangle
            cr.X += Me.Left
            cr.Y += Me.Top
            screenCap(cr)
        ElseIf e.KeyData = Keys.Insert Then
            autoPaste = Not autoPaste
        End If

    End Sub

    Private Sub Form1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseDown
        cr.Location = e.Location
        sp = e.Location
        capping = True
    End Sub

    Dim fnt As Font = New Font("Segoe UI", 10, GraphicsUnit.Pixel)
    Dim capping As Boolean = False
    Private Sub Form1_MouseMove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseMove
        fg.Clear(Me.BackColor)
        Dim urap As String = ""
        If autoPaste Then
            urap = vbCrLf + "Autopaste"
        End If
        If capping Then

            Dim exIn As Boolean = False
            If sp.X > e.X Then
                cr.X = e.X
                cr.Width = sp.X - e.X
                exIn = True
            Else
                cr.Width = e.X - cr.X
            End If
            If sp.Y > e.Y Then
                cr.Y = e.Y
                cr.Height = sp.Y - e.Y
                exIn = True
            Else
                cr.Height = e.Y - cr.Y
            End If
            fg.DrawRectangle(Pens.Black, cr)
            Dim s As String = String.Format("{0}" + vbCrLf + "{1}{2}", cr.Width, cr.Height, urap)
            Dim ts As SizeF = fg.MeasureString(s, fnt)
            Dim tp As Point
            If exIn Then
                tp.X = e.X - ts.Width
                tp.Y = e.Y - ts.Height
            Else
                tp = e.Location
            End If
            fg.DrawString(s, fnt, Brushes.Black, tp)
        Else
            fg.DrawString(String.Format("{0}" + vbCrLf + "{1}{2}", e.Location.X, e.Location.Y, urap), fnt, Brushes.Black, e.Location)
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyBase.MouseUp
        Dim exIn As Boolean = False
        If sp.X > e.X Then
            cr.X = e.X
            cr.Width = sp.X - e.X
            exIn = True
        Else
            cr.Width = e.X - cr.X
        End If
        If sp.Y > e.Y Then
            cr.Y = e.Y
            cr.Height = sp.Y - e.Y
            exIn = True
        Else
            cr.Height = e.Y - cr.Y
        End If
        cr.X += Me.Left
        cr.Y += Me.Top
        screenCap(cr)
    End Sub
End Class
