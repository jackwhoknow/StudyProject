   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)    J� CommandBindings 5	      )  #System.Windows.Input.CommandBinding  5
   
   -   5           Command$ ApplicationCommands.New��5
      6�   .��5      � $��
0,0,-8,-21q�6    � RowDefinitions 5   
   �� 5      $��30 �6   6*   �� 5      $��* �6   6)   .+�5   
   c� $��0��6   .@�5      �� $	��Cut��6   .��6#   Cut6(   5      �� 5   
   -   5        $	��tb1��5      $
��Left=�6   $	��112��68   $��
146,24,0,0q�6E   $��1��6Y    �� TextWrapping$
 Wrap=�6f   $	��Top=�6�   $	��477��6�   ��6z   �� *MyPerson.Name+6�   �� 5   
   $	��New��6   $	��New��6   $
��Left=�6-   $��44��6H   $��624,330,0,0q�6T   $��1��6i   $	��Top=�6v   $	��147��6�   �� 5   
   -   5        $	��tb2��5      $
��Left=�6X   $��98��6s   $��146,159,0,0q�6   $��1��6�   $
 Wrap=�6�   $	��Top=�6�   $	��477��6�   ��6   ��  �� ElementName$ sliderFontSize�� �� Path$ Value���6�   �� *MyAnimal.Name+.��6�   5      �� 5   
   -   5        $��sliderFontSize��5       � Minimum$ 20Q�6'    � Maximum$	 200Q�64   $
��Left=�6B   $��44��6]   $��146,279,0,0q�6i   $��1��6~   $	��Top=�6�   $	��477��6�   �� 5   
   -   5        $	��tb3��5      $
��Left=�6   $��46��6:   $��169,328,0,0q�6F   $��1��6[   $
��Wrap=�6h   $	��Top=�6}   $	��216��6�   ��5      �� 5      	 �� RelativeSource	 5      �� 5      
 �� Mode$
 FindAncestor=�6)    �� AncestorType" �6=   6^   5!      5#      5$      5%      