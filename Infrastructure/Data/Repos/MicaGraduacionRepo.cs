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

        public async Task DeleteMicaGraduacion(int id)
        {
            var micaGraduacionToDelete = await _micasGraduaciones.Where(mg => mg.Id == id).ToListAsync();
            _micasGraduaciones.RemoveRange(micaGraduacionToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MicaGraduacion>> GetMicaGraduacion()
        {
            return await _micasGraduaciones.ToListAsync();
        }

        public async Task<List<MicaGraduacion>> GetMicaGraduacionByMicaId(int idMica)
        {
            return await _micasGraduaciones.Where(mg => mg.IdMica == idMica).ToListAsync();
        }


        public async Task<MicaGraduacion> GetMicaGraduacionById(int idMica)
        {
            var micaGraduacion = await _micasGraduaciones.FirstOrDefaultAsync(mg => mg.Id == idMica);
            if (micaGraduacion == null)
            {
                    throw new Exception($"No se encontró la mica_graduacion con id{idMica}");
            }
            return micaGraduacion;
        }

        public async Task InsertMicaGraduacion(IEnumerable<MicaGraduacion> micasGraduaciones)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    //validar que no haya graduaciones repetidas
                    foreach (var micaGraduacion in micasGraduaciones)
                    {
                        if (await _micasGraduaciones.AnyAsync(mg => mg.IdMica == micaGraduacion.IdMica 
                                                                    && mg.Graduacioncil == micaGraduacion.Graduacioncil 
                                                                    && mg.Graduacionesf == micaGraduacion.Graduacionesf))
                        {
                            //quita la mica de la lista, para que no se inserte repetida
                            micasGraduaciones = micasGraduaciones.Where(mg => mg.IdMica != mg.IdMica && mg.Graduacioncil != mg.Graduacionesf);
                        }
                    }
                    await _micasGraduaciones.AddRangeAsync(micasGraduaciones);
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
