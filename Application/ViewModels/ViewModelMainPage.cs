using Application.ViewModels.Base;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.ViewModels
{
    public class ViewModelMainPage : ViewModelBase
    {
        private readonly IUsuarioRepo _usuarioRepo;
        private readonly ILogger _logger;
        public ViewModelMainPage(IUsuarioRepo usuarioRepo, ILogger logger)
        {
            _usuarioRepo = usuarioRepo;
            _logger = logger;
        }
    }
}
