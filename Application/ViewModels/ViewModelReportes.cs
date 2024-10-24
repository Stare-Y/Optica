using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.Reportes.Entities;
using Infrastructure.Data.Repos;
using System;
using System.Collections.ObjectModel;


namespace Application.ViewModels
{
    public class ViewModelReportes : ViewModelBase
    {


        private ObservableCollection<ReportePedido> _reportePedidos = new ObservableCollection<ReportePedido>();
        private readonly IPedidoRepo? _pedidoRepo;
        public ViewModelReportes() { }

        public ViewModelReportes(IPedidoRepo pedidoRepo)
        {
            _pedidoRepo = pedidoRepo;
        }

        public ObservableCollection<ReportePedido> ReportePedidos
        {
            get => _reportePedidos;
            set
            {
                _reportePedidos = value;
                OnPropertyChanged(nameof(ReportePedidos));
            }
        }

        public DateTime FechaInicio { get; set; } = DateTime.Now;
        public DateTime FechaFin { get; set; } = DateTime.Now;

        public async Task GetReportePedidos()
        {
            if (_pedidoRepo is null)
            {
                throw new Exception("No se ha inyectado el repositorio de pedidos");
            }

            if(FechaInicio > FechaFin)
            {
                throw new Exception("La fecha de inicio no puede ser mayor a la fecha de fin");
            }

            var reportePedidos = await _pedidoRepo.GenerarReporte(FechaInicio, FechaFin);
            ReportePedidos = new ObservableCollection<ReportePedido>(reportePedidos);
            if (ReportePedidos.Count <= 0)
            {
                throw new Exception("No se encontraron pedidos en el rango de fechas especificado");
            }
        }

        public async Task ExportarReporteObtenido()
        {
            if(_reportePedidos.Count <= 0)
                throw new Exception("No hay datos para exportar");
            await Task.Delay(1000);
        }
    }
}




