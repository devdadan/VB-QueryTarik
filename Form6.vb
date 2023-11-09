Imports MySql.Data.MySqlClient
Imports System.Net
Imports System.Net.Sockets
Public Class Form6
    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cekip()
    End Sub
    Public Sub cekip()
        Dim hostName As String = Dns.GetHostName()
        Dim ipHostEntry As IPHostEntry = Dns.GetHostEntry(hostName)

        For Each ip As IPAddress In ipHostEntry.AddressList
            If ip.AddressFamily = AddressFamily.InterNetwork Then
                txt_ip.Text = ip.ToString
            End If
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txt_nik.Text = "" Or txt_nama.Text = "" Or txt_pass.Text = "" Then
            MsgBox("Mohon lengkapi form")
        Else
            proses()
        End If
    End Sub
    Sub proses()
        Try

            sql = "select count(*) from userlogin where nik='" + txt_nik.Text + "'"
            cmd = New MySqlCommand(sql, con)
            If cmd.ExecuteScalar() <> "0" Then
                MsgBox("Nik sudah terdaftar", MsgBoxStyle.Critical)
            Else

            End If
        Catch ex As Exception

        End Try
    End Sub
End Class