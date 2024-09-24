using Application.ViewModels.Base;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

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
