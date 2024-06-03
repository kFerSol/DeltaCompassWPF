using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class SlotViewModel : ViewModelBase
    {
        private Jogo _jogoSelecionado;
        private SlotConfiguracao _slot;
        private ObservableCollection<SlotConfiguracao> _sensibilidades;
        private Usuario _currentUser;

        private IJogoRepository _jogoRepository;
        private ISlotRepository _slotRepository;
        private readonly UserService _userService;
        private readonly UserRepository _userRepository;

        public ObservableCollection<Jogo> Jogos { get; set; }
        public Usuario CurrentUser => _userService.CurrentUser;

        public Jogo JogoSelecionado
        {
            get => _jogoSelecionado;
            set
            {
                _jogoSelecionado = value;
                OnPropertyChanged(nameof(JogoSelecionado));
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

        public bool IsLoggedIn => _userService.IsLoggedIn;
        public ICommand SalvarSensibilidadeCommand { get; }

        public SlotViewModel()
        {
            _userService = UserService.Instance;
            _userService.UserChanged += OnUserChanged;
            _userRepository = new UserRepository();
            _jogoRepository = new JogoRepository();
            _slotRepository = new SlotRepository();
            Jogos = new ObservableCollection<Jogo>();
            Sensibilidades = new ObservableCollection<SlotConfiguracao>();
            SalvarSensibilidadeCommand = new RelayCommand(ExecuteSalvarSensCommand);
            CarregarJogos();
            CarregarSensibilidades();
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
                    filename = "settings.cfg";
                    config = "mouse_sensitivity";

                    discos = DriveInfo.GetDrives();
                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string caminho = Path.Combine(disco.RootDirectory.FullName, "Users");
                            string caminhoArquivo = ProcurarCaminho(filename, caminho);
                            if (caminhoArquivo != null)
                            {
                                _slot = new SlotConfiguracao
                                {
                                    Sensibilidade = double.Parse(ProcurarSens(caminhoArquivo, config), CultureInfo.InvariantCulture),
                                    Imagem = "../resource/apex-logo.png",
                                    Nome = "Apex Legends",
                                    IdUser = CurrentUser.Id,
                                    IdJogo = 1
                                };
                                _slotRepository.Add(_slot);
                            }
                        }
                    }
                    break;
            }
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
