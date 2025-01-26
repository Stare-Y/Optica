using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class LoteRepo : ILoteRepo
    {
        private readonly OpticaDbContext _dbContext;
        private readonly DbSet<Lote> _lotes;
        private readonly ILoteMicaRepo _loteMicaRepo;
        private readonly IMicaGraduacionRepo _micaGraduacionRepo;
        public LoteRepo(OpticaDbContext dbContext, ILoteMicaRepo loteMicaRepo, IMicaGraduacionRepo micaGraduacionRepo)
        {
            _dbContext = dbContext;
            _lotes = dbContext.Set<Lote>();
            _loteMicaRepo = loteMicaRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
        }

        public async Task<Lote?> GetLote(int idLote)
        {
            return await _lotes.FirstOrDefaultAsync(m => m.Id == idLote);
        }

        public async Task<IEnumerable<Lote>> GetAllLotesAsync()
        {
            return await _lotes.ToListAsync();
        }

        public async Task<IEnumerable<Lote>> GetValidLotesAsync()
        {
            return await _lotes.Where(l => l.Existencias > 0).ToListAsync();
        }


        public async Task<Lote> AddLote(Lote lote, IEnumerable<LoteMica> lotesMicas)
        {
            try
            {
                ValidarLotesMicas(lotesMicas);

                if (lote.Id != 0)
                {
                    throw new BadRequestException("El id del lote debe ser 0");
                }

                if(lotesMicas.Sum(lm => lm.Cantidad) != lote.Existencias)
                {
                    throw new Exception("La cantidad de micas en el lote no coincide con la cantidad de existencias");
                }

                await _lotes.AddAsync(lote);

                foreach (var lm in lotesMicas)
                {
                    if (lm.IdLote != 0)
                    {
                        throw new BadRequestException("Inicialmente, el id de lote de la fila LoteMica debe ser 0");
                    }
                    lm.IdLote = lote.Id;
                }

                await _loteMicaRepo.AgregarLoteMica(lotesMicas);

                await _dbContext.SaveChangesAsync();

                return lote;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al añadir el lote: ({e.Message})(Inner:{e.InnerException}.)");
            }
        }

        public async Task<Lote> DeleteLote(int idLote)
        {
            try
            {
                var loteEliminar = await _lotes.FirstOrDefaultAsync(l => l.Id == idLote);
                if (loteEliminar == null)
                {
                    throw new NotFoundException("El lote no existe en el repositorio");
                }
                _lotes.Remove(loteEliminar);

                await _loteMicaRepo.EliminarLoteMicaByLote(idLote);

                await _dbContext.SaveChangesAsync();

                return loteEliminar;
                
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al eliminar el lote: ({e})(Inner: {e.InnerException})");
            }
        }

        public void ValidarLote(Lote lote)
        {
            if (lote.Proveedor == string.Empty)
            {
                throw new Exception("El lote debe tener un proveedor");
            }
            if (lote.FechaCaducidad < DateTime.Now)
            {
                throw new Exception("El lote debe tener fecha de caducidad mayor a la fecha actual");
            }
            if (lote.FechaEntrada > DateTime.Now)
            {
                throw new Exception("El lote debe tener fecha de entrada menor a la fecha actual");
            }
        }

        public void ValidarLotesMicas(IEnumerable<LoteMica> lotesMicas)
        {
            foreach (var lm in lotesMicas)
            {
                if (lm.IdMicaGraduacion == 0)
                {
                    throw new Exception("El lote debe tener todas las micas validas, se recibio una con id 0");
                }

                if (lm.Cantidad <= 0)
                {
                    throw new Exception("El lote debe tener stock mayor a 0");
                }
            }
        }
    }
}

