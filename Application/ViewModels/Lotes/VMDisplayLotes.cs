using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Repos;
using System.Collections.ObjectModel;

namespace Application.ViewModels.Lotes
{
    public class VMDisplayLotes : ViewModelBase
    {
        private ILoteRepo _loteRepo = null!;
        public ObservableCollection<Lote> _lotes = null!;
        public ObservableCollection<string> _pickerProveedor = new ObservableCollection<string>();
        public ObservableCollection<string> _pickerReferencia = new ObservableCollection<string>();
        public ObservableCollection<DateTime> _pickerFechaIngreso = new ObservableCollection<DateTime>();
        public ObservableCollection<DateTime> _pickerFechaCaducidad = new ObservableCollection<DateTime>();
        public ObservableCollection<Lote> Lotes 
        { 
            get => _lotes; 
            set 
            { 
                OnCollectionChanged(nameof(Lotes));
                _lotes = value; 
            } 
        }

        public VMDisplayLotes(ILoteRepo loteRepo)
        {
            _loteRepo = loteRepo;
        }

        public VMDisplayLotes() { }

        public async Task FetchLotes()
        {
            _lotes = new(await _loteRepo.GetValidLotesAsync());
            OnCollectionChanged(nameof(Lotes));
        }

        public ObservableCollection<String> PickerProveedor
        {
            get => _pickerProveedor;
            set
            {
                _pickerProveedor = value;
                OnPropertyChanged(nameof(PickerProveedor));
            }
        }

        public ObservableCollection<String> PickerReferencia
        {
            get => _pickerReferencia;
            set
            {
                _pickerReferencia = value;
                OnPropertyChanged(nameof(PickerReferencia));
            }
        }

        public ObservableCollection<DateTime> PickerFechaIngreso
        {
            get => _pickerFechaIngreso;
            set
            {
                _pickerFechaIngreso = value;
                OnPropertyChanged(nameof(PickerFechaIngreso));
            }
        }

        public ObservableCollection<DateTime> PickerFechaCaducidad
        {
            get => _pickerFechaCaducidad;
            set
            {
                _pickerFechaCaducidad = value;
                OnPropertyChanged(nameof(PickerFechaCaducidad));
            }
        }


        /*public async Task AplicarFiltros(string proveedor, string referencia, DateTime? fechaingreso, DateTime? fechacaducidad)
        {
            if (_loteRepo == null) { throw new Exception("No se ha inyectado el repositorio de lotes"); }

            if (proveedor == "Todos") { proveedor = string.Empty; }
            if (referencia == "Todos") { referencia = string.Empty; }
            DateTime? ingreso = fechaingreso.HasValue ? fechaingreso.Value : null;
            DateTime? caducidad = fechacaducidad.HasValue ? fechacaducidad.Value : null;

            //Lotes = new ObservableCollection<Lote>(await _loteRepo.GetMicasByFiltros(tipo, material, fabricante, tratamiento, proposito));
        }*/
    }
}
