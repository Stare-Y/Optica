﻿using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.ViewModels
{
    public class VMNuevaMica : ViewModelBase
    {
        private readonly IMicaRepo _micaRepo = null!;
        private Mica _mica = new();
        public VMNuevaMica() { }
        public VMNuevaMica(IMicaRepo micaRepo)
        {
            _micaRepo = micaRepo;
        }

        public Mica Mica
        {
            get => _mica;
            set
            {
                _mica = value;
                OnPropertyChanged(nameof(Mica));
            }
        }

        public async Task GuardarMica()
        {
            if (_micaRepo == null)
            {
                throw new Exception("No se ha inyectado el repositorio de micas");
            }
            await _micaRepo.InsertMica(_mica);
        }

        /// <summary>
        /// Se inicializa la mica con el siguiente id, pero no llamar si estas creando una mica nueva
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Initialize()
        {
            // get next id for mica
            if (_micaRepo == null)
            {
                throw new Exception("No se ha inyectado el repositorio de micas");
            }
            _mica.Id = await _micaRepo.GetSiguienteId();
        }

        /// <summary>
        /// Actualiza la info de la mica, debe ser ya existente
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ActualizarMica()
        {
            if (_micaRepo == null)
            {
                throw new Exception("No se ha inyectado el repositorio de micas");
            }

            if(_mica.Id == 0)
            {
                throw new Exception("No se puede actualizar una mica sin id");
            }

            await _micaRepo.UpdateMica(_mica);
        }
    }
}
