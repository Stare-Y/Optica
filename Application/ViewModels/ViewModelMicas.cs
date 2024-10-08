using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class ViewModelMicas : ViewModelBase
    {
        private readonly IMicaRepo? _micaRepo;
        private ObservableCollection<Mica> _micas = new ObservableCollection<Mica>();
        private ObservableCollection<String> _pickerTipo = new ObservableCollection<String>();
        private ObservableCollection<String> _pickerMaterial = new ObservableCollection<String>();
        private ObservableCollection<String> _pickerFabricante = new ObservableCollection<String>();
        private ObservableCollection<String> _pickerTratamiento = new ObservableCollection<String>();
        private ObservableCollection<String> _pickerProposito = new ObservableCollection<String>();
        public ViewModelMicas(IMicaRepo micaRepo)
        {
            _micaRepo = micaRepo;
        }
        public ViewModelMicas()  { }

        public async Task Initialize()
        {
            if (_micaRepo == null) { throw new Exception("No se ha inyectado el repositorio de micas"); }

            _micas = new ObservableCollection<Mica>(await _micaRepo.GetAllMicas());
          
            _pickerTipo = new ObservableCollection<string>(await _micaRepo.GetTiposMicas());
            _pickerTipo.Insert(0, "Todos");
            _pickerMaterial = new ObservableCollection<string>(await _micaRepo.GetMaterialMicas());
            _pickerMaterial.Insert(0, "Todos");
            _pickerFabricante = new ObservableCollection<string>(await _micaRepo.GetFabricanteMicas());
            _pickerFabricante.Insert(0, "Todos");
            _pickerTratamiento = new ObservableCollection<string>(await _micaRepo.GetTratamientoMicas());
            _pickerTratamiento.Insert(0, "Todos");
            _pickerProposito = new ObservableCollection<string>(await _micaRepo.GetPropositoMicas());
            _pickerProposito.Insert(0, "Todos");

            OnCollectionChanged(nameof(Micas));
            OnCollectionChanged(nameof(PickerTipo));
            OnCollectionChanged(nameof(PickerMaterial));
            OnCollectionChanged(nameof(PickerFabricante));
            OnCollectionChanged(nameof(PickerTratamiento));
            OnCollectionChanged(nameof(PickerProposito));
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
            get => _pickerFabricante;
            set
            {
                _pickerFabricante = value;
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

        public ObservableCollection<String> PickerTratamiento
        {
            get => _pickerTratamiento;
            set
            {
                _pickerTratamiento = value;
                OnCollectionChanged(nameof(PickerTratamiento));
            }
        }

        public ObservableCollection<String> PickerProposito 
        {
            get => _pickerProposito;
            set
            {
                _pickerProposito = value;
                OnCollectionChanged(nameof(PickerProposito));
            }
        }

    }
}
