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
            _usuarios = dbContext.Set<Usuario>();
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

        public async Task<List<Usuario>> GetAllUsuarios()
        {
            return await _usuarios.AsNoTracking().ToListAsync();
        }
        public async Task<Usuario> AddUsuario(Usuario usuario)
        {
            try
            {
                ValidarUsuario(usuario);

                if (await _usuarios.AsNoTracking().AnyAsync(u => u.NombreDeUsuario == usuario.NombreDeUsuario))
                {
                    throw new BadRequestException("Usuario ya existe");
                }

                if (!await _usuarios.AsNoTracking().AnyAsync(u => u.Id == usuario.Id))
                {
                    //si no hay usuarios en la base de datos, asignar id 1
                    usuario.Id = 1;
                    _usuarios.Add(usuario);
                }
                else
                {
                    //obtener el valor maximo de la columna id y sumarle 1
                    usuario.Id = await _usuarios.AsNoTracking().MaxAsync(u => u.Id) + 1;

                    _usuarios.Add(usuario);
                }

                await _dbContext.SaveChangesAsync();

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
                var usuarioActualizar = await _usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == usuario.Id);
                if (usuarioActualizar == null)
                {
                    throw new NotFoundException("Usuario no encontrado");
                }
                usuarioActualizar.NombreDeUsuario = usuario.NombreDeUsuario;
                usuarioActualizar.Password = usuario.Password;
                usuarioActualizar.Rol = usuario.Rol;
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario> DeleteUsuario(int idUsuario)
        {
            try
            {
                var usuarioEliminar = await _usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == idUsuario);
                if (usuarioEliminar == null)
                {
                    throw new NotFoundException("Usuario no encontrado");
                }
                _usuarios.Remove(usuarioEliminar);
                await _dbContext.SaveChangesAsync();
                return usuarioEliminar;
            }
            catch
            {
                throw;
            }
        }

        public async Task<Usuario?> AutenticarUsuario(string nombreDeUsuario, string password)
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

