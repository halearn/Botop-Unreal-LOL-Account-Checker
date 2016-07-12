Imports Newtonsoft.Json.Linq
Imports System.ComponentModel
Imports MahApps.Metro.Controls.Dialogs
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
Public Class dataloader
    Dim key As String = My.Settings.apikey
    Dim ids As New List(Of Integer)
    Dim mydata As New Data.Data
    Dim file As String = Environment.CurrentDirectory & "/data.json"
    Dim Complete As TaskCompletionSource(Of String) = New TaskCompletionSource(Of String)()

    
  
   
    Private Async Sub OnDownloadComplete(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        Try
            If Not e.Cancelled AndAlso e.Error Is Nothing Then
                'ctr.SetProgress(1)
                Complete.TrySetResult(True)
                Await ShowMessageAsync("Program will update now.", "Program will update now :)")
                Try : Process.Start(System.AppDomain.CurrentDomain.BaseDirectory & "\up.exe") : Catch : End Try
                End
            Else

                MsgBox((e.Error.InnerException.Message()), "Error")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub

    Private Async Sub MetroWindow_Loaded(sender As Object, e As RoutedEventArgs)
        Dim updata As String = New Net.WebClient().DownloadString("https://raw.githubusercontent.com/halearn/Botop-Unreal-LOL-Account-Checker/master/version" & "?t=" & Now.Ticks)
        Dim assembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim ve As String = updata.Split("#")(0)
        Dim url As String = updata.Split("#")(1)
        Dim vv As System.Version = assembly.GetName().Version
        If Not ve = vv.Major & "." & vv.Minor & ".0.0" Then
            Dim funkynoise As New System.Net.WebClient
            AddHandler funkynoise.DownloadFileCompleted, AddressOf OnDownloadComplete
            funkynoise.DownloadFileAsync(New System.Uri(url), System.AppDomain.CurrentDomain.BaseDirectory & "\up.exe")
            Dim l As Boolean = Await Complete.Task
            If l = True Then
                Exit Sub
            Else

            End If
        End If


        Dim Data As String = New System.Net.WebClient().DownloadString("https://global.api.pvp.net/api/lol/static-data/eune/v1.2/versions?api_key=" & key)
        Dim Riot_Version As String = Data.Split(""",").GetValue(1)
        If My.Computer.FileSystem.FileExists(file) Then
            Dim job As JObject = Newtonsoft.Json.Linq.JObject.Parse(IO.File.ReadAllText(file))

            Dim v As JToken = job("Version")
            If v.ToString = Riot_Version Then
                Dim m As New MainWindow
                Try
                    m.ShowDialog()
                Catch ex As Exception
                    MsgBox(ex.Message)
                    MsgBox(ex.StackTrace)

                End Try
                System.Windows.Threading.Dispatcher.Run()
                Me.Close()

            Else

            End If
        Else

        End If

        lbl_status.Content += " (" & Riot_Version & ") ..."
        GetChampList()
        mydata.Version = Riot_Version
        Dim t As New System.Threading.Thread(AddressOf getids)
        t.Start()
    End Sub
    Sub getids()
        Parallel.ForEach(ids, Sub(i)
                                  Try

                                      Dim j As String = New Net.WebClient().DownloadString("https://global.api.pvp.net/api/lol/static-data/eune/v1.2/champion/" & i & "?champData=skins&api_key=" & key)
                                      Dim champ As New Data.Champion
                                      Dim json As New Web.Script.Serialization.JavaScriptSerializer
                                      champ = json.Deserialize(Of Data.Champion)(j)
                                      mydata.Champions.Add(champ)



                                  Catch ex As Exception
                                      MsgBox(ex.Message)
                                      MsgBox(ex.StackTrace)

                                  End Try
                                  Dispatcher.Invoke(Sub()
                                                        Try : prog.Value += 1 : Catch ex As Exception
                                                            : End Try
                                                        lbl_prog.Content = prog.Value & " / " & ids.Count & "  (" & Math.Round(prog.Value / ids.Count * 100) & " %)" & " Completed."
                                                    End Sub)
                              End Sub)

        IO.File.WriteAllText(Environment.CurrentDirectory & "\data.json", New System.Web.Script.Serialization.JavaScriptSerializer().Serialize(mydata))
        Try
            Dispatcher.Invoke(Sub()

                                  Dim m As New MainWindow
                                  Try
                                      Me.Close()
                                      m.ShowDialog()
                                  Catch ex As Exception
                                      MsgBox(ex.Message)
                                      MsgBox(ex.StackTrace)

                                  End Try
                                  System.Windows.Threading.Dispatcher.Run()

                              End Sub)

        Catch eee As Exception
            MsgBox(eee.Message)
        End Try
    End Sub
    Sub GetChampList()
        Dim json As String = New Net.WebClient().DownloadString("https://global.api.pvp.net/api/lol/static-data/eune/v1.2/champion?champData=tags&api_key=" & key)
        Dim job As JObject = Newtonsoft.Json.Linq.JObject.Parse(json)("data")
        Dim ii As New List(Of Integer)
        For Each JToken In job
            ii.Add(JToken.Value("id"))

        Next
        ids = ii
        Dispatcher.Invoke(Sub()
                              prog.Maximum = ids.Count
                          End Sub)
    End Sub
End Class
Namespace Data
    Public Class Champion
        Public Property id As Integer
        Public Property title As String
        Public Property name As String
        Public Property skins As Skin()
        Public Property key As String
    End Class
    Public Class Data
        Property Champions As New List(Of Champion)
        Property Version As String
    End Class
    Public Class Skin
        Public Property id As Integer
        Public Property num As Integer
        Public Property name As String
    End Class
End Namespace