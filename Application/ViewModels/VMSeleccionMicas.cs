﻿using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMSeleccionMicas : ViewModelBase
    {
        private ObservableCollection<Mica> _micas = new ObservableCollection<Mica>();
        private Lote _lote = new();
        private ILoteRepo _loteRepo = null!;
        private IMicaGraduacionRepo _micaGraduacionRepo = null!;

        public Usuario Usuario { get; set; } = new();
        public VMSeleccionMicas() { }

        public VMSeleccionMicas(ILoteRepo loteRepo, IMicaGraduacionRepo micaGraduacionRepo)
        {
            _loteRepo = loteRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
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

        /// <summary>
        /// Llenar al capturar en view de tabla graduaciones, y utilizar para guardar en la base de datos
        /// </summary>
        public List<LoteMica> LoteMicas { get; set; } = new List<LoteMica>();

        public async Task<List<LoteMica>> AlreadySelectedLoteMicas(int idMica)
        {
            if (_loteRepo == null)
            {
                throw new Exception("No se ha inyectado el repositorio de lotes");
            }
            var corresponding = new List<LoteMica>();
            foreach (var item in LoteMicas)
            {
                var graduacion = await _micaGraduacionRepo.GetMicaGraduacionById(item.IdMicaGraduacion);
                if (graduacion.IdMica == idMica)
                {
                    corresponding.Add(item);
                }
            }
            return corresponding;
        }

        public void EliminarLoteMicaPorId(int idMica)
        {
           
            LoteMicas = LoteMicas.Where(item => item.IdMicaGraduacion != idMica).ToList();
            OnPropertyChanged(nameof(LoteMicas));
        }

        public async Task SaveLote()
        {
            if (_loteRepo == null)
            {
                throw new Exception("No se ha inyectado el repositorio de lotes");
            }

            await ValidarLoteMica();

            _lote.Existencias = LoteMicas.Sum(lm => lm.Cantidad);

            await _loteRepo.AddLote(_lote, LoteMicas);
        }

        private async Task ValidarLoteMica()
        {
            //valdar que lotemicas tenga elementos y todos tengan stock
            if (LoteMicas.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos una Graduacion para cada mica");
            }
            foreach (var loteMica in LoteMicas)
            {
                if (loteMica.Cantidad <= 0)
                {
                    throw new Exception("El stock de las micas debe ser mayor a 0");
                }
            }

            //si hay una micaseleccionada que no esta en la lista de lotesmicas, tirar excepcion
            foreach (var mica in MicasSeleccionadas)
            {
                var existentes = await AlreadySelectedLoteMicas(mica.Id);
                if (existentes.Count < 1)
                    throw new Exception("No se ha seleccionado la graduacion de la mica de tipo: " + mica.Tipo);
            }
        }
    }
}
