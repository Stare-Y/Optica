using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services;

namespace Application.ViewModels
{
    public class ViewModelMainPage : ViewModelBase
    {
        private Usuario _usuario = null!;
        public ViewModelMainPage()
        {
        }

        public Usuario Usuario
        {
            get { return _usuario; }
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }
    }
}
