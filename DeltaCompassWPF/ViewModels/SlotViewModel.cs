 using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using DeltaCompassWPF.Views.UserControls;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace DeltaCompassWPF.ViewModels
{
    public class SlotViewModel : ViewModelBase
    {
        private Jogo _jogoSelecionado;
        private SlotConfiguracao _slot;
        private ObservableCollection<SlotConfiguracao> _sensibilidades;
        private string _sensibilidade;
        private string _novaSensibilidade;
        private bool _sensManual = false;
        private string _messagemErro;
        private bool _isEditing;

        private IJogoRepository _jogoRepository;
        private ISlotRepository _slotRepository;
        private readonly UserService _userService;
        private readonly UserRepository _userRepository;

        public ObservableCollection<Jogo> Jogos { get; set; }
        public Usuario CurrentUser => _userService.CurrentUser;

        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }

        public string NovaSensibilidade
        {
            get => _novaSensibilidade;
            set
            {
                _novaSensibilidade = value;
                OnPropertyChanged(nameof(NovaSensibilidade));
            }
        }

        public string MessagemErro
        {
            get => _messagemErro;
            set
            {
                _messagemErro = value;
                OnPropertyChanged(nameof(MessagemErro));
            }
        }

        public bool SensManual
        {
            get => _sensManual;
            set
            {
                _sensManual = value;
                OnPropertyChanged(nameof(SensManual));
            }
        }

        public string Sensibilidade
        {
            get => _sensibilidade;
            set
            {
                _sensibilidade = value;
                OnPropertyChanged(nameof(Sensibilidade));
            }
        }

        public Jogo JogoSelecionado
        {
            get => _jogoSelecionado;
            set
            {
                _jogoSelecionado = value;
                OnPropertyChanged(nameof(JogoSelecionado));
                OnPropertyChanged(nameof(LogoPath));
                ((RelayCommand<object>)SalvarSensibilidadeCommand).RaiseCanExecuteChanged();
            }
        }

        public SlotConfiguracao Slot
        {
            get => _slot;
            set
            {
                _slot = value;
                OnPropertyChanged(nameof(Slot));
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

        public string LogoPath => GetLogo(JogoSelecionado?.Nome);
        public bool IsLoggedIn => _userService.IsLoggedIn;
        public Action CloseAction { get; set; }
        public ICommand SalvarSensibilidadeCommand { get; }
        public ICommand VoltarCommand { get; }
        public ICommand SelecionarArquivoCommand { get; }
        public ICommand ExcluirSensibilidadeCommand { get; }
        public ICommand ToggleEditarCommand { get; }
        public ICommand SalvarEdicaoSensCommand { get; }
        public ICommand AplicarSensCommand { get; }

        public event EventHandler SlotDeleted;
        public event EventHandler SlotAdded;

        public SlotViewModel(SlotConfiguracao slot)
        {
            Slot = slot;
            _slotRepository = new SlotRepository();
            ExcluirSensibilidadeCommand = new RelayCommand<object>(ExecuteExcluirSensibilidadeCommand, CanExecuteExcluirSensCommand);
            ToggleEditarCommand = new RelayCommand<object>(ExecuteToggleEditarCommand);
            SalvarEdicaoSensCommand = new RelayCommand<object>(ExecuteEdicaoSensCommand);
            AplicarSensCommand = new RelayCommand<object>(ExecuteAplicarSensCommand);
        }

        public SlotViewModel()
        {
            _userService = UserService.Instance;
            _userService.UserChanged += OnUserChanged;
            _userRepository = new UserRepository();
            _jogoRepository = new JogoRepository();
            _slotRepository = new SlotRepository();
            Jogos = new ObservableCollection<Jogo>();
            Sensibilidades = new ObservableCollection<SlotConfiguracao>();
            SalvarSensibilidadeCommand = new RelayCommand<object>(ExecuteSalvarSensCommand, CanExecuteSalvarSensCommand);
            VoltarCommand = new RelayCommand<object>(ExecuteVoltarCommand);
            SelecionarArquivoCommand = new RelayCommand<object>(ExecuteSelecionarArquivoCommand);
            Slot = new SlotConfiguracao()
            {
                Nome = null,
                Imagem = "../resource/delta-logo.png"
            };
            CarregarJogos();
            CarregarSensibilidades();
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
            });
        }

        private void ExecuteAplicarSensCommand(object obj)
        {
            string filename, config;
            bool fileFound = false;
            DriveInfo[] discos = DriveInfo.GetDrives();

            switch (Slot.Nome)
            {
                case "Apex Legends":
                    filename = "Saved Games\\Respawn\\Apex\\Local\\settings.cfg";
                    config = "mouse_sensitivity";

                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Users");
                            if (Directory.Exists(caminho))
                            {
                                foreach (string userDir in Directory.GetDirectories(caminho))
                                {
                                    string userPath = Path.Combine(userDir, filename);
                                    if (File.Exists(userPath))
                                    {
                                        string sens = Slot.Sensibilidade.ToString();
                                        sens = sens.Replace(",", ".");
                                        string sensOriginal = ProcurarSens(userPath, config);
                                        string procurarLinha = File.ReadAllText(userPath);
                                        procurarLinha = procurarLinha.Replace(config + " " + sensOriginal, config + " " + sens);
                                        File.WriteAllText(userPath, procurarLinha);
                                        Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                        fileFound = true;
                                        MostrarNotificacao("Sensibilidade Aplicada!");
                                        break;
                                    }
                                }
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (!fileFound)
                    {
                        MessagemErro = "Não foi possível encontrar o arquivo de configuração!" +
                            " Por favor, insira manualmente essa sensibilidade ao seu jogo.";
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
                                            string sens = Slot.Sensibilidade.ToString();
                                            sens = sens.Replace(",", ".");
                                            string sensOriginal = ProcurarSens(filePath, config);
                                            string procurarLinha = File.ReadAllText(filePath);
                                            procurarLinha = procurarLinha.Replace(config + "=" + sensOriginal, config + "=" + sens);
                                            File.WriteAllText(userPath, procurarLinha);
                                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                            fileFound = true;
                                            MostrarNotificacao("Sensibilidade Aplicada!");
                                            break;
                                        }
                                    }
                                }
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (!fileFound)
                    {
                        MessagemErro = "Não foi possível encontrar o arquivo de configuração!" +
                            " Por favor, insira manualmente essa sensibilidade ao seu jogo.";
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
                                            string sens = Slot.Sensibilidade.ToString();
                                            sens = sens.Replace(",", ".");
                                            string sensOriginal = ProcurarSens(filePath, config);
                                            string procurarLinha = File.ReadAllText(filePath);
                                            procurarLinha = procurarLinha.Replace(config + " = " + sensOriginal, config + " = \"" + sens + "\"");
                                            File.WriteAllText(filePath, procurarLinha);
                                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                            fileFound = true;
                                            MostrarNotificacao("Sensibilidade Aplicada!");
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
                    if (!fileFound)
                    {
                        MessagemErro = "Não foi possível encontrar o arquivo de configuração!" +
                            " Por favor, insira manualmente essa sensibilidade ao seu jogo.";
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
                                string sens = Slot.Sensibilidade.ToString(CultureInfo.InvariantCulture);
                                string fileContent = File.ReadAllText(filePath);

                                string pattern = $@"{config}\s*""[^""]*""";
                                string replacement = $"{config} \"{sens}\"";

                                string newContent = Regex.Replace(fileContent, pattern, replacement);

                                File.WriteAllText(filePath, newContent);
                                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                fileFound = true;
                                MostrarNotificacao("Sensibilidade Aplicada!");
                                break;
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (!fileFound)
                    {
                        MessagemErro = "Não foi possível encontrar o arquivo de configuração!" +
                            " Por favor, insira manualmente essa sensibilidade ao seu jogo.";
                    }
                    break;

                case "Portal 2":
                    filename = "config.cfg";
                    config = "sensitivity";

                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Program Files (x86)", "Steam", "steamapps", "common", "Portal 2", "portal2", "cfg");
                            string filePath = ProcurarCaminho(filename, caminho);
                            if (File.Exists(filePath))
                            {
                                string sens = Slot.Sensibilidade.ToString();
                                sens = sens.Replace(",", ".");
                                string sensOriginal = ProcurarSens(filePath, config);
                                string procurarLinha = File.ReadAllText(filePath);
                                procurarLinha = procurarLinha.Replace(config + " " + sensOriginal, "sensitivity \"" + sens + "\"");
                                File.WriteAllText(filePath, procurarLinha);
                                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                fileFound = true;
                                MostrarNotificacao("Sensibilidade Aplicada!");
                                break;
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (!fileFound)
                    {
                        MessagemErro = "Não foi possível encontrar o arquivo de configuração!" +
                            " Por favor, insira manualmente essa sensibilidade ao seu jogo.";
                    }
                    break;

                case "Valorant":
                    if (!fileFound)
                    {
                        MessagemErro = "Não temos permissão para alterar o arquivo de configuração!" +
                            " Por favor, insira manualmente essa sensibilidade ao seu jogo.";
                    }
                    break;
            }
        }

        private void ExecuteEdicaoSensCommand(object obj)
        { 
            Slot.Sensibilidade = double.Parse(NovaSensibilidade);
            _slotRepository.Edit(Slot);
            MostrarNotificacao("Sensibilidade Salva!");
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
        }

        private void ExecuteToggleEditarCommand(object obj)
        {
            IsEditing = !IsEditing;
        }

        private bool CanExecuteExcluirSensCommand(object arg)
        {
            return Slot != null;
        }

        private void ExecuteExcluirSensibilidadeCommand(object obj)
        {
            _slotRepository.Remove(Slot);
            SlotDeleted?.Invoke(this, EventArgs.Empty);
            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
        }

        private void ExecuteSelecionarArquivoCommand(object obj)
        {
            if(_jogoSelecionado != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Config Files (*.cfg;*.cst;*.vcfg)|*.cfg;*.cst;*.vcfg|All Files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    if (File.Exists(selectedFilePath))
                    {
                        string config = GetConfigKey(_jogoSelecionado.Nome);
                        string sens = ProcurarSens(selectedFilePath, config);
                        Console.Write(sens);
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(ProcurarSens(selectedFilePath, config).Trim('"'), CultureInfo.InvariantCulture),
                            Nome = _jogoSelecionado.Nome,
                            IdUser = CurrentUser.Id,
                            IdJogo = _jogoSelecionado.Id
                        };

                        _slotRepository.Add(_slot);
                        SlotAdded?.Invoke(this, EventArgs.Empty);
                        Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                    }
                }
            }
        }
          
        private string GetConfigKey(string gameName)
        {
            switch (gameName)
            {
                case "Counter Strike 2":
                    return "\"sensitivity\"";
                case "Apex Legends":
                    return "mouse_sensitivity";
                case "Call of Duty MWIII":
                    return "MouseHorizontalSensibility:0.0";
                case "Portal 2":
                    return "sensitivity";
                case "Rainbow Six Siege":
                    return "sens";
                case "Valorant":
                    return "sens";
                default:
                    throw new ArgumentException("Jogo não suportado");
            }
        }

        public void AtivarSensManual()
        {
            SensManual = true;
            MessagemErro = null;
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

        private void ExecuteVoltarCommand(object obj)
        {
            SensManual = false;
        }

        private void OnUserChanged(Usuario usuario)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(CurrentUser));
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

        private async void CarregarSensibilidades()
        {
            if (!IsLoggedIn) return;

            try
            {
                var sensibilidades = await Task.Run(() => _userRepository.GetSensibilidadeByUserId(CurrentUser.Id));
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Sensibilidades.Clear();
                    foreach (var sensibilidade in sensibilidades)
                    {
                        Sensibilidades.Add(sensibilidade);
                    }
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ExecuteSalvarSensCommand(object obj)
        {
            string filename;
            string config;
            bool fileFound = false;
            DriveInfo[] discos = DriveInfo.GetDrives();

            switch (_jogoSelecionado.Nome)
            {
                case "Apex Legends":
                    filename = "Saved Games\\Respawn\\Apex\\Local\\settings.cfg";
                    config = "mouse_sensitivity";

                    discos = DriveInfo.GetDrives();
                    foreach (DriveInfo disco in discos)
                    {
                        if (SensManual) break;

                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Users");
                            if (Directory.Exists(caminho))
                            {
                                foreach (string userDir in Directory.GetDirectories(caminho))
                                {
                                    string userPath = Path.Combine(userDir, filename);
                                    if (File.Exists(userPath))
                                    {
                                        _slot = new SlotConfiguracao
                                        {
                                            Sensibilidade = double.Parse(ProcurarSens(userPath, config), CultureInfo.InvariantCulture),
                                            Imagem = "../resource/apex-logo.png",
                                            Nome = "Apex Legends",
                                            IdUser = CurrentUser.Id,
                                            IdJogo = 1
                                        };
                                        _slotRepository.Add(_slot);
                                        SlotAdded?.Invoke(this, EventArgs.Empty);
                                        Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                        fileFound = true;
                                        MostrarNotificacao("Sensibilidade Salva!");
                                        break;
                                    }
                                }
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (SensManual == true)
                    {
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(Sensibilidade),
                            Imagem = "../resource/apex-logo.png",
                            Nome = "Apex Legends",
                            IdUser = CurrentUser.Id,
                            IdJogo = 1
                        };
                        if (double.Parse(Sensibilidade) != 0)
                        {
                            _slotRepository.Add(_slot);
                            SlotAdded?.Invoke(this, EventArgs.Empty);
                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                            MostrarNotificacao("Sensibilidade Salva!");
                        }
                    }
                    else
                    {
                        if (!fileFound)
                            MessagemErro = "Arquivo não encontrado!";
                        SensManual = true;
                    }
                    break;

                case "Rainbow Six Siege":
                    filename = "GameSettings.ini";
                    config = "MouseSensitivityMultiplierUnit";

                    foreach (DriveInfo disco in discos)
                    {
                        if (SensManual) break;

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
                                            _slot = new SlotConfiguracao
                                            {
                                                Sensibilidade = double.Parse(ProcurarSens(filePath, config), CultureInfo.InvariantCulture),
                                                Imagem = "../resource/rainbow-logo.jpg",
                                                Nome = "Rainbow Six Siege",
                                                IdUser = CurrentUser.Id,
                                                IdJogo = 5
                                            };
                                            _slotRepository.Add(_slot);
                                            SlotAdded?.Invoke(this, EventArgs.Empty);
                                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                            fileFound = true;
                                            MostrarNotificacao("Sensibilidade Salva!");
                                            break;
                                        }
                                    }
                                }
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (SensManual == true)
                    {
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(Sensibilidade),
                            Imagem = "../resource/rainbow-logo.jpg",
                            Nome = "Rainbow Six Siege",
                            IdUser = CurrentUser.Id,
                            IdJogo = 5
                        };
                        if (double.Parse(Sensibilidade) != 0)
                        {
                            _slotRepository.Add(_slot);
                            SlotAdded?.Invoke(this, EventArgs.Empty);
                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                            MostrarNotificacao("Sensibilidade Salva!");
                        }
                    }
                    else
                    {
                        if (!fileFound)
                            MessagemErro = "Arquivo não encontrado!";
                        SensManual = true;
                    }
                    break;

                case "Call of Duty MWIII":
                    filename = "gamerprofile.0.BASE.cst";
                    config = "MouseHorizontalSensibility:0.0";

                    foreach (DriveInfo disco in discos)
                    {
                        if (SensManual) break;

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
                                            _slot = new SlotConfiguracao
                                            {
                                                Sensibilidade = double.Parse(ProcurarSens(filePath, config).Trim('"'), CultureInfo.InvariantCulture),
                                                Imagem = "../resource/MWIII-logo.png",
                                                Nome = "Call of Duty MWIII",
                                                IdUser = CurrentUser.Id,
                                                IdJogo = 2
                                            };
                                            _slotRepository.Add(_slot);
                                            SlotAdded?.Invoke(this, EventArgs.Empty);
                                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                            fileFound = true;
                                            MostrarNotificacao("Sensibilidade Salva!");
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
                    if (SensManual == true)
                    {
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(Sensibilidade),
                            Imagem = "../resource/MWIII-logo.png",
                            Nome = "Call of Duty MWIII",
                            IdUser = CurrentUser.Id,
                            IdJogo = 2
                        };
                        if (double.Parse(Sensibilidade) != 0)
                        {
                            _slotRepository.Add(_slot);
                            SlotAdded?.Invoke(this, EventArgs.Empty);
                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                            MostrarNotificacao("Sensibilidade Salva!");
                        }
                    }
                    else
                    {
                        if (!fileFound)
                            MessagemErro = "Arquivo não encontrado!";
                        SensManual = true;
                    }
                    break;

                case "Counter Strike 2":
                    filename = @"cs2_user_convars_0_slot0.vcfg";
                    config = "\"sensitivity\"";

                    foreach (DriveInfo disco in discos)
                    {
                        if (SensManual) break;

                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Program Files (x86)", "Steam", "userdata");
                            string filePath = ProcurarCaminho(filename, caminho);
                            if (File.Exists(filePath))
                            {
                                string sens = ProcurarSens(filePath, config);
                                if (sens != null && sens != "")
                                {
                                    _slot = new SlotConfiguracao
                                    {
                                        Sensibilidade = double.Parse(sens.Trim('"'), CultureInfo.InvariantCulture),
                                        Imagem = "../resource/cslogo.png",
                                        Nome = "Counter Strike 2",
                                        IdUser = CurrentUser.Id,
                                        IdJogo = 3
                                    };
                                    _slotRepository.Add(_slot);
                                    SlotAdded?.Invoke(this, EventArgs.Empty);
                                    Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                    fileFound = true;
                                    MostrarNotificacao("Sensibilidade Salva!");
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (SensManual == true)
                    {
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(Sensibilidade),
                            Imagem = "../resource/cslogo.png",
                            Nome = "Counter Strike 2",
                            IdUser = CurrentUser.Id,
                            IdJogo = 3
                        };
                        if (double.Parse(Sensibilidade) != 0)
                        {
                            _slotRepository.Add(_slot);
                            SlotAdded?.Invoke(this, EventArgs.Empty);
                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                            MostrarNotificacao("Sensibilidade Salva!");
                        }
                    }
                    else
                    {
                        if (!fileFound)
                            MessagemErro = "Arquivo não encontrado!";
                        SensManual = true;
                    }
                    break;

                case "Portal 2":
                    filename = "config.cfg";
                    config = "sensitivity";

                    foreach (DriveInfo disco in discos)
                    {
                        if (SensManual) break;

                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Program Files (x86)", "Steam", "steamapps", "common", "Portal 2", "portal2", "cfg");
                            string filePath = ProcurarCaminho(filename, caminho);
                            if (File.Exists(filePath))
                            {
                                _slot = new SlotConfiguracao
                                {
                                    Sensibilidade = double.Parse(ProcurarSens(filePath, config), CultureInfo.InvariantCulture),
                                    Imagem = "../resource/portal-logo.png",
                                    Nome = "Portal 2",
                                    IdUser = CurrentUser.Id,
                                    IdJogo = 4
                                };
                                _slotRepository.Add(_slot);
                                SlotAdded?.Invoke(this, EventArgs.Empty);
                                Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                                fileFound = true;
                                MostrarNotificacao("Sensibilidade Salva!");
                                break;
                            }
                            if (fileFound)
                                break;
                        }
                    }
                    if (SensManual == true)
                    {
                        if (Sensibilidade == null) return;
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(Sensibilidade),
                            Imagem = "../resource/portal-logo.png",
                            Nome = "Portal 2",
                            IdUser = CurrentUser.Id,
                            IdJogo = 4
                        };
                        if (double.Parse(Sensibilidade) != 0)
                        {
                            _slotRepository.Add(_slot);
                            SlotAdded?.Invoke(this, EventArgs.Empty);
                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                            MostrarNotificacao("Sensibilidade Salva!");
                        }
                    }
                    else
                    {
                        if (!fileFound)
                            MessagemErro = "Arquivo não encontrado!";
                        SensManual = true;
                    }
                    break;

                case "Valorant":
                    SensManual = true;
                    if (SensManual == true)
                    {
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(Sensibilidade),
                            Imagem = "../resource/valorant-logo.png",
                            Nome = "Valorant",
                            IdUser = CurrentUser.Id,
                            IdJogo = 6
                        };
                        if (double.Parse(Sensibilidade) != 0)
                        {
                            _slotRepository.Add(_slot);
                            SlotAdded?.Invoke(this, EventArgs.Empty);
                            Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.DataContext == this)?.Close();
                            MostrarNotificacao("Sensibilidade Salva!");
                        }
                    }
                    break;
            }
        }

        private bool CanExecuteSalvarSensCommand(object obj)
        {
            return _jogoSelecionado != null;
        }

        private string ProcurarCaminho(string filename, string caminho)
        {
            try
            {
                string[] arquivo = Directory.GetFiles(caminho, filename);
                if (arquivo.Length > 0)
                {
                    return arquivo[0];
                }

                string[] subPastas = Directory.GetDirectories(caminho);
                foreach (string subPasta in subPastas)
                {
                    string caminhoArquivo = ProcurarCaminho(filename, subPasta);
                    if (caminhoArquivo != null)
                    {
                        return caminhoArquivo;
                    }
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
