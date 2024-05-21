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
    /// Interação lógica para PaginaConfiguracao.xam
    /// </summary>
    public partial class PaginaConfiguracao : Page
    {
        public PaginaConfiguracao()
        {
            InitializeComponent();
        }

        private void BtnConfigGeral_MouseEnter(object sender, MouseEventArgs e)
        {
            lblGeral.Foreground = new SolidColorBrush(Colors.DarkGray);
            this.Cursor = Cursors.Hand;
        }

        private void BtnConfigGeral_MouseLeave(object sender, MouseEventArgs e)
        {
            lblGeral.Foreground = new SolidColorBrush(Colors.White);
            this.Cursor = Cursors.Arrow;
        }

        private void BtnConfigPerfil_MouseEnter(Object sender, MouseEventArgs e)
        {

            lblPerfil.Foreground = new SolidColorBrush(Colors.DarkGray);
            this.Cursor = Cursors.Hand;
        }

        private void BtnConfigPerfil_MouseLeave(Object sender, MouseEventArgs e)
        {
            lblPerfil.Foreground = new SolidColorBrush(Colors.White);
            this.Cursor = Cursors.Arrow;
        }

        private void BtnConfigGeral_Click(Object sender, RoutedEventArgs e)
        {
            bordaGeral.Visibility = Visibility.Visible;
            bordaPerfil.Visibility = Visibility.Hidden;
            scrollConfigGeral.Visibility = Visibility.Visible;
            scrollConfigPerfil.Visibility = Visibility.Collapsed;
            bordaSalvarPerfil.Visibility = Visibility.Collapsed;
            bordaSalvarGeral.Visibility = Visibility.Visible;
        }

        private void BtnConfigPerfil_Click(Object sender, RoutedEventArgs e)
        {
            bordaGeral.Visibility = Visibility.Hidden;
            bordaPerfil.Visibility = Visibility.Visible;
            scrollConfigGeral.Visibility = Visibility.Collapsed;
            scrollConfigPerfil.Visibility = Visibility.Visible;
            bordaSalvarPerfil.Visibility = Visibility.Collapsed;
            bordaSalvarGeral.Visibility = Visibility.Visible;
        }

        private void bordaSalvarPerfil_MouseEnter(object sender, MouseEventArgs e)
        {
            bordaSalvarPerfil.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            btnSalvarPerfil.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }

        private void bordaSalvarPerfil_MouseLeave(object sender, MouseEventArgs e)
        {
            bordaSalvarPerfil.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            btnSalvarPerfil.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }

        private void bordaSalvarFundo_MouseEnter(object sender, MouseEventArgs e)
        {
            bordaSalvarFundo.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            btnSalvarFundo.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }

        private void bordaSalvarFundo_MouseLeave(object sender, MouseEventArgs e)
        {
            bordaSalvarFundo.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            btnSalvarFundo.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }

        private void bordaSalvarPerfil_MouseEnter_1(object sender, MouseEventArgs e)
        {
            bordaSalvarImagemPerfil.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            btnSalvarImagemPerfil.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }

        private void bordaSalvarPerfil_MouseLeave_1(object sender, MouseEventArgs e)
        {
            bordaSalvarImagemPerfil.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            btnSalvarImagemPerfil.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }

        private void btnTelefone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(lblTelefone.Visibility == Visibility.Visible)
            {
                lblTelefone.Visibility = Visibility.Collapsed;
                txtTelefone.Visibility = Visibility.Visible;
            }
            else
            {
                lblTelefone.Visibility = Visibility.Visible;
                txtTelefone.Visibility = Visibility.Collapsed;
            }
        }

        private void btnTelefone_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void btnTelefone_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void btnSalvarGeral_Click(object sender, RoutedEventArgs e)
        {
            txtTelefone.Visibility = Visibility.Collapsed;
            lblTelefone.Visibility = Visibility.Visible;
        }

        private void bordaSalvarGeral_MouseEnter(object sender, MouseEventArgs e)
        {
            bordaSalvarGeral.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            btnSalvarGeral.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            Cursor = Cursors.Hand;
        }

        private void bordaSalvarGeral_MouseLeave(object sender, MouseEventArgs e)
        {
            bordaSalvarGeral.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            btnSalvarGeral.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }
    }
}
