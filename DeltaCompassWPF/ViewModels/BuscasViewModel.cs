using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.ViewModels
{
    public class BuscasViewModel : ViewModelBase
    {
        private ObservableCollection<Usuario> _usuarios;
        private string _procura;

        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set
            {
                _usuarios = value;
                OnPropertyChanged(nameof(Usuarios));
            }
        }

        public string Procura
        {
            get => _procura;
            set
            {
                _procura = value;
                OnPropertyChanged(nameof(Procura));
            }
        }

        public IUserRepository _userRepository;

        public BuscasViewModel()
        {
            Usuarios = new ObservableCollection<Usuario>();
            _userRepository = new UserRepository();
            CarregarUsuarios();
        }

        public void CarregarUsuarios()
        {
            try
            {
                var usuarios = _userRepository.GetAllUsers();
                Usuarios = new ObservableCollection<Usuario>(usuarios);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }
    }
}
