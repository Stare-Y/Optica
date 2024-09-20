using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILoteRepo
    {
        /// <summary>
        /// Retorna un lote en base a su id, si no existe retorna null
        /// </summary>
        /// <param name="idLote"></param>
        /// <returns>null si no hay coincidencia</returns>
        Task<Lote?> GetLote(int idLote);

        /// <summary>
        /// Retorna una lista con todos los lotes, recuerda jalar la tabla intermedia para operaciones mas complejas
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Lote>> GetAllLotes();

        /// <summary>
        /// primero agrega las relaciones lotesmicas, despues, agrega un lote a la base de datos, no se puede agregar un lote con id ya existente, 
        /// </summary>
        /// <param name="lote"></param>
        /// <returns>Relacion intermedia, y lote, hace rollback si salta excepcion</returns>
        Task AddLote(Lote lote, IEnumerable<LoteMica> lotesmicas);

        /// <summary>
        /// Elimina primero las relaciones con las micas, y luego elimina el lote NO USAMOS UPDATE, DELETE NOMAS
        /// </summary>
        /// <param name="lote"></param>
        /// <returns>task, pero si hay error da excepcion, y hace rollback, no guarda</returns>
        Task DeleteLote(int idLote);
    }
}

