Imports MySql.Data.MySqlClient
Imports System.Threading
Imports System.Net
Imports System.Net.Sockets
Imports System.Threading.Tasks
Module koneksi
    Public mysql, mysql2, sql As String
    Public con As New MySqlConnection
    Public con2 As New MySqlConnection
    Public cmd As New MySqlCommand
    Public mdr As MySqlDataReader
    Public dt As DataTable
    Public da As New MySqlDataAdapter
    Public ip As String
    Public ds As DataSet



    Public Sub konek2(ip As String)
        mysql2 = "server=" + ip.ToString + ";uid=root;pwd=WiA8E/q8aS/5NOgQE5ZobzpOCn3IhyKzE=ofv9imr2vN;database=pos;Pooling=False"
        con2 = New MySqlConnection(mysql2)
        Try
            con2.Open()

        Catch ex As Exception
            mysql2 = "server=" + ip.ToString + ";uid=root;pwd=v4dg4IDbVLYJnB7zOv3lKg8jw8WPvrwd4=NqpoGGrLCX;database=pos;Pooling=False"
            con2 = New MySqlConnection(mysql2)
            Try
                con2.Open()

            Catch ex2 As Exception
                Class1.tokonok("TIDAK KONEK : " + ip.ToString)
            End Try
        End Try
    End Sub
    Sub Main()
        AddHandler Application.ThreadException, AddressOf HandleThreadException
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf HandleUnhandledException


    End Sub
    Public Sub cekip()
        Dim hostName As String = Dns.GetHostName()
        Dim ipHostEntry As IPHostEntry = Dns.GetHostEntry(hostName)

        For Each ip As IPAddress In ipHostEntry.AddressList
            If ip.AddressFamily = AddressFamily.InterNetwork Then
                MessageBox.Show("IP Address: " & ip.ToString())
            End If
        Next
    End Sub

    Private Sub HandleThreadException(ByVal sender As Object, ByVal e As ThreadExceptionEventArgs)
        ' Menampilkan pesan error untuk unhandled exception
        Dim ex As Exception = e.Exception
        MessageBox.Show("Terjadi kesalahan yang tidak ditangani:" & Environment.NewLine & ex.Message,
                        "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub HandleUnhandledException(ByVal sender As Object, ByVal e As UnhandledExceptionEventArgs)
        Dim ex As Exception = DirectCast(e.ExceptionObject, Exception)
        MessageBox.Show("Terjadi kesalahan yang tidak ditangani:" & Environment.NewLine & ex.Message,
                        "Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub
End Module

