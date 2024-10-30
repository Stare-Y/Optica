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
        /// Actualiza, en la tabla intermedia, el stock de una mica, restando la cantidad indicada, del respectivo lote, o del siguiente si falta mas
        /// </summary>
        /// <param name="mica"></param>
        /// <param name="restar"></param>
        /// <returns>Tarea asincrona</returns>
        Task<bool> TakeStock(int idMica, int cantidad);

        /// <summary>
        /// Retorna el stock de una mica, sumando la cantidad indicada, en el lote mas proximo
        /// </summary>
        /// <param name="idMica"></param>
        /// <param name="cantidad"></param>
        /// <returns>Task</returns>
        Task ReturnStock(int idMica, int cantidad);

        /// <summary>
        /// Obtiene la caducidad de una mica, en base al lote mas antiguo
        /// </summary>
        /// <param name="mica"></param>
        /// <returns>DateTime con la fecha de caducidad solicitada</returns>
        Task<DateTime?> GetCaducidad(int idMica);

        /// <summary>
        /// Elimina los registros de la tabla intermedia que coincidan con el lote indicado
        /// </summary>
        /// <param name="idLote"></param>
        /// <returns>task, si error rollback</returns>
        Task EliminarLoteMicaByLote(int idLote);
    }
}

