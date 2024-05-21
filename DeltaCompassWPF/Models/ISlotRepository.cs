using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaCompassWPF.Models
{
    public interface ISlotRepository
    {
        void Add(SlotConfiguracao slot, int idUsuario, int idJogo);
        void Remove(SlotConfiguracao slot, int idUsuario);
        void Edit(int idUsuario);
    }
}
