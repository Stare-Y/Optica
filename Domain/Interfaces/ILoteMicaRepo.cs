using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILoteMicaRepo
    {
        Task<bool> DescontarCantidadMica(int micaId, int cantidad);
        Task AgregarLoteMica(Lote lote, Dictionary<Mica, int> micas);
    }
}

