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
    /// Interação lógica para PaginaBuscas.xam
    /// </summary>
    public partial class PaginaBuscas : Page
    {
        public PaginaBuscas()
        {
            InitializeComponent();
        }

        private void BtnBuscarCla_MouseEnter(object sender, MouseEventArgs e)
        {
            lblCla.Foreground = new SolidColorBrush(Colors.DarkGray);
            this.Cursor = Cursors.Hand;
        }
        private void BtnBuscarCla_MouseLeave(object sender, MouseEventArgs e)
        {
            lblCla.Foreground = new SolidColorBrush(Colors.White);
            this.Cursor = Cursors.Arrow;
        }

        private void btnBuscarCla_Click(object sender, RoutedEventArgs e)
        {
            bordaCla.Visibility = Visibility.Visible;
            bordaUsuario.Visibility = Visibility.Hidden;
        }

        private void BtnBuscarUsuario_MouseEnter(object sender, MouseEventArgs e)
        {
            lblUsuario.Foreground = new SolidColorBrush(Colors.DarkGray);
            this.Cursor = Cursors.Hand;
        }

        private void BtnBuscarUsuario_MouseLeave(object sender, MouseEventArgs e)
        {
            lblUsuario.Foreground = new SolidColorBrush(Colors.White);
            this.Cursor = Cursors.Arrow;
        }

        private void btnBuscarUsuario_Click(object sender, RoutedEventArgs e)
        {
            bordaCla.Visibility = Visibility.Hidden;
            bordaUsuario.Visibility = Visibility.Visible;
        }
    }
}
