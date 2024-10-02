using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IMicaGraduacionRepo
    {
        /// <summary>
        /// Obtiene todas las graduaciones de las micas
        /// </summary>
        /// <returns>Lista con todas las micas awas</returns>
        Task<IEnumerable<MicaGraduacion>> GetMicaGraduacion();

        /// <summary>
        /// Obtiene las graduaciones de una mica por su id
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns>Instancia de MicaGraduacion</returns>
        Task<MicaGraduacion> GetMicaGraduacionById(int id);

        /// <summary>
        /// Retorna las graduaciones de una mica por su id
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns></returns>
        Task<List<MicaGraduacion>> GetMicaGraduacionByMicaId(int idMica);

        /// <summary>
        /// Inserta una lista de graduaciones de una mica. validando que no haya graduaciones repetidas, por eso puede ser lenta
        /// </summary>
        /// <param name="micaGraduacion"></param>
        /// <returns></returns>
        Task InsertMicaGraduacion(IEnumerable<MicaGraduacion> micaGraduacion);

        /// <summary>
        /// Elimina todas las graduaciones de una mica
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns>Task</returns>
        Task DeleteMicaGraduacion(int id);

        Task<int> GetSiguienteId();
    }
}
