﻿using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Services.Reportes.Entities;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel; // Ensure you have installed the ClosedXML package via NuGet

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

        public async Task<Pedido> AddPedido(Pedido pedido, IEnumerable<PedidoMica>? pedidosMicas)
        {
            try
            {
                if(pedido.Id == 0)
                    throw new BadRequestException("El id del pedido no puede ser 0");

                await _pedidos.AddAsync(pedido);

                if (pedidosMicas != null)
                {
                    await _pedidoMicaRepo.AddPedidoMica(pedidosMicas);
                }
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
            }
        }

        public async Task<List<Pedido>> GetPedidosByDate(DateTime fechaInicio, DateTime fechaFin)
        {
            var pedidos = await _pedidos.AsNoTracking().Where(p => p.FechaSalida.Date >= fechaInicio.Date && p.FechaSalida.Date <= fechaFin.Date).ToListAsync();
            return pedidos;
        }

        public async Task<IEnumerable<ReportePedido>> GenerarReporte(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var pedidos = await GetPedidosByDate(fechaInicio, fechaFin);
                var reporte = new List<Domain.Interfaces.Services.Reportes.Entities.ReportePedido>();

                foreach (var pedido in pedidos)
                {
                    var usuario = await _usuarioRepo.GetUsuarioById(pedido.IdUsuario);
                    var pedidosMicas = await _pedidoMicaRepo.GetPedidoMicasByPedidoId(pedido.Id);

                    foreach (var pm in pedidosMicas)
                    {
                        ReportePedido reportePedido = new();

                        reportePedido.IdPedido = pedido.Id;
                        reportePedido.FechaSalida = pedido.FechaSalida;
                        reportePedido.Usuario = usuario.NombreDeUsuario;

                        var micaGraduacion = await _micaGraduacionRepo.GetMicaGraduacionById(pm.IdMicaGraduacion);
                        var mica = await _micaRepo.GetMica(micaGraduacion.IdMica);

                        reportePedido.Fabricante = mica.Fabricante;
                        reportePedido.Tratamiento = mica.Tratamiento;
                        reportePedido.Proposito = mica.Proposito;
                        reportePedido.GraduacionEsferica = micaGraduacion.Graduacionesf;
                        reportePedido.GraduacionCilindrica = micaGraduacion.Graduacioncil;
                        reportePedido.Cantidad = pm.Cantidad;
                        reportePedido.RazonSocial = pedido.RazonSocial;

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

        public async Task GenerarReporteExcel(string path, IEnumerable<ReportePedido> reporte)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Reporte");

                        worksheet.Cell(1, 1).Value = "ID Pedido";
                        worksheet.Cell(1, 2).Value = "Usuario";
                        worksheet.Cell(1, 3).Value = "Fabricante";
                        worksheet.Cell(1, 4).Value = "Tratamiento";
                        worksheet.Cell(1, 5).Value = "Propósito";
                        worksheet.Cell(1, 6).Value = "Razón Social";
                        worksheet.Cell(1, 7).Value = "Graduación Esférica";
                        worksheet.Cell(1, 8).Value = "Graduación Cilíndrica";
                        worksheet.Cell(1, 9).Value = "Cantidad";
                        worksheet.Cell(1, 10).Value = "Precio";
                        worksheet.Cell(1, 11).Value = "Fecha Salida";

                        int row = 2;

                        foreach (var item in reporte) 
                        {
                            worksheet.Cell(row, 1).Value = item.IdPedido;
                            worksheet.Cell(row, 2).Value = item.Usuario;
                            worksheet.Cell(row, 3).Value = item.Fabricante;
                            worksheet.Cell(row, 4).Value = item.Tratamiento;
                            worksheet.Cell(row, 5).Value = item.Proposito;
                            worksheet.Cell(row, 6).Value = item.RazonSocial;
                            worksheet.Cell(row, 7).Value = item.GraduacionEsferica;
                            worksheet.Cell(row, 8).Value = item.GraduacionCilindrica;
                            worksheet.Cell(row, 9).Value = item.Cantidad;
                            worksheet.Cell(row, 10).Value = item.Precio;
                            worksheet.Cell(row, 11).Value = item.FechaSalida.ToString("dd-MM-yyyy");
                            row++;
                        }
                        workbook.SaveAs(path);
                    }
                });
            }
            catch (IOException ioEx)
            {
                if (ioEx.Message.Contains("being used by another process"))
                {
                    throw new Exception("El archivo está siendo utilizado por otro proceso. Por favor, ciérralo e inténtalo nuevamente.");
                }
                else
                {
                    throw new Exception($"Error de entrada/salida al generar el reporte: {ioEx.Message}");
                }
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al generar el reporte en excel: ({e.Message})(Inner: {e.InnerException})");
            }
        }
    }
}