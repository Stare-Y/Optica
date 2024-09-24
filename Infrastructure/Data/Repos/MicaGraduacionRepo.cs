using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Infrastructure.Data.Repos
{
    public class MicaGraduacionRepo : IMicaGraduacionRepo
    {
        private readonly OpticaDbContext _dbContext;
        private readonly DbSet<MicaGraduacion> _micasGraduaciones;
        public MicaGraduacionRepo(OpticaDbContext dbContext)
        {
            _dbContext = dbContext;
            _micasGraduaciones = _dbContext.Set<MicaGraduacion>();
        }

        public async Task DeleteMicaGraduacion(int idMica)
        {
            var micaGraduacionToDelete = await _micasGraduaciones.Where(mg => mg.IdMica == idMica).ToListAsync();
            _micasGraduaciones.RemoveRange(micaGraduacionToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MicaGraduacion>> GetMicaGraduacion()
        {
            return await _micasGraduaciones.ToListAsync();
        }

        public async Task<MicaGraduacion?> GetMicaGraduacionById(int idMica)
        {
            return await _micasGraduaciones.FirstOrDefaultAsync(mg => mg.IdMica == idMica);
        }

        public async Task InsertMicaGraduacion(IEnumerable<MicaGraduacion> micaGraduacion)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //validar que no haya graduaciones repetidas
                    foreach (var mg in micaGraduacion)
                    {
                        if (await _micasGraduaciones.AnyAsync(mg => mg.IdMica == mg.IdMica && mg.Graduacioncil == mg.Graduacionesf))
                        {
                            //quita la mica de la lista, para que no se inserte repetida
                            micaGraduacion = micaGraduacion.Where(mg => mg.IdMica != mg.IdMica && mg.Graduacioncil != mg.Graduacionesf);
                        }
                    }
                    await _micasGraduaciones.AddRangeAsync(micaGraduacion);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<int> GetSiguienteId()
        {
            return await _micasGraduaciones.MaxAsync(mg => mg.Id) + 1;
        }

    }
}
