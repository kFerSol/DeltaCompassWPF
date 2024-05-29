using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeltaCompassWPF.ViewModels
{
    public class SlotViewModel : ViewModelBase
    {
        private Jogo _jogoSelecionado;
        private SlotConfiguracao _slot;

        private ISlotRepository _slotRepository;

        public ObservableCollection<Jogo> Jogos { get; set; }

        public Jogo JogoSelecionado
        {
            get => _jogoSelecionado;
            set
            {
                _jogoSelecionado = value;
                OnPropertyChanged(nameof(JogoSelecionado));
            }
        }

        public SlotConfiguracao Slot
        {
            get => _slot;
            set
            {
                _slot = value;
                OnPropertyChanged(nameof(Slot));
            }
        }

        public ICommand SalvarSensibilidadeCommand { get; }

        public SlotViewModel()
        {
            _slotRepository = new SlotRepository();
            SalvarSensibilidadeCommand = new RelayCommand(ExecuteSalvarSensCommand);
        }

        private void ExecuteSalvarSensCommand(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
