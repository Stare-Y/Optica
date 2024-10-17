using System.Collections.ObjectModel;
using Domain.Entities;
using Application.ViewModels.Base;
using Domain.Interfaces;

namespace Application.ViewModels
{
    class ViewModelEditarUsuario : ViewModelBase
    {
        private readonly IUsuarioRepo? _usuarioRepo;
        private Usuario _usuarioSeleccionado = new();
        private ObservableCollection<Usuario> _usuarios = new ObservableCollection<Usuario>();
        public ViewModelEditarUsuario(IUsuarioRepo usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }
        public ViewModelEditarUsuario()
        {

        }
        public ObservableCollection<Usuario> Usuarios
        {
            get { return _usuarios; }
            set
            {

                _usuarios = value;
                OnPropertyChanged(nameof(Usuarios));

            }
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
        public async Task Inicializar()
        {
            if (_usuarioRepo == null)
                throw new Exception("El repositorio de usuarios es nulo");
            _usuarios = new(await _usuarioRepo.GetAllUsuarios());
            OnCollectionChanged(nameof(Usuarios));
        }

    }
}
