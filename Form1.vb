Imports MySql.Data.MySqlClient
Imports System.IO
Imports Excel = Microsoft.Office.Interop.Excel
Imports Newtonsoft.Json
Imports System.Text
Imports System.ComponentModel

Public Class Form1
    Dim conn As MySqlConnection
    Dim conndb As MySqlConnection
    Dim da As MySqlDataAdapter
    Dim rd As MySqlDataReader
    Dim sql As String
    Public dt2 As New DataTable
    Public kodetoko, iptoko, namatoko As String
    Dim dt As New DataTable
    Dim txt As String
    Dim dtrow As Integer
    Dim dtcol As Integer
    Dim totrow As Integer
    Dim percentt As Decimal
    Public tipetoko As String
    Public gudang As String
    Public kodegdg As String
    Private stopLoop As Boolean = False
    Public no, nil2 As Double
    Dim iptoko2 As String

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False
        LinkLabel1.Visible = False
        LinkLabel2.Visible = False
        'Form2.Show()
        Me.Text = "QueryTarik v." + Application.ProductVersion
        ComboBox1.SelectedItem = "ALL TOKO"
        cb_gdg.SelectedItem = "ALL"
        DataGridView1.Enabled = False
        Button1.Enabled = False
        isikodecabang()
        konek()
        paket()

    End Sub
    Private Sub konek()
        Dim str As String = "Server=192.168.190.100;user id=root;password=15032012;database=SiEDP; pooling=false;Allow User Variables=true;Convert Zero Datetime=True"
        conn = New MySqlConnection(str)
        Try
            conn.Open()
        Catch ex As Exception
            conn.Close()
            MsgBox(ex.Message)
        End Try
    End Sub



    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'BackgroundWorker1.RunWorkerAsync()
        If FastColoredTextBox1.Text.Contains("update") Or FastColoredTextBox1.Text.Contains("insert") Or FastColoredTextBox1.Text.Contains("delete") Then
            MsgBox("MAAF QUERY TIDAK DIIZINKAN, HANYA UNTUK QUERY SELECT", MsgBoxStyle.Critical)
            Me.Close()
        Else
            dt.Clear()
            proses()
            Class1.fileLOG("Tarik data selesai")
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        csv()
    End Sub
    Sub excel()
        Button1.Enabled = False
        Try
            Dim jamtgl As String = Format(Now, "yyMMdd")
            Dim detik As String = Format(Now, "hhmm")
            If File.Exists(Application.StartupPath + "\hasil\QUERYTARIK_" + jamtgl.ToString + ".xlsx") Then
                File.Delete(Application.StartupPath + "\hasil\QUERYTARIK_" + jamtgl.ToString + ".xlsx")
            End If
            Dim xlap As Excel.Application
            Dim xlworkbook As Excel.Workbook
            Dim xlworksheet As Excel.Worksheet
            Dim misvalue As Object = Reflection.Missing.Value
            xlap = New Excel.Application
            xlworkbook = xlap.Workbooks.Add(misvalue)
            xlworksheet = xlworkbook.Sheets("Sheet1")
            Label1.Text = "Proses export ke excel"
            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = DataGridView1.Rows.Count - 1
            For i = 0 To DataGridView1.RowCount - 2
                For j = 0 To DataGridView1.ColumnCount - 1
                    For k As Integer = 1 To DataGridView1.Columns.Count
                        xlworksheet.Cells(1, k) = DataGridView1.Columns(k - 1).HeaderText
                        xlworksheet.Cells(i + 2, j + 1) = DataGridView1(j, i).Value.ToString()
                    Next
                Next
                ProgressBar1.Value = i
            Next

            xlworksheet.SaveAs(Application.StartupPath & "\hasil\QUERYTARIK_" + jamtgl.ToString + "_" + detik.ToString + ".xlsx")
            xlworkbook.Close()
            xlap.Quit()
            Label1.Text = "selesai"
            ProgressBar1.Value = 0
            MsgBox("Eksport berhasil, file ada di : " + Application.StartupPath & "\hasil\QUERYTARIK_" + jamtgl.ToString + "_" + detik.ToString + ".xlsx", MsgBoxStyle.Information)
            Label1.Text = "Loading. ."
        Catch ex As Exception
            Class1.fileLOG("proses eksport :" + ex.Message)
            MsgBox("eksport gagal, cek file log!", MsgBoxStyle.Critical)

        End Try
        Button1.Enabled = True
    End Sub

    Sub treeview()

    End Sub

    Sub csv()
        Try
            Dim headers = (From header As DataGridViewColumn In DataGridView1.Columns.Cast(Of DataGridViewColumn)()
                           Select header.HeaderText).ToArray
            Dim rows = From row As DataGridViewRow In DataGridView1.Rows.Cast(Of DataGridViewRow)()
                       Where Not row.IsNewRow
                       Select Array.ConvertAll(row.Cells.Cast(Of DataGridViewCell).ToArray, Function(c) If(c.Value IsNot Nothing, c.Value.ToString, ""))
            Using sw As New IO.StreamWriter("Hasil.txt")
                sw.WriteLine(String.Join("|", headers))
                For Each r In rows
                    sw.WriteLine(String.Join("|", r))
                Next
            End Using
            Process.Start("Hasil.txt")
        Catch ex As Exception
            Class1.fileLOG("Download file CSV | " + ex.Message)
            MsgBox("Download Gagal karena : " + ex.Message)
        End Try

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        konek()
        paket()
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start(Application.StartupPath + "\toko.txt")
    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked

        If cb_gdg.Text = "ALL" Then
            gudang = ""
        Else
            gudang = cb_gdg.Text
        End If
        Form4.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        Dim th As String = Format(Now, "yyyyMM")
        If File.Exists(Application.StartupPath + "\LOG_" + th + ".txt") Then
            Process.Start(Application.StartupPath + "\LOG_" + th + ".txt")
        Else
            MsgBox("Data log belum ada", MsgBoxStyle.Critical)
        End If

    End Sub

    Public Sub paket()
        dt2.Clear()

        Try
            If ComboBox1.Text = "ALL TOKO" Then
                tipetoko = ""
                LinkLabel1.Visible = False
                'LinkLabel2.Visible = False
            ElseIf ComboBox1.Text = "REGULER" Then
                tipetoko = "and toko like 'T%'"
                LinkLabel1.Visible = False
                'LinkLabel2.Visible = False
            ElseIf ComboBox1.Text = "FRANCHISE" Then
                tipetoko = "and toko like 'F%'"
                LinkLabel1.Visible = False
                'LinkLabel2.Visible = False
            ElseIf ComboBox1.Text = "CERIAMART" Then
                tipetoko = "and toko like 'R%'"
                LinkLabel1.Visible = False
                ' LinkLabel2.Visible = False
            Else
                tipetoko = "and toko in(select kdtk from tokoini)"
                LinkLabel1.Visible = True
                'LinkLabel2.Visible = True
                Label5.Text = 0
            End If
            Try
                Dim Path, ss As String
                konek()
                Dim sqla As String = "DELETE FROM tokoini"
                Dim cmda As MySqlCommand = New MySqlCommand(sqla, conn)
                cmda.ExecuteNonQuery()

                Path = Application.StartupPath & "\TOKO.txt"
                ss = Replace(Path, "\", "\\")
                sql = "Load data local infile '" + ss + "' into table tokoini ignore 1 lines"
                cmd = New MySqlCommand(sql, conn)
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            sql = "SELECT * FROM m_ip where station='01' " + tipetoko.ToString + " " + kodegdg + " and cabang in(select kode_cab from master_cabang) order by toko"
            da = New MySqlDataAdapter(sql, conn)
            da.Fill(dt2)
            DataGridView2.DataSource = dt2
            Label5.Text = DataGridView2.Rows.Count - 1
        Catch ex As Exception
            Class1.fileLOG("proses paket toko : " + ex.StackTrace + sql)
            MsgBox("Gagal load toko, periksa log!", MsgBoxStyle.Critical)
        End Try
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim th As String = Format(Now, "yyyyMM")
        If File.Exists(Application.StartupPath + "\TOKO_NOK_" + th + ".txt") Then
            Process.Start(Application.StartupPath + "\TOKO_NOK_" + th + ".txt")
        Else
            MsgBox("Data log belum ada", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub cb_gdg_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_gdg.SelectedIndexChanged
        If cb_gdg.Text = "ALL" Then
            kodegdg = ""
        Else
            kodegdg = "and cabang='" + cb_gdg.Text + "'"
        End If

        konek()
        paket()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        paket()
    End Sub


    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    End Sub
    Private Sub isikodecabang()
        Try


            konek()
            'cb_gdg.Items.Clear()
            Dim dts As DataTable = New DataTable
            sql = "SELECT KODE_CAB,NAMA_CAB FROM master_cabang"
            cmd = New MySqlCommand(sql, conn)
            Dim das = New MySqlDataAdapter(sql, conn)
            das.Fill(dts)
            Dim newRow As DataRow = dts.NewRow()
            newRow("KODE_CAB") = "ALL"
            newRow("NAMA_CAB") = "ALL"
            dts.Rows.Add(newRow)
            If dts IsNot Nothing AndAlso dts.Rows.Count > 0 Then
                cb_gdg.DataSource = dts
                cb_gdg.DisplayMember = "KODE_CAB"
                cb_gdg.ValueMember = "KODE_CAB"

            Else
                MsgBox("No data found in the master_cabang table.")
            End If
            con.Close()


        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        no = no - 1
        Try
            If My.Computer.Network.Ping(iptoko, 1000) Then
                nil2 = nil2 + 1
            Else
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        If no = 0 Then
            Timer1.Stop()
            Me.Close()
        End If
    End Sub

    Private Sub proses()
        DataGridView1.DataSource = Nothing
        DataGridView1.Enabled = True
        Button1.Enabled = True
        Button3.Enabled = False
        Try

            Dim a As Integer = 0
            Dim rows As Integer = DataGridView2.Rows.Count - 1
            Dim cols As Integer = DataGridView2.Columns.Count
            'Form3.ShowDialog()
            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = rows
            ProgressBar1.Value = 0
            Dim dtx As DataTable = New DataTable
            stopLoop = False
            For b As Integer = a To rows
                Label3.Text = b
                ProgressBar1.Value = b
                If Label3.Text = rows Then
                    MsgBox("selesai")
                    ProgressBar1.Value = 0
                    Label1.Text = "Loading . ."
                    Label3.Text = "0"
                Else
                    kodetoko = DataGridView2.Rows(b).Cells(1).Value.ToString
                    namatoko = DataGridView2.Rows(b).Cells(2).Value.ToString
                    iptoko = DataGridView2.Rows(b).Cells(4).Value.ToString
                    Label1.Text = "Processed : " + kodetoko + "-" + namatoko
                    Dim form3 As New Form3()
                    form3.IptokoValue = iptoko
                    form3.ShowDialog()

                    If form3.nil2 > 2 Then

                        Try
                            konek2(iptoko.ToString)
                            sql = FastColoredTextBox1.Text
                            da = New MySqlDataAdapter(sql, con2)
                            da.Fill(dtx)
                            dtrow = dtx.Rows.Count - 1
                            dtcol = dtx.Columns.Count
                            DataGridView1.DataSource = dtx
                            totrow = DataGridView1.Rows.Count - 1
                            Label2.Text = "Rows :" + totrow.ToString

                        Catch ex As Exception
                            Class1.tokonok(kodetoko + " : " + ex.Message)
                        End Try
                    Else
                        Class1.tokonok(kodetoko + " | Ping request timed out")
                    End If

                End If

            Next

        Catch ex1 As Exception
            Class1.fileLOG("Eror pada button proses1 |" + ex1.Message + ex1.StackTrace)
            MsgBox(ex1.Message)

        End Try
        Button3.Enabled = True
    End Sub

    Sub save()
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Using writer As JsonWriter = New JsonTextWriter(sw)

            writer.Formatting = Formatting.Indented

            writer.WriteStartObject()
            writer.WritePropertyName("CPU")
            writer.WriteValue("Intel")
            writer.WritePropertyName("PSU")
            writer.WriteValue("500W")
            writer.WritePropertyName("Drives")
            writer.WriteStartArray()
            writer.WriteValue("DVD read/writer")
            writer.WriteComment("(broken)")
            writer.WriteValue("500 gigabyte hard drive")
            writer.WriteValue("200 gigabyte hard drive")
            writer.WriteEnd()
            writer.WriteEndObject()
        End Using
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted

    End Sub
End Class
