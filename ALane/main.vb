Imports System.Net
Imports System.Net.Sockets

Public Class main

    Dim req = 0
    Dim rec = 0


    Public Function GetStringBetween(ByVal InputText As String,
    ByVal starttext As String,
    ByVal endtext As String)

        Dim lnTextStart As Long
        Dim lnTextEnd As Long

        lnTextStart = InStr(StartPosition, InputText, starttext, vbTextCompare) + Len(starttext)
        lnTextEnd = InStr(lnTextStart, InputText, endtext, vbTextCompare)
        If lnTextStart >= (StartPosition + Len(starttext)) And lnTextEnd > lnTextStart Then
            GetStringBetween = Mid$(InputText, lnTextStart, lnTextEnd - lnTextStart)
        Else
            GetStringBetween = "ERROR"
        End If
    End Function
    Sub wait(ByVal interval As Integer)
        Dim stopW As New Stopwatch
        stopW.Start()
        Do While stopW.ElapsedMilliseconds < interval
            Application.DoEvents()
        Loop
        stopW.Stop()
    End Sub

    Dim server As New AServer
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub


    Dim start = False
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If start Then
            start = False
            Timer1.Stop()
            Button1.Text = "Start"

        Else

            Button1.Text = "Stop"

            start = True
            req = 0
            rec = 0

            Label4.Text = "Player: 0/" & TextBox3.Text

            server.init(TextBox1.Text, Integer.Parse(TextBox2.Text), TextBox3.Text)

            Label9.ForeColor = Color.DarkGreen
            Label9.Text = "Bot Running"


            wait(2000)

            Timer1.Start()
            Timer1.Interval = TextBox5.Text


        End If

        wait(2000)


        While start


            Application.DoEvents()
            Dim resp = server.receive()

            If resp = "false" Then
            Else


                Label4.Text = "Player: " + resp + "/" + TextBox3.Text
                rec += 1


                If Integer.Parse(resp) < Integer.Parse(TextBox3.Text) Then

                    start = False

                    doclick("LClick")
                    doclick("LClick")

                    Label9.ForeColor = Color.Blue
                    Label9.Text = "Free Slot!"

                    Timer1.Stop()
                    Button1.Text = "Start"

                End If

            End If

        End While
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        req += 1
        server.send()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click

        Dim splits As String() = TextBox4.Text.Split(New Char() {"/"c})


        Dim code = splits(splits.Length - 1)


        Dim webClient As New System.Net.WebClient
        Dim result As String = webClient.DownloadString("https://api.battlemetrics.com/servers/" + code)

        TextBox1.Text = GetStringBetween(result, ",""ip"":""", """,""port")
        TextBox2.Text = GetStringBetween(result, """,""portQuery"":", ",""country"":")
        TextBox3.Text = GetStringBetween(result, "maxPlayers"":", ",""rank")
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick

        Label10.Text = "Requests: " & req
        Label7.Text = "Answers: " & rec

    End Sub

    Private Declare Sub mouse_event Lib "user32.dll" (ByVal dwFlags As Integer, ByVal dx As Integer, ByVal dy As Integer, ByVal cButtons As Integer, ByVal dwExtraInfo As IntPtr)
    Sub doclick(ByVal LClick_RClick_DClick As String)
        Const MOUSEEVENTF_LEFTDOWN As Integer = 2
        Const MOUSEEVENTF_LEFTUP As Integer = 4
        Const MOUSEEVENTF_RIGHTDOWN As Integer = 6
        Const MOUSEEVENTF_RIGHTUP As Integer = 8
        If LClick_RClick_DClick = "RClick" Then
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, IntPtr.Zero)
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, IntPtr.Zero)
        ElseIf LClick_RClick_DClick = "LClick" Then
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero)
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero)
        ElseIf LClick_RClick_DClick = "DClick" Then
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero)
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero)
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, IntPtr.Zero)
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, IntPtr.Zero)
        End If
    End Sub

End Class
