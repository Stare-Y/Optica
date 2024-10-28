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
        /// Obtiene las graduaciones de una mica por su graduacion esferica y cilindrica
        /// </summary>
        /// <param name="graduacionEsferica"></param>
        /// <param name="graduacionCilindrica"></param>
        /// <returns>NULL IF NO MATCHES, micagraduacion obj with all fields filled</returns>
        Task<MicaGraduacion?> GetMicaGraduacionByGraduacion(float graduacionEsferica, float graduacionCilindrica);

        /// <summary>
        /// Actualiza la graduacion, solo la graduacion, no la mica
        /// </summary>
        /// <param name="micaGraduacion"></param>
        /// <returns>If no idMica throws</returns>
        Task UpdateMicaGradiacion(MicaGraduacion micaGraduacion);

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
        /// obj should not have pk id, it will be generated
        /// </summary>
        /// <param name="micaGraduacion"></param>
        /// <returns>resulting micagraduacion instance that is in the db</returns>
        Task<MicaGraduacion> AddMicaGraduacion(MicaGraduacion micaGraduacion);

        /// <summary>
        /// Valida que los precios de las graduaciones sean correctos, si es difertente, lo actualiza
        /// </summary>
        /// <param name="micaGraduaciones"></param>
        Task ValidarPrecios(IEnumerable<MicaGraduacion> micaGraduaciones);

        /// <summary>
        /// Elimina todas las graduaciones de una mica
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns>Task</returns>
        Task DeleteMicaGraduacion(int id);

        Task<int> GetSiguienteId();
    }
}
