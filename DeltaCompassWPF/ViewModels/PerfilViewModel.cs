using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class PerfilViewModel : ViewModelBase
    {
        private Usuario _currentUser;
        private ObservableCollection<SlotConfiguracao> _slots;

        private ISlotRepository _slotRepository;

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

        public ICommand AbrirSlotCommand { get; }
        public ICommand LogoutCommand { get; }

        public PerfilViewModel()
        {
            _userService = UserService.Instance;
            UserService.Instance.UserChanged += UpdateCurrentUser;
            UserService.Instance.UserDetailsChanged += UpdateCurrentUser;
            _userService.UserChanged += OnUserChanged;

            //AbrirSlotCommand = new RelayCommand(ExecuteAbrirSlotCommand);
            LogoutCommand = new RelayCommand(ExecuteLogout);

            _slotRepository = new SlotRepository();

            Slots = new ObservableCollection<SlotConfiguracao>
            {
                new SlotConfiguracao{ Nome = null, Imagem = null, Sensibilidade = 0 }
            };

            UpdateCurrentUser(UserService.Instance.CurrentUser);
        }

        private void ExecuteLogout(object obj)
        {
            UserService.Instance.Logout();
            ResetUserProfile();
        }

        private void ResetUserProfile()
        {
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

        private void ExecuteAbrirSlotCommand(object obj)
        {

            //_slotRepository.Add(_slots, _currentUser.Id, 3);
        }

        public void AdicionarNovoSlot(object obj)
        {
            Slots.Add(new SlotConfiguracao { Nome = "Novo Jogo", Imagem = null, Sensibilidade = 0 });
        }

        private void UpdateCurrentUser(Usuario user)
        {
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
