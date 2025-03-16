using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.ObjectModel;

namespace Application.ViewModels.Lotes
{
    public class VMDisplayLotes : ViewModelBase
    {
        private ILoteRepo _loteRepo = null!;
        public ObservableCollection<Lote> _lotes = null!;
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
            Lotes = new(await _loteRepo.GetValidLotesAsync());
            OnCollectionChanged(nameof(Lotes));
        }
    }
}
