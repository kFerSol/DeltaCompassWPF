using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DeltaCompassWPF.Models
{
    public class Usuario : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propriedade)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propriedade));
        }

        private int _id;
        private string _nome;
        private string _email;
        private string _apelidoPerfil;
        private string _telefone;
        private string _senha;
        private string _biografia;
        private int _dpiMouse;
        private string _modeloMonitor;
        private string _modeloMouse;
        private int _resolucaoY;
        private int _resolucaoX;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        public string Nome
        {
            get { return _nome; }
            set
            {
                if (_nome != value)
                {
                    _nome = value;
                    OnPropertyChanged("Nome");
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Email");
                }
            }
        }

        public string ApelidoPerfil
        {
            get { return _apelidoPerfil; }
            set
            {
                if (_apelidoPerfil != value)
                {
                    _apelidoPerfil = value;
                    OnPropertyChanged("ApelidoPerfil");
                }
            }
        }

        public string Telefone
        {
            get { return _telefone; }
            set
            {
                if (_telefone != value)
                {
                    _telefone = value;
                    OnPropertyChanged("Telefone");
                }
            }
        }

        public string Senha
        {
            get { return _senha; }
            set
            {
                if (_senha != value)
                {
                    _senha = value;
                    OnPropertyChanged("Senha");
                }
            }
        }

        public string Biografia
        {
            get { return _biografia; }
            set
            {
                if (_biografia != value)
                {
                    _biografia = value;
                    OnPropertyChanged("Biografia");
                }
            }
        }

        public int DpiMouse
        {
            get { return _dpiMouse; }
            set
            {
                if (_dpiMouse != value)
                {
                    _dpiMouse = value;
                    OnPropertyChanged("DpiMouse");
                }
            }
        }

        public string ModeloMonitor
        {
            get { return _modeloMonitor; }
            set
            {
                if (_modeloMonitor != value)
                {
                    _modeloMonitor = value;
                    OnPropertyChanged("ModeloMonitor");
                }
            }
        }

        public string ModeloMouse
        {
            get { return _modeloMouse; }
            set
            {
                if (_modeloMouse != value)
                {
                    _modeloMouse = value;
                    OnPropertyChanged("ModeloMouse");
                }
            }
        }

        public int ResolucaoY
        {
            get { return _resolucaoY; }
            set
            {
                if (_resolucaoY != value)
                {
                    _resolucaoY = value;
                    OnPropertyChanged("ResolucaoY");
                }
            }
        }

        public int ResolucaoX
        {
            get { return _resolucaoX; }
            set
            {
                if (_resolucaoX != value)
                {
                    _resolucaoX = value;
                    OnPropertyChanged("ResolucaoX");
                }
            }
        }
    }
}
