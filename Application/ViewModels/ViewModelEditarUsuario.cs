using System.Collections.ObjectModel;
using Domain.Entities;
using Application.ViewModels.Base;
using Domain.Interfaces;

namespace Application.ViewModels
{
    public class ViewModelEditarUsuario : ViewModelBase
    {
        private readonly IUsuarioRepo? _usuarioRepo;
        private Usuario _usuarioSeleccionado = new();

        public ViewModelEditarUsuario(IUsuarioRepo usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public ViewModelEditarUsuario()
        {

        }

        public Usuario UsuarioSeleccionado
        {
            get { return _usuarioSeleccionado; }
            set
            {

                _usuarioSeleccionado = value;
                OnPropertyChanged(nameof(UsuarioSeleccionado));

            }
        }

        public async Task GuardarUsuario()
        {
            if (_usuarioRepo == null)
                throw new Exception("El repositorio de usuarios es nulo");

            if (_usuarioSeleccionado == null)
                throw new Exception("El usuario seleccionado es nulo");

            if(_usuarioSeleccionado.Id == 0)
                await _usuarioRepo.AddUsuario(_usuarioSeleccionado);
            else
                await _usuarioRepo.UpdateUsuario(_usuarioSeleccionado);
        }
    }
}
