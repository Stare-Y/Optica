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
        private IMicaGraduacionRepo _micaGraduacionRepo = null!;

        /// <summary>
        /// Llenar al capturar en view de tabla graduaciones, y utilizar para guardar en la base de datos
        /// </summary>
        public List<MicaGraduacion> MicaGraduaciones { get; set; } = new List<MicaGraduacion>();

        public VMSeleccionarMicasPedido() { }

        public VMSeleccionarMicasPedido(IPedidoRepo pedidoRepo, IMicaGraduacionRepo micaGraduacionRepo)
        {
            _pedidoRepo = pedidoRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
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

        public async Task DiscardMica(int idMica)
        {
            foreach (var mica in _micas)
            {
                if (mica.Id == idMica)
                {
                    _micas.Remove(mica);

                    foreach(var pedidoMica in PedidosMicas)
                    {
                        var micaGraduacion = await _micaGraduacionRepo.GetMicaGraduacionById(pedidoMica.IdMicaGraduacion);

                        if (micaGraduacion.IdMica == idMica)
                        {
                            PedidosMicas.Remove(pedidoMica);
                        }
                    }

                    OnCollectionChanged(nameof(MicasSeleccionadas));
                    return;
                }
            }

            throw new Exception("No se ha encontrado la mica seleccionada");
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
                if (pedidoMica.Cantidad < 1)
                {
                    throw new Exception("El stock de las micas debe ser mayor a 0");
                }
            }
        }
    }
}
