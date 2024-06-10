using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class SlotViewModel : ViewModelBase
    {
        private Jogo _jogoSelecionado;
        private SlotConfiguracao _slot;
        private ObservableCollection<SlotConfiguracao> _sensibilidades;
        private double _sensibilidade;
        private bool _sensManual = false;
        private bool _messagemErro;

        private IJogoRepository _jogoRepository;
        private ISlotRepository _slotRepository;
        private readonly UserService _userService;
        private readonly UserRepository _userRepository;

        public ObservableCollection<Jogo> Jogos { get; set; }
        public Usuario CurrentUser => _userService.CurrentUser;

        public bool MessagemErro
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

        public double Sensibilidade
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
                ((RelayCommand)SalvarSensibilidadeCommand).RaiseCanExecuteChanged();
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

        public SlotViewModel()
        {
            _userService = UserService.Instance;
            _userService.UserChanged += OnUserChanged;
            _userRepository = new UserRepository();
            _jogoRepository = new JogoRepository();
            _slotRepository = new SlotRepository();
            Jogos = new ObservableCollection<Jogo>();
            Sensibilidades = new ObservableCollection<SlotConfiguracao>();
            SalvarSensibilidadeCommand = new RelayCommand(ExecuteSalvarSensCommand, CanExecuteSalvarSensCommand);
            VoltarCommand = new RelayCommand(ExecuteVoltarCommand);
            SelecionarArquivoCommand = new RelayCommand(ExecuteSelecionarArquivoCommand);
            Slot = new SlotConfiguracao()
            {
                Nome = null,
                Imagem = "../resource/delta-logo.png"
            };
            CarregarJogos();
            CarregarSensibilidades();
        }

        private void ExecuteSelecionarArquivoCommand(object obj)
        {
            if(_jogoSelecionado != null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Config Files (*.cfg)|*.cfg|All Files (*.*)|*.*"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    if (File.Exists(selectedFilePath))
                    {
                        string config = GetConfigKey(_jogoSelecionado.Nome);
                        _slot = new SlotConfiguracao
                        {
                            Sensibilidade = double.Parse(ProcurarSens(selectedFilePath, config), CultureInfo.InvariantCulture),
                            Nome = _jogoSelecionado.Nome,
                            IdUser = CurrentUser.Id,
                            IdJogo = _jogoSelecionado.Id
                        };

                        _slotRepository.Add(_slot);
                        CloseAction?.Invoke();
                    }
                }
            }
        }

        private string GetConfigKey(string gameName)
        {
            switch (gameName)
            {
                case "Counter Strike 2":
                    return "sensitivity";
                case "Apex Legends":
                    return "mouse_sensitivity";
                case "Call of Duty MWIII":
                    return "";
                case "Portal 2":
                    return "";
                case "Rainbow Six Siege":
                    return "";
                case "Valorant":
                    return "";
                default:
                    throw new ArgumentException("Jogo não suportado");
            }
        }

        public void AtivarSensManual()
        {
            SensManual = true;
            MessagemErro = false;
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
            switch (_jogoSelecionado.Nome)
            {
                case "Counter Strike 2":
                    //filename = @"\sens.cfg";
                    config = "sensitivity";

                    DriveInfo[] discos = DriveInfo.GetDrives();
                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string path = Path.Combine(disco.RootDirectory.FullName, "Program Files (x86)", "Steam", "steamapps", "common", "Counter-Strike Global Offensive", "game", "csgo", "cfg");
                            if (Directory.Exists(path))
                            {
                                _slot.Sensibilidade = double.Parse(ProcurarSens(path, config));
                            }
                        }
                    }
                    break;
                case "Apex Legends":
                    filename = "\\Saved Games\\Respawn\\Apex\\Local\\settings.cfg";
                    config = "mouse_sensitivity";

                    discos = DriveInfo.GetDrives();
                    bool fileFound = false;
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
                                        _slot = new SlotConfiguracao
                                        {
                                            Sensibilidade = double.Parse(ProcurarSens(userPath, config), CultureInfo.InvariantCulture),
                                            Imagem = "../resource/apex-logo.png",
                                            Nome = "Apex Legends",
                                            IdUser = CurrentUser.Id,
                                            IdJogo = 1
                                        };
                                        _slotRepository.Add(_slot);
                                        CloseAction?.Invoke();
                                        fileFound = true;
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
                            Sensibilidade = Sensibilidade,
                            Imagem = "../resource/apex-logo.png",
                            Nome = "Apex Legends",
                            IdUser = CurrentUser.Id,
                            IdJogo = 1
                        };
                        if (Sensibilidade != 0)
                        {
                            _slotRepository.Add(_slot);
                            CloseAction?.Invoke();
                        }
                    }
                    else
                    {
                        if (!fileFound)
                            MessagemErro = true;
                        SensManual = true;
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
                    string[] palavras = linha.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries); 
                    if (palavras.Length > 1)
                    {
                        string sens = palavras[1];
                        return sens;
                    }
                }
            }
            return null;
        }
    }
}
