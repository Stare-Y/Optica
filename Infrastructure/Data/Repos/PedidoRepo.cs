using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.Reportes.Entities;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class PedidoRepo : IPedidoRepo
    {
        private readonly OpticaDbContext _dbContext;
        private readonly DbSet<Pedido> _pedidos;

        private readonly IPedidoMicaRepo _pedidoMicaRepo;
        private readonly IUsuarioRepo _usuarioRepo;
        private readonly IMicaRepo _micaRepo;
        private readonly IMicaGraduacionRepo _micaGraduacionRepo;
        public PedidoRepo(OpticaDbContext dbContext, IPedidoMicaRepo pedidoMicaRepo, IUsuarioRepo usuarioRepo, IMicaRepo micaRepo ,IMicaGraduacionRepo micaGraduacionRepo)
        {
            _dbContext = dbContext;
            _pedidos = dbContext.Set<Pedido>();
            _pedidoMicaRepo = pedidoMicaRepo;
            _usuarioRepo = usuarioRepo;
            _micaRepo = micaRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
        }

        

        public async Task<Pedido?> GetPedido(int idPedido)
        {
            return await _pedidos.FirstOrDefaultAsync(p => p.Id == idPedido);
        }

        public async Task<List<Pedido>> GetAllPedidos()
        {
            return await _pedidos.ToListAsync();
        }

        public async Task<Pedido> AddPedido(Pedido pedido, IEnumerable<PedidoMica> pedidosMicas)
        {
            try
            {
                if(pedido.Id == 0)
                {
                    pedido.Id = await GetSiguienteId();
                    foreach (var pm in pedidosMicas)
                    {
                        pm.IdPedido = pedido.Id;
                    }
                }
                await _pedidos.AddAsync(pedido);

                await _pedidoMicaRepo.AddPedidoMica(pedidosMicas);

                await _dbContext.SaveChangesAsync();

                return pedido;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al añadir el pedido: ({e.Message})(Inner: {e.InnerException})");
            }
        }

        public async Task DeletePedido(int idPedido)
        {
            try 
            {
                var pedidoEliminar = await _pedidos.FirstOrDefaultAsync(p => p.Id == idPedido);
                if (pedidoEliminar == null)
                {
                    throw new NotFoundException("El pedido no existe en el repositorio");
                }
                else
                {
                    await _pedidoMicaRepo.DeletePedidoMicaByPedidoId(idPedido);
                    _pedidos.Remove(pedidoEliminar);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al eliminar el pedido: ({e.Message})(Inner: {e.InnerException})");
            }
        }

        public async Task<int> GetSiguienteId()
        {
            try
            {
                if(!await _pedidos.AnyAsync())
                {
                    return 1;
                }
                return await _pedidos.MaxAsync(p => p.Id) + 1;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al obtener el siguiente id de pedido: ({e.Message})(Inner: {e.InnerException})");
            }
        }

        public void ValidarPedidosMicas(IEnumerable<PedidoMica> pedidosMicas)
        {
            foreach (var pedidoMica in pedidosMicas)
            {
                if (pedidoMica.Cantidad <= 0)
                {
                    throw new BadRequestException("La cantidad de micas debe ser mayor a 0");
                }
                if (pedidoMica.IdMicaGraduacion == 0)
                {
                    throw new BadRequestException("El id de la mica no puede ser 0");
                }
                if (pedidoMica.FechaAsignacion == DateTime.MinValue)
                {
                    throw new BadRequestException("La fecha de asignación no puede ser nula");
                }
            }
        }

        public async Task<List<Pedido>> GetPedidosByDate(DateTime fechaInicio, DateTime fechaFin)
        {
            var pedidos = await _pedidos.AsNoTracking().Where(p => p.FechaSalida >= fechaInicio && p.FechaSalida <= fechaFin).ToListAsync();
            return pedidos;
        }

        public async Task<List<ReportePedido>> GenerarReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var pedidos = await GetPedidosByDate(fechaInicio, fechaFin);
                var reporte = new List<Domain.Interfaces.Services.Reportes.Entities.ReportePedido>();
                foreach(var pedido in pedidos)
                {
                    ReportePedido reportePedido = new();

                    reportePedido.IdPedido = pedido.Id;
                    reportePedido.FechaSalida = pedido.FechaSalida;
                    var usuario = await _usuarioRepo.GetUsuarioById(pedido.IdUsuario);
                    reportePedido.Usuario = usuario.NombreDeUsuario;
                    var pedidosMicas = await _pedidoMicaRepo.GetPedidoMicasByPedidoId(pedido.Id);
                    foreach(var pm in pedidosMicas)
                    {
                        var micaGraduacion = await _micaGraduacionRepo.GetMicaGraduacionById(pm.IdMicaGraduacion);
                        var mica = await _micaRepo.GetMica(micaGraduacion.IdMica);
                        reportePedido.Fabricante = mica.Fabricante;
                        reportePedido.Tratamiento = mica.Tratamiento;
                        reportePedido.Proposito = mica.Proposito;
                        reportePedido.GraduacionEsferica = micaGraduacion.Graduacionesf;
                        reportePedido.GraduacionCilindrica = micaGraduacion.Graduacioncil;
                        reportePedido.Cantidad = pm.Cantidad;
                        reportePedido.Precio = micaGraduacion.Precio;
                        reporte.Add(reportePedido);
                    }
                }
                return reporte;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al generar el reporte: ({e.Message})(Inner: {e.InnerException})");
            }
        }
    }
}