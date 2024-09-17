using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILoteMicaRepo
    {
        /// <summary>
        /// Dar de alta una entrada a almacen
        /// </summary>
        /// <param name="lote"></param>
        /// <param name="micas"></param>
        /// <returns>Tarea para ejecutar asincronamente</returns>
        Task AgregarLoteMica(Lote lote, Dictionary<Mica, int> micas);

        ///// <summary>
        ///// Obtiene el stock de una mica, la sumatoria de todas las cantidades de micas en los lotes
        ///// </summary>
        ///// <param name="mica"></param>
        ///// <returns>Tarea para ejecucion asincrona</returns>
        //Task GetStock(Mica mica);

        ///// <summary>
        ///// Actualiza, en la tabla intermedia, el stock de una mica, restando la cantidad indicada, del respectivo lote
        ///// </summary>
        ///// <param name="mica"></param>
        ///// <param name="restar"></param>
        ///// <returns>Tarea asincrona</returns>
        //Task TakeStock(Mica mica, int cantidad);
         
        ///// <summary>
        ///// Obtiene la caducidad de una mica, en base al lote mas antiguo
        ///// </summary>
        ///// <param name="mica"></param>
        ///// <returns>DateTime con la fecha de caducidad solicitada</returns>
        //Task<DateTime> GetCaducidad(Mica mica);
    }
}
