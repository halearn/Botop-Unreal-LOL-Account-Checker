   M S B A M L   `   `   `  ����J  FMahApps.Metro, Version=1.3.0.173, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   
     Botop Unreal Account Checker)   #Botop_Unreal_Account_Checker.viewer   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   
   X TXceed.Wpf.Toolkit, Version=2.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5      -   5           AccentColorBrush#���   5      $��1q�5        � Title$+  %Botop Unreal Checker - Project Viewer��5      $	��511��63   $
��1016��6@   .��5	      � $��	10,10,0,8q�6    � RowDefinitions 5
   
   �� 5      $	��49* �6   �� 5      $
��153* �6   �� 5      $	��28* �6   .+�5   
   �� -   5         ��Name$ 	tbcontrol��5      $��Stretch=�6(   $��Stretch=�6G   $��2��6c   .@�5      �� $
��Main��6   $��Right=�6$   .��5      � ���	#FFE5E5E5$��0,0,6,0q�6.   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ viewdatagrid1��5       $��Stretch=�67   $��Stretch=�6U    � ContextMenu 5      �� 5      .@�5   "   m� -   5        $��View Details��5   +   6A   m� 5   "   -   5        $��Export��5   +   6;   m� 5   "   -   5        $��Copy User:Pass��5   +   6C   5      5      5!      5#      �� 5$      $��Filter��6   .��5%      � ���	#FFE5E5E5.+�5&        -   5        $ viewdatagrid2��5&      $��Stretch=�66   $��Stretch=�6T   $��	0,59,-6,0q�6p    5'      �� 5(      .@�5)   "   m� -   5        $��View Details��5)   +   6A   m� 5*   "   -	   5        $��Export��5*   +   6;   m� 5+   "   -
   5        $��Copy User:Pass��5+   +   6C   5-      5/      �� 50      $��Champion : ��6   $
��Left=�62   $��	10,10,0,0q�6M   $	��Top=�6`   �� 51      -   5         ��Name$ filter_champs��51      $
��Left=�66   $��	89,12,0,0q�6Q   $	��Top=�6d   $	��210��6|   �� 52      $��Skin : ��6   $
��Left=�6.   $��
299,10,0,0q�6I   $	��Top=�6]   �� 53      -   5        $ filter_skins��53      $
��Left=�65   $��
345,12,0,0q�6P   $	��Top=�6d   $	��210��6|   �� 54      $��Level > ��6   $
��Left=�62   $��
560,12,0,0q�6M   $	��Top=�6a   �� 55      -   5         ��Name$ 
btn_filter��55      $��Filter��61   $
��Left=�6B   $��
683,14,0,0q�6]   $	��Top=�6q   $��75��6�   �� 56      $l��fHint: Choose NONE in an fulter to not use it for example setting skin to NONE will not filter by skins��6   $
��Left=�6�   $��	10,33,0,0q�6�   $	��Top=�6�   $	��550��6�   �� 57      -   5        $ btn_filter_export��57      $��
Export All��68   $
��Left=�6M   $��
763,13,0,0q�6h   $	��Top=�6|   $��75��6�   %  Xceed.Wpf.Toolkit.IntegerUpDown  58      -   5          Name$ lvlint��58   )   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Maximum$ 30X�69   	   Minimum$	 1X�6F   
   Value$
 1X�6R   $
��Left=�6\   $��
612,16,0,0q�6w   $	��Top=�6�   $��66��6�   59      5:      5;      �� 5<   
   -   5        $ btn_openproj��5<      $��Open Project��6'   $
��Left=�6>   $��	10,3,0,10q�6Y   $	��165��6l   $��2��6x   �� 5=   
   -   5        $ 
btn_export��5=      $��Export To Txt File��6%   $
��Left=�6B   $��
180,3,0,10q�6]   $	��165��6q   $��2��6}   5?      5@      