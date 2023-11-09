Public Class Form3
    Public no, nil2 As Double
    Dim iptoko As String
    Public Delegate Sub SetLabelText_Delegate(Label As Label, text As String)
    Public Property IptokoValue As String
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        no = no - 1
        Try
            If My.Computer.Network.Ping(iptoko, 1000) Then
                TextBox1.AppendText("Ping Server " + iptoko + " Pinged Successfully" & Environment.NewLine)
                nil2 = nil2 + 1
            Else
                TextBox1.AppendText("Ping request timed out" & Environment.NewLine)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            TextBox1.AppendText("Network is Offline" & Environment.NewLine)
        End Try

        If no = 0 Then
            Timer1.Stop()
            Me.Close()
        End If
    End Sub

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        TextBox1.Text = ""
        no = 6
        nil2 = 0
        iptoko = IptokoValue
        Application.DoEvents()
        Timer1.Start()
    End Sub

End Class