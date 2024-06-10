using DeltaCompassWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Models
{
    public class Jogo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Imagem { get; set; }
        public double Calculo { get; set; }
    }
}
