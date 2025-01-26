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


        public void ValidarLote()
        {
            string mensaje = string.Empty;
            if (string.IsNullOrEmpty(_lote.Referencia) && string.IsNullOrWhiteSpace(_lote.Referencia))
            {
                mensaje += "La referencia no puede estar vacia\n";
            }
            if (string.IsNullOrEmpty(_lote.Proveedor) && string.IsNullOrWhiteSpace(_lote.Proveedor))
            {
                mensaje += "El proveedor no puede estar vacio\n";
            }
            if (_lote.FechaCaducidad < _lote.FechaEntrada)
            {
                mensaje += "La fecha de caducidad no puede ser menor a la fecha de entrada\n";
            }
            if (!string.IsNullOrEmpty(mensaje))
            {
                throw new Exception(mensaje);
            }
        }

        public async Task Initialize()
        {
            if (_loteRepo is null)
                throw new Exception("No se ha inyectado el repositorio de lotes");
        }
    }
}
