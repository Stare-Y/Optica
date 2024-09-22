using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repos
{
    public class LoteMicaRepo : ILoteMicaRepo
    {
        private readonly DbSet<Lote> _lotes;
        private readonly DbSet<Mica> _micas;
        private readonly DbSet<LoteMica> _loteMicasIntermedia;
        private readonly DbContext _dbContext;

        public LoteMicaRepo(DbContext dbContext)
        {
            _lotes = dbContext.Set<Lote>();
            _micas = dbContext.Set<Mica>();
            _loteMicasIntermedia = dbContext.Set<LoteMica>();
            _dbContext = dbContext;
        }

        public async Task AgregarLoteMica(IEnumerable<LoteMica> lotesMicas)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    await _loteMicasIntermedia.AddRangeAsync(lotesMicas);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<int> GetStock(int idMica)
        {
            //counts all the currentstock of a mica in LoteMica table
            return await _loteMicasIntermedia.Where(lm => lm.IdMica == idMica).SumAsync(lm => lm.Stock);
        }

        public async Task<bool> TakeStock(int idMica, int cantidad)
        {
            try
            {
                if(await GetStock(idMica) < cantidad)
                {
                    return false;
                }
                var loteMicas = await _loteMicasIntermedia.Where(lm => lm.IdMica == idMica).ToListAsync();
                //sort the loteMicas by expiration date
                loteMicas = loteMicas.OrderBy(lm => lm.FechaCaducidad).ToList();

                //now they are sorted, we can take the stock from the soonest to expire
                foreach (var loteMica in loteMicas)
                {
                    if (loteMica.Stock >= cantidad)
                    {
                        loteMica.Stock -= cantidad;
                        break;
                    }
                    else
                    {
                        cantidad -= loteMica.Stock;
                        loteMica.Stock = 0;
                    }
                }

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch(Exception e)
            {
                throw new Exception($"({e.GetType})Error al restar stock: ({e.Message})(Inner: {e.InnerException})");
            }

        }

        public async Task ReturnStock(int idMica, int cantidad)
        {
            try
            {
                var loteMica = await _loteMicasIntermedia
                    .Where(lm => lm.IdMica == idMica)
                    .OrderBy(lm => lm.FechaCaducidad) // Ordenar por fecha de caducidad más cercana
                    .FirstOrDefaultAsync(); // Tomar el primero (el más próximo)

                // Realiza el cambio en loteMica, si se encontró
                if (loteMica != null)
                {
                    // Aquí haces el cambio necesario
                    loteMica.Stock += cantidad;

                    // Guarda los cambios
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new NotFoundException("No se encontró la mica en el lote");
                }
            }
            catch 
            {
                throw;
            }
        }

        public async Task<DateTime> GetCaducidad(int idMica)
        {
            //gets the soonest expiration date of a mica in LoteMica table
            return await _loteMicasIntermedia.Where(lm => lm.IdMica == idMica).MinAsync(lm => lm.FechaCaducidad);
        }

        public async Task EliminarLoteMicaByLote(int idLote)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var loteMicas = await _loteMicasIntermedia.Where(lm => lm.IdLote == idLote).ToListAsync();
                    _loteMicasIntermedia.RemoveRange(loteMicas);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}

