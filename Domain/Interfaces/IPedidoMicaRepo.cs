using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPedidoMicaRepo
    {
        Task<IEnumerable<PedidoMica>> GetAllPedidoMicas();
        Task<PedidoMica> GetPedidoMicaById(int id);
        Task AddPedidoMica(PedidoMica pedidoMica);
        Task UpdatePedidoMica(PedidoMica pedidoMica);
        Task DeletePedidoMica(int id);
    }
}

