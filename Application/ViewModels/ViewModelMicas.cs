using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ViewModelMicas : ViewModelBase
    {
        private readonly IMicaRepo _micaRepo;
        private ObservableCollection<Mica> _micas;
        public ViewModelMicas(IMicaRepo micaRepo)
        {
            _micaRepo = micaRepo;
        }

        public ViewModelMicas()  { }

        public async Task Initialize()
        {
            _micas = new ObservableCollection<Mica>(await _micaRepo.GetAllMicas());
            OnCollectionChanged(nameof(Micas));
        }

        public ObservableCollection<Mica> Micas
        {
            get => _micas; 
            set
            {
                _micas = value;
                OnCollectionChanged(nameof(Micas));
            }
        }

    }
}
