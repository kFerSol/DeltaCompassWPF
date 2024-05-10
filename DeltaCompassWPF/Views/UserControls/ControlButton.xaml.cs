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

        private void btnBorda_MouseEnter(object sender, MouseEventArgs e)
        {
           
        }

        private void btnBorda_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void BtnCursor_MouseEnter(Object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void BtnCursor_MouseLeave(Object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        public string btnText { get; set; }
    }
}
