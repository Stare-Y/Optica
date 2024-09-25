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
        private ObservableCollection<String> _pickerTipo;
        private ObservableCollection<String> _pickerMaterial;
        private ObservableCollection<String> pickerFabricante;
        public ViewModelMicas(IMicaRepo micaRepo)
        {
            _micaRepo = micaRepo;
        }

        public ViewModelMicas()  { }

        public async Task Initialize()
        {
            _micas = new ObservableCollection<Mica>(await _micaRepo.GetAllMicas());

            
            _pickerTipo = new ObservableCollection<string>(await _micaRepo.GetTiposMicas());
            _pickerTipo.Insert(0, "Todos");
            _pickerMaterial = new ObservableCollection<string>(await _micaRepo.GetMaterialMicas());
            _pickerMaterial.Insert(0, "Todos");
            pickerFabricante = new ObservableCollection<string>(await _micaRepo.GetFabricanteMicas());
            pickerFabricante.Insert(0, "Todos");

            OnCollectionChanged(nameof(Micas));
            OnCollectionChanged(nameof(PickerTipo));
            OnCollectionChanged(nameof(PickerMaterial));
            OnCollectionChanged(nameof(PickerFabricante));
        }

        public ObservableCollection<String> PickerTipo
        {
            get => _pickerTipo;
            set
            {
                _pickerTipo = value;
                OnPropertyChanged(nameof(PickerTipo));
            }
        }

        public ObservableCollection<String> PickerMaterial
        {
            get => _pickerMaterial;
            set
            {
                _pickerMaterial = value;
                OnPropertyChanged(nameof(PickerMaterial));
            }
        }

        public ObservableCollection<String> PickerFabricante
        {
            get => pickerFabricante;
            set
            {
                pickerFabricante = value;
                OnPropertyChanged(nameof(PickerFabricante));
            }
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
