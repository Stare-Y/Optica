using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILoteRepo
    {
        Task<Lote> GetLote(int id);
        Task<IEnumerable<Lote>> GetAllLotes();
        Task AddLote(Lote lote);
        Task UpdateLote(Lote lote);
        Task DeleteLote(Lote lote);
    }
}

