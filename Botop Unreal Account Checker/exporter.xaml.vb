Imports Microsoft.Win32
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
Public Class exporter
    Dim List As New List(Of Viewed.Account)
    Sub New(database As List(Of Viewed.Account))
        List = database
        InitializeComponent()
    End Sub
    Private Async Sub btn_ex_Click(sender As Object, e As RoutedEventArgs) Handles btn_ex.Click
        Dim s As New SaveFileDialog
        s.Title = "Choose where to save the accounts"
        s.Filter = "Text File|*.txt"
        s.FileName = "Saved.txt"

        If s.ShowDialog = True Then
            IO.File.WriteAllText(s.FileName, "")
            For Each ret In List
                Dim template As String = txtbox.Text
                template = template.Replace("%user%", ret.Username)
                template = template.Replace("%pass%", ret.Password)
                template = template.Replace("%sname%", ret.Summoner_Name)
                template = template.Replace("%rank%", ret.Rank)
                template = template.Replace("%lvl%", ret.Level)
                template = template.Replace("%scount%", ret.Skins)
                template = template.Replace("%ccount%", ret.Champions)
                template = template.Replace("%ip%", ret.IP)
                template = template.Replace("%rp%", ret.RP)
                template = template.Replace("%email%", ret.Email_Status)
                IO.File.AppendAllText(s.FileName, template & vbNewLine)
            Next
            Await ShowMessageAsync(List.Count & " Accounts saved to " & s.FileName, List.Count & " Accounts saved to " & s.FileName)
        Else

        End If
    End Sub
End Class
