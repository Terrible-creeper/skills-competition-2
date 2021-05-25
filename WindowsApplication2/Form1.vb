Public Class Form1
#Region "Dim"
    Dim link_time_now, link_time_last As DateTime
    Dim data(10), data2(10) As Byte
    Dim VR As Integer
    Dim passwd16, passwd As Integer
    Dim LED_value As Integer
#End Region

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBox1.Items.Add(sp)
        Next
        ComboBox1.SelectedIndex = 0
        DataTimer.Enabled = True
    End Sub

    Private Sub SerialPort1_DataReceived(ByVal sender As System.Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim buff(SerialPort1.BytesToRead - 1) As Byte
        SerialPort1.Read(buff, 0, buff.Length)
        If buff.Length >= 9 Then
            If (buff(0) = &H3A) Then

            End If
        End If
    End Sub

    Private Sub DataTimer_Tick(sender As Object, e As EventArgs) Handles DataTimer.Tick

        Try
            data(0) = &H3A
            data(1) = &H0
            data(2) = LED_value
            data(3) = &H0
            data(4) = &H0
            data(5) = &H0
            data(6) = &H0
            data(7) = &H0
            data(8) = &H0
            data(9) = &H0
            SerialPort1.Write(data, 0, 10)
        Catch ex As Exception
            Button2.PerformClick()
        End Try
    End Sub


    Private Sub relink()
        Try
            If SerialPort1.IsOpen = True Then
                SerialPort1.Close()
            End If
            SerialPort1.Open()
        Catch ex As Exception

        End Try
    End Sub


    Function IntToBCD(value As Integer)
        Dim num As String = value

        If num.Length = 1 Then
            num = "0" & num
        End If

        Dim data1 As String = ""

        Dim bcd(9) As String
        bcd(0) = "0000"
        bcd(1) = "1000"
        bcd(2) = "0100"
        bcd(3) = "1100"
        bcd(4) = "0010"
        bcd(5) = "1010"
        bcd(6) = "0110"
        bcd(7) = "1110"
        bcd(8) = "0001"
        bcd(9) = "1001"

        For i As Integer = 1 To num.Length
            data1 = bcd(num.Substring(i - 1, 1)) & data1
        Next

        Dim require As Integer = Convert.ToUInt64(data1, 2)
        Return require
    End Function

    Private Sub ComputerTimer_Tick(sender As Object, e As EventArgs) Handles ComputerTimer.Tick
        TextBox1.Text = "Current Time : " & DateTime.Now.ToString("hh:mm:ss")
    End Sub

    Private Sub ComboBox1_Click(sender As Object, e As EventArgs) Handles ComboBox1.Click
        ComboBox1.Items.Clear()
        For Each sp As String In My.Computer.Ports.SerialPortNames
            ComboBox1.Items.Add(sp)
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        SerialPort1.PortName = ComboBox1.SelectedItem

        Try
            SerialPort1.Open()
            Button1.Enabled = False
            CheckBox1.Enabled = True
            CheckBox2.Enabled = True
            CheckBox3.Enabled = True
            CheckBox4.Enabled = True
            CheckBox5.Enabled = True
            CheckBox6.Enabled = True
            CheckBox7.Enabled = True
            CheckBox8.Enabled = True
            TextBox2.Enabled = True
            Button3.Enabled = True
            Button4.Enabled = True
            Button5.Enabled = True
            Label2.Text = "Online"
            Label2.BackColor = Color.Green
        Catch ex As Exception
            SerialPort1.Close()
            Button1.Enabled = True
            CheckBox1.Enabled = False
            CheckBox2.Enabled = False
            CheckBox3.Enabled = False
            CheckBox4.Enabled = False
            CheckBox5.Enabled = False
            CheckBox6.Enabled = False
            CheckBox7.Enabled = False
            CheckBox8.Enabled = False
            TextBox2.Enabled = False
            Button3.Enabled = False
            Button4.Enabled = False
            Button5.Enabled = False
            Label2.Text = "Offline"
            Label2.BackColor = Color.Red
        End Try
        
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        SerialPort1.Close()
        Button1.Enabled = True
        CheckBox1.Enabled = False
        CheckBox2.Enabled = False
        CheckBox3.Enabled = False
        CheckBox4.Enabled = False
        CheckBox5.Enabled = False
        CheckBox6.Enabled = False
        CheckBox7.Enabled = False
        CheckBox8.Enabled = False
        TextBox2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
        Button5.Enabled = False
        Label2.Text = "Offline"
        Label2.BackColor = Color.Red
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(48) Or e.KeyChar = Chr(49) Or e.KeyChar = Chr(50) Or e.KeyChar = Chr(51) Or e.KeyChar = Chr(52) Or e.KeyChar = Chr(53) Or e.KeyChar = Chr(54) Or e.KeyChar = Chr(55) Or e.KeyChar = Chr(56) Or e.KeyChar = Chr(57) Or e.KeyChar = Chr(13) Or e.KeyChar = Chr(8) Then
            e.Handled = False
        Else
            e.Handled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If TextBox2.TextLength < 4 Then
            TextBox2.Text = ""
        ElseIf TextBox2.TextLength = 4 Then
            Dim eeprom As String = TextBox2.Text
            Try
                data2(0) = &H3A
                data2(1) = &H3
                data2(2) = eeprom.Substring(0, 2)
                data2(3) = eeprom.Substring(2, 2)
                data2(4) = IntToBCD(eeprom.Substring(2, 2))
                data2(5) = &H0
                data2(6) = &H0
                data2(7) = &H0
                data2(8) = &H0
                data2(9) = &H0
                SerialPort1.Write(data2, 0, 10)
            Catch ex As Exception
                MsgBox(ex.ToString)
            End Try
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H1
        ElseIf CheckBox1.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H1
        End If
    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        If CheckBox2.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H2
        ElseIf CheckBox2.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H2
        End If
    End Sub

    Private Sub CheckBox3_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox3.CheckedChanged
        If CheckBox3.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H4
        ElseIf CheckBox3.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H4
        End If
    End Sub

    Private Sub CheckBox4_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox4.CheckedChanged
        If CheckBox4.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H8
        ElseIf CheckBox4.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H8
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H10
        ElseIf CheckBox5.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H10
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H20
        ElseIf CheckBox6.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H20
        End If
    End Sub

    Private Sub CheckBox7_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox7.CheckedChanged
        If CheckBox7.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H40
        ElseIf CheckBox7.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H40
        End If
    End Sub

    Private Sub CheckBox8_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox8.CheckedChanged
        If CheckBox8.CheckState = CheckState.Checked Then
            LED_value = LED_value + &H80
        ElseIf CheckBox8.CheckState = CheckState.Unchecked Then
            LED_value = LED_value - &H80
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        CheckBox1.Checked = True
        CheckBox2.Checked = True
        CheckBox3.Checked = True
        CheckBox4.Checked = True
        CheckBox5.Checked = True
        CheckBox6.Checked = True
        CheckBox7.Checked = True
        CheckBox8.Checked = True

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        CheckBox1.Checked = False
        CheckBox2.Checked = False
        CheckBox3.Checked = False
        CheckBox4.Checked = False
        CheckBox5.Checked = False
        CheckBox6.Checked = False
        CheckBox7.Checked = False
        CheckBox8.Checked = False
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
    End Sub
End Class