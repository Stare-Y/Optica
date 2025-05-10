using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMLotesView : ViewModelBase
    {
        private IPedidoRepo _pedidoRepo = null!;
        public ObservableCollection<Lote> LotesElegidos { get; set; } = new();

        private Pedido _pedido = new();
        public Pedido PedidoLevantado
        {
            get => _pedido;
            set 
            { 
                OnPropertyChanged(nameof(PedidoLevantado));
                _pedido = value;
            }
        }

        public VMLotesView(IPedidoRepo pedidoRepo)
        {
            _pedidoRepo = pedidoRepo;
        }

        public VMLotesView() { }

        public void NotifyPedidoRegistered()
        {
            OnPropertyChanged(nameof(PedidoLevantado));
        }

        public List<PedidoMica> PedidoMicas = new();

        public async Task GenerarPedido()
        {
            ValidarPedido();

            await _pedidoRepo.AddPedido(PedidoLevantado, PedidoMicas);

            //TODO: after this, navigate to a view to display the generated pedido.
        }

        private void ValidarPedido()
        {
            PedidoLevantado.EnsureIsValid();

            foreach (var pedidoMica in PedidoMicas)
                pedidoMica.EnsureIsValid();
        }
    }
}
