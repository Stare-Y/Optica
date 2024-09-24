using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.ViewModels
{
    public class ViewModelCapturas : ViewModelBase
    {
        private Lote _lote = new Lote();

        private readonly ILoteRepo _loteRepo;

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

        public void GoToCapturarMicas()
        {
            _loteRepo.ValidarLote(_lote);
            //viermodel de captura de micas, recibe la instancia de lote capturada
            //await Shell.Current.GoToAsync("CapturarMicas");
        }

        public async Task Initialize()
        {
            _lote.Id = await _loteRepo.GetSiguienteId();
            OnPropertyChanged(nameof(Lote));
        }
    }
}
