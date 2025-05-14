using Application.ViewModels.Base;
using Application.ViewModels.Lotes.DisplayHelpers;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.ObjectModel;

namespace Application.ViewModels.Lotes
{
    public class VMLoteRelationsDetails : ViewModelBase
    {
        private readonly IMicaRepo _micaRepo = null!;
        private readonly ILoteMicaRepo _loteMicaRepo = null!;
        private readonly IMicaGraduacionRepo _micaGraduacionRepo = null!;
        public Lote ParentLote = new();
        public Pedido ParentPedido = new();
        public ObservableCollection<DisplayLoteAndTaken> MicasDelLote 
        {
            get => _micasDelLote; 
            set
            {
                _micasDelLote = value;

                OnCollectionChanged(nameof(MicasDelLote));
            } 
        }

        private ObservableCollection<DisplayLoteAndTaken> _micasDelLote = new();
        private List<LoteMica> _loteMicas = new();
        private List<MicaGraduacion> _micasGraduaciones = new();

        public VMLoteRelationsDetails(IMicaRepo micaRepo, ILoteMicaRepo loteMicaRepo, IMicaGraduacionRepo micaGraduacionRepo)
        {
            _micaRepo = micaRepo;
            _loteMicaRepo = loteMicaRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
        }

        public VMLoteRelationsDetails() { }

        public async Task FetchMicas()
        {
            ParentLote.EnsureIsValid();

            _loteMicas = new(await _loteMicaRepo.GetLotesMicasByLoteIdAsync(ParentLote.Id));

            _micasGraduaciones = new(await _micaGraduacionRepo.GetMicaGraduacionesByMultipleIds(_loteMicas.Select(lm => lm.IdMicaGraduacion).ToList()));

            List<Mica> micas = new(await _micaRepo.GetMicasByIds(_micasGraduaciones.Select(mg => mg.IdMica).ToList()));

            _micasDelLote.Clear();

            foreach (var mica in micas)
            {
                _micasDelLote.Add(new DisplayLoteAndTaken(mica));
            }

            OnCollectionChanged(nameof(MicasDelLote));
        }
    }
}
