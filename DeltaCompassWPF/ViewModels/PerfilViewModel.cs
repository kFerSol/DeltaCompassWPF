using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Database;
using MySql.Data.MySqlClient;
using System.Xml.Linq;
using Mysqlx.Crud;
using DeltaCompassWPF.Repositories;

namespace DeltaCompassWPF.ViewModels
{
    public class PerfilViewModel : ViewModelBase
    {
        private Usuario _currentUser;

        public PerfilViewModel()
        {
            UserService.Instance.UserChanged += UpdateCurrentUser;
            UserService.Instance.UserDetailsChanged += UpdateCurrentUser;
            UpdateCurrentUser(UserService.Instance.CurrentUser);
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
