   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.Window1   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	     � Title$  Window1��5   	   $��265.617��6   $��609.169��6*    � 	Resources 5	         BigFontButtonStyle �� Property �� Value �� Triggers :� RoutedEvent g� Duration �� TargetProperty	 W� To  MouseOverHighlightStyle��%�  &	        &	 #    �� 5
   
   �5      �� 5       � ��$ Times New Roman)�67   6Q   �� 5       � ��$ 18(�65   6B   �� 5       � ��$
 Bold"�67   6F    5      :� 5&      $ Mouse.MouseEnter��6   [�5'      �� 5(      .��5)      �� .��5*   "   W� $ 0:0:0:2D�62   $ FontSize�6E   $ 22X�6j   6t   5+      5,      5.      :� 5/      $ Mouse.MouseLeave��6   [�50      �� 51      .��52      �� .��53   "   W� $ 0:0:1D�62   $ FontSize�6C   6j   54      55      57      59      �� 5:   
   .�5;      -   5        �� 5=       �@��Padding$ 5q�62   6>   5>      5?      .��5@      � 	 � ColumnDefinitions	 5A   
   �� 5B      $	��79* �6   �� 5C      $	��13* �6   
 � RowDefinitions
 5E   
   �� 5F      $��60 �6   6*   �� 5G      $��* �6   6)   .+�5I   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.    +� IsFocusScope . $��2��6\   .+�5J      �� $	��Cut��6   $��5q�6#   .��6.   Cut63   �� 5K      $
��Copy��6   $��5q�6$   .��6/   Copy65   �� 5L      $��Paste��6   $��5q�6%   .��60   Paste67   5M      �� 5N   
   #��[  6     	TileBrush#���  6=   $��1��6f   $��2��6s   $	��100��6�   $��50��6�   $	��Top=�6�   $
��Left=�6�   .��6�   	NBA.com6�   �� 5O   
   $��5q�6   $��5q�6   $
��Bold"�6(   $��14(�6:    5P        
TileBrush1 j� TileMode j� ViewportUnits j� Viewport �� ImageSource �� Opacity��%~   &	       �� 5Q      $
 Tile=�60   $ Absolute=�6@   $ 	0 0 10 10�5R      $ 	Happy.jpg��6*   $	 0.3Q�6B   6R   5S      ��5T      �� 5U       �� ResourceKey$ 
TileBrush1��6!   $��Another Tiled Button��5W   B   5X      �� 5Y   
   #��� ��6   $��Label��6Z   $
��Left=�6j   $��32��6�   $��207,112,0,0q�6�   $��1��6�   $	��Top=�6�   $��95��6�   �� 5Z   
   $
��Left=�6   $��48��6/   $��
347,96,0,0q�6;   $��1��6O   $
��Wrap=�6\   $��我想说的是��6p   $	��Top=�6}   $	��131��6�   #��[ 6�   5[      5\      