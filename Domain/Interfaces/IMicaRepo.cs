using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Domain.Interfaces
{
    public interface IMicaRepo
    {
        Task<Mica?> GetMica(int idMica);
        Task<IEnumerable<Mica>> GetAllMicas();

        /// <summary>
        /// Agrega una mica al repositorio, si ya existe una mica con el mismo id, lanza una excepción
        /// </summary>
        /// <param name="mica"></param>
        /// <returns></returns>
        Task<Mica> AddMica(Mica mica, IEnumerable<MicaGraduacion>? micaGraduacions);
        Task UpdateMica(Mica mica);
        Task DeleteMica(int idMica);
        Task<int> GetStock(int idMica);
        Task<DateTime> GetCaducidad(int idMica);
        Task<int> GetSiguienteId();

        Task<IEnumerable<String>> GetTiposMicas();
        Task<IEnumerable<String>> GetFabricanteMicas();
        Task<IEnumerable<String>> GetMaterialMicas();
    }
}
