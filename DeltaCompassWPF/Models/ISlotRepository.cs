using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Models
{
    public interface ISlotRepository
    {
        void Add(ObservableCollection<SlotConfiguracao> slot, int idUsuario, int idJogo);
        void Remove(SlotConfiguracao slot, int idUsuario);
        void Edit(int idUsuario);
    }
}
