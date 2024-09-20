using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class PedidoRepo : IPedidoRepo
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Pedido> _pedidos;
        private readonly IPedidoMicaRepo _pedidoMicaRepo;
        public PedidoRepo(DbContext dbContext, IPedidoMicaRepo pedidoMicaRepo)
        {
            _dbContext = dbContext;
            _pedidos = dbContext.Set<Pedido>();
            _pedidoMicaRepo = pedidoMicaRepo;
        }

        public async Task<Pedido?> GetPedido(int idPedido)
        {
            return await _pedidos.FirstOrDefaultAsync(p => p.Id == idPedido);
        }

        public async Task<IEnumerable<Pedido>> GetAllPedidos()
        {
            return await _pedidos.ToListAsync();
        }

        public async Task AddPedido(Pedido pedido, IEnumerable<PedidoMica> pedidosMicas)
        {
            try
            {
                await _pedidoMicaRepo.AddPedidoMica(pedidosMicas);
                await _pedidos.AddAsync(pedido);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al añadir el pedido: {e}");
            }
        }

        public async Task DeletePedido(int idPedido)
        {
            try 
            {
                var pedidoEliminar = await _pedidos.FirstOrDefaultAsync(p => p.Id == idPedido);
                if (pedidoEliminar == null)
                {
                    throw new NotFoundException("El pedido no existe en el repositorio");
                }
                else
                {
                    await _pedidoMicaRepo.DeletePedidoMicaByPedidoId(idPedido);
                    _pedidos.Remove(pedidoEliminar);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}