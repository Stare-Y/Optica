using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMSeleccionarMicasPedido : ViewModelBase
    {
        private ObservableCollection<Mica> _micas = new ObservableCollection<Mica>();
        private Pedido _pedido = new();
        private IPedidoRepo _pedidoRepo = null!;

        public VMSeleccionarMicasPedido() { }

        public VMSeleccionarMicasPedido(IPedidoRepo pedidoRepo)
        {
            _pedidoRepo = pedidoRepo;
        }

        public Pedido Pedido
        {
            get => _pedido;
            set
            {
                _pedido = value;
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

        public List<PedidoMica> PedidosMicas { get; set; } = new();

        public async Task SavePedido()
        {
            if (_pedidoRepo == null)
            {
                throw new Exception("No se ha inyectado el repositorio de lotes");
            }
            ValidarPedidoMica();
            await _pedidoRepo.AddPedido(_pedido, PedidosMicas);
        }

        private void ValidarPedidoMica()
        {
            //valdar que lotemicas tenga elementos y todos tengan stock
            if (PedidosMicas.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos una mica, con al menos una graduacion");
            }
            foreach (var pedidoMica in PedidosMicas)
            {
                if (pedidoMica.Cantidad <= 0)
                {
                    throw new Exception("El stock de las micas debe ser mayor a 0");
                }
            }
        }
    }
}
