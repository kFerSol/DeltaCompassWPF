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
    /// Lógica interna para JanelaAbrirSlot.xaml
    /// </summary>
    public partial class JanelaAbrirSlot : Window
    {
        public JanelaAbrirSlot()
        {
            InitializeComponent();
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

        private void btnExcluir_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void btnExcluir_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}
