using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using DeltaCompassWPF.Views;
using DeltaCompassWPF.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DeltaCompassWPF.ViewModels
{
    public class ConversaoViewModel : ViewModelBase
    {
        public ObservableCollection<Jogo> Jogos { get; set; }

        private Jogo _jogoSelecionado1;
        private Jogo _jogoSelecionado2;
        private string _sensibilidade;
        private int _dpiMouse;
        private ObservableCollection<SlotConfiguracao> _sensibilidades;
        private SlotConfiguracao _sensibilidadeAtual;
        private double _sensibilidadeSalva;
        private bool _sensSalvaVisibilidade;
        private bool _sensVisibilidade;
        private double _sensInserida;
        private bool _alternarSensVisibilidade;
        private int _sensIndex;
        private bool _dpiVisibilidade;
        private bool _dpiSalvoVisibilidade;
        private int _dpiSalvo;
        private double _resultado;
        private string _sensTemporaria;
        private int _dpiTemporaria;
        private string _messageError;
        private bool _isSensitivityApplied;
        
        public bool IsSensitivityApplied
        {
            get { return _isSensitivityApplied; }
            set
            {
                _isSensitivityApplied = value;
                OnPropertyChanged(nameof(IsSensitivityApplied));
            }
        }

        public string MessageError
        {
            get { return _messageError; }
            set 
            {
                _messageError = value;
                OnPropertyChanged(nameof(MessageError));
            }
        }

        public int DpiTemporaria
        {
            get { return _dpiTemporaria; }
            set
            {
                _dpiTemporaria = value;
                OnPropertyChanged(nameof(DpiTemporaria));
                AtualizarResultado();
            }
        }

        public string SensTemporaria
        {
            get { return _sensTemporaria; }
            set 
            {
                if (!string.IsNullOrWhiteSpace(value) && double.TryParse(value, out _resultado))
                {
                    _sensTemporaria = value;
                }
                else
                {
                    _sensTemporaria = "0";
                }
                OnPropertyChanged(nameof(SensTemporaria));
                AtualizarResultado();
            }
        }

        public double Resultado
        {
            get { return _resultado; }
            set
            {
                _resultado = value;
                OnPropertyChanged(nameof(Resultado));
            }
        }

        public int DpiSalvo
        {
            get { return _dpiSalvo; }
            set
            {
                _dpiSalvo = value;
                OnPropertyChanged(nameof(DpiSalvo));
            }
        }

        public bool DpiSalvoVisibilidade
        {
            get { return _dpiSalvoVisibilidade; }
            set
            {
                _dpiSalvoVisibilidade = value;
                OnPropertyChanged(nameof(DpiSalvoVisibilidade));
            }
        }

        public bool DpiVisibilidade
        {
            get { return _dpiVisibilidade; }
            set
            {
                _dpiVisibilidade = value;
                OnPropertyChanged(nameof(DpiVisibilidade));
            }
        }

        public bool AlternarSensVisibilidade
        {
            get { return _alternarSensVisibilidade; }
            set
            {
                _alternarSensVisibilidade = value;
                OnPropertyChanged(nameof(AlternarSensVisibilidade));
            }
        }

        public double SensInserida
        {
            get { return _sensInserida; }
            set
            {
                _sensInserida = value;
                OnPropertyChanged(nameof(SensInserida));
            }
        }

        public double SensibilidadeSalva
        {
            get { return _sensibilidadeSalva; }
            set
            {
                _sensibilidadeSalva = value;
                OnPropertyChanged(nameof(SensibilidadeSalva));
            }
        }

        public bool SensSalvaVisibilidade
        {
            get { return _sensSalvaVisibilidade; }
            set
            {
                _sensSalvaVisibilidade = value;
                OnPropertyChanged(nameof(SensSalvaVisibilidade));
            }
        }

        public bool SensVisibilidade
        {
            get { return _sensVisibilidade; }
            set
            {
                _sensVisibilidade = value;
                OnPropertyChanged(nameof(SensVisibilidade));
            }
        }

        public SlotConfiguracao SensibilidadeAtual
        {
            get { return _sensibilidadeAtual; }
            set
            {
                _sensibilidadeAtual = value;
                OnPropertyChanged(nameof(SensibilidadeAtual));
            }
        }
        public ObservableCollection<SlotConfiguracao> Sensibilidades
        {
            get { return _sensibilidades; }
            set
            {
                _sensibilidades = value;
                OnPropertyChanged(nameof(Sensibilidades));
            }
        }
        public Jogo JogoSelecionado1
        {
            get { return _jogoSelecionado1; }
            set
            {
                _jogoSelecionado1 = value;
                OnPropertyChanged(nameof(JogoSelecionado1));
                OnPropertyChanged(nameof(LogoPath1));
                CarregarSensibilidades();
                VerificarSensSalva();
                AtualizarResultado();
            }
        }
        public Jogo JogoSelecionado2
        {
            get { return _jogoSelecionado2; }
            set
            {
                _jogoSelecionado2 = value;
                OnPropertyChanged(nameof(JogoSelecionado2));
                OnPropertyChanged(nameof(LogoPath2));
                AtualizarSensStatus(false);
                AtualizarResultado();
            }
        }
        public string Sensibilidade
        {
            get => _sensibilidade;
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && double.TryParse(value, out _resultado))
                {
                    _sensibilidade = value;
                }
                else
                {
                    _sensibilidade = "0";
                }
                OnPropertyChanged(nameof(Sensibilidade));
            }
        }
        public int DPIMouse
        {
            get => _dpiMouse;
            set
            {
                _dpiMouse = value;
                OnPropertyChanged(nameof(DPIMouse));
            }
        }

        private readonly IUserRepository _userRepository;
        private IJogoRepository _jogoRepository;
        private readonly ISlotRepository _slotRepository;
        private readonly UserService _userService;

        public string LogoPath1 => GetLogo(JogoSelecionado1?.Nome);
        public string LogoPath2 => GetLogo(JogoSelecionado2?.Nome);
        public bool IsLoggedIn => _userService.IsLoggedIn;
        public Usuario CurrentUser => _userService.CurrentUser;

        public ICommand SalvarCommand { get; }
        public ICommand AlternarSensCommand { get; }
        public ICommand SalvarDpiCommand { get; }
        public ICommand SalvarDpiTemporariaCommand { get; }
        public ICommand SalvarSensTemporariaCommand { get; }
        public ICommand TrocarJogosCommand { get; }
        public ICommand AplicarSensCommand { get; }
        public ICommand CopiarSensCommand { get; }

        public event EventHandler SlotSalvo;

        public ConversaoViewModel()
        {
            _userRepository = new UserRepository();
            _jogoRepository = new JogoRepository();
            _slotRepository = new SlotRepository();
            _userService = UserService.Instance;
            _userService.UserChanged += OnUserChanged;
            Jogos = new ObservableCollection<Jogo>();
            Sensibilidades = new ObservableCollection<SlotConfiguracao>();
            SalvarCommand = new RelayCommand<object>(ExecuteSalvarCommand);
            AlternarSensCommand = new RelayCommand<object>(ExecuteAlternarSensCommand);
            SalvarDpiCommand = new RelayCommand<object>(ExecuteSalvarDpiCommand);
            SalvarDpiTemporariaCommand = new RelayCommand<object>(ExecuteSalvarDpiTemporariaCommand);
            SalvarSensTemporariaCommand = new RelayCommand<object>(ExecuteSalvarSensTemporariaCommand);
            TrocarJogosCommand = new RelayCommand<object>(ExecuteTrocarJogosCommand);
            AplicarSensCommand = new RelayCommand<object>(ExecuteAplicarSensCommand);
            CopiarSensCommand = new RelayCommand<object>(ExecuteCopiarSensCommand);
            CarregarJogos();
            CarregarSensibilidades();
        }

        private void AtualizarSensStatus(bool isApplied)
        {
            IsSensitivityApplied = isApplied;
        }

        private void MostrarNotificacao(string v)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var notification = new ControlNotificacao { Message = v };
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null)
                {
                    var notificationContainer = mainWindow.NotificationContainer;
                    notificationContainer.Children.Add(notification);

                    var slideInAnimation = mainWindow.FindResource("SlideInAnimation") as Storyboard;
                    if (slideInAnimation != null)
                    {
                        Storyboard.SetTarget(slideInAnimation, notification);
                        slideInAnimation.Begin();
                    }

                    Task.Delay(3000).ContinueWith(_ =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            notificationContainer.Children.Remove(notification);
                        });
                    });
                }
                AtualizarSensStatus(true);
            });
        }

        private void ExecuteCopiarSensCommand(object obj)
        {
            Clipboard.SetText(Resultado.ToString());
        }

        private void ExecuteAplicarSensCommand(object obj)
        {
            string filename, config;
            bool fileFound = false;
            DriveInfo[] discos = DriveInfo.GetDrives();
            switch (JogoSelecionado2.Nome)
            {
                // Caso o jogo selecionado seja Apex Legends.
                case "Apex Legends":
                    // Variáveis predefinidas do caminho do arquivo e da palavra-chave.
                    filename = "Saved Games\\Respawn\\Apex\\Local\\settings.cfg";
                    config = "mouse_sensitivity";

                    // Loop para verificar em cada disco que o usuário tiver.
                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            // Combina o diretório raíz do disco e a palavra "Users", criando um caminho inicial, exemplo: "C:\Users"
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Users");
                            // Se esse caminho existir, ele cria um segundo loop.
                            if (Directory.Exists(caminho))
                            {
                                /* Segundo loop que vai verificar em qual pasta do diretório "Users"
                                   se encontra o caminho que está dentro da variável "filename"*/
                                foreach (string userDir in Directory.GetDirectories(caminho))
                                {
                                    string userPath = Path.Combine(userDir, filename);
                                    /* Caso ele ache o caminho dentro da pasta do usuário, ele vai vasculhar o arquivo e substituir
                                       valores se necessário*/
                                    if (File.Exists(userPath))
                                    {
                                        /* A variável "sens" está pegando o valor da propriedade 
                                           "Resultado" (que é o valor convertido da sensibilidade)
                                           e transformando em string (formato de texto). */
                                        string sens = Resultado.ToString();
                                        sens = sens.Replace(",", ".");
                                        /* "sensOriginal" é o nome da variável que vai receber a sensibilidade 
                                           que o software encontrar dentro do arquivo do jogo. */
                                        string sensOriginal = ProcurarSens(userPath, config);
                                        /* Por fim, o sistema vai procurar pela palavra-chave da propriedade que controla a sensibilidade
                                           dentro do texto do arquivo do jogo, quando encontrar ele vai substituir o valor original
                                           pelo valor convertido. */
                                        string procurarLinha = File.ReadAllText(userPath);
                                        procurarLinha = procurarLinha.Replace(config + " " + sensOriginal, config + " " + sens);
                                        File.WriteAllText(userPath, procurarLinha);
                                        MostrarNotificacao("Sensibilidade Aplicada!");
                                        MessageError = null;
                                        fileFound = true;
                                        break;
                                    }
                                }
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    AtualizarSensStatus(fileFound);
                    if (!fileFound)
                    {
                        MessageError = "Arquivo não encontrado.";
                    }
                    break;

                case "Rainbow Six Siege":
                    filename = "GameSettings.ini";
                    config = "MouseSensitivityMultiplierUnit";

                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Users");
                            if (Directory.Exists(caminho))
                            {
                                foreach (string userDir in Directory.GetDirectories(caminho))
                                {
                                    string userPath = Path.Combine(caminho, userDir, "Documents", "My Games", "Rainbow Six - Siege");
                                    if (Directory.Exists(userPath))
                                    {
                                        string filePath = ProcurarCaminho(filename, userPath);
                                        if (File.Exists(filePath))
                                        {
                                            string sens = Resultado.ToString();
                                            sens = sens.Replace(",", ".");
                                            string sensOriginal = ProcurarSens(filePath, config);
                                            string procurarLinha = File.ReadAllText(filePath);
                                            procurarLinha = procurarLinha.Replace(config + "=" + sensOriginal, config + "=" + sens);
                                            File.WriteAllText(userPath, procurarLinha);
                                            MostrarNotificacao("Sensibilidade Aplicada!");
                                            MessageError = null;
                                            fileFound = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    AtualizarSensStatus(fileFound);
                    if (!fileFound)
                    {
                        MessageError = "Sensibilidade não encontrada.";
                    }
                    break;

                case "Call of Duty MWIII":
                    filename = "gamerprofile.0.BASE.cst";
                    config = "MouseHorizontalSensibility:0.0";

                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Users");
                            if (Directory.Exists(caminho))
                            {
                                foreach (string userDir in Directory.GetDirectories(caminho))
                                {
                                    string userPath = Path.Combine(caminho, userDir, "Documents", "Call of Duty", "players");
                                    if (Directory.Exists(userPath))
                                    {
                                        string filePath = ProcurarCaminho(filename, userPath);
                                        if (File.Exists(filePath))
                                        {
                                            string sens = Resultado.ToString();
                                            sens = sens.Replace(",", ".");
                                            string sensOriginal = ProcurarSens(filePath, config);
                                            string procurarLinha = File.ReadAllText(filePath);
                                            procurarLinha = procurarLinha.Replace(config + " = " + sensOriginal, config + " = \"" + sens + "\"");
                                            File.WriteAllText(filePath, procurarLinha);
                                            MostrarNotificacao("Sensibilidade Aplicada!");
                                            MessageError = null;
                                            fileFound = true;
                                            break;
                                        }
                                    }
                                    if (fileFound)
                                        break;
                                }
                            }
                        }
                        if (fileFound)
                            break;
                    }
                    AtualizarSensStatus(fileFound);
                    if (!fileFound)
                    {
                        MessageError = "Sensibilidade não encontrada.";
                    }
                    break;

                case "Counter Strike 2":
                    filename = @"cs2_user_convars_0_slot0.vcfg";
                    config = "\"sensitivity\"";

                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Program Files (x86)", "Steam", "userdata");
                            string filePath = ProcurarCaminho(filename, caminho);
                            if (File.Exists(filePath))
                            {
                                string sens = Resultado.ToString(CultureInfo.InvariantCulture);
                                string fileContent = File.ReadAllText(filePath);

                                string pattern = $@"{config}\s*""[^""]*""";
                                string replacement = $"{config} \"{sens}\"";

                                string newContent = Regex.Replace(fileContent, pattern, replacement);

                                File.WriteAllText(filePath, newContent);
                                MostrarNotificacao("Sensibilidade Aplicada!");
                                MessageError = null;
                                fileFound = true;
                                break;
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    AtualizarSensStatus(fileFound);
                    if (!fileFound)
                    {
                        MessageError = "Sensibilidade não encontrada.";
                    }
                    break;

                case "Portal 2":
                    filename = "config.cfg";
                    config = "sensitivity";

                    foreach(DriveInfo disco in discos)
                    {
                        if(disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Program Files (x86)", "Steam", "steamapps", "common", "Portal 2", "portal2", "cfg");
                            string filePath = ProcurarCaminho(filename, caminho);
                            if (File.Exists(filePath))
                            {
                                string sens = Resultado.ToString();
                                sens = sens.Replace(",", ".");
                                string sensOriginal = ProcurarSens(filePath, config);
                                string procurarLinha = File.ReadAllText(filePath);
                                procurarLinha = procurarLinha.Replace(config + " " + sensOriginal, "sensitivity \"" + sens + "\"");
                                File.WriteAllText(filePath, procurarLinha);
                                MostrarNotificacao("Sensibilidade Aplicada!");
                                MessageError = null;
                                fileFound = true; 
                                break;
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    AtualizarSensStatus(fileFound);
                    if (!fileFound)
                    {
                        MessageError = "Sensibilidade não encontrada.";
                    }
                    break;

                case "Valorant":
                    AbrirJanelaAviso(Resultado);
                    break;
            }
        }

        private void AbrirJanelaAviso(double Resultado)
        {
            ConversaoViewModel cvm = new ConversaoViewModel
            {
                Resultado = Resultado
            };
            JanelaAviso ja = new JanelaAviso
            {
                DataContext = cvm
            };
            Window mainWindow = Application.Current.MainWindow;
            ja.WindowStartupLocation = WindowStartupLocation.Manual;
            ja.Left = mainWindow.Left + (mainWindow.Width - ja.Width) / 2;
            ja.Top = mainWindow.Top + (mainWindow.Height - ja.Height) / 2;
            ja.Show();
        }

        private void ExecuteTrocarJogosCommand(object obj)
        {
            (JogoSelecionado2, JogoSelecionado1) = (JogoSelecionado1, JogoSelecionado2);
        }

        private void ExecuteSalvarSensTemporariaCommand(object obj)
        {
            
            AtualizarResultado();
        }

        private void ExecuteSalvarDpiTemporariaCommand(object obj)
        {
            AtualizarResultado();
        }

        private void AtualizarResultado()
        {
            if (!string.IsNullOrEmpty(JogoSelecionado1?.Nome) && !string.IsNullOrEmpty(JogoSelecionado2?.Nome))
            {
                double sens;
                int dpi;
                
                if (IsLoggedIn)
                {
                    if (Sensibilidade == null)
                        Sensibilidade = "0";
                    sens = SensibilidadeAtual?.Sensibilidade ?? double.Parse(Sensibilidade);
                    dpi = DpiSalvo != 0 ? DpiSalvo : DpiTemporaria;
                }
                else
                {
                    if (SensTemporaria == null)
                        SensTemporaria = "0";
                    sens = double.Parse(SensTemporaria) != 0 ? double.Parse(SensTemporaria) : SensInserida;
                    dpi = DpiTemporaria;
                }

                if (sens != 0 && dpi != 0)
                    Resultado = CalcularConversao(sens, dpi);
                else
                    Resultado = 0;
            }
        }

        private double CalcularConversao(double sens, int dpi)
        {
            double gameFrom, gameTo, fromDpi, toDpi, gameSens;

            gameFrom = ValorCalcJogo(JogoSelecionado1.Nome);
            gameTo = ValorCalcJogo(JogoSelecionado2.Nome);
            fromDpi = dpi;
            toDpi = fromDpi;
            gameSens = sens;
            double calc360 = gameFrom * gameSens;
            double calcSens = calc360 / gameTo;
            Resultado = Math.Round(calcSens * (fromDpi / toDpi), 5);
            double in360 = Math.Round(360 / (calc360 * fromDpi), 2);
            double cm360 = Math.Round((360 / (calc360 * fromDpi)) * 2.54, 2);
            return Resultado;
        }

        private double ValorCalcJogo(string nomeJogo)
        {
            double valorCalc;
            switch (nomeJogo)
            {
                case "Apex Legends":
                    valorCalc = 0.02199999511;
                    break;
                case "Counter Strike 2":
                    valorCalc = 0.02199999511;
                    break;
                case "Call of Duty MWIII":
                    valorCalc = 0.00660000176;
                    break;
                case "Rainbow Six Siege":
                    valorCalc = 0.00572957914;
                    break;
                case "Portal 2":
                    valorCalc = 0.02199999511;
                    break;
                case "Valorant":
                    valorCalc = 0.07;
                    break;
                default:
                    valorCalc = 0;
                    break;
            }
            return valorCalc;
        }

        private string GetLogo(string nomeJogo)
        {
            switch (nomeJogo)
            {
                case "Counter Strike 2":
                    return "../resource/cslogo.png";
                case "Apex Legends":
                    return "../resource/apex-logo.png";
                case "Call of Duty MWIII":
                    return "../resource/MWIII-logo.png";
                case "Portal 2":
                    return "../resource/portal-logo.png";
                case "Rainbow Six Siege":
                    return "../resource/rainbow-logo.jpg";
                case "Valorant":
                    return "../resource/valorant-logo.png";
                default:
                    return "../resource/delta-logo.png";
            }
        }

        private async void ExecuteSalvarDpiCommand(object obj)
        {
            if (IsLoggedIn)
            {
                try
                {
                    Usuario CurrentUser = new Usuario
                    {
                        DpiMouse = DPIMouse
                    };
                    await Task.Run(() => _userRepository.Edit(CurrentUser));
                    DpiSalvo = DPIMouse;
                    DpiSalvoVisibilidade = true;
                    DpiVisibilidade = false;
                }
                catch (Exception ex) 
                {
                    Console.Write(ex);  
                }
                AtualizarResultado();
            }
        }

        private void ExecuteAlternarSensCommand(object obj)
        {
            if (JogoSelecionado1 != null)
            {
                var sensibilidadesJogo = Sensibilidades.Where(s => s.IdJogo == JogoSelecionado1.Id).ToList();
                if (sensibilidadesJogo.Count > 1)
                {
                    _sensIndex = (_sensIndex + 1) % sensibilidadesJogo.Count;
                    SensibilidadeAtual = sensibilidadesJogo[_sensIndex];
                }
                AtualizarResultado();
            }
        }

        private async void ExecuteSalvarCommand(object obj)
        {
            if (JogoSelecionado1 == null || !IsLoggedIn) return;
            var novaSensibilidade = new SlotConfiguracao
            {
                IdJogo = JogoSelecionado1.Id,
                Sensibilidade = double.Parse(Sensibilidade),
                IdUser = CurrentUser.Id
            };
            await Task.Run(() => _slotRepository.Add(novaSensibilidade));
            CarregarSensibilidades();
            AtualizarResultado();
            SlotSalvo?.Invoke(this, EventArgs.Empty);
        }

        private void OnUserChanged(Usuario user)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(CurrentUser));
            SensTemporaria = "0";
            DpiTemporaria = 0;
            CarregarSensibilidades();
        }

        private async void CarregarSensibilidades()
        {
            if (JogoSelecionado1 == null || !IsLoggedIn) return;

            try
            {
                var sensibilidades = await Task.Run(() => _userRepository.GetSensibilidadeByUserId(CurrentUser.Id));
                var dpimouse = await Task.Run(() => _userRepository.GetInformacoesAutenticadas(CurrentUser.Nome));
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Sensibilidades.Clear();
                    foreach(var sensibilidade in sensibilidades)
                    {
                        Sensibilidades.Add(sensibilidade);
                    }

                    if (dpimouse.DpiMouse.HasValue)
                    {
                        DpiSalvo = dpimouse.DpiMouse.Value;
                        DpiSalvoVisibilidade = true;
                        DpiVisibilidade = false;
                    }
                    else
                    {
                        DpiSalvoVisibilidade = false;
                        DpiVisibilidade = true;
                    }
                    VerificarSensSalva();
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void VerificarSensSalva()
        {
            if (!IsLoggedIn) return;

            if (JogoSelecionado1 != null)
            {
                var sensibilidadesJogo = Sensibilidades.Where(s => s.IdJogo == JogoSelecionado1.Id).ToList();
                if (sensibilidadesJogo.Count > 0)
                {
                    SensibilidadeAtual = sensibilidadesJogo[0];
                    SensSalvaVisibilidade = true;
                    SensVisibilidade = false;
                    AlternarSensVisibilidade = sensibilidadesJogo.Count > 1;
                    _sensIndex = 0;
                }
                else
                {
                    SensibilidadeAtual = null;
                    Sensibilidade = null;
                    SensSalvaVisibilidade = false;
                    SensVisibilidade = true;
                    AlternarSensVisibilidade = false;
                }
            }
            else
            {
                SensibilidadeAtual = null;
                Sensibilidade = null;
                SensSalvaVisibilidade = false;
                SensVisibilidade = true;
                AlternarSensVisibilidade = false;
            }
        }

        private async void CarregarJogos()
        {
            var jogos = await Task.Run(() => _jogoRepository.GetJogo());
            Application.Current.Dispatcher.Invoke(() =>
            {
                Jogos.Clear();
                foreach (var jogo in jogos)
                {
                    Jogos.Add(jogo);
                }
            });
        }

        private string ProcurarCaminho(string filename, string caminho)
        {
            try
            {
                string[] arquivos = Directory.GetFiles(caminho, filename, SearchOption.AllDirectories);
                if (arquivos.Length > 0)
                {
                    return arquivos[0];
                }
                return null;
            }
            catch (UnauthorizedAccessException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
        }

        public static string ProcurarSens(string caminhoArquivo, string configDesejada)
        {
            string[] linhas = File.ReadAllLines(caminhoArquivo);
            foreach (string linha in linhas)
            {
                if (linha.Contains(configDesejada))
                {
                    if (configDesejada == "\"sensitivity\"")
                    {
                        string linhaSemEspacosExtras = System.Text.RegularExpressions.Regex.Replace(linha, @"\s+", " ");

                        string[] palavrasCs = linhaSemEspacosExtras.Split(' ');

                        int index = Array.IndexOf(palavrasCs, configDesejada);

                        if (index != -1 && index + 1 < palavrasCs.Length)
                        {
                            string sens = palavrasCs[index + 1];
                            return sens;
                        }
                    }

                    string[] palavras = linha.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries);
                    if (palavras.Length > 1)
                    {
                        string sens;
                        if (palavras[1] == "=")
                            sens = palavras[2];
                        else
                            sens = palavras[1];
                        return sens;
                    }
                }
            }
            return null;
        }
    }
}
