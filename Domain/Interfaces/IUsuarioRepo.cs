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
        Task<Usuario> GetUsuarioById(int idUsuario);

        /// <summary>
        /// trae una lista de todos los usuarios, objetos completos
        /// </summary>
        /// <returns>Lista de objetos usuario</returns>
        Task<List<Usuario>> GetAllUsuarios();

        /// <summary>
        /// Gets the usuarios by the provided ids
        /// </summary>
        /// <param name="idsUsuarios"></param>
        /// <returns></returns>
        Task<List<Usuario>> GetUsuariosByIds(List<int> idsUsuarios);

        /// <summary>
        /// Agrega un usuario a la base de datos, se valida que no exista ya, y que todos los datos sean validos, y si pasa la validacion, aqui se le asigna el id, aquiu
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>Instancia de usuario completa, con su ID</returns>
        Task<Usuario> AddUsuario(Usuario usuario);

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
        Task<Usuario> DeleteUsuario(int idUsuario);

        /// <summary>
        /// Valida que los campos no esten vacios y las longitudes maximas y minimas
        /// </summary>
        /// <param name="usuario"></param>
        void ValidarUsuario(Usuario usuario);

        /// <summary>
        /// Autentica un usuario, si no existe, retorna null
        /// </summary>
        /// <param name="nombreDeUsuario"></param>
        /// <param name="password"></param>
        /// <returns>una instancia de usuario valida</returns>
        Task<Usuario?> AutenticarUsuario(string nombreDeUsuario, string password);
    }
}
