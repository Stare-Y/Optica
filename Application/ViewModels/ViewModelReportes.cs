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
        private Pedido _pedido = new Pedido();
        private readonly IPedidoRepo? _pedidoRepo;
        public ViewModelReportes() { }

        public ViewModelReportes(IPedidoRepo pedidoRepo)
        {
            _pedidoRepo = pedidoRepo;
        }

        public Pedido Pedido
        {
            get => _pedido;
            set
            {
                _pedido = value;
                OnPropertyChanged(nameof(Pedido));
            }
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

        public async void GetReportePedidos(DateTime fechaInicio, DateTime fechaFin)
        {
            if (_pedidoRepo is null)
            {
                throw new Exception("No se ha inyectado el repositorio de pedidos");
            }
            var reportePedidos = await _pedidoRepo.GenerarReporte(fechaInicio, fechaFin);
            ReportePedidos = new ObservableCollection<ReportePedido>(reportePedidos);
        }
    }
}




