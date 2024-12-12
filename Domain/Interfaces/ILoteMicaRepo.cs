using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILoteMicaRepo
    {
        /// <summary>
        /// Agregar un lote de micas a la tabla intermedia
        /// </summary>
        /// <param name="lotesMicas"></param>
        /// <returns>trask, si error, hace rollback y to bien</returns>
        Task AgregarLoteMica(IEnumerable<LoteMica> lotesMicas);

        /// <summary>
        /// Obtiene el stock de una mica, la sumatoria de todas las cantidades de micas en los lotes
        /// Si la mica no existe, lanza una excepcion
        /// </summary>
        /// <param name="mica"></param>
        /// <returns>entero con la cantidad de stock de la mica solicitada</returns>
        Task<int> GetStock(int idMica);

        /// <summary>
        /// Busca un lote con la mica solicitada, y resta la cantidad indicada
        /// </summary>
        /// <param name="mica"></param>
        /// <param name="restar"></param>
        /// <returns>Tarea asincrona</returns>
        Task<bool> TakeStock(int idMicaGraduacion, int idLote, int cantidad);

        /// <summary>
        /// Retorna el stock tomado por un pedido al lote correspondiente
        /// </summary>
        /// <param name="idMica"></param>
        /// <param name="cantidad"></param>
        /// <returns>Task</returns>
        Task ReturnStock(PedidoMica pedidoMica);

        /// <summary>
        /// Elimina los registros de la tabla intermedia que coincidan con el lote indicado
        /// </summary>
        /// <param name="idLote"></param>
        /// <returns>task, si error rollback</returns>
        Task EliminarLoteMicaByLote(int idLote);
    }
}

