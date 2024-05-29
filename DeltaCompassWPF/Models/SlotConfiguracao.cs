using DeltaCompassWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeltaCompassWPF.Models
{
    public class SlotConfiguracao
    {
        public int IdUser { get; set; }
        public int IdJogo { get; set; }
        public string Nome { get; set; }
        public double Sensibilidade { get; set; }
        public string Imagem { get; set; }
    }
}
