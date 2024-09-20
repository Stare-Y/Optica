using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPedidoMicaRepo
    {
        Task<IEnumerable<PedidoMica>> GetAllPedidoMicas();

        /// <summary>
        /// se obtienen las relaciones de micas por pedido
        /// </summary>
        /// <param name="idPedido"></param>
        /// <returns></returns>
        Task<IEnumerable<PedidoMica?>> GetPedidoMicasByPedidoId(int idPedido);

        /// <summary>
        /// Se agrega una lista de registros, ya que es una tabla intermedia, y cada registro es un pedidoMica, con una mica y stock precisos
        /// </summary>
        /// <param name="pedidosMicas"></param>
        /// <returns>Task, para correr async</returns>
        Task AddPedidoMica(IEnumerable<PedidoMica> pedidosMicas);

        /// <summary>
        /// Se elimina un pedidoMica, y se regresa el stock a la mica
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeletePedidoMicaByPedidoId(int id);
    }
}

