Imports System.Net
Imports System.Net.Sockets

Public Class AServer

    Private client As Socket = New Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
    Private endPoint
    Private Buffer(1400) As Byte
    Private ASlots

    Private Function arraytostring(ByRef array() As Byte) As String
        Dim Encoding As System.Text.Encoding = System.Text.Encoding.Default
        Return Encoding.GetString(array)
    End Function

    Sub init(ip, port, slots)
        ASlots = slots
        endPoint = New IPEndPoint(IPAddress.Parse(ip), port)
    End Sub

    Sub send()
        Dim A2S_INFO As String = "ÿÿÿÿTSource Engine Query" & Chr(0)
        client.SendTo(System.Text.Encoding.[Default].GetBytes(A2S_INFO), endPoint)
    End Sub

    Private Function getPlayer(Text)
        Try

            Dim split As String() = Text.Split(New Char() {"\x00"})
            Dim i = 0
            Dim c = 0
            Dim s As String
            For Each s In split
                If (s.Replace("x00", "").Contains("OWNINGID")) Then
                    i = c
                End If
                c += 1
            Next

            Dim player = split(i).Replace("x00", "").ToString
            Dim split2 As String() = player.ToString.Split(New Char() {","})
            player = split2(3).Replace("x00", "")
            Dim split3 As String() = player.ToString.Split(New Char() {":"})

            If split3(1) > 500 Then
                player = split2(4).Replace("x00", "")
                split3 = player.ToString.Split(New Char() {":"})
            End If

            If ASlots = 0 Then
                player = split3(1)
            Else
                player = ASlots - split3(1)
            End If

            Return player
        Catch ex As Exception
            Return "error"
        End Try
    End Function

    Function receive()

        Application.DoEvents()
        If client.Available Then

            client.Receive(Buffer)
            Dim resp

            resp = arraytostring(Buffer).Remove(0, 6)
            resp = (resp.Replace(Chr(&H0), "\x00")).Replace("\x00\x00", "")

            Return getPlayer(resp)

        End If

        Return "false"
    End Function

End Class
