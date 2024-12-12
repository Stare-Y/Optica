using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class PedidoMicaRepo : IPedidoMicaRepo
    {
        private readonly ILoteMicaRepo _loteMicaRepo; 
        private readonly DbSet<PedidoMica> _pedidoMicas;
        private readonly OpticaDbContext _dbContext;

        /// <summary>
        /// Necesitamos el LoteMicaRepo para poder descontar stock de los lotes al asignar un pedido.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="loteMicaRepo"></param>
        public PedidoMicaRepo(OpticaDbContext dbContext, ILoteMicaRepo loteMicaRepo)
        {
            _pedidoMicas = dbContext.Set<PedidoMica>();
            _dbContext = dbContext;
            _loteMicaRepo = loteMicaRepo;
        }

        public async Task<IEnumerable<PedidoMica>> GetAllPedidoMicas()
        {
            return await _pedidoMicas.ToListAsync();
        }

        public async Task<IEnumerable<PedidoMica>> GetPedidoMicasByPedidoId(int idPedido)
        {
            var pedidoMicas = await _pedidoMicas.Where(pm => pm.IdPedido == idPedido).ToListAsync();
            return pedidoMicas;
        }

        public async Task AddPedidoMica(IEnumerable<PedidoMica> pedidosMicas)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach(var pedidoMica in pedidosMicas) 
                    {
                        bool descontado = await _loteMicaRepo.TakeStock(pedidoMica.IdMicaGraduacion, pedidoMica.IdLoteOrigen, pedidoMica.Cantidad);
                        if (descontado)
                        {
                            await _pedidoMicas.AddAsync(pedidoMica);
                        }
                        else
                        {
                            throw new Exception("Hubo un error descontando stock de los lotes");
                        }
                    }
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch(Exception e)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"({e.GetType})Error al añadir filas de Pedido/Mica: {e.Message}");
                }
            }
        }

        public async Task DeletePedidoMicaByPedidoId(int idPedido)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    //validar si el pedido existe
                    if (!await _pedidoMicas.AnyAsync(pm => pm.IdPedido == idPedido))
                    {
                        throw new NotFoundException("El pedido no existe en el repositorio");
                    }

                    //regresamos el stock a las micas
                    var pedidosMicas = await _pedidoMicas.Where(pm => pm.IdPedido == idPedido).ToListAsync();
                    foreach (var pm in pedidosMicas)
                    {
                        await _loteMicaRepo.ReturnStock(pm);
                    }

                    //eliminamos los registros de la tabla intermedia
                    _pedidoMicas.RemoveRange(pedidosMicas);

                    //guardamos los cambios y confirmamos la transaccion
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch(Exception e)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error al eliminar las filas de Pedido/Mica: {e.Message}");
                }
            }

        }

        public async Task<int> GetMicasVendidas(int idMicaGraduacion)
        {
            return await _pedidoMicas.Where(pm => pm.IdMicaGraduacion == idMicaGraduacion).SumAsync(pm => pm.Cantidad);
        }
    }
}
