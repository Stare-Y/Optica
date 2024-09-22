using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUsuarioRepo
    {
        /// <summary>
        /// trae un usuario por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Null si no hay coincidencias con el id</returns>
        Task<Usuario?> GetUsuario(int idUsuario);

        /// <summary>
        /// trae una lista de todos los usuarios, objetos completos
        /// </summary>
        /// <returns>Lista de objetos usuario</returns>
        Task<IEnumerable<Usuario>> GetAllUsuarios();

        /// <summary>
        /// Agrega un usuario a la base de datos, se valida que no exista ya, y que todos los datos sean validos
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Task, pero lanza ecepcion si falta algun dato, o el usuario esta repetido</returns>
        Task AddUsuario(Usuario usuario);

        /// <summary>
        /// Actualiza un usuario en la base de datos, se valida que exista, y que todos los datos sean validos
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Error si faltan datos</returns>
        Task UpdateUsuario(Usuario usuario);

        /// <summary>
        /// si encuentra coincidencia primero, borra un usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Task, pero si hay error o no coincidencia, lanza excepcion</returns>
        Task DeleteUsuario(Usuario usuario);

        /// <summary>
        /// Valida que los campos no esten vacios y las longitudes maximas y minimas
        /// </summary>
        /// <param name="usuario"></param>
        void ValidarUsuario(Usuario usuario);
    }
}
