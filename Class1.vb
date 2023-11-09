Imports System
Imports System.IO
Imports System.Environment
Imports System.Net.Mail
Imports System.Text
Imports System.Net
Imports System.Data
Public Class Class1
    Public Shared Sub fileLOG(slog As String)
        Try
            Dim thn As String = Format(Now, "yyyy-MM-dd")
            Dim th As String = Format(Now, "yyyyMM")
            Dim thnn As String = Format(Now, "yyyy-MM-dd HH:mm:ss")
            Dim tulis As StreamWriter
            If Not File.Exists(Application.StartupPath & "\LOG_" + th + ".txt") Then
                tulis = File.CreateText(Application.StartupPath & "\LOG_" + th + ".txt")
                tulis.WriteLine("*************** LOG APP ***************")
                tulis.WriteLine(thnn & " | " & slog)
                tulis.Flush()
                tulis.Close()
            Else
                tulis = File.AppendText(Application.StartupPath & "\LOG_" + th + ".txt")
                tulis.WriteLine(thnn & " | " & slog)
                tulis.Flush()
                tulis.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Public Shared Sub tokonok(xlog As String)
        Try
            Dim thn As String = Format(Now, "yyyy-MM-dd")
            Dim th As String = Format(Now, "yyyyMM")
            Dim thnn As String = Format(Now, "yyyy-MM-dd HH:mm:ss")
            Dim jam As String = Format(Now, "HH:mm:ss")
            Dim tulis As StreamWriter
            If Not File.Exists(Application.StartupPath & "\TOKO_NOK_" + th + ".txt") Then
                tulis = File.CreateText(Application.StartupPath & "\TOKO_NOK_" + th + ".txt")
                tulis.WriteLine("*************** HISTORY TOKO GAGAL AMBIL DATA ***************")
                tulis.WriteLine(thnn & " | " & xlog)
                tulis.Flush()
                tulis.Close()
            Else
                tulis = File.AppendText(Application.StartupPath & "\TOKO_NOK_" + th + ".txt")
                'tulis.WriteLine("*************** HISTORY TOKO GAGAL AMBIL DATA ***************")
                tulis.WriteLine(thnn & " | " & xlog)
                tulis.Flush()
                tulis.Close()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
End Class
