using Application.ViewModels.Base;
using Domain.Interfaces;
using Domain.Interfaces.Services.Reportes.Entities;
using System.Collections.ObjectModel;
using ClosedXML.Excel;



namespace Application.ViewModels
{
    public class ViewModelReportes : ViewModelBase
    {


        private ObservableCollection<ReportePedido> _reportePedidos = new ObservableCollection<ReportePedido>();
        private readonly IPedidoRepo? _pedidoRepo;
        public ViewModelReportes() { }

        public ViewModelReportes(IPedidoRepo pedidoRepo)
        {
            _pedidoRepo = pedidoRepo;
        }

        public ObservableCollection<ReportePedido> ReportePedidos
        {
            get => _reportePedidos;
            set
            {
                _reportePedidos = value;
                OnPropertyChanged(nameof(ReportePedidos));
            }
        }

        public DateTime FechaInicio { get; set; } = DateTime.Now.Date;
        public DateTime FechaFin { get; set; } = DateTime.Now.Date;

        public async Task GetReportePedidos()
        {
            if (_pedidoRepo is null)
            {
                throw new Exception("No se ha inyectado el repositorio de pedidos");
            }

            if (FechaInicio > FechaFin)
            {
                throw new Exception("La fecha de inicio no puede ser mayor a la fecha de fin");
            }

            var reportePedidos = await _pedidoRepo.GenerarReporte(FechaInicio, FechaFin);
            ReportePedidos = new ObservableCollection<ReportePedido>(reportePedidos);
            if (ReportePedidos.Count <= 0)
            {
                throw new Exception("No se encontraron pedidos en el rango de fechas especificado");
            }
        }

        public async Task ExportarReporteObtenido()
        {
            if (_reportePedidos.Count <= 0)
                throw new Exception("No hay datos para exportar");
            await Task.Delay(1000);
        }

        public void GenerarReporteExcel()
        {
            var ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "ReportesTechLens");

            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            var nombreArchivo = "ReportesTechLens.xlsx";
            var path = Path.Combine(ruta, nombreArchivo);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Reporte");

                worksheet.Cell(1, 1).Value = "ID Pedido";
                worksheet.Cell(1, 2).Value = "Usuario";
                worksheet.Cell(1, 3).Value = "Fabricante";
                worksheet.Cell(1, 4).Value = "Tratamiento";
                worksheet.Cell(1, 5).Value = "Propósito";
                worksheet.Cell(1, 6).Value = "Fecha Salida";
                worksheet.Cell(1, 7).Value = "Razón Social";
                worksheet.Cell(1, 8).Value = "Graduación Esférica";
                worksheet.Cell(1, 9).Value = "Graduación Cilíndrica";
                worksheet.Cell(1, 10).Value = "Cantidad";
                worksheet.Cell(1, 11).Value = "Precio";

                for (int i = 0; i < ReportePedidos.Count; i++)
                {
                    var pedido = ReportePedidos[i];
                    worksheet.Cell(i + 2, 1).Value = pedido.IdPedido;
                    worksheet.Cell(i + 2, 2).Value = pedido.Usuario;
                    worksheet.Cell(i + 2, 3).Value = pedido.Fabricante;
                    worksheet.Cell(i + 2, 4).Value = pedido.Tratamiento;
                    worksheet.Cell(i + 2, 5).Value = pedido.Proposito;
                    worksheet.Cell(i + 2, 6).Value = pedido.FechaSalida.ToString("dd-MM-yyyy");
                    worksheet.Cell(i + 2, 7).Value = pedido.RazonSocial;
                    worksheet.Cell(i + 2, 8).Value = pedido.GraduacionEsferica;
                    worksheet.Cell(i + 2, 9).Value = pedido.GraduacionCilindrica;
                    worksheet.Cell(i + 2, 10).Value = pedido.Cantidad;
                    worksheet.Cell(i + 2, 11).Value = pedido.Precio;
                }
                workbook.SaveAs(path);
            }

            Console.WriteLine($"Reporte generado exitosamente en: {path}");
        }

    }
}




