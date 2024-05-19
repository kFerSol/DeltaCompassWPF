using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class ConfigurarPerfilViewModel : ViewModelBase
    {
        private Usuario _usuario;
        private string _email;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public Usuario Usuario
        {
            get { return _usuario; }
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        private IUserRepository _userRepository;

        public ICommand SalvarConfigCommand { get; }

        public ConfigurarPerfilViewModel()
        {
            SalvarConfigCommand = new RelayCommand(ExecuteSalvarConfigCommand, CanExecuteSalvarConfigCommand);
            _userRepository = new UserRepository();
            Usuario = new Usuario();
            CarregarInformacoes();
        }

        private bool CanExecuteSalvarConfigCommand(object obj)
        {
            return Thread.CurrentPrincipal.Identity.IsAuthenticated;
        }

        private void ExecuteSalvarConfigCommand(object obj)
        {
            _userRepository.Edit(Usuario);
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                string username = Thread.CurrentPrincipal.Identity.Name;
                Usuario usuario = _userRepository.GetInformacoesAutenticadas(username);
                Email = usuario.Email;
            }
        }

        private void CarregarInformacoes()
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                string username = Thread.CurrentPrincipal.Identity.Name;
                Usuario usuario = _userRepository.GetInformacoesAutenticadas(username);
                Email = usuario.Email;
            }
        }
    }
}
