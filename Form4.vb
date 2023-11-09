Imports MySql.Data.MySqlClient
Public Class Form4
    Dim conn As MySqlConnection
    Dim sql As String
    Dim cmd As MySqlCommand
    Dim path, ss As String
    Dim da As MySqlDataAdapter
    Dim ds As DataSet
    Dim gdg As String
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Form1.gudang.ToString = "ALL" Then
            gdg = ""
        Else
            gdg = "and cabang='" + Form1.gudang.ToString + "'"
        End If
        lbl_gudang.Text = Form1.gudang
        MsgBox(Form1.gudang)
        cek()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Label5.Text = Label3.Text
        Form1.paket()
        Me.Close()
    End Sub

    Sub cek()
        Label1.Text = "Sedang menampilkan data kr grid .."
        If Not conn Is Nothing Then conn.Close()
        Dim connstr As String = "server=192.168.190.100;user id=root; password=15032012; database=SiEDP; pooling=false"
        Try
            conn = New MySqlConnection(connstr)
            conn.Open()
            Dim sqla As String = "DELETE FROM tokoini"
            Dim cmda As MySqlCommand = New MySqlCommand(sqla, conn)
            cmda.ExecuteNonQuery()

            path = Application.StartupPath & "\TOKO.txt"
            ss = Replace(path, "\", "\\")
            sql = "Load data local infile '" + ss + "' into table tokoini ignore 1 lines"
            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()

            da = New MySqlDataAdapter("SELECT toko,nama,ip FROM m_ip WHERE station='01' and toko IN(SELECT KDTK FROM TOKOINI) and cabang='" + gdg.ToString + "' GROUP BY toko", conn)
            ds = New DataSet
            da.Fill(ds, "m_ip")
            DataGridView1.DataSource = ds.Tables("m_ip")
            Label3.Text = DataGridView1.Rows.Count - 1
            conn.Close()
            Label1.Text = ""
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
End Class