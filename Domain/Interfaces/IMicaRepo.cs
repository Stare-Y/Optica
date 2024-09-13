using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMicaRepo
    {
        Task<Mica> GetMica(int id);
        Task<IEnumerable<Mica>> GetAllMicas();
        Task AddMica(Mica mica);
        Task UpdateMica(Mica mica);
        Task DeleteMica(Mica mica);// ajlsdfkasjflña
    }
}
