using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repos
{
    public class PedidoMicaRepo : IPedidoMicaRepo
    {
        private readonly ILoteMicaRepo _loteMicaRepo; 
        private readonly DbSet<PedidoMica> _pedidoMicas;
        private readonly DbContext _dbContext;

        public PedidoMicaRepo(DbContext dbContext, ILoteMicaRepo loteMicaRepo)
        {
            _pedidoMicas = dbContext.Set<PedidoMica>();
            _dbContext = dbContext;
            _loteMicaRepo = loteMicaRepo;
        }

        public async Task<IEnumerable<PedidoMica>> GetAllPedidoMicas()
        {
            return await _pedidoMicas.ToListAsync();
        }

        public async Task<PedidoMica> GetPedidoMicaById(int id)
        {
            return await _pedidoMicas.FindAsync(id);
        }

        public async Task AddPedidoMica(PedidoMica pedidoMica)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    
                    bool descontado = await _loteMicaRepo.DescontarCantidadMica(pedidoMica.MicaId, pedidoMica.Cantidad);

                    if (descontado)
                    {
                        pedidoMica.FechaAsignacion = DateTime.Now;
                        await _pedidoMicas.AddAsync(pedidoMica);
                        await _dbContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        throw new Exception("No hay suficiente stock en el lote para cubrir el pedido.");
                    }
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdatePedidoMica(PedidoMica pedidoMica)
        {
            _pedidoMicas.Update(pedidoMica);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePedidoMica(int id)
        {
            var pedidoMica = await _pedidoMicas.FindAsync(id);
            if (pedidoMica != null)
            {
                _pedidoMicas.Remove(pedidoMica);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
