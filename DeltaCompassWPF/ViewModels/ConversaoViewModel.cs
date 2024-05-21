using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DeltaCompassWPF.ViewModels
{
    public class ConversaoViewModel : ViewModelBase
    {
        public ObservableCollection<Jogo> Jogos { get; set; }

        private Jogo _jogoSelecionado1;
        private Jogo _jogoSelecionado2;
        private double _sensibilidade;
        private int _dpiMouse;
        private ObservableCollection<SlotConfiguracao> _sensibilidades;
        private SlotConfiguracao _sensibilidadeAtual;

        public SlotConfiguracao SensibilidadeAtual
        {
            get { return _sensibilidadeAtual; }
            set
            {
                _sensibilidadeAtual = value;
                OnPropertyChanged(nameof(SensibilidadeAtual));
            }
        }
        public ObservableCollection<SlotConfiguracao> Sensibilidades
        {
            get { return _sensibilidades; }
            set
            {
                _sensibilidades = value;
                OnPropertyChanged(nameof(Sensibilidades));
            }
        }
        public Jogo JogoSelecionado1
        {
            get { return _jogoSelecionado1; }
            set
            {
                _jogoSelecionado1 = value;
                OnPropertyChanged(nameof(JogoSelecionado1));
                Console.WriteLine("JogoSelecionado atualizado: " + (value?.Nome ?? "null"));
                AtualizarSensibilidadeAtual();
            }
        }
        public Jogo JogoSelecionado2
        {
            get { return _jogoSelecionado2; }
            set
            {
                _jogoSelecionado2 = value;
                OnPropertyChanged(nameof(JogoSelecionado2));
            }
        }
        public double Sensibilidade
        {
            get => _sensibilidade;
            set
            {
                _sensibilidade = value;
                OnPropertyChanged(nameof(Sensibilidade));
            }
        }
        public int DPIMouse
        {
            get => _dpiMouse;
            set
            {
                _dpiMouse = value;
                OnPropertyChanged(nameof(DPIMouse));
            }
        }

        private readonly IUserRepository _userRepository;
        private IJogoRepository _jogoRepository;
        private readonly UserService _userService;

        public bool IsLoggedIn => _userService.IsLoggedIn;
        public Usuario CurrentUser => _userService.CurrentUser;

        public ConversaoViewModel()
        {
            _userRepository = new UserRepository();
            _jogoRepository = new JogoRepository();
            _userService = UserService.Instance;
            _userService.UserChanged += OnUserChanged;
            Jogos = new ObservableCollection<Jogo>();
            Sensibilidades = new ObservableCollection<SlotConfiguracao>();
            CarregarJogos();
            CarregarSensibilidades();
        }

        private void OnUserChanged(Usuario user)
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(CurrentUser));
            CarregarSensibilidades();
        }

        private async void CarregarSensibilidades()
        {
            if (!IsLoggedIn) return;

            try
            {
                var sensibilidades = await Task.Run(() => _userRepository.GetSensibilidadeByUserId(CurrentUser.Id));
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Sensibilidades.Clear();
                    foreach(var sensibilidade in sensibilidades)
                    {
                        Sensibilidades.Add(sensibilidade);
                    }
                    AtualizarSensibilidadeAtual();
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AtualizarSensibilidadeAtual()
        {
            if(JogoSelecionado1 != null && Sensibilidades != null)
            {
                SensibilidadeAtual = Sensibilidades.FirstOrDefault(s => s.JogoId == JogoSelecionado1.Id);
            }
            else
            {
                SensibilidadeAtual = null;
            }
        }

        private async void CarregarJogos()
        {
            var jogos = await Task.Run(() => _jogoRepository.GetJogo());
            Application.Current.Dispatcher.Invoke(() =>
            {
                Jogos.Clear();
                foreach (var jogo in jogos)
                {
                    Jogos.Add(jogo);
                }
            });
        }
    }
}
