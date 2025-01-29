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
            try
            {
                foreach(var pedidoMica in pedidosMicas) 
                {
                    await _loteMicaRepo.TakeStock(pedidoMica.IdMicaGraduacion, pedidoMica.IdLoteOrigen, pedidoMica.Cantidad);
                }
            }
            catch(Exception e)
            {
                throw new Exception($"({e.GetType})Error al añadir filas de Pedido/Mica: {e.Message}");
            }
        }

        public async Task<List<PedidoMica>> DeletePedidoMicaByPedidoId(int idPedido)
        {
            //validar si el pedido existe
            if (!await _pedidoMicas.AnyAsync(pm => pm.IdPedido == idPedido))
            {
                return new List<PedidoMica>();
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

            return pedidosMicas;
        }

        public async Task<int> GetMicasVendidas(int idMicaGraduacion)
        {
            return await _pedidoMicas.Where(pm => pm.IdMicaGraduacion == idMicaGraduacion).SumAsync(pm => pm.Cantidad);
        }
    }
}
