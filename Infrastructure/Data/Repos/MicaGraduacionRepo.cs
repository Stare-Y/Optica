using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class MicaGraduacionRepo : IMicaGraduacionRepo
    {
        private readonly OpticaDbContext _dbContext;
        private readonly DbSet<MicaGraduacion> _micasGraduaciones;
        public MicaGraduacionRepo(OpticaDbContext dbContext)
        {
            _dbContext = dbContext;
            _micasGraduaciones = _dbContext.MicaGraduacionIntermedia;
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

        public async Task<MicaGraduacion?> GetMicaGraduacionByGraduacion(float graduacionEsferica, float graduacionCilindrica, int idMica)
        {
            return await _micasGraduaciones.FirstOrDefaultAsync(mg => mg.Graduacionesf == graduacionEsferica && mg.Graduacioncil == graduacionCilindrica
                                                                        && mg.IdMica == idMica);
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
                            micasGraduaciones = micasGraduaciones.Where(mg => mg.IdMica != micaGraduacion.IdMica && mg.Graduacioncil != micaGraduacion.Graduacionesf);
                        }
                    }
                    //if range is empty, return
                    if (!micasGraduaciones.Any())
                    {
                        throw new Exception("No se insertaron graduaciones, todas las graduaciones ya existen");
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

        public async Task<MicaGraduacion> AddMicaGraduacion(MicaGraduacion micaGraduacion)
        {
            try
            {
                if (micaGraduacion.Id != 0)
                {
                    throw new Exception("No se puede insertar una graduacion si ya tiene Id, intenta actualizar en su lugar");
                }

                await _micasGraduaciones.AddAsync(micaGraduacion);
                await _dbContext.SaveChangesAsync();

                //Remove the tracking of the entity
                _dbContext.Entry(micaGraduacion).State = EntityState.Detached;

                return micaGraduacion;
            }
            catch(Exception e)
            {
                throw new Exception($"No se pudo insertar la mica con AddMicaGraduacion: {e.Message}");
            }
        }

        public async Task ValidarPrecios(IEnumerable<MicaGraduacion> micaGraduaciones)
        {
            foreach (var micaGraduacion in micaGraduaciones)
            {
                if (micaGraduacion.Precio <= 0)
                {
                    throw new Exception($"El precio de la graduacion {micaGraduacion.Id} no puede ser menor o igual a 0, abortando transaccion");
                }
                //find micagraduacion in db
                var micaGraduacionDb = await _micasGraduaciones.FirstOrDefaultAsync(mg => mg.Id == micaGraduacion.Id);
                if (micaGraduacionDb == null)
                {
                    throw new Exception($"No se encontró la mica con id {micaGraduacion.Id}, abortando transaccion");
                }
                //update precio
                micaGraduacionDb.Precio = micaGraduacion.Precio;
            }
            await _dbContext.SaveChangesAsync();

            _dbContext.ChangeTracker.Clear();  // Elimina cualquier tracking residual
        }


        public async Task UpdateMicaGradiacion(MicaGraduacion micaGraduacion)
        {
            try
            {
                if (micaGraduacion.Id == 0)
                {
                    throw new Exception("No se puede actualizar una mica sin id");
                }
                _micasGraduaciones.Update(micaGraduacion);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"No se pudo actualizar la mica con UpdateMicaGraduacion: {e.Message}");

            }
        }

        public async Task<int> GetSiguienteId()
        {
            return await _micasGraduaciones.MaxAsync(mg => mg.Id) + 1;
        }
    }
}
