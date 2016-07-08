
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
Public Class Moreviewer
    Dim champions As New List(Of CheckingData.Champion)
    Dim Skinss As New List(Of CheckingData.Skin)
    Dim daataa As Data.Data
    Dim a As Viewed.Account

    Sub New(champs As List(Of CheckingData.Champion), skins As List(Of CheckingData.Skin), data As Data.Data, r As Viewed.Account)
        champions = champs
        Skinss = skins
        a = r
        daataa = data
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub btn_export_Click(sender As Object, e As RoutedEventArgs) Handles btn_export.Click
        Dim l As New List(Of Viewed.Account)
        l.Add(a)
        Dim ex As New exporter(l)
        ex.ShowDialog()
    End Sub

    Private Sub MetroWindow_Loaded(sender As Object, e As RoutedEventArgs)
        For Each champ In champions
            Try
                Dim c As New Data.Champion
                c = daataa.Champions.Find(Function(cc) cc.id = champ.id)
                list_champ.Items.Add(c.name)
            Catch

            End Try
        Next
        For Each S In Skinss
            Try
                Dim cha As Data.Champion = daataa.Champions.Find(Function(c) c.id = S.championid)
                For Each Sss In cha.skins
                    If Sss.id = S.id Then
                        list_skin.Items.Add(Sss.name)
                    End If
                Next
            Catch

            End Try
        Next
    End Sub
End Class
