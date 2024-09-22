using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class PedidoMicaRepo : IPedidoMicaRepo
    {
        private readonly ILoteMicaRepo _loteMicaRepo; 
        private readonly DbSet<PedidoMica> _pedidoMicas;
        private readonly DbContext _dbContext;

        /// <summary>
        /// Necesitamos el LoteMicaRepo para poder descontar stock de los lotes al asignar un pedido.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="loteMicaRepo"></param>
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

        public async Task<IEnumerable<PedidoMica?>> GetPedidoMicasByPedidoId(int idPedido)
        {
            return await _pedidoMicas.Where(pm => pm.PedidoId == idPedido).ToListAsync();
        }

        public async Task AddPedidoMica(IEnumerable<PedidoMica> pedidosMicas)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach(var pedidoMica in pedidosMicas) 
                    {
                        bool descontado = await _loteMicaRepo.TakeStock(pedidoMica.MicaId, pedidoMica.Cantidad);

                        if (descontado)
                        {
                            pedidoMica.FechaAsignacion = DateTime.Now;
                            await _pedidoMicas.AddAsync(pedidoMica);
                        }
                        else
                        {
                            throw new Exception("No hay suficiente stock en el lote para cubrir el pedido.");
                        }
                    }
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

        public async Task DeletePedidoMicaByPedidoId(int idPedido)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    //regresamos el stock a las micas
                    var pedidosMicas = await _pedidoMicas.Where(pm => pm.PedidoId == idPedido).ToListAsync();
                    foreach (var pm in pedidosMicas)
                    {
                        await _loteMicaRepo.ReturnStock(pm.MicaId, pm.Cantidad);
                    }

                    //eliminamos los registros de la tabla intermedia
                    _pedidoMicas.RemoveRange(pedidosMicas);

                    //guardamos los cambios y confirmamos la transaccion
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

        public async Task<int> GetMicasVendidas(int idMica)
        {
            return await _pedidoMicas.Where(pm => pm.MicaId == idMica).SumAsync(pm => pm.Cantidad);
        }
    }
}
