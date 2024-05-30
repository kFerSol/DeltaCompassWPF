using DeltaCompassWPF.Commands;
using DeltaCompassWPF.Models;
using DeltaCompassWPF.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
            switch (_jogoSelecionado.Nome)
            {
                case "Counter Strike 2":
                    string filename = @"\sens.cfg";
                    string config = "sensitivity";

                    DriveInfo[] discos = DriveInfo.GetDrives();
                    foreach (DriveInfo disco in discos)
                    {
                        if (disco.IsReady && disco.DriveType == DriveType.Fixed)
                        {
                            string path = Path.Combine(disco.RootDirectory.FullName, "Program Files (x86)", "Steam", "steamapps", "common", "Counter-Strike Global Offensive", "game", "csgo", "cfg");
                            if (Directory.Exists(path))
                            {
                                ProcurarSens(filename, path);
                            }
                        }
                    }
                    break;
            }
        }

        public static string ProcurarSens(string caminhoArquivo, string configDesejada)
        {
            string[] linhas = File.ReadAllLines(caminhoArquivo);
            foreach (string linha in linhas)
            {
                if (linha.Contains(configDesejada))
                {
                    string[] palavras = linha.Split(' ', (char)StringSplitOptions.RemoveEmptyEntries); 
                    if (palavras.Length > 1)
                    {
                        string sens = palavras[1];
                        return sens;
                    }
                }
            }
            return null;
        }
    }
}
