   M S B A M L   `   `   `  ����B  >WpfApp4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp4WpfApp4  5   	       WpfApp4.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp4     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      �� .+�5
   
   �� -   5        $��
scrollBar1��5
   ,   $	��237��6   $��24��6     �� Orientation$ 
Horizontal=�6>    �� Left$	 103��5      
 �� Top$ 51��6&    � Minimum$ 1Q�66    � Maximum$	 100Q�6B    � SmallChange$ 1Q�6P   5      �� 5   
   -   5        $��lable1��5   O   $ 41��6   $	 121��6!   $��OneWay��62   $��28��6C   6O   �� 5   
   -   5        $��textBox1��5   5   $	 165��6   $	 121��6$   $	��120��6E   $��23��6Q   ��5      ��  �� ElementName$ 
scrollBar1�� �� Path$ Value�	 �� Mode$	 OneWay=��� 5   
   -   5        $��lable2��5   W   $ 41��6   $	 160��6!   $��OneWayToSource��62   $��28��6K   6W   �� 5   
   -   5        $��textBox2��5   5   $	 165��6   $	 160��6$   $	��120��6E   $��23��6Q   ��5      �� $ 
scrollBar1��$ Value�$	 OneWayToSource=��� 5   
   -   5        $��lable3��5   O   $ 41��6   $	 202��6!   $��TwoWay��62   $��28��6C   6O   �� 5   
   -   5        $��textBox3��5   5   $	 165��6   $	 202��6$   $	��120��6E   $��23��6Q   ��5      �� $ 
scrollBar1��$ Value�$	 TwoWay=��� 5   
   -   5        $��lable4��5   P   $ 41��6   $	 231��6!   $��OneTime��62   $��28��6D   6P   �� 5   
   -	   5        $��textBox4��5   5   $	 165��6   $	 231��6$   $	��120��6E   $��23��6Q   ��5      �� $ 
scrollBar1��$ Value�$	 OneTime=�5      5      