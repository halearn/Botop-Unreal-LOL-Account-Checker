Imports System.Windows.Forms
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
Public Class viewer
    Dim path As String
    Dim Accounts As New List(Of Viewed.Account)
    Sub New(projectfile As String)
        path = projectfile
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btn_openproj_Click(sender As Object, e As RoutedEventArgs) Handles btn_openproj.Click
        Accounts.Clear()
        Try
            Dim o As New OpenFileDialog
            With o
                .FileName = "project.botop"
                .Filter = "Botop Project|project.botop"
                .Title = "Choose the project that you want to load"
                If .ShowDialog = Forms.DialogResult.OK Then

                    Dim path As String = .FileName
                    Dim a = path.Split("\")
                    Dim b As Integer = a(a.Length - 1).Length
                    Dim c As String = path.Substring(0, path.Length - b)

                    Dim projectdata As String = IO.File.ReadAllText(path)
                    Dim accs = projectdata.Split(New String() {Environment.NewLine},
                                              StringSplitOptions.None)
                    Parallel.ForEach(accs, Sub(account As String)
                                               Try
                                                   Dim json As String = IO.File.ReadAllText(c & account)
                                                   Dim acc As New CheckingData.Account
                                                   acc = New Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of CheckingData.Account)(json)
                                                   Dim accv As New Viewed.Account
                                                   accv.Username = acc.Username
                                                   accv.Password = acc.Password
                                                   accv.Summoner_ID = acc.SummonerID
                                                   accv.Summoner_Name = acc.SummonerName
                                                   accv.Level = acc.Level
                                                   accv.LastLogin = acc.LastLogin.Date
                                                   accv.Rank = acc.Rank
                                                   accv.RP = acc.RP
                                                   accv.IP = acc.IP
                                                   accv.Email_Status = acc.Email
                                                   accv.Skins = acc.SkinCount
                                                   accv.Champions = acc.ChampionCount
                                                   accv.ChampionData = acc.Champions
                                                   accv.SkinData = acc.Skins
                                                   Accounts.Add(accv)
                                               Catch ex As Exception
                                                   '  MsgBox(ex.Message)
                                                   ' MsgBox(ex.StackTrace)
                                               End Try
                                           End Sub)
                    viewdatagrid1.ItemsSource = ""
                    viewdatagrid1.ItemsSource = Accounts
                    viewdatagrid1.Items.Refresh()
                End If
            End With
        Catch exc As Exception
            ' MsgBox(exc.Message)
            '  MsgBox(exc.StackTrace)

        End Try
    End Sub
    Dim dat As New Data.Data

    Private Sub MetroWindow_Loaded(sender As Object, e As RoutedEventArgs)
  
        Try
            viewdatagrid1.ItemsSource = ""
            viewdatagrid1.ItemsSource = Accounts
            viewdatagrid1.Items.Refresh()
            viewdatagrid2.ItemsSource = finalfilter
            filter_champs.Items.Add("NONE")
            filter_skins.Items.Add("NONE")
            filter_champs.SelectedIndex = 0
            filter_skins.SelectedIndex = 0
            dat = New Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of Data.Data)(IO.File.ReadAllText(Environment.CurrentDirectory & "\data.json"))
            For Each ch In dat.Champions.OrderBy(Function(o) o.name)
                filter_champs.Items.Add(ch.name)
                For Each sk In ch.skins
                    '   filter_skins.Items.Add(sk.name)
                Next
            Next

            If path = "" Then

            Else
                Dim a = path.Split("\")
                Dim b As Integer = a(a.Length - 1).Length
                Dim c As String = path.Substring(0, path.Length - b)

                Dim projectdata As String = IO.File.ReadAllText(path)
                Dim accs = projectdata.Split(New String() {Environment.NewLine},
                                          StringSplitOptions.None)
                Parallel.ForEach(accs, Sub(account As String)
                                           Try
                                               Dim json As String = IO.File.ReadAllText(c & account)
                                               Dim acc As New CheckingData.Account
                                               acc = New Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of CheckingData.Account)(json)

                                               Dim accv As New Viewed.Account
                                               accv.Username = acc.Username
                                               accv.Password = acc.Password
                                               accv.Summoner_ID = acc.SummonerID
                                               accv.Summoner_Name = acc.SummonerName
                                               accv.Level = acc.Level
                                               accv.LastLogin = acc.LastLogin.Date
                                               accv.Rank = acc.Rank
                                               accv.RP = acc.RP
                                               accv.IP = acc.IP
                                               accv.Email_Status = acc.Email
                                               accv.Skins = acc.SkinCount
                                               accv.Champions = acc.ChampionCount
                                               accv.ChampionData = acc.Champions
                                               accv.SkinData = acc.Skins
                                               Accounts.Add(accv)
                                           Catch ex As Exception
                                               '   MsgBox(ex.Message)
                                               '      MsgBox(ex.StackTrace)
                                           End Try
                                       End Sub)
                viewdatagrid1.ItemsSource = ""
                viewdatagrid1.ItemsSource = Accounts
                viewdatagrid1.Items.Refresh()
                'MsgBox("hi")

            End If
        Catch
        End Try
    End Sub
    Private Sub SearchDataGrid_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles viewdatagrid1.AutoGeneratingColumn
        If DirectCast(e.Column.Header, String) = "ChampionData" Then
            e.Cancel = True
        End If
        If DirectCast(e.Column.Header, String) = "SkinData" Then
            e.Cancel = True
        End If
        If DirectCast(e.Column.Header, String) = "Summoner_Name" Then
            e.Column.Header = "Summoner Name"
        End If
        If DirectCast(e.Column.Header, String) = "Summoner_ID" Then
            e.Column.Header = "Summoner ID"
        End If
        If DirectCast(e.Column.Header, String) = "Email_Status" Then
            e.Column.Header = "Email Status"
        End If
    End Sub
    Private Sub SearchDataGrid2_AutoGeneratingColumn(sender As Object, e As DataGridAutoGeneratingColumnEventArgs) Handles viewdatagrid2.AutoGeneratingColumn
        If DirectCast(e.Column.Header, String) = "ChampionData" Then
            e.Cancel = True
        End If
        If DirectCast(e.Column.Header, String) = "SkinData" Then
            e.Cancel = True
        End If
        If DirectCast(e.Column.Header, String) = "Summoner_Name" Then
            e.Column.Header = "Summoner Name"
        End If
        If DirectCast(e.Column.Header, String) = "Summoner_ID" Then
            e.Column.Header = "Summoner ID"
        End If
        If DirectCast(e.Column.Header, String) = "Email_Status" Then
            e.Column.Header = "Email Status"
        End If
    End Sub
    Async Sub Menu_copy()
        If viewdatagrid1.SelectedItems.Count > 0 Then
            Dim cp As String = ""
            For Each item In viewdatagrid1.SelectedItems
                Dim acc As New Viewed.Account
                acc = item
                cp += acc.Username & ":" & acc.Password & vbNewLine
            Next
            My.Computer.Clipboard.SetText(cp)
            Await ShowMessageAsync(viewdatagrid1.SelectedItems.Count & " Account Copied To Clipboard.", viewdatagrid1.SelectedItems.Count & " Account Copied To Clipboard.")
        End If
    End Sub
    Async Sub Menu2_copy()
        If viewdatagrid2.SelectedItems.Count > 0 Then
            Dim cp As String = ""
            For Each item In viewdatagrid2.SelectedItems
                Dim acc As New Viewed.Account
                acc = item
                cp += acc.Username & ":" & acc.Password & vbNewLine
            Next
            My.Computer.Clipboard.SetText(cp)
            Await ShowMessageAsync(viewdatagrid2.SelectedItems.Count & " Account Copied To Clipboard.", viewdatagrid2.SelectedItems.Count & " Account Copied To Clipboard.")
        End If
    End Sub
    Sub Menu_view()
        If viewdatagrid1.SelectedItems.Count = 1 Then
            Dim acc As New Viewed.Account
            acc = viewdatagrid1.SelectedItem

            Dim v As New Moreviewer(acc.ChampionData, acc.SkinData, dat, acc)
            v.ShowDialog()
        End If
    End Sub
    Sub Menu_exp()
        If viewdatagrid1.SelectedItems.Count > 0 Then
            Dim list As New List(Of Viewed.Account)
            For Each item In viewdatagrid1.SelectedItems
                list.Add(item)
            Next
            Dim ex As New exporter(list)
            ex.ShowDialog()
        End If
    End Sub
    Sub Menu2_view()
        If viewdatagrid2.SelectedItems.Count = 1 Then
            Dim acc As New Viewed.Account
            acc = viewdatagrid2.SelectedItem

            Dim v As New Moreviewer(acc.ChampionData, acc.SkinData, dat, acc)
            v.ShowDialog()
        End If
    End Sub
    Sub Menu2_exp()
        If viewdatagrid2.SelectedItems.Count > 0 Then
            Dim list As New List(Of Viewed.Account)
            For Each item In viewdatagrid2.SelectedItems
                list.Add(item)
            Next
            Dim ex As New exporter(list)
            ex.ShowDialog()
        End If
    End Sub
    Private Sub viewdatagrid1_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles viewdatagrid1.MouseDoubleClick
        If viewdatagrid1.SelectedItems.Count > 0 Then
            Dim acc As New Viewed.Account
            acc = viewdatagrid1.SelectedItem
           
            Dim v As New Moreviewer(acc.ChampionData, acc.SkinData, New Web.Script.Serialization.JavaScriptSerializer().Deserialize(Of Data.Data)(IO.File.ReadAllText(Environment.CurrentDirectory & "\data.json")), acc)
            v.ShowDialog()
        End If
    End Sub

    Private Sub btn_export_Click(sender As Object, e As RoutedEventArgs) Handles btn_export.Click
      
        Dim ex As New exporter(Accounts)
        ex.ShowDialog()
    End Sub
    Dim filtered As New List(Of Viewed.Account)
    Dim finalfilter As New List(Of Viewed.Account)
    Private Sub btn_filter_Click(sender As Object, e As RoutedEventArgs) Handles btn_filter.Click
        filtered.Clear()
        finalfilter.Clear()
        viewdatagrid2.ItemsSource = ""
        viewdatagrid2.ItemsSource = finalfilter
        Try
            If filter_champs.SelectedItem = "NONE" Then
            ElseIf filter_skins.SelectedItem = "NONE" Then
                For Each acc In Accounts
                    For Each c In acc.ChampionData
                        Dim name As String = dat.Champions.Find(Function(h) h.id = c.id).name
                        If name = filter_champs.Text Then

                            filtered.Add(acc)
                        End If
                    Next
                Next

            Else
                For Each acc In Accounts
                    For Each c In acc.ChampionData
                        Dim name As String = dat.Champions.Find(Function(h) h.id = c.id).name
                        If name = filter_champs.Text Then
                            Dim idd As String
                            For Each cc In dat.Champions
                                For Each skk In cc.skins
                                    If skk.name = filter_skins.Text Then
                                        idd = skk.id
                                    End If
                                Next
                            Next
                            For Each sk In acc.SkinData
                                If sk.id = idd Then
                                    filtered.Add(acc)
                                End If
                            Next
                        End If
                    Next
                Next
            End If
            For Each acc In filtered
                If acc.Level >= lvlint.Value Then
                    finalfilter.Add(acc)
                End If
            Next


            viewdatagrid2.ItemsSource = ""
            viewdatagrid2.ItemsSource = finalfilter

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub filter_champs_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles filter_champs.SelectionChanged
        If Not filter_champs.Text = "NONE" Or e.AddedItems(0) = "NONE" Then
            Try
                Dim c As Data.Champion = dat.Champions.Find(Function(cc) cc.name = e.AddedItems(0))
                filter_skins.Items.Clear()
                filter_skins.Items.Add("NONE")
                For Each sk In c.skins

                    filter_skins.Items.Add(sk.name)
                Next

            Catch

            End Try
        End If
        filter_skins.SelectedIndex = 0

    End Sub

    Private Sub btn_filter_export_Click(sender As Object, e As RoutedEventArgs) Handles btn_filter_export.Click
        Dim ex As New exporter(filtered)
        ex.ShowDialog()
    End Sub
End Class

Namespace Viewed
    Public Class Account
        Property Username As String
        Property Password As String
        Property Summoner_Name As String
        Property Summoner_ID As String
        Property Level As Integer
        Property Email_Status As String
        Property IP As Integer
        Property RP As Integer
        Property Rank As String
        Property Skins As Integer
        Property Champions As Integer
        Property LastLogin As Date
        Property ChampionData As New List(Of CheckingData.Champion)
        Property SkinData As New List(Of CheckingData.Skin)
    End Class
End Namespace
