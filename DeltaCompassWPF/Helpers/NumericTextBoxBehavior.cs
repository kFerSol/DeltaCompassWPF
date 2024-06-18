using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeltaCompassWPF.Helpers
{
    public class NumericTextBoxBehavior
    {
        public static void Attach(TextBox textBox)
        {
            textBox.PreviewTextInput += OnPreviewTextInput;
            DataObject.AddPastingHandler(textBox, OnPaste);
        }

        public static void Detach(TextBox textBox)
        {
            textBox.PreviewTextInput -= OnPreviewTextInput;
            DataObject.RemovePastingHandler(textBox, OnPaste);
        }

        private static void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            var fullText = GetFullText(textBox, e.Text);
            e.Handled = !IsTextValid(fullText);
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null) return;

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pasteText = e.DataObject.GetData(typeof(string)) as string;
                var fullText = GetFullText(textBox, pasteText);

                if (!IsTextValid(fullText))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static string GetFullText(TextBox textBox, string inputText)
        {
            var text = textBox.Text;
            var selectionStart = textBox.SelectionStart;
            var selectionLength = textBox.SelectionLength;

            return text.Remove(selectionStart, selectionLength).Insert(selectionStart, inputText);
        }

        private static bool IsTextValid(string text)
        {
            return double.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out _);
        }
    }
}
