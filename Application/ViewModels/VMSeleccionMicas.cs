using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMSeleccionMicas : ViewModelBase
    {
        private ObservableCollection<Mica> _micas = new ObservableCollection<Mica>();
        private Lote _lote = new();
        private ILoteRepo? _loteRepo;

        public VMSeleccionMicas() { }

        public VMSeleccionMicas(ILoteRepo loteRepo)
        {
            _loteRepo = loteRepo;
        }

        public Lote Lote
        {
            get => _lote;
            set
            {
                _lote = value;
            }
        }

        public ObservableCollection<Mica> MicasSeleccionadas
        {
            get => _micas;
            set
            {
                _micas = value;
                OnPropertyChanged(nameof(MicasSeleccionadas));
            }
        }

        public List<LoteMica> LoteMicas { get; set; } = new List<LoteMica>();

        public async Task SaveLote()
        {
            if (_loteRepo == null)
            {
                throw new Exception("No se ha inyectado el repositorio de lotes");
            }
            ValidarLoteMica();
            await _loteRepo.AddLote(_lote, LoteMicas);
        }

        private void ValidarLoteMica()
        {
            //valdar que lotemicas tenga elementos y todos tengan stock
            if (LoteMicas.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos una mica");
            }
            foreach (var loteMica in LoteMicas)
            {
                if (loteMica.Stock <= 0)
                {
                    throw new Exception("El stock de las micas debe ser mayor a 0");
                }
            }
        }
    }
}
