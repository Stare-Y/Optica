using Application.ViewModels.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.DisplayEntities;
using System.Collections.ObjectModel;

namespace Application.ViewModels
{
    public class VMTablaGraduaciones : ViewModelBase
    {
        private readonly IMicaGraduacionRepo _micaGraduacionRepo = null!;
        private Lote? _lote = null;
        private Pedido? _pedido = null;
        private Mica _mica = null!;
        private ObservableCollection<DisplayMicaGraduacionAndDetails> _micasGraduacionAndDetails = null!;
        public VMTablaGraduaciones(IMicaGraduacionRepo micaGraduacionRepo)
        {
            _micaGraduacionRepo = micaGraduacionRepo;
        }
        public VMTablaGraduaciones()
        {
        }

        public ObservableCollection<DisplayMicaGraduacionAndDetails> MicasGraduacion
        {
            get => _micasGraduacionAndDetails;
            set
            {
                _micasGraduacionAndDetails = value;
                OnCollectionChanged(nameof(MicasGraduacion));
            }
        }

        public Lote? Lote
        {
            get => _lote;
            set
            {
                _lote = value;
                OnPropertyChanged(nameof(Lote));
            }
        }

        public Pedido? Pedido
        {
            get => _pedido;
            set
            {
                _pedido = value;
                OnPropertyChanged(nameof(Pedido));
            }
        }

        public Mica Mica
        {
            get => _mica;
            set
            {
                _mica = value;
                OnPropertyChanged(nameof(Mica));
            }
        }

        public async Task AddSelectedMicaGraduacion(MicaGraduacion micaGraduacion, int cantidad)
        {
            if (micaGraduacion.IdMica == 0)
            {
                throw new Exception("(DEV)Debes asignar la mica a la que pertenece la graduacion");
            }
            if (cantidad <= 0)
            {
                throw new Exception("(DEV)La cantidad/stock no puede ser menor o igual a 0");
            }

            var graduacion = await _micaGraduacionRepo.GetMicaGraduacionByGraduacion(micaGraduacion.Graduacioncil, micaGraduacion.Graduacionesf);

            //Si no existe la graduacion, la agregamos y obtenemos el id
            graduacion ??= await _micaGraduacionRepo.AddMicaGraduacion(micaGraduacion);

            graduacion.Precio = micaGraduacion.Precio;

            DisplayMicaGraduacionAndDetails? displayMicaGraduacionAndDetails = null;

            if (_lote is null)
            {
                if (_pedido is null)
                {
                    throw new Exception("(DEV)No se ha asignado ni pedido ni lote a la tabla de graduaciones");
                }
                displayMicaGraduacionAndDetails = new DisplayMicaGraduacionAndDetails
                {
                    MicaGraduacion = graduacion,
                    LoteMica = null,
                    PedidoMica = new PedidoMica
                    {
                        Cantidad = cantidad,
                        IdMicaGraduacion = graduacion.Id,
                        IdPedido = _pedido.Id
                    }
                };
            }
            else
            {
                displayMicaGraduacionAndDetails = new DisplayMicaGraduacionAndDetails
                {
                    MicaGraduacion = graduacion,
                    LoteMica = new LoteMica
                    {
                        IdLote = _lote.Id,
                        Stock = cantidad,
                        IdMicaGraduacion = graduacion.Id,
                        FechaCaducidad = _lote.FechaCaducidad
                    },
                    PedidoMica = null
                };
            }

            if (displayMicaGraduacionAndDetails is null)
            {
                throw new Exception("(DEV)No se ha asignado ni pedido ni lote a la tabla de graduaciones");
            }
            else
                MicasGraduacion.Add(displayMicaGraduacionAndDetails);

            OnCollectionChanged(nameof(MicasGraduacion));
        }

        public async Task ValidarPrecios()
        {
            try
            {
                if (MicasGraduacion.Count <= 0)
                {
                    throw new Exception("No hay graduaciones para validar");
                }

                await _micaGraduacionRepo.ValidarPrecios(MicasGraduacion.Select(mg => mg.MicaGraduacion));
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al validar precios: {ex.Message}");
            }
        }

        public async Task FillListFromPedidoMica(List<PedidoMica> pedidoMicas)
        {
            foreach (var pedidoMica in pedidoMicas)
            {
                var micaGraduacion = await _micaGraduacionRepo.GetMicaGraduacionById(pedidoMica.IdMicaGraduacion);
                MicasGraduacion.Add(new DisplayMicaGraduacionAndDetails
                {
                    MicaGraduacion = micaGraduacion,
                    LoteMica = null,
                    PedidoMica = pedidoMica
                });
            }
        }

        public async Task FillListFromLoteMica(List<LoteMica> lotesMicas)
        {
            //obtener micasgraduaciones de lotesmicas
            foreach (var loteMica in lotesMicas)
            {
                var micaGraduacion = await _micaGraduacionRepo.GetMicaGraduacionById(loteMica.IdMicaGraduacion);
                MicasGraduacion.Add(new DisplayMicaGraduacionAndDetails
                {
                    MicaGraduacion = micaGraduacion,
                    LoteMica = loteMica,
                    PedidoMica = null
                });
            }
        }
    }
}
