using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPedidoRepo
    {
        /// <summary>
        /// pide un objeto pedido por su id
        /// </summary>
        /// <param name="idPedido"></param>
        /// <returns>null si no hay coincidencias</returns>
        Task<Pedido?> GetPedido(int idPedido);

        /// <summary>
        /// Todos los pedidos jeje, pero recuerda traer los pedidosMicas si vas a mover mas cosas
        /// </summary>
        /// <returns>Retorna los pedidos enlistados</returns>
        Task<IEnumerable<Pedido>> GetAllPedidos();

        /// <summary>
        /// Agrega un pedido y un pedidoMica a la base de datos, si hay problema con el pedidoMica se lanza una excepcion
        /// </summary>
        /// <param name="pedido"></param>
        /// <param name="pedidoMica"></param>
        /// <returns>nada, significa que todo bien</returns>
        Task AddPedido(Pedido pedido, IEnumerable<PedidoMica> pedidoMica);

        /// <summary>
        /// no tenemos update porque no se puede modificar un pedido, solo se puede eliminar, al eliminar aqui, regresa el stock a lotemicas
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns>Task, para llamar async</returns>
        Task DeletePedido(int idPedido);
    }
}

