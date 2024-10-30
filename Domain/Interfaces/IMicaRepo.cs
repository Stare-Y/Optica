using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Domain.Interfaces
{
    public interface IMicaRepo
    {
        /// <summary>
        /// Obtener una mica por su Id
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns></returns>
        Task<Mica> GetMica(int idMica);

        /// <summary>
        /// Obtiene todas las micas del repositorio
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Mica>> GetAllMicas();

        /// <summary>
        /// Obtiene una lista de micas por sus ids
        /// </summary>
        /// <param name="idsMicas"></param>
        /// <returns></returns>
        Task<List<Mica>> GetMicasByIds(IEnumerable<int> idsMicas);

        /// <summary>
        /// Inserta una mica sola, sin graduaciones
        /// </summary>
        /// <param name="mica"></param>
        /// <returns>La mica tal y como quedo en la db</returns>
        Task<Mica> InsertMica(Mica mica);

        /// <summary>
        /// Agrega una mica al repositorio, si ya existe una mica con el mismo id, lanza una excepción
        /// micagraduaciones puede ser nulo, para agregar solo la mica
        /// </summary>
        /// <param name="mica"></param>
        /// <returns></returns>
        Task<Mica> AddMica(Mica mica, IEnumerable<MicaGraduacion>? micaGraduaciones);

        /// <summary>
        /// Actualiza una mica en el repositorio
        /// </summary>
        /// <param name="mica"></param>
        /// <returns></returns>
        Task UpdateMica(Mica mica);

        /// <summary>
        /// Elimina una mica del repositorio, pero NO DEBERIAMOS USARLO
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns></returns>
        Task DeleteMica(int idMica);

        /// <summary>
        /// Obtiene el stock de una mica
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns></returns>
        Task<int> GetStock(int idMica);

        /// <summary>
        /// Retorna la caducidad de una mica por su Id
        /// </summary>
        /// <param name="idMica"></param>
        /// <returns></returns>
        Task<DateTime?> GetCaducidad(int idMica);

        /// <summary>
        /// Obtiene el siguiente id de mica disponible xD
        /// </summary>
        /// <returns></returns>
        Task<int> GetSiguienteId();

        /// <summary>
        /// Obtiene los tipos de micas disponibles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<String>> GetTiposMicas();

        /// <summary>
        /// Obtiene los fabricantes de micas disponibles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<String>> GetFabricanteMicas();

        /// <summary>
        /// Obtiene los materiales de micas disponibles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<String>> GetMaterialMicas();

        /// <summary>
        /// Obtiene los tratamientos de micas disponibles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<String>> GetTratamientoMicas();

        /// <summary>
        /// Obtiene los propositos de micas disponibles
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<String>> GetPropositoMicas();

        /// <summary>
        /// obtiene una lista de micas por filtros
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="material"></param>
        /// <param name="fabricante"></param>
        /// <param name="tratamiento"></param>
        /// <param name="proposito"></param>
        /// <returns></returns>
        Task<IEnumerable<Mica>> GetMicasByFiltros(string tipo, string material, string fabricante, string tratamiento, string proposito);
    }
}
