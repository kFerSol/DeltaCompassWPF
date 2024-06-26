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

namespace DeltaCompassWPF.Views
{
    /// <summary>
    /// Interação lógica para PaginaSuporte.xam
    /// </summary>
    public partial class PaginaSuporte : Page
    {
        public PaginaSuporte()
        {
            InitializeComponent();
        }

        private void btnAplicar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void btnAplicar_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void txtDetalhes_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtDetalhes.Text != "")
                placeDetalhes.Visibility = Visibility.Collapsed;
            else
                placeDetalhes.Visibility = Visibility.Visible;
        }

        private void txtAssunto_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtAssunto.Text != "")
                placeAssunto.Visibility = Visibility.Collapsed;
            else
                placeAssunto.Visibility = Visibility.Visible;
        }
    }
}
