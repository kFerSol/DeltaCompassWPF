using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
    /// Interação lógica para BindablePasswordBox.xam
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public string Placeholder { get; set; }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString), typeof(BindablePasswordBox));

        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
            
        }

        public BindablePasswordBox()
        {
            InitializeComponent();
            txtSenha.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = txtSenha.SecurePassword;
        }

        private void txtSenha_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtSenha.Password != "")
                placeSenha.Visibility = Visibility.Collapsed;
            else
                placeSenha.Visibility = Visibility.Visible;
        }
    }
}
