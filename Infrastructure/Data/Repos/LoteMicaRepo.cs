using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class LoteMicaRepo : ILoteMicaRepo
    {
        private readonly DbSet<Lote> _lotes;
        private readonly DbSet<Mica> _micas;
        private readonly DbSet<LoteMica> _loteMicasIntermedia;
        private readonly DbSet<MicaGraduacion> _micasGraduaciones;
        private readonly OpticaDbContext _dbContext;

        public LoteMicaRepo(OpticaDbContext dbContext)
        {
            _lotes = dbContext.Lotes;
            _micas = dbContext.Micas;
            _loteMicasIntermedia = dbContext.LoteMicaIntermedia;
            _micasGraduaciones = dbContext.MicaGraduacionIntermedia;
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

        public async Task<IEnumerable<LoteMica>> GetLotesMicasByLoteIdAsync(int idLote)
        {
            return await _loteMicasIntermedia.AsNoTracking().Where(lm => lm.IdLote == idLote).ToListAsync();
        }


        public async Task<int> CountLotesMicas(int idMica)
        {
            return await _loteMicasIntermedia.Where(lm => lm.IdMicaGraduacion == idMica).CountAsync();
        }


        public async Task<int> GetStock(int idMicaGraduacion)
        {
            //validates if the mica exists
            if (!await _micasGraduaciones.AnyAsync(m => m.Id == idMicaGraduacion))
            {
                throw new NotFoundException("La graduacion no existe en el repositorio");
            }
            //counts all the currentstock of a mica in LoteMica table
            return await _loteMicasIntermedia.Where(lm => lm.IdMicaGraduacion == idMicaGraduacion).SumAsync(lm => lm.Cantidad);
        }

        public async Task TakeStock(int idMicaGraduacion, int idLote, int cantidad)
        {
            try
            {
                var loteMica = await _loteMicasIntermedia.Where(lm => lm.IdMicaGraduacion == idMicaGraduacion && lm.IdLote == idLote).FirstOrDefaultAsync();

                if(loteMica == null)
                {
                    throw new NotFoundException("No se encontró la mica en el lote");
                }

                if(loteMica.Cantidad < cantidad)
                {
                    throw new BadRequestException("No hay suficiente cantidad en el lote para cubrir la cantidad solicitada.");
                }

                loteMica.Cantidad -= cantidad;

                //update the changes
                await _dbContext.SaveChangesAsync();
                Console.WriteLine($"Stock restado: {cantidad}"); 
            }
            catch(Exception e)
            {
                throw new Exception($"({e.GetType})Error al restar stock: ({e.Message})");
            }
        }

        public async Task ReturnStock(int idMicaGraduacion, int idLote, int cantidad)
        {
            try
            {
                var loteMica = await _loteMicasIntermedia.Where(lm => lm.IdMicaGraduacion == idMicaGraduacion && lm.IdLote == idLote).FirstOrDefaultAsync();

                if(loteMica == null)
                {
                    throw new NotFoundException("No se encontró la mica en el lote");
                }
                if(cantidad < 0)
                {
                    throw new BadRequestException("La cantidad a devolver no puede ser negativa, esa mmda que");
                }

                loteMica.Cantidad += cantidad;

                await _dbContext.SaveChangesAsync();

                Console.WriteLine($"Stock devuelto: {cantidad}");
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al devolver stock: ({e.Message}, Inner: {e.InnerException})");
            }
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

