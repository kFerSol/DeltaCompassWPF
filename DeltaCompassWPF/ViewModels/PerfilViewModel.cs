using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class PerfilViewModel : ViewModelBase
    {
        private SlotConfiguracao _slotConfiguracao;
        private Usuario _currentUser;
        private ObservableCollection<SlotConfiguracao> _slots;
        
        public SlotConfiguracao SlotConfiguracao
        {
            get { return _slotConfiguracao; }
            set
            {
                _slotConfiguracao = value;
                OnPropertyChanged(nameof(SlotConfiguracao));
            }
        }

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

        public ICommand SalvarSlotCommand { get; }

        public PerfilViewModel()
        {
            UserService.Instance.UserChanged += UpdateCurrentUser;
            UserService.Instance.UserDetailsChanged += UpdateCurrentUser;
            UpdateCurrentUser(UserService.Instance.CurrentUser);
            SalvarSlotCommand = new RelayCommand(ExecuteSalvarSlotCommand, CanExecuteSalvarSlotCommand);
            _slotRepository = new SlotRepository();

            Slots = new ObservableCollection<SlotConfiguracao>
            {
                new SlotConfiguracao{ NomeJogo = null, ImagemJogo = null, Sensibilidade = 0, 
                    ConfigurarCommand = new RelayCommand(AdicionarNovoSlot) }
            };
        }

        private bool CanExecuteSalvarSlotCommand(object obj)
        {
            return true;
        }

        private void ExecuteSalvarSlotCommand(object obj)
        {
            throw new NotImplementedException();
        }

        public void AdicionarNovoSlot(object obj)
        {
            Slots.Add(new SlotConfiguracao { NomeJogo = "Novo Jogo", ImagemJogo = null, Sensibilidade = 0, 
                ConfigurarCommand = new RelayCommand(AdicionarNovoSlot) });
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
    }
}
