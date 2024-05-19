using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using DeltaCompassWPF.Repositories.Authentication;
using DeltaCompassWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private SecureString _password;
        private string _email;
        private string _errorMessage;

        private IUserRepository _userRepository;

        public string Username
        {
            get => _username;
            set 
            { 
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public SecureString Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RecuperarSenhaCommand { get; }
        public ICommand MostrarSenhaCommand { get; }
        public ICommand LembrarSenhaCommand { get; }

        public Action CloseAction { get; set; }

        public LoginViewModel()
        {
            _userRepository = new UserRepository();
            LoginCommand = new RelayCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecuperarSenhaCommand = new RelayCommand(p => ExecuteRecuperarSenhaCommand("", ""));
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = _userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            if (isValidUser)
            {
                int userId = _userRepository.GetUserId(Username);
                var identity = new CustomIdentity(Username, userId);
                var principal = new CustomPrincipal(identity);
                Thread.CurrentPrincipal = principal;
                CloseAction?.Invoke();
            }
            else
            {
                ErrorMessage = "*Nome de usuário ou senha inválida.";
            }
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteRecuperarSenhaCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
