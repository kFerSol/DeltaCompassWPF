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

        public int CaracterMaximo { get; set; }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInput.Text != "")
                lblPlaceHolder.Visibility = Visibility.Hidden;
            else
                lblPlaceHolder.Visibility = Visibility.Visible;
        }
    }
}
