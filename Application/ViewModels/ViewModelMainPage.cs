using Application.ViewModels.Base;
using Domain.Interfaces;

namespace Application.ViewModels
{
    public class ViewModelMainPage : ViewModelBase
    {
        private readonly IUsuarioRepo _usuarioRepo;
        public ViewModelMainPage(IUsuarioRepo usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

    }
}
