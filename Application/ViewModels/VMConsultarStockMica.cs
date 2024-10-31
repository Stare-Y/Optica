using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.DisplayEntities;
using Microsoft.Extensions.Primitives;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMConsultarStockMica : ViewModelBase
    {
        private readonly IMicaRepo _micaRepo = null!;
        private readonly IMicaGraduacionRepo _micaGraduacionRepo = null!;
        private ObservableCollection<ShowConsultaStock> _showConsultaStock = new ();
        private List<PedidoMica> _pedidoMicas { get; set; } = new List<PedidoMica>();
        public Mica mica {  get; set; } = null!;
        public Pedido pedido { get; set; } = null!;
        public VMConsultarStockMica(IMicaRepo micaRepo, IMicaGraduacionRepo micaGraduacionRepo)
        {
            _micaRepo = micaRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
        }
        public VMConsultarStockMica () {}

        public ObservableCollection<ShowConsultaStock> ShowConsultaStock
        {
            get => _showConsultaStock;
            set
            {
                _showConsultaStock = value;
                OnCollectionChanged(nameof(ShowConsultaStock));
            }
        }

        public async Task Initialize()
        {
            if (this.mica == null)
                throw new Exception("No se ha asignado una mica o pedido a la vista");

            var micasGraduaciones = await _micaGraduacionRepo.GetMicaGraduacionByMicaId(this.mica.Id);
            foreach (var micaGraduacion in micasGraduaciones)
            {
                var stock = await _micaRepo.GetStock(micaGraduacion.Id);
                var caducidad = await _micaRepo.GetCaducidad(micaGraduacion.Id) ?? DateTime.MinValue;
                if(stock <= 0)
                    continue;
                _showConsultaStock.Add(new ShowConsultaStock
                {
                    MicaGraduacion = micaGraduacion,
                    Stock = stock,
                    Caducidad = caducidad
                });
            }
            OnCollectionChanged(nameof(ShowConsultaStock));
        }

        public void FillFromPedidoMicas(List<PedidoMica> pedidos)
        {
            foreach (var pedido in pedidos)
            {
                var mica = _showConsultaStock.FirstOrDefault(mg => mg.MicaGraduacion.Id == pedido.IdMicaGraduacion);
                if (mica == null)
                    continue;
                mica.Taken = pedido.Cantidad;
            }
            OnCollectionChanged(nameof(ShowConsultaStock));
        }

        public List<PedidoMica> GetPedidosMicas()
        {
            if (this.mica == null)
                throw new Exception("No se ha asignado una mica a la vista");
            if (this.pedido == null)
                throw new Exception("No se ha asignado un pedido a la vista");
            _pedidoMicas.Clear();

            foreach (var micaGraduacion in _showConsultaStock)
            {
                if(micaGraduacion.Taken == 0)
                    continue;

                var pedidosMicas = new PedidoMica
                {
                    IdPedido = pedido.Id,
                    IdMicaGraduacion = micaGraduacion.MicaGraduacion.Id,
                    Cantidad = micaGraduacion.Taken,
                    FechaAsignacion = pedido.FechaSalida

                };
                _pedidoMicas.Add(pedidosMicas);
            }
            return _pedidoMicas;
        }
    }
}
