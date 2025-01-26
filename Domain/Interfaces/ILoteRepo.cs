using Domain.Entities;

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
        /// Retorna una lista con todos los lotes, SOLO PARA IMPLEMENTACION DE REPORTES, USAR GET VALID LOTES INSTEAD
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Lote>> GetAllLotesAsync();

        /// <summary>
        /// Retorna una lista con todos los lotes validos, es decir, que tengan Existencias > 0
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Lote>> GetValidLotesAsync();

        /// <summary>
        /// primero agrega las relaciones lotesmicas, despues, agrega un lote a la base de datos, no se puede agregar un lote con id ya existente, 
        /// </summary>
        /// <param name="lote"></param>
        /// <returns>Relacion intermedia, y lote, hace rollback si salta excepcion</returns>
        Task<Lote> AddLote(Lote lote, IEnumerable<LoteMica> lotesmicas);

        /// <summary>
        /// Elimina primero las relaciones con las micas, y luego elimina el lote NO USAMOS UPDATE, DELETE NOMAS
        /// </summary>
        /// <param name="lote"></param>
        /// <returns>task, pero si hay error da excepcion, y hace rollback, no guarda</returns>
        Task<Lote> DeleteLote(int idLote);

        /// <summary>
        /// Retorna el siguiente id disponible para un lote
        /// </summary>
        /// <returns>entero con el siguiente id valido</returns>
        Task<int> GetSiguienteId();

        /// <summary>
        /// Valida que los lotes micas tengan micas validas
        /// </summary>
        /// <param name="lotesMicas"></param>
        /// <returns>Task</returns>
        void ValidarLotesMicas(IEnumerable<LoteMica> lotesMicas);

        /// <summary>
        /// Valida que un lote sea valido, que tenga los campos requeridos not null, excepto el id
        /// </summary>
        /// <param name="lote"></param>
        void ValidarLote(Lote lote);
    }
}

