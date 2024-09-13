using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repos
{
    public class UsuarioRepo : IUsuarioRepo
    {
        private List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario
            {
                Id = 1,
                NombreDeUsuario = "admin",
                Password = "admin123",
                Rol = "Administrador"
            },
            new Usuario
            {
                Id = 2,
                NombreDeUsuario = "user1",
                Password = "user123",
                Rol = "Usuario"
            },
            new Usuario
            {
                Id = 3,
                NombreDeUsuario = "user2",
                Password = "user456",
                Rol = "Usuario"
            }
        };
        public Task<Usuario> GetUsuario(int id)
        {
            return Task.FromResult(usuarios.FirstOrDefault(u => u.Id == id));
        }
        public Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            return Task.FromResult(usuarios.AsEnumerable());
        }
        public Task AddUsuario(Usuario usuario)
        {
            usuarios.Add(usuario);
            return Task.CompletedTask;
        }
        public Task UpdateUsuario(Usuario usuario)
        {
            var index = usuarios.FindIndex(u => u.Id == usuario.Id);
            if (index == -1)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            usuarios[index] = usuario;
            return Task.CompletedTask;
        }
        public Task DeleteUsuario(Usuario usuario)
        {
            var index = usuarios.FindIndex(u => u.Id == usuario.Id);
            if (index == -1)
            {
                throw new NotFoundException("Usuario no encontrado");
            }
            usuarios.RemoveAt(index);
            return Task.CompletedTask;
        }
    }
}

