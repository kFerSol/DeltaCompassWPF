using DeltaCompassWPF.ViewModels;
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
    /// Interação lógica para PaginaPerfil.xam
    /// </summary>
    public partial class PaginaPerfil : Page
    {
        public PaginaPerfil()
        {
            InitializeComponent();
            DataContext = new PerfilViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSair_MouseEnter(object sender, MouseEventArgs e)
        {
            gridSair.Background = Brushes.DarkRed;
            Cursor = Cursors.Hand;
        }

        private void btnSair_MouseLeave(object sender, MouseEventArgs e)
        {
            gridSair.Background = new SolidColorBrush(Color.FromArgb(255, 74, 74, 74));
            Cursor = Cursors.Arrow;
        }
    }
}
