using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace DeltaCompassWPF.ViewModels
{
    public class ConfigurarPerfilViewModel : ViewModelBase
    {
        private Usuario _usuario;
        private Usuario _currentUser;

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
        public ICommand TrocarImagemPerfilCommand { get; }
        public ICommand TrocarImagemFundoCommand { get; }

        public ConfigurarPerfilViewModel()
        {
            SalvarConfigCommand = new RelayCommand(ExecuteSalvarConfigCommand, CanExecuteSalvarConfigCommand);
            TrocarImagemPerfilCommand = new RelayCommand(ExecuteTrocarImagemPerfilCommand, CanExecuteTrocarImagemPerfilCommand);
            TrocarImagemFundoCommand = new RelayCommand(ExecuteTrocarImagemFundoCommand, CanExecuteTrocarImagemFundoCommand);
            _userRepository = new UserRepository();
            Usuario = new Usuario();

            if(LoginViewModel.CurrentUser != null)
                _currentUser = LoginViewModel.CurrentUser;
            else
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
            }
        }

        private bool CanExecuteTrocarImagemFundoCommand(object obj)
        {
            return true;
        }

        private void ExecuteTrocarImagemFundoCommand(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            dialog.FilterIndex = 1;
            if(dialog.ShowDialog() == true)
            {
                byte[] imageBytes = File.ReadAllBytes(dialog.FileName);
                _usuario.ImagemFundo = imageBytes;
            }
        }

        private bool CanExecuteTrocarImagemPerfilCommand(object obj)
        {
            return true;
        }

        private void ExecuteTrocarImagemPerfilCommand(object obj)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            dialog.FilterIndex = 1;
            if (dialog.ShowDialog() == true)
            {
                byte[] imageBytes = File.ReadAllBytes(dialog.FileName);
                _usuario.ImagemPerfil = imageBytes;
            }
        }

        private bool CanExecuteSalvarConfigCommand(object obj)
        {
            return Thread.CurrentPrincipal.Identity.IsAuthenticated;
        }

        private void ExecuteSalvarConfigCommand(object obj)
        {
            _userRepository.Edit(Usuario);
        }
    }
}
