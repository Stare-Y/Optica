using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            var viewModel = _serviceProvider.GetService(typeof(TViewModel));
            if (viewModel == null)
            {
                throw new InvalidOperationException($"No se encontró el ViewModel {typeof(TViewModel).Name}");
            }
            var viewType = viewModel.GetType().BaseType?.GenericTypeArguments[0];
            var view = _serviceProvider.GetService(viewType);
            if (view == null)
            {
                throw new InvalidOperationException($"No se encontró la vista para el ViewModel {typeof(TViewModel).Name}");
            }
            var navigationService = _serviceProvider.GetService(typeof(INavigationService));
            var navigateMethod = navigationService.GetType().GetMethod("Navigate");
            navigateMethod?.Invoke(navigationService, new object[] { view });

        }
    }
}
