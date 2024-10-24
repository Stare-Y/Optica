using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class VMLogin : ViewModelBase
    {
        private IUsuarioRepo _usuarioRepo = null!;
        private Usuario _usuarioSeleccionado = new();

        public VMLogin(IUsuarioRepo usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        public VMLogin()
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

        public async Task IniciarSesion(string user, string pass)
        {
            _usuarioSeleccionado = await _usuarioRepo.AutenticarUsuario(user, pass);
        }
    }
}
