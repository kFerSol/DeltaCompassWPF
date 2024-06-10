using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
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
using System.Windows.Shapes;

namespace DeltaCompassWPF.Views
{
    /// <summary>
    /// Lógica interna para JanelaSalvarSensibilidade.xaml
    /// </summary>
    public partial class JanelaSalvarSensibilidade : Window
    {
        public JanelaSalvarSensibilidade()
        {
            InitializeComponent();
            var viewmodel = new SlotViewModel();
            DataContext = viewmodel;

            if (viewmodel.CloseAction == null)
                viewmodel.CloseAction = new Action(this.Close);
        }

        private void janela_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnFechar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnFechar.Fill = new SolidColorBrush(Color.FromArgb(255, 46, 43, 194));
        }

        private void btnFechar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnFechar.Fill = new SolidColorBrush(Color.FromArgb(255, 16, 16, 16));
        }

        private void btnFechar_MouseDown(object sender, MouseEventArgs e)
        {
            Close();
        }

        private void btnVoltar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void btnVoltar_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(DataContext is SlotViewModel viewModel)
            {
                viewModel.AtivarSensManual();
            }
        }

        private void bordaAplicar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnConfirmar.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }

        private void bordaAplicar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnConfirmar.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }

        private void bordaConfirmar_MouseEnter(object sender, MouseEventArgs e)
        {
            btnConfirmar2.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }
            
        private void bordaConfirmar_MouseLeave(object sender, MouseEventArgs e)
        {
            btnConfirmar2.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }

        private void txtManual_MouseEnter(object sender, MouseEventArgs e)
        {
            txtManual.Foreground = Brushes.Gray;
            this.Cursor = Cursors.Hand;
        }

        private void txtManual_MouseLeave(object sender, MouseEventArgs e)
        {
            txtManual.Foreground = Brushes.White;
            this.Cursor = Cursors.Arrow;
        }
    }
}
