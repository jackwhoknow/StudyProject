   M S B A M L   `   `   `  ����B  >WpfApp6, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp6WpfApp6  5   	       WpfApp6.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp6     5   	     � Title$  
MainWindow��5   	   $	��300��6   $	��400��6)    � 	Resources 5	         
CustomFont  MouseOverHighlightStyle �� 
TargetType �� Property �� Value��%�   &	        &	      *�5
   
   Calibri)�61   �� 5   
   " ��60   .�5      -   5        �� 5       �@��Padding$ 5q�62   6>   5      5       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ ApplicationCommands.New��5      5      .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      6   �� 5      6   �� 5      6   �� 5      6    � ColumnDefinitions 5    
   �� 5!      $	��120 �6   6-   �� 5"      6!   �� 5#      6!   .+�5%   
   �� $��原始字符串��6   $��0��6    $��Center=�6-   6J   �� 5&   
   $��加密后的字符串��6   $��1��6"   $��Center=�6/   6L   �� 5'   
   $��解密后的字符串��6   $��2��6"   $��Center=�6/   6L   �� 5(   
   $��输入加密密码��6   $��3��6!   $��Center=�6.   6K   �� 5)   
   -   5        $��
tbOriginal��5)      $��0��6$   $��1��61   $��2��6A   $	��250��6U   $��40��6a   6o   �� 5*   
   -   5        $��	tbEncrypt��5*      $��1��6#   $��1��60   $��2��6@   $	��250��6T   $��40��6`   6n   �� 5+   
   -   5        $��	tbDecrypt��5+      $��2��6#   $��1��60   $��2��6@   $	��250��6T   $��40��6`   6n   �� 5,   
   -   5        $��
tbPassword��5,      $��3��6$   $��1��61   $��2��6A   $	��250��6U   $��40��6a   6o   �� 5-   
   -   5        $��
btnEncrypt��5-      $��4��6#   $��1��60   $��30��6@   $	��100��6L   .��6q   加密6u   �� 5.   
   -   5        $��
btnDecrypt��5.      $��4��6#   $��2��60   $��30��6@   $	��100��6L   .��6q   解密6u   �� 5/   
   -	   5        $��tblock��5/      $��4��6"   #��[ 6/   6b   50      51      