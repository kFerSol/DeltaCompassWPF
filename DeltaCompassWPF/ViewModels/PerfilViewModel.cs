using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using DeltaCompassWPF.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class PerfilViewModel : ViewModelBase
    {
        private Usuario _currentUser;
        private ObservableCollection<SlotConfiguracao> _slots;
        private readonly UserRepository _userRepository;

        public ObservableCollection<SlotConfiguracao> Slots
        {
            get => _slots;
            set
            {
                _slots = value;
                OnPropertyChanged(nameof(Slots));
            }
        }

        private readonly UserService _userService;

        public Usuario CurrentUser => _userService.CurrentUser;
        public bool IsLoggedIn => _userService.IsLoggedIn;

        public ICommand AbrirJanelaSlotCommand { get; }
        public ICommand AbrirSalvarSlotCommand { get; }
        public ICommand LogoutCommand { get; }

        public PerfilViewModel()
        {
            _userService = UserService.Instance;
            UserService.Instance.UserChanged += UpdateCurrentUser;
            UserService.Instance.UserDetailsChanged += UpdateCurrentUser;
            _userRepository = new UserRepository();
            _userService.UserChanged += OnUserChanged;
            LogoutCommand = new RelayCommand<object>(ExecuteLogout);
            AbrirJanelaSlotCommand = new RelayCommand<SlotConfiguracao>(ExecuteAbrirJanelaSlotCommand);
            AbrirSalvarSlotCommand = new RelayCommand<object>(ExecuteAbrirSalvarSlotCommand, CanExecuteAbrirSalvarSlotCommand);
            CarregarSlot();
            UpdateCurrentUser(UserService.Instance.CurrentUser);
        }

        private void ExecuteAbrirSalvarSlotCommand(object obj)
        {
            SlotViewModel slotViewModel = new SlotViewModel();
            slotViewModel.SlotAdded += OnSlotUpdated;

            JanelaSalvarSensibilidade janelaSalvarSensibilidade = new JanelaSalvarSensibilidade
            {
                DataContext = slotViewModel
            };
            Window mainWindow = Application.Current.MainWindow;

            janelaSalvarSensibilidade.WindowStartupLocation = WindowStartupLocation.Manual;
            janelaSalvarSensibilidade.Left = mainWindow.Left + (mainWindow.Width - janelaSalvarSensibilidade.Width) / 2;
            janelaSalvarSensibilidade.Top = mainWindow.Top + (mainWindow.Height - janelaSalvarSensibilidade.Height) / 2;
            janelaSalvarSensibilidade.Show();
        }

        private bool CanExecuteAbrirSalvarSlotCommand(object arg)
        {
            return IsLoggedIn;
        }

        private void ExecuteAbrirJanelaSlotCommand(SlotConfiguracao slot)
        {
            SlotViewModel slotViewModel = new SlotViewModel(slot);

            slotViewModel.SlotDeleted += OnSlotDeleted;

            JanelaAbrirSlot janelaAbrirSlot = new JanelaAbrirSlot
            {
                DataContext = slotViewModel
            };
            Window mainWindow = Application.Current.MainWindow;

            janelaAbrirSlot.WindowStartupLocation = WindowStartupLocation.Manual;
            janelaAbrirSlot.Left = mainWindow.Left + (mainWindow.Width - janelaAbrirSlot.Width) / 2;
            janelaAbrirSlot.Top = mainWindow.Top + (mainWindow.Height - janelaAbrirSlot.Height) / 2;
            janelaAbrirSlot.Show();
        }

        private void OnSlotUpdated(object sender, EventArgs e)
        {
            CarregarSlot();
        }

        private void OnSlotDeleted(object sender, EventArgs e)
        {
            CarregarSlot();
        }

        private void CarregarSlot()
        {
            if (IsLoggedIn)
            {
                int userId = _userService.CurrentUser.Id;
                var sensibilidades = _userRepository.GetSensibilidadeByUserId(userId);

                if (sensibilidades != null)
                {
                    Slots = new ObservableCollection<SlotConfiguracao>(sensibilidades);
                }
                else
                {
                    Slots = new ObservableCollection<SlotConfiguracao>();
                }
            }
            else
            {
                Slots = new ObservableCollection<SlotConfiguracao>();
            }
        }

        private void ExecuteLogout(object obj)
        {
            UserService.Instance.Logout();
            ResetUserProfile();
            CarregarSlot();
        }

        private void ResetUserProfile()
        {
            CarregarSlot();
            _currentUser = new Usuario
            {
                Nome = "Usuário não logado",
                Email = "",
                ApelidoPerfil = "",
                Biografia = "",
                ModeloMonitor = "",
                ModeloMouse = "",
                ResolucaoX = null,
                ResolucaoY = null,
                DpiMouse = null,
                ImagemPerfil = null,
                ImagemFundo = null
            };

            OnPropertyChanged(nameof(Nome));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Apelido));
            OnPropertyChanged(nameof(Biografia));
            OnPropertyChanged(nameof(Mouse));
            OnPropertyChanged(nameof(Monitor));
            OnPropertyChanged(nameof(ResolucaoX));
            OnPropertyChanged(nameof(ResolucaoY));
            OnPropertyChanged(nameof(Dpi));
            OnPropertyChanged(nameof(ImagemPerfil));
            OnPropertyChanged(nameof(ImagemFundo));
            OnPropertyChanged(nameof(CurrentUser));
            OnPropertyChanged(nameof(IsLoggedIn));
        }

        private void OnUserChanged(Usuario usuario)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(CurrentUser));
        }

        private void UpdateCurrentUser(Usuario user)
        {
            CarregarSlot();
            _currentUser = user ?? new Usuario
            {
                Nome = "Usuário não logado",
                Email = "",
                ApelidoPerfil = "",
                Biografia = "",
                ModeloMonitor = "",
                ModeloMouse = "",
                ResolucaoX = null,
                ResolucaoY = null,
                DpiMouse = null,
                ImagemPerfil = null,
                ImagemFundo = null
            };

            OnPropertyChanged(nameof(Nome));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Apelido));
            OnPropertyChanged(nameof(Biografia));
            OnPropertyChanged(nameof(Mouse));
            OnPropertyChanged(nameof(Monitor));
            OnPropertyChanged(nameof(ResolucaoX));
            OnPropertyChanged(nameof(ResolucaoY));
            OnPropertyChanged(nameof(Dpi));
            OnPropertyChanged(nameof(ImagemPerfil));
            OnPropertyChanged(nameof(ImagemFundo));
        }

        public string Nome => _currentUser?.Nome;
        public string Email => _currentUser?.Email;
        public string Apelido
        {
            get
            {
                if (string.IsNullOrEmpty(_currentUser?.ApelidoPerfil))
                {
                    return _currentUser.Nome;
                }
                return _currentUser?.ApelidoPerfil;
            }
        }
        public string Biografia => _currentUser.Biografia;
        public string Mouse => _currentUser.ModeloMouse;
        public string Monitor => _currentUser.ModeloMonitor;
        public int? ResolucaoX => _currentUser.ResolucaoX;
        public int? ResolucaoY => _currentUser.ResolucaoY;
        public int? Dpi => _currentUser.DpiMouse;
        public byte[] ImagemPerfil => _currentUser.ImagemPerfil;
        public byte[] ImagemFundo => _currentUser.ImagemFundo;

        public string NomeJogo { get; set; }
        public string ImagemJogo { get; set; }
        public string Sensibilidade { get; set; }
    }
}
