using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Data.Repos
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private readonly OpticaDbContext _dbContext;
        private readonly DbSet<Usuario> _usuarios;
        public UsuarioRepo(OpticaDbContext dbContext)
        {
            _dbContext = dbContext;
            _usuarios = dbContext.Usuarios;
        }
        public async Task<Usuario> GetUsuarioById(int idUsuario)
        {
            var usuario = await _usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == idUsuario);
            if (usuario == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            return usuario;
        }

        public async Task<Usuario> GetUsuarioByNombreDeUsuario(string nombreDeUsuario)
        {
            var usuario = await _usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.NombreDeUsuario == nombreDeUsuario);
            if (usuario == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            return usuario;
        }

        public async Task<List<Usuario>> GetAllUsuarios()
        {
            return await _usuarios.AsNoTracking().ToListAsync();
        }
        public async Task<Usuario> AddUsuario(Usuario usuario)
        {
            try
            {
                ValidarUsuario(usuario);

                if (usuario.Id != 0)
                {
                    throw new BadRequestException("El ID del usuario debe ser 0");
                }

                if (await _usuarios.AsNoTracking().AnyAsync(u => u.NombreDeUsuario == usuario.NombreDeUsuario))
                {
                    throw new BadRequestException("El Usuario ya existe");
                }
                else
                {
                    _usuarios.Add(usuario);
                }

                await _dbContext.SaveChangesAsync();

                _dbContext.Entry(usuario).State = EntityState.Detached;

                return usuario;
            }
            catch
            {

               throw;
            }
        }
        public async Task UpdateUsuario(Usuario usuario)
        {
            try
            {
                ValidarUsuario(usuario);

                // Deshabilitar el tracking
                var existingUsuario = await _dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == usuario.Id);

                if (existingUsuario == null)
                {
                    throw new Exception("Usuario no encontrado.");
                }

                // Adjuntar el usuario y marcarlo como modificado
                _dbContext.Entry(usuario).State = EntityState.Modified;

                await _dbContext.SaveChangesAsync();

                // Remover el tracking
                _dbContext.Entry(usuario).State = EntityState.Detached;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario> DeleteUsuario(int idUsuario)
        {
            
            var usuarioEliminar = await _usuarios.FirstOrDefaultAsync(u => u.Id == idUsuario);
            if (usuarioEliminar == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            _usuarios.Remove(usuarioEliminar);
            await _dbContext.SaveChangesAsync();

            // Remover el tracking
            _dbContext.Entry(usuarioEliminar).State = EntityState.Detached;

            return usuarioEliminar;
        }

        public async Task<Usuario> AutenticarUsuario(string nombreDeUsuario, string password)
        {
            var usuario = await _usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.NombreDeUsuario == nombreDeUsuario && u.Password == password);
            if (usuario == null)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            return usuario;
        }

        public void ValidarUsuario(Usuario usuario)
        {
            if(usuario == null)
            {
                throw new BadRequestException("Usuario no puede ser nulo");
            }
            if(usuario.NombreDeUsuario == "admin")
            {
                throw new BadRequestException("admin no puede cambiar");
            }
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

        public async Task<List<Usuario>> GetUsuariosByIds(List<int> idsUsuarios)
        {
           return await _usuarios.AsNoTracking().Where(u => idsUsuarios.Contains(u.Id)).ToListAsync();
        }
    }
}

