using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.ViewModels
{
    public class ViewModelCapturas : ViewModelBase
    {
        private Lote _lote = new Lote();

        private readonly ILoteRepo? _loteRepo;

        public ViewModelCapturas(ILoteRepo loteRepo)
        {
            _loteRepo = loteRepo;
        }

        public ViewModelCapturas() { }

        public Lote Lote
        {
            get => _lote;
            set
            {
                _lote = value;
                OnPropertyChanged(nameof(Lote));
            }
        }

        public async Task Initialize()
        {
            if (_loteRepo is not null)
            {
                _lote.Id = await _loteRepo.GetSiguienteId();
                OnPropertyChanged(nameof(Lote));
            }
            else
            {
                throw new Exception("No se ha inyectado el repositorio de lotes");
            }
        }
    }
}
