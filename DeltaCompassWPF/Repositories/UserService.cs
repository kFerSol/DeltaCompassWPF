using DeltaCompassWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Repositories
{
    public class UserService : INotifyPropertyChanged
    {
        private static UserService _instance;
        private Usuario _currentUser;

        public event Action<Usuario> UserChanged;
        public event Action<Usuario> UserDetailsChanged;

        private UserService() { }
        
        public static UserService Instance => _instance ?? (_instance = new UserService());

        public Usuario CurrentUser
        {
            get => _currentUser;
            set 
            { 
                _currentUser = value;
                UserChanged?.Invoke(value);
                UserDetailsChanged?.Invoke(value);
                OnPropertyChanged(nameof(IsLoggedIn));  
            }
        }

        public bool IsLoggedIn => _currentUser != null;

        public void Logout()
        {
            CurrentUser = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
