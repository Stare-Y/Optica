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
            _pedido.Id = await _pedidoRepo.GetSiguienteId();
            OnPropertyChanged(nameof(Pedido));

        }
    }

}



