using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeltaCompassWPF.Helpers
{
    public static class AttachedBehavior
    {
        public static readonly DependencyProperty EnterCommandProperty =
            DependencyProperty.RegisterAttached(
                "EnterCommand",
                typeof(ICommand),
                typeof(AttachedBehavior),
                new PropertyMetadata(null, OnEnterCommandChanged));

        public static void SetEnterCommand(UIElement element, ICommand value)
        {
            element.SetValue(EnterCommandProperty, value);
        }

        public static ICommand GetEnterCommand(UIElement element)
        {
            return (ICommand)element.GetValue(EnterCommandProperty);
        }

        private static void OnEnterCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if (e.OldValue == null && e.NewValue != null) 
                {
                    textBox.KeyDown += TextBox_KeyDown;
                }
                else if (e.OldValue != null && e.NewValue == null)
                {
                    textBox.KeyDown -= TextBox_KeyDown;
                }
            }
        }

        private static void TextBox_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                var command = GetEnterCommand(textBox);
                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }
    }
}
