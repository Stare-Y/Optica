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
        private Pedido _pedido;
        private readonly IPedidoRepo _pedidoRepo;
        public ViewModelCrearPedido() { }

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

        public ObservableCollection<Pedido> Pedidos
        {
            get => _pedidos;
            set
            {
                _pedidos = value;
                OnPropertyChanged(nameof(Pedidos));
            }
        }

        //public async void AddPedido()
        //{
         //   await _pedidoRepo.AddPedido(Pedido);
       // }

        public async void GetPedidos()
        {
            var pedidos = await _pedidoRepo.GetAllPedidos();
            Pedidos = new ObservableCollection<Pedido>(pedidos);
        }




    }
}


