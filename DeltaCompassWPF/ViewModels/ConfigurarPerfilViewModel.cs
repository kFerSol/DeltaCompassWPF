using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Helpers;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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

        public string Email => _currentUser.Email;
        public string Telefone => _currentUser.Telefone;

        private IUserRepository _userRepository;
        private ImageHelper _imageHelper;

        public ICommand SalvarConfigCommand { get; }
        public ICommand SalvarConfigGeralCommand { get; }
        public ICommand TrocarImagemPerfilCommand { get; }
        public ICommand TrocarImagemFundoCommand { get; }

        public ConfigurarPerfilViewModel()
        {
            SalvarConfigCommand = new RelayCommand(ExecuteSalvarConfigCommand, CanExecuteSalvarConfigCommand);
            SalvarConfigGeralCommand = new RelayCommand(ExecuteSalvarConfigGeralCommand, CanExecuteSalvarConfigGeralCommand);
            TrocarImagemPerfilCommand = new RelayCommand(ExecuteTrocarImagemPerfilCommand, CanExecuteTrocarImagemPerfilCommand);
            TrocarImagemFundoCommand = new RelayCommand(ExecuteTrocarImagemFundoCommand, CanExecuteTrocarImagemFundoCommand);
            _userRepository = new UserRepository();
            _imageHelper = new ImageHelper();
            Usuario = new Usuario();

            UserService.Instance.UserChanged += UpdateCurrentUser;
            UserService.Instance.UserDetailsChanged += UpdateCurrentUser;
            UpdateCurrentUser(UserService.Instance.CurrentUser);
        }

        private bool CanExecuteSalvarConfigGeralCommand(object obj)
        {
            return Thread.CurrentPrincipal.Identity.IsAuthenticated;
        }

        private void ExecuteSalvarConfigGeralCommand(object obj)
        {
            _userRepository.Edit(Usuario);
        }

        private void UpdateCurrentUser(Usuario currentUser)
        {
            _currentUser = currentUser ?? new Usuario
            {
                Email = "",
                Telefone = "+55 (00) 00000-0000"
            };

            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Telefone));
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
                var image = new Bitmap(dialog.FileName);
                var resizedImage = _imageHelper.ResizeImage(image, 700, 300);
                _usuario.ImagemFundo = _imageHelper.ConvertImageToByte(resizedImage);
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
                var image = new Bitmap(dialog.FileName);
                var resizedImage = _imageHelper.ResizeImage(image, 300, 300);
                _usuario.ImagemPerfil = _imageHelper.ConvertImageToByte(resizedImage);
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
