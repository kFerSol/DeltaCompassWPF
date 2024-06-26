using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using DeltaCompassWPF.Views.UserControls;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace DeltaCompassWPF.ViewModels
{
    public class SuporteViewModel : ViewModelBase
    {
        private readonly UserService _userService;
        private SuporteModel _suporte;
        private readonly IUserRepository _userRepository;
        private string _titulo;
        private string _comentario;

        public string Titulo
        {
            get { return _titulo; }
            set
            {
                _titulo = value;
                OnPropertyChanged(nameof(Titulo));
            }
        }

        public string Comentario
        {
            get { return _comentario; }
            set
            {
                _comentario = value;
                OnPropertyChanged(nameof(Comentario));
            }
        }

        public Usuario CurrentUser => _userService.CurrentUser;

        public SuporteModel Suporte
        {
            get { return _suporte; }
            set
            {
                _suporte = value;
                OnPropertyChanged(nameof(Suporte));
            }
        }

        public ICommand EnviarCommand { get; }

        public bool IsLoggedIn => _userService.IsLoggedIn;

        public SuporteViewModel() 
        {
            _userService = UserService.Instance;
            _userService.UserChanged += OnUserChanged;
            _userRepository = new UserRepository();
            Suporte = new SuporteModel();
            EnviarCommand = new RelayCommand<object>(ExecuteEnviarCommand, CanExecuteEnviarCommand);
        }

        private void OnUserChanged(Usuario usuario)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(CurrentUser));
        }

        private bool CanExecuteEnviarCommand(object arg)
        {
            return IsLoggedIn;
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

        private void ExecuteEnviarCommand(object obj)
        {
            if (Titulo != null && Comentario != null)
            {
                Suporte.Titulo = Titulo;
                Suporte.Comentario = Comentario;
                _userRepository.SendSuport(CurrentUser.Id, Suporte);
                MostrarNotificacao("Suporte Enviado");
            }
            Titulo = "";
            Comentario = "";
        }
    }
}
