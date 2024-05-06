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
    /// Interação lógica para ControlTextboxPesquisa.xam
    /// </summary>
    public partial class ControlTextboxPesquisa : UserControl
    {
        public ControlTextboxPesquisa()
        {
            InitializeComponent();
        }

        private void txtboxPesquisar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(txtboxPesquisar.Text == "")
                lblPlaceHolder.Visibility = Visibility.Visible;
            else
                lblPlaceHolder.Visibility = Visibility.Collapsed;
        }
    }
}
