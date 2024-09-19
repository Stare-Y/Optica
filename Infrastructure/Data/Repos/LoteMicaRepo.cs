using Domain.Entities;
using Domain.Interfaces;
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

        public async Task AgregarLoteMica(Lote lote, Dictionary<Mica, int> micas)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var micaIds = micas.Keys.Select(m => m.Id).ToList();
                    var micasExistentes = await _micas.Where(m => micaIds.Contains(m.Id)).ToListAsync();

                    foreach (var mica in micas.Keys)
                    {
                        if (!micasExistentes.Any(me => me.Id == mica.Id))
                        {
                            await _micas.AddAsync(mica);
                        }
                    }

                    foreach (var mica in micas)
                    {
                        await _loteMicasIntermedia.AddAsync(new LoteMica
                        {
                            Lote = lote.Id,
                            Mica = mica.Key.Id,
                            Stock = mica.Value
                        });
                    }

                    await _lotes.AddAsync(lote);
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

        /// <summary>
        /// método para descontar el stock de las micas
        /// </summary>
        /// <param name="micaId"></param>
        /// <param name="cantidad"></param>
        /// <returns></returns>
        public async Task<bool> DescontarCantidadMica(int micaId, int cantidad)
        {
            var loteMica = await _loteMicasIntermedia.FirstOrDefaultAsync(lm => lm.Mica == micaId);

            if (loteMica == null || loteMica.Stock < cantidad)
            {
                return false; 
            }

            
            loteMica.Stock -= cantidad;

            
            await _dbContext.SaveChangesAsync();

            return true; 
        }
    }
}

