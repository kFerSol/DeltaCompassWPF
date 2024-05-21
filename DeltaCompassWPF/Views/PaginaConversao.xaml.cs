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
    /// Interação lógica para PaginaConversao.xam
    /// </summary>
    public partial class PaginaConversao : Page
    {
        public PaginaConversao()
        {
            InitializeComponent();
            DataContext = new ConversaoViewModel();
        }

        private void BtnCursor_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void BtnCursor_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void BordaAplicar_MouseEnter(object sender, MouseEventArgs e)
        {
            bordaAplicar.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            btnAplicar.Background = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }

        private void BordaAplicar_MouseLeave(object sender, MouseEventArgs e)
        {
            bordaAplicar.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            btnAplicar.Background = new SolidColorBrush(Color.FromArgb(250, 46, 43, 194));
            this.Cursor = Cursors.Arrow;
        }
    }
}
