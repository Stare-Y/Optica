﻿using Domain.Entities;
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

        public async Task<int> GetStock(int idMicaGraduacion)
        {
            //validates if the mica exists
            if (!await _micasGraduaciones.AnyAsync(m => m.Id == idMicaGraduacion))
            {
                throw new NotFoundException("La graduacion no existe en el repositorio");
            }
            //counts all the currentstock of a mica in LoteMica table
            return await _loteMicasIntermedia.Where(lm => lm.IdMicaGraduacion == idMicaGraduacion).SumAsync(lm => lm.Stock);
        }

        public async Task<bool> TakeStock(int idMicaGraduacion, int cantidad)
        {
            try
            {
                if(await GetStock(idMicaGraduacion) < cantidad)
                {
                    return false;
                }
                var loteMicas = await _loteMicasIntermedia.Where(lm => lm.IdMicaGraduacion == idMicaGraduacion).ToListAsync();
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

        public async Task ReturnStock(int idMicaGraduacion, int cantidad)
        {
            try
            {
                var loteMica = await _loteMicasIntermedia
                    .Where(lm => lm.IdMicaGraduacion == idMicaGraduacion)
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

        public async Task<DateTime?> GetCaducidad(int idMicaGraduacion)
        {
            // Obtiene la fecha de caducidad más cercana de un lote de mica en la tabla LoteMica
            var caducidades = await _loteMicasIntermedia
                .Where(lm => lm.IdMicaGraduacion == idMicaGraduacion)
                .Select(lm => (DateTime?)lm.FechaCaducidad) // Convierte a DateTime? para permitir el valor nulo
                .ToListAsync();

            return caducidades.Any() ? caducidades.Min() : null;
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

