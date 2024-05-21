using DeltaCompassWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeltaCompassWPF.Models
{
    public class SlotConfiguracao : ViewModelBase
    {
        private int _jogoId;
        private string _nomeJogo;
        private string _imagemJogo;
        private double _sensibilidade;
        private ICommand _configurarCommand;

        public string NomeJogo
        {
            get => _nomeJogo;
            set 
            {
                _nomeJogo = value;
                OnPropertyChanged(NomeJogo);
            }
        }

        public string ImagemJogo
        {
            get => _imagemJogo;
            set
            {
                _imagemJogo = value;
                OnPropertyChanged(nameof(ImagemJogo));
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

        public ICommand ConfigurarCommand
        {
            get => _configurarCommand;
            set
            {
                _configurarCommand = value;
                OnPropertyChanged(nameof(ConfigurarCommand));
            }
        }

        public int JogoId
        {
            get => _jogoId;
            set
            { 
                _jogoId = value; 
            }
        }
    }
}
