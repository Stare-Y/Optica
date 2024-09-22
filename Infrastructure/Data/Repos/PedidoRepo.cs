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

        public async Task<Pedido> AddPedido(Pedido pedido, IEnumerable<PedidoMica> pedidosMicas)
        {
            try
            {
                if(pedido.Id == 0)
                {
                    pedido.Id = await GetSiguienteId();
                    foreach (var pm in pedidosMicas)
                    {
                        pm.PedidoId = pedido.Id;
                    }
                }
                await _pedidos.AddAsync(pedido);

                await _pedidoMicaRepo.AddPedidoMica(pedidosMicas);

                await _dbContext.SaveChangesAsync();

                return pedido;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al añadir el pedido: ({e.Message})(Inner: {e.InnerException})");
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
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al eliminar el pedido: ({e.Message})(Inner: {e.InnerException})");
            }
        }

        public async Task<int> GetSiguienteId()
        {
            try
            {
                if(!await _pedidos.AnyAsync())
                {
                    return 1;
                }
                return await _pedidos.MaxAsync(p => p.Id) + 1;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al obtener el siguiente id de pedido: ({e.Message})(Inner: {e.InnerException})");
            }
        }

        public void ValidarPedidosMicas(IEnumerable<PedidoMica> pedidosMicas)
        {
            foreach (var pedidoMica in pedidosMicas)
            {
                if (pedidoMica.Cantidad <= 0)
                {
                    throw new BadRequestException("La cantidad de micas debe ser mayor a 0");
                }
                if (pedidoMica.MicaId == 0)
                {
                    throw new BadRequestException("El id de la mica no puede ser 0");
                }
                if (pedidoMica.FechaAsignacion == DateTime.MinValue)
                {
                    throw new BadRequestException("La fecha de asignación no puede ser nula");
                }
            }
        }
    }
}