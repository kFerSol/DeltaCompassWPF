   M S B A M L   `   `   `  ���� J  FDeltaCompassWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:DeltaCompassWPF.ViewsDeltaCompassWPF.Views  G(clr-namespace:DeltaCompassWPF.ConvertersDeltaCompassWPF.Converters  W0clr-namespace:DeltaCompassWPF.Views.UserControls"DeltaCompassWPF.Views.UserControls  ,    &DeltaCompassWPF.Views.PaginaCriacaoCla   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:DeltaCompassWPF.Views     ?
converters(clr-namespace:DeltaCompassWPF.Converters     EControls0clr-namespace:DeltaCompassWPF.Views.UserControls       N� 
Background  �#1C1C1C N� Title$ PaginaCriacaoCla�� � 	Resources ;   5DeltaCompassWPF.Converters.BytesToImageBrushConverter   BytesToImageBrushConverter  ScrollBarLineButton �� 
TargetType �� Property �� Value �� 
TargetType  ScrollBarThumb �� CornerRadius  CustomScrollBarStyle ]�Name	 ]� DecreaseRepeatButton
 �� LineUpCommand ]� Thumb ]� IncreaseRepeatButton �� LineDownCommand�� %�  &	        &	      &	 j     &	 �     7  7  7    �� " ��.���  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� 
������ �� " n�.���  �@�OverridesDefaultStyle . ��  � �� �� " n�.R��� ���#232323$ 8���� " ��.���  � �� �#424242��  � ��$ 4����  � �� �� " ��.R�� .+��� #��z��$ 5��]� $ 
PART_Track��$��1����. 	 �� 8��  #��Z
  n� 8�� $	��2,0q� �� 8�� #��Z .���  � ColumnDefinitions �� $��52 ��� ��  � RowDefinitions �� $
��auto ��� .+��� $��0��$��1�� �� Orientation$ Vertical=�$��80��$��20, 20, 0, 0q�$
��Left=�.+��� ���#1C1C1C���� " ��.���  � �� �#101010��  � �� �� " ��.R���  ��Name$ Border��#��z��._��� $��Center=�$��Center=�.���� $��
Criar Clã��$��..\fonts\#Poppins)�
�������$��15(�$��Center=�$��0q��� -   $ bordaCla�����#2E2BC2$��
0, 0, 0, 3q�$��80�����#1C1C1C$y�Visible=��� $��1��$��1��$ Vertical=�$��Center=�$��0, 0, 0, 50q�.+��� $��3q����#2E2BC2$	��200��$	��200��$	 100��._�A� ����  �� ImageSource$# ..\resource\profile-delta.jpg���� -    ��Name$ 
btnAplicar�� � ToolTip$ Aplicar sensibilidade�����#2E2BC2$��0q�$��Center=�$��0, 10, 0, 0q�$��34��$	��200��$��3��$��3��$��Bottom=����� *SelecionarImagemCommand+���� " ��.���  � �� �#101010��  � �� �� " ��.R��� $ Border��#��z��#��z��#��z��$ 10��._��� $��Center=�$��Center=� �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  �� 
TargetName$ Border�� � �� �#16145C�� $ Border�� � �� �#16145C.���� $��Selecionar Logo��$��..\fonts\#ModeSeven)�
�������$��15(��� $��1��$��2��$��0, 0, 20, 60q�$��Center=�$��Center=�  �� BasedOn��%   '��       7  �� " ��8   .���� $��0, 0, 20, 0q�.+��� $��Nome:��$
��Left=�
�������$��16(�$��..\Fonts\#Poppins)�� $
��Left=�.+�7   1DeltaCompassWPF.Views.UserControls.ControlTextBox     Largura$	 300Q�   Placeholder$ Entre 3-20 Caracteres��   Altura$ 33Q�   CaracterMaximo$ 20��   AlinhamentoVertical$ Center��   
AlturaText$ 35Q�    ScrollVisibility$  Disable���� $��
Biografia:��$
��Left=�
�������$��16(�$��..\Fonts\#Poppins)�$��0, 10, 0, 0q�� $
��Left=�.+�  $	 300Q�$  ��$	 110Q�!   Wrap$
! Wrap��"   Return" . $	 Top��$	 110Q�$
  Auto���� $��Método de aprovação:��$
��Left=�
�������$��16(�$��..\Fonts\#Poppins)�$��0, 10, 0, 0q��� $ 
Horizontal=�.+�	� $��..\Fonts\#Poppins)�
�������$��12(�.��Automático	� $��..\Fonts\#Poppins)�
�������$��12(�$��60, 0, 0, 0q�.��Manual�� $!��Número máximo de membros:��$
��Left=�
�������$��16(�$��..\Fonts\#Poppins)�$��0, 10, 0, 0q�  $ 80Q�$ 50��$ 33Q�$ 2��$
��Left=�$ Center��$ 35Q�$  Disable���� $��Plano de fundo:��$
��Left=�
�������$��16(�$��..\Fonts\#Poppins)�$��0, 10, 0, 0q��� $ 20��$��2q����#2E2BC2$��80������ $ ..\resource\bg-delta.png��