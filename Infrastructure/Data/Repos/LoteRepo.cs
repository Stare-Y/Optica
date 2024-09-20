using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class LoteRepo : ILoteRepo
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Lote> _lotes;
        private readonly ILoteMicaRepo _loteMicaRepo;
        public LoteRepo(DbContext dbContext, ILoteMicaRepo loteMicaRepo)
        {
            _dbContext = dbContext;
            _lotes = dbContext.Set<Lote>();
            _loteMicaRepo = loteMicaRepo;
        }

        public async Task<Lote?> GetLote(int idLote)
        {
            return await _lotes.FirstOrDefaultAsync(m => m.Id == idLote);
        }
        public async Task<IEnumerable<Lote>> GetAllLotes()
        {
            return await _lotes.ToListAsync();
        }

        public async Task AddLote(Lote lote, IEnumerable<LoteMica> lotesMicas)
        {
            try
            {
                await _loteMicaRepo.AgregarLoteMica(lotesMicas);
                await _lotes.AddAsync(lote);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al añadir el lote: {e}");
            }
        }

        public async Task DeleteLote(int idLote)
        {
            try
            {
                var loteEliminar = await _lotes.FirstOrDefaultAsync(l => l.Id == idLote);
                if (loteEliminar == null)
                {
                    throw new NotFoundException("El lote no existe en el repositorio");
                }
                else
                {
                    await _loteMicaRepo.EliminarLoteMicaByLote(idLote);
                    _lotes.Remove(loteEliminar);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al eliminar el lote: {e}");
            }
        }
    }
}

