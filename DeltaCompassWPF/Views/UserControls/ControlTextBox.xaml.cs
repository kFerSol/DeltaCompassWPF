using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeltaCompassWPF.Views.UserControls
{
    /// <summary>
    /// Interação lógica para ControlTextBox.xam
    /// </summary>
    public partial class ControlTextBox : UserControl
    {
        public static readonly DependencyProperty TextoProperty =
            DependencyProperty.Register("Texto", typeof(string), typeof(ControlTextBox), new PropertyMetadata("", TextValueChanged));

        public ControlTextBox()
        {
            InitializeComponent();
            this.DataContext= this;
        }

        public string Placeholder { get; set; }
        public double Largura { get; set; }
        public string Wrap { get; set; }
        public bool Return { get; set; }
        public double Altura { get; set; }
        public string AlinhamentoVertical { get; set; }
        public double AlturaText { get; set; }
        public string ScrollVisibility { get; set; }
        public string AlinhamentoHorizontal { get; set; }
        public bool ReadOnly {  get; set; }
        public string Texto 
        {
            get { return (string)GetValue(TextoProperty); }
            set { SetValue(TextoProperty, value); }
        }

        public int CaracterMaximo { get; set; }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInput.Text != "")
                lblPlaceHolder.Visibility = Visibility.Hidden;
            else
                lblPlaceHolder.Visibility = Visibility.Visible;

            Texto = txtInput.Text;
            TextValueChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler TextValueChangedEvent;

        private static void TextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is ControlTextBox controlTextBox)
            {
                controlTextBox.txtInput.Text = e.NewValue.ToString();
            }
        }
    }
}
