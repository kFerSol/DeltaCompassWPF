using DeltaCompassWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.Xml.Linq;
using DeltaCompassWPF.Database;

namespace DeltaCompassWPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        //Instância de cada página
        PaginaPerfil pp = new PaginaPerfil();
        PaginaConversao pc = new PaginaConversao();
        PaginaBuscas pb = new PaginaBuscas();
        PaginaConfiguracao pcf = new PaginaConfiguracao();
        PaginaCriacaoCla pcc = new PaginaCriacaoCla();
        PaginaPerfilCla ppc = new PaginaPerfilCla();
        PaginaPerfilOutro ppo = new PaginaPerfilOutro();
        PaginaSuporte ps = new PaginaSuporte();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var conexao = new Conexao();
        }

        //Construtor
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Eventos
        //Eventos da titlebar
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnMinimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void DockPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Eventos da sidebar
        private void GridMenu_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(GridMenu.Width < 230)
            {
                sideEncolhida.Visibility = Visibility.Visible;
                sideExtendida.Visibility = Visibility.Collapsed;
            }
            else if(GridMenu.Width >= 250)
            {
                sideEncolhida.Visibility = Visibility.Collapsed;
                sideExtendida.Visibility = Visibility.Visible;
            }
        }

        private void btnPerfil_Click(object sender, RoutedEventArgs e)
        {
            main.Navigate(pp);
            
        }

        private void btnConversao_Click(object sender, RoutedEventArgs e)
        {
            main.Navigate(pc);
            
        }

        private void btnCla_Click(object sender, RoutedEventArgs e)
        {
            main.Navigate(pcc);
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            main.Navigate(pb);
        }

        private void btnConfiguracao_Click(object sender, RoutedEventArgs e)
        {
            main.Navigate(pcf);
        }

        private void btnSuporte_Click(object sender, RoutedEventArgs e)
        {
            main.Navigate(ps);
        }

        private void sidebarBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void sidebarBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void main_Navigated(object sender, NavigationEventArgs e)
        {
            var fundo = new SolidColorBrush(Color.FromArgb(255, 28, 28, 28));
            var borda = new SolidColorBrush(Color.FromArgb(255, 46, 43, 194));
            var corPadrao = new SolidColorBrush(Color.FromArgb(255, 74, 74, 74));
            switch (main.Content)
            {
                case PaginaPerfil pp:
                    //Botão perfil
                    btnPerfil.Background = fundo;
                    perfilBorda.BorderBrush = borda;
                    btnPerfilEx.Background = fundo;
                    perfilBordaEx.BorderBrush = borda;
                    //Botão conversão
                    btnConversao.Background = corPadrao;
                    conversaoBorda.BorderBrush = null;
                    btnConversaoEx.Background = corPadrao;
                    bordaConversaoEx.BorderBrush = null;
                    //Botão buscas
                    btnBuscar.Background = corPadrao;
                    buscarBorda.BorderBrush = null;
                    btnBuscasEx.Background = corPadrao;
                    bordaBuscasEx.BorderBrush = null;
                    //Botão clã
                    btnCla.Background = corPadrao;
                    claBorda.BorderBrush = null;
                    btnClaEx.Background = corPadrao;
                    bordaClaEx.BorderBrush = null;
                    //Botão suporte
                    btnSuporte.Background = fundo;
                    btnSuporteEx.Background = fundo;
                    btnSuporteExtendido.Background = fundo;
                    //Botão configuração
                    btnConfiguracao.Background = fundo;
                    btnConfiguracaooEx.Background = fundo;
                    btnConfiguracaoExtendido.Background = fundo;
                    break;

                case PaginaConversao pc:
                    //Botão conversão
                    btnConversao.Background = fundo;
                    conversaoBorda.BorderBrush = borda;
                    btnConversaoEx.Background = fundo;
                    bordaConversaoEx.BorderBrush = borda;
                    //Botão perfil
                    btnPerfil.Background = corPadrao;
                    perfilBorda.BorderBrush = null;
                    btnPerfilEx.Background = corPadrao;
                    perfilBordaEx.BorderBrush = null;
                    //Botão buscas
                    btnBuscar.Background = corPadrao;
                    buscarBorda.BorderBrush = null;
                    btnBuscasEx.Background = corPadrao;
                    bordaBuscasEx.BorderBrush = null;
                    //Botão clã
                    btnCla.Background = corPadrao;
                    claBorda.BorderBrush = null;
                    btnClaEx.Background = corPadrao;
                    bordaClaEx.BorderBrush = null;
                    //Botão suporte
                    btnSuporte.Background = fundo;
                    btnSuporteEx.Background = fundo;
                    btnSuporteExtendido.Background = fundo;
                    //Botão configuração
                    btnConfiguracao.Background = fundo;
                    btnConfiguracaooEx.Background = fundo;
                    btnConfiguracaoExtendido.Background = fundo;
                    break;

                case PaginaBuscas pb:
                    //Botão buscas
                    btnBuscar.Background = fundo;
                    buscarBorda.BorderBrush = borda;
                    btnBuscasEx.Background = fundo;
                    bordaBuscasEx.BorderBrush = borda;
                    //Botão perfil
                    btnPerfil.Background = corPadrao;
                    perfilBorda.BorderBrush = null;
                    btnPerfilEx.Background = corPadrao;
                    perfilBordaEx.BorderBrush = null;
                    //Botão conversão
                    btnConversao.Background = corPadrao;
                    conversaoBorda.BorderBrush = null;
                    btnConversaoEx.Background = corPadrao;
                    bordaConversaoEx.BorderBrush = null;
                    //Botão clã
                    btnCla.Background = corPadrao;
                    claBorda.BorderBrush = null;
                    btnClaEx.Background = corPadrao;
                    bordaClaEx.BorderBrush = null;
                    //Botão suporte
                    btnSuporte.Background = fundo;
                    btnSuporteEx.Background = fundo;
                    btnSuporteExtendido.Background = fundo;
                    //Botão configuração
                    btnConfiguracao.Background = fundo;
                    btnConfiguracaooEx.Background = fundo;
                    btnConfiguracaoExtendido.Background = fundo;
                    break;

                case PaginaCriacaoCla pcc:
                    //Botão clã
                    btnCla.Background = fundo;
                    claBorda.BorderBrush = borda;
                    btnClaEx.Background = fundo;
                    bordaClaEx.BorderBrush = borda;
                    //Botão perfil
                    btnPerfil.Background = corPadrao;
                    perfilBorda.BorderBrush = null;
                    btnPerfilEx.Background = corPadrao;
                    perfilBordaEx.BorderBrush = null;
                    //Botão conversão
                    btnConversao.Background = corPadrao;
                    conversaoBorda.BorderBrush = null;
                    btnConversaoEx.Background = corPadrao;
                    bordaConversaoEx.BorderBrush = null;
                    //Botão buscas
                    btnBuscar.Background = corPadrao;
                    buscarBorda.BorderBrush = null;
                    btnBuscasEx.Background = corPadrao;
                    bordaBuscasEx.BorderBrush = null;
                    //Botão suporte
                    btnSuporte.Background = fundo;
                    btnSuporteEx.Background = fundo;
                    btnSuporteExtendido.Background = fundo;
                    //Botão configuração
                    btnConfiguracao.Background = fundo;
                    btnConfiguracaooEx.Background = fundo;
                    btnConfiguracaoExtendido.Background = fundo;
                    break;

                case PaginaSuporte ps:
                    //Botão suporte
                    btnSuporte.Background = borda;
                    btnSuporteEx.Background = borda;
                    btnSuporteExtendido.Background = corPadrao;
                    //Botão configuração
                    btnConfiguracao.Background = fundo;
                    btnConfiguracaooEx.Background = fundo;
                    btnConfiguracaoExtendido.Background = fundo;
                    //Botão clã
                    btnCla.Background = corPadrao;
                    claBorda.BorderBrush = null;
                    btnClaEx.Background = corPadrao;
                    bordaClaEx.BorderBrush = null;
                    //Botão perfil
                    btnPerfil.Background = corPadrao;
                    perfilBorda.BorderBrush = null;
                    btnPerfilEx.Background = corPadrao;
                    perfilBordaEx.BorderBrush = null;
                    //Botão conversão
                    btnConversao.Background = corPadrao;
                    conversaoBorda.BorderBrush = null;
                    btnConversaoEx.Background = corPadrao;
                    bordaConversaoEx.BorderBrush = null;
                    //Botão buscas
                    btnBuscar.Background = corPadrao;
                    buscarBorda.BorderBrush = null;
                    btnBuscasEx.Background = corPadrao;
                    bordaBuscasEx.BorderBrush = null;
                    break;

                case PaginaConfiguracao pcc:
                    //Botão suporte
                    btnSuporte.Background = fundo;
                    btnSuporteEx.Background = fundo;
                    btnSuporteExtendido.Background = fundo;
                    //Botão configuração
                    btnConfiguracao.Background = borda;
                    btnConfiguracaooEx.Background = borda;
                    btnConfiguracaoExtendido.Background = corPadrao;
                    //Botão clã
                    btnCla.Background = corPadrao;
                    claBorda.BorderBrush = null;
                    btnClaEx.Background = corPadrao;
                    bordaClaEx.BorderBrush = null;
                    //Botão perfil
                    btnPerfil.Background = corPadrao;
                    perfilBorda.BorderBrush = null;
                    btnPerfilEx.Background = corPadrao;
                    perfilBordaEx.BorderBrush = null;
                    //Botão conversão
                    btnConversao.Background = corPadrao;
                    conversaoBorda.BorderBrush = null;
                    btnConversaoEx.Background = corPadrao;
                    bordaConversaoEx.BorderBrush = null;
                    //Botão buscas
                    btnBuscar.Background = corPadrao;
                    buscarBorda.BorderBrush = null;
                    btnBuscasEx.Background = corPadrao;
                    bordaBuscasEx.BorderBrush = null;
                    break;
            }
        }
        #endregion
    }
}
