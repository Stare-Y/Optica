using Domain.Entities;
using Domain.Interfaces.Services.Reportes.Entities;
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
        Task<List<Pedido>> GetAllPedidos();

        /// <summary>
        /// Retorna los pedidos dentro del rango de fechas especificado
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        Task<List<Pedido>> GetPedidosByDate(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Agrega un pedido y un pedidoMica a la base de datos, si hay problema con el pedidoMica se lanza una excepcion
        /// </summary>
        /// <param name="pedido"></param>
        /// <param name="pedidoMica"></param>
        /// <returns>nada, significa que todo bien</returns>
        Task<Pedido> AddPedido(Pedido pedido, IEnumerable<PedidoMica>? pedidoMica);

        /// <summary>
        /// no tenemos update porque no se puede modificar un pedido, solo se puede eliminar, al eliminar aqui, regresa el stock a lotemicas
        /// </summary>
        /// <param name="pedido"></param>
        /// <returns>Task, para llamar async</returns>
        Task DeletePedido(int idPedido);

        Task<int> GetSiguienteId();

        /// <summary>
        /// Valida que los pedidosMicas no tengan errores, y esten listas para guardar
        /// </summary>
        /// <param name="pedidosMicas"></param>
        void ValidarPedidosMicas(IEnumerable<PedidoMica> pedidosMicas);

        /// <summary>
        /// Genera un reporte de pedidos en un rango de fechas especificado con la vista especial
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns>Lista con las filas del reporte</returns>
        Task<List<ReportePedido>> GenerarReporte(DateTime fechaInicio, DateTime fechaFin);

        Task<List<ReportePedido>> GenerarReporteExcel(DateTime fechaInicio, DateTime fechaFin);


    }
}

