using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPedidoRepo
    {
        Task<Pedido> GetPedido(int id);
        Task<IEnumerable<Pedido>> GetAllPedidos();
        Task AddPedido(Pedido pedido);
        Task UpdatePedido(Pedido pedido);
        Task DeletePedido(Pedido pedido);
    }
}

