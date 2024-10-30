using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.DisplayEntities;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMConsultarStockMica : ViewModelBase
    {
        private readonly IMicaRepo _micaRepo = null!;
        private readonly IMicaGraduacionRepo _micaGraduacionRepo = null!;
        private ObservableCollection<ShowConsultaStock> _showConsultaStock = new ();
        public Mica mica {  get; set; } = null!;
        public VMConsultarStockMica(IMicaRepo micaRepo, IMicaGraduacionRepo micaGraduacionRepo)
        {
            _micaRepo = micaRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
        }
        public VMConsultarStockMica () {}

        public ObservableCollection<ShowConsultaStock> ShowConsultaStock
        {
            get => _showConsultaStock;
            set
            {
                _showConsultaStock = value;
                OnCollectionChanged(nameof(ShowConsultaStock));
            }
        }

        public async Task Initialize()
        {
            if (this.mica == null)
                throw new Exception("No se ha asignado una mica a la vista");

            var micasGraduaciones = await _micaGraduacionRepo.GetMicaGraduacionByMicaId(this.mica.Id);
            foreach (var micaGraduacion in micasGraduaciones)
            {
                var stock = await _micaRepo.GetStock(micaGraduacion.Id);
                var caducidad = await _micaRepo.GetCaducidad(micaGraduacion.Id) ?? DateTime.MinValue;
                _showConsultaStock.Add(new ShowConsultaStock
                {
                    MicaGraduacion = micaGraduacion,
                    Stock = stock,
                    Caducidad = caducidad
                });
            }
            OnCollectionChanged(nameof(ShowConsultaStock));
        }

    }
}
