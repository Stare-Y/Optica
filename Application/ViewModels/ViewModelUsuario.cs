﻿using System.Collections.ObjectModel;
using Domain.Entities;
using Application.ViewModels.Base;
using Domain.Interfaces;

namespace Application.ViewModels
{
    public class ViewModelUsuario : ViewModelBase
    {
        private readonly IUsuarioRepo? _usuarioRepo;
        private Usuario _usuarioSeleccionado = new();
        private ObservableCollection<Usuario> _usuarios = new ObservableCollection<Usuario>();

        // Constructor que recibe el repositorio
        public ViewModelUsuario(IUsuarioRepo usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
            // Carga los usuarios al iniciar el ViewModel
        }

        public ViewModelUsuario()
        {
            
        }

        // Colección de usuarios
        public ObservableCollection<Usuario> Usuarios
        {
            get { return _usuarios; }
            set
            {
                _usuarios = value;
                OnPropertyChanged(nameof(Usuarios));
            }
        }

        // Usuario seleccionado
        public Usuario UsuarioSeleccionado
        {
            get { return _usuarioSeleccionado; }
            set
            {
                
                _usuarioSeleccionado = value;
                OnPropertyChanged(nameof(UsuarioSeleccionado));
                
            }
        }

        public async Task Inicializar ()
        {
            if (_usuarioRepo == null)
                throw new Exception("El repositorio de usuarios es nulo");

            var usuarios = await _usuarioRepo.GetAllUsuarios();

            // quita el usuario de la lista si su nombre de usuarios es igual a stare
            usuarios.RemoveAll(u => u.NombreDeUsuario == "stare");

            _usuarios = new(usuarios);
            OnCollectionChanged(nameof(Usuarios));
        }

        public async Task EliminarUsuario(Usuario usuario)
        {
            if (_usuarioRepo == null)
                throw new Exception("El repositorio de usuarios es nulo");
            await _usuarioRepo.DeleteUsuario(usuario.Id);
            await Inicializar();
        }
    }
}