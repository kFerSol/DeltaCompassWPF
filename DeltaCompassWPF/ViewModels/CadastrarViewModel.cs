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
using DeltaCompassWPF.Helpers;
using System.Windows;
using DeltaCompassWPF.Views.UserControls;
using System.Windows.Media.Animation;

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
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Email) &&
                   Password?.Length > 3 &&
                   PasswordConfirm?.Length > 3;
        }

        private void ExecuteCadastroCommand(object obj)
        {
            try
            {
                if (!Password.SecureStringEqual(PasswordConfirm))
                {
                    ErrorMessage = "*Senhas digitadas são diferentes.";
                    return;
                }

                var usuario = new Usuario
                {
                    Nome = Username,
                    Email = Email
                };
                var senhaHash = PasswordHelper.HashPassword(Password);

                userRepository.Add(usuario, senhaHash);
                CloseAction?.Invoke();

                MostrarNotificacao("Conta Cadastrada!");
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }
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
    }
}
