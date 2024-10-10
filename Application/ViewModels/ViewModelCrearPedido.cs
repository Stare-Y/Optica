using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.Reportes.Entities;
using Infrastructure.Data.Repos;
using System;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    internal class ViewModelCrearPedido : ViewModelBase 
    {
        private ObservableCollection<Pedido> _pedidos = new ObservableCollection<Pedido>();
        private Pedido _pedido = new Pedido();
        private readonly IPedidoRepo? _pedidoRepo;
        public ViewModelCrearPedido() { }

        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ViewModelCrearPedido(IPedidoRepo pedidoRepo)
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

        public ObservableCollection<Pedido> pedidos
        {
            get => _pedidos;
            set
            {
                _pedidos = value;
                OnPropertyChanged(nameof(Pedidos));
            }
        }

        public async void AddPedido(IEnumerable<PedidoMica>? pedidoMica)
        {
            if (_pedidoRepo is null)
            {
                throw new Exception("No se ha inyectado el repositorio de pedidos");
            }
            var nuevoPedido = await _pedidoRepo.AddPedido(_pedido, pedidoMica);
            Pedidos.Add(nuevoPedido);
        }

        /*public async void GetPedidos()
        {
            if (_pedidoRepo is null)
            {
                throw new Exception("No se ha inyectado el repositorio de pedidos");
            }
            var pedidos = await _pedidoRepo.GetAllPedidos();
            Pedidos = new ObservableCollection<Pedido>(pedidos);
        }*/ 
    }

}



