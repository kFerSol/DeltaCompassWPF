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
    /// Interação lógica para ControlButton.xam
    /// </summary>
    public partial class ControlButton : UserControl
    {
        public ControlButton()
        {
            InitializeComponent();
        }

        public string BtnTexto { get; set; }
        public double Largura { get; set; }

        private void btnBorda_MouseEnter(object sender, MouseEventArgs e)
        {
            btnBorda.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            botao.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }

        private void btnBorda_MouseLeave(object sender, MouseEventArgs e)
        {
            btnBorda.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            botao.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }

        private void BtnCursor_MouseEnter(Object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void BtnCursor_MouseLeave(Object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
    }
}
