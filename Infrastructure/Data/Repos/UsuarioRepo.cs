using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data.Repos
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<Usuario> _usuarios;
        public UsuarioRepo(DbContext dbContext)
        {
            _dbContext = dbContext;
            _usuarios = dbContext.Set<Usuario>();
        }
        public async Task<Usuario?> GetUsuario(int idUsuario)
        {
            return await _usuarios.FirstOrDefaultAsync(u => u.Id == idUsuario);
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            return await _usuarios.ToListAsync();
        }
        public async Task AddUsuario(Usuario usuario)
        {
            ValidarUsuario(usuario);

            if (await _usuarios.AnyAsync(u => u.NombreDeUsuario == usuario.NombreDeUsuario))
            {
                throw new BadRequestException("Usuario ya existe");
            }
            _usuarios.Add(usuario);

            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateUsuario(Usuario usuario)
        {
            ValidarUsuario(usuario);
            var usuarioActualizar = await _usuarios.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            if (usuarioActualizar == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            usuarioActualizar.NombreDeUsuario = usuario.NombreDeUsuario;
            usuarioActualizar.Password = usuario.Password;
            usuarioActualizar.Rol = usuario.Rol;
            await _dbContext.SaveChangesAsync();

        }

        public async Task DeleteUsuario(Usuario usuario)
        {
            var usuarioEliminar = await _usuarios.FirstOrDefaultAsync(u => u.Id == usuario.Id);
            if (usuarioEliminar == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            _usuarios.Remove(usuarioEliminar);
            await _dbContext.SaveChangesAsync();
        }

        public void ValidarUsuario(Usuario usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.NombreDeUsuario))
            {
                throw new BadRequestException("Nombre de usuario no puede ser vacio");
            }
            
            if (string.IsNullOrWhiteSpace(usuario.Password))
            {
                throw new BadRequestException("Password no puede ser vacio");
            }
            if (string.IsNullOrWhiteSpace(usuario.Rol))
            {
                throw new BadRequestException("Rol no puede ser vacio");
            }
            if (usuario.NombreDeUsuario.Length < 3)
            {
                throw new BadRequestException("Nombre de usuario debe tener al menos 3 caracteres");
            }
            if (usuario.NombreDeUsuario.Length > 20)
            {
                throw new BadRequestException("Nombre de usuario no puede tener mas de 20 caracteres");
            }
            if (usuario.Password.Length < 4)
            {
                throw new BadRequestException("Password debe tener al menos 4 caracteres");
            }
        }
    }
}

