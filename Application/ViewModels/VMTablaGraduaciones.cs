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
        private readonly ILoteMicaRepo _loteMicaRepo = null!;

        private Lote? _lote = null;
        private Pedido? _pedido = null;
        private Mica _mica = null!;
        private ObservableCollection<DisplayMicaGraduacionAndDetails> _micasGraduacionAndDetails = new();
        public VMTablaGraduaciones(IMicaGraduacionRepo micaGraduacionRepo, ILoteMicaRepo loteMicaRepo)
        {
            _micaGraduacionRepo = micaGraduacionRepo;
            _loteMicaRepo = loteMicaRepo;
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

        public List<LoteMica> LotesMicas = null!;

        public List<MicaGraduacion> MicasGraduacionList = null!;

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

            DisplayMicaGraduacionAndDetails? displayMicaGraduacionAndDetails = null;

            if (_lote is null)
            {
                if (_pedido is null)
                {
                    throw new Exception("(DEV)No se ha asignado ni pedido ni lote a la tabla de graduaciones");
                }
                displayMicaGraduacionAndDetails = new DisplayMicaGraduacionAndDetails
                {
                    MicaGraduacion = micaGraduacion,
                    LoteMica = null,
                    PedidoMica = new PedidoMica
                    {
                        Cantidad = cantidad,
                        IdMicaGraduacion = micaGraduacion.Id,
                        IdPedido = _pedido.Id
                    }
                };
            }
            else
            {
                displayMicaGraduacionAndDetails = new DisplayMicaGraduacionAndDetails
                {
                    MicaGraduacion = micaGraduacion,
                    LoteMica = new LoteMica
                    {
                        IdLote = _lote.Id,
                        Cantidad = cantidad,
                        IdMicaGraduacion = micaGraduacion.Id,
                    },
                    PedidoMica = null
                };
            }

            if (displayMicaGraduacionAndDetails is null)
            {
                throw new Exception("(DEV)No se ha asignado ni pedido ni lote a la tabla de graduaciones");
            }
            else
            {
                var existente = MicasGraduacion
                    .FirstOrDefault(mg =>
                        mg.MicaGraduacion.Graduacionesf == displayMicaGraduacionAndDetails.MicaGraduacion.Graduacionesf &&
                        mg.MicaGraduacion.Graduacioncil == displayMicaGraduacionAndDetails.MicaGraduacion.Graduacioncil);

                if (existente != null)
                {
                    MicasGraduacion.Remove(existente);
                }

                MicasGraduacion.Add(displayMicaGraduacionAndDetails);
            }

            OnCollectionChanged(nameof(MicasGraduacion));
        }

        public async Task EnsureGraduacionesExist()
        {
            foreach(var micaGraduacion in MicasGraduacion)
            {
                var graduacion = await _micaGraduacionRepo.GetMicaGraduacionByGraduacion(micaGraduacion.MicaGraduacion.Graduacionesf, micaGraduacion.MicaGraduacion.Graduacioncil, micaGraduacion.MicaGraduacion.IdMica);

                //Si no existe la graduacion, la agregamos y obtenemos el id
                if(graduacion is null)
                {
                    graduacion = await _micaGraduacionRepo.AddMicaGraduacion(micaGraduacion.MicaGraduacion);
                }

                //update the record on MicasGraduacion, because the repo generated or found an micagraduacion.id
                micaGraduacion.MicaGraduacion.Id = graduacion.Id;

                if (_lote is null)
                {
                    if (micaGraduacion.PedidoMica is not null)
                        micaGraduacion.PedidoMica.IdMicaGraduacion = graduacion.Id;
                    else
                        throw new Exception("Hubo un error de logica, se intento guardar el registro de pedidomica, pero pedidomica salio nulo en esta iteracion");
                }
                else
                {
                    if (micaGraduacion.LoteMica is not null)
                        micaGraduacion.LoteMica.IdMicaGraduacion = graduacion.Id;
                    else
                        throw new Exception("Error de logica, se intento guardar el registro lotemica pero lotemica fue nulo en esta iteracion");
                }
            }
        }

        public async Task PrepareLotesMicas(int idMica, int idLote)
        {
            LotesMicas = new (await _loteMicaRepo.GetLotesMicasByLoteIdAsync(idLote));


            MicasGraduacionList = new(await _micaGraduacionRepo.GetMicaGraduacionByMicaId(idMica));

            //remove from micasgraduacionlist where idmica != idMica
            MicasGraduacionList = MicasGraduacionList
                .Where(mg => mg.IdMica == idMica)
                .ToList();

            //remove from lotesmicas where idmicagraduacion not in micasgraduacionlist id
            LotesMicas = LotesMicas
                .Where(lm => MicasGraduacionList.Any(mg => mg.Id == lm.IdMicaGraduacion))
                .ToList();

            //remove from micasgraduacion where id not in lotesmicas idmicagraduacion
            MicasGraduacionList = MicasGraduacionList
                .Where(mg => LotesMicas.Any(lm => lm.IdMicaGraduacion == mg.Id))
                .ToList();
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
