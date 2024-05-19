using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using DeltaCompassWPF.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class CadastrarViewModel : ViewModelBase
    {
        private string _username;
        private string _email;
        private SecureString _password;
        private SecureString _passwordConfirm;
        private string _errorMessage;

        private IUserRepository userRepository;

        public string Username 
        { 
            get => _username;
            set 
            { 
                _username = value;
                OnPropertyChanged(nameof(Username));
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
        public SecureString Password 
        { 
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public SecureString PasswordConfirm 
        { 
            get => _passwordConfirm;
            set
            {
                _passwordConfirm = value;
                OnPropertyChanged(nameof(PasswordConfirm));
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

        public ICommand CadastrarCommand { get; }
        public Action CloseAction { get; set; }

        public CadastrarViewModel()
        {
            userRepository = new UserRepository();
            CadastrarCommand = new RelayCommand(ExecuteCadastroCommand, CanExecuteCadastroCommand);
        }

        private bool CanExecuteCadastroCommand(object obj)
        {
            return true;
        }

        private void ExecuteCadastroCommand(object obj)
        {
            try
            {
                if (Password.Equals(PasswordConfirm))
                {
                    ErrorMessage = "*Senhas digitadas são diferentes.";
                    return;
                }

                var usuario = new Usuario
                {
                    Nome = Username,
                    Senha = Password.ConvertToSecureString(),
                    Email = Email
                };

                userRepository.Add(usuario);
                CloseAction?.Invoke();
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
