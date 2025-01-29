using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.ViewModels
{
    public class ViewModelCrearPedido : ViewModelBase 
    {
        private Pedido _pedido = new Pedido();
        private readonly IPedidoRepo _pedidoRepo = null!;
        public ViewModelCrearPedido() { }

        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public ViewModelCrearPedido(IPedidoRepo pedidoRepo)
        {
            _pedidoRepo = pedidoRepo;
            _pedido.FechaSalida = DateTime.Now.Date;
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

        public async Task Initialize()
        {
            if (_pedidoRepo is null)
                throw new Exception("No se ha inyectado el repositorio de pedidos");
            await Task.Delay(200);
        }
    }
}



