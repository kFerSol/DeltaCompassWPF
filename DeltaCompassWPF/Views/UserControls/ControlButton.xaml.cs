using DeltaCompassWPF.Models;
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
    public partial class ControlButton : UserControl
    {
        public ControlButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty LabelContentProperty =
        DependencyProperty.Register("LabelContent", typeof(string), typeof(ControlButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CustomToolTipProperty =
            DependencyProperty.Register("CustomToolTip", typeof(string), typeof(ControlButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CustomBackgroundProperty =
            DependencyProperty.Register("CustomBackground", typeof(Brush), typeof(ControlButton), new PropertyMetadata(Brushes.Transparent));

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ControlButton), new PropertyMetadata(null));

        public static readonly DependencyProperty CustomIsEnabledProperty =
            DependencyProperty.Register("CustomIsEnabled", typeof(bool), typeof(ControlButton), new PropertyMetadata(true));

        public static readonly DependencyProperty JogoSelecionadoProperty =
            DependencyProperty.Register("JogoSelecionado", typeof(Jogo), typeof(ControlButton), new PropertyMetadata(null, JogoSelecionadoPropertyChanged));

        public Jogo JogoSelecionado
        {
            get { return (Jogo)GetValue(JogoSelecionadoProperty); }
            set { SetValue(JogoSelecionadoProperty, value); }
        }

        private static void JogoSelecionadoPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var controlButton = (ControlButton)d;
            controlButton.IsEnabled = e.NewValue != null;
        }

        public string LabelContent
        {
            get { return (string)GetValue(LabelContentProperty); }
            set { SetValue(LabelContentProperty, value); }
        }

        public string CustomToolTip
        {
            get { return (string)GetValue(CustomToolTipProperty); }
            set { SetValue(CustomToolTipProperty, value); }
        }

        public Brush CustomBackground
        {
            get { return (Brush)GetValue(CustomBackgroundProperty); }
            set { SetValue(CustomBackgroundProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public bool CustomIsEnabled
        {
            get { return (bool)GetValue(CustomIsEnabledProperty); }
            set { SetValue(CustomIsEnabledProperty, value); }
        }

        private void bordaAplicar_MouseEnter(object sender, MouseEventArgs e)
        {
            botao.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 22, 20, 92));
            this.Cursor = Cursors.Hand;
        }

        private void bordaAplicar_MouseLeave(object sender, MouseEventArgs e)
        {
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
