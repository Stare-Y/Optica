using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUsuarioRepo
    {
        Task<Usuario> GetUsuario(int id);
        Task<IEnumerable<Usuario>> GetAllUsuarios();
        Task AddUsuario(Usuario usuario);
        Task UpdateUsuario(Usuario usuario);
        Task DeleteUsuario(Usuario usuario);
    }
}
