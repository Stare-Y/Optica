﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services.Reportes.Entities
{
    public class ReportePedido
    {
        public int IdPedido { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string Proposito { get; set; } = string.Empty;
        public DateTime FechaSalida { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public float GraduacionEsferica { get; set; }
        public float GraduacionCilindrica { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }

        /// <summary>
        /// Objeto que representa las columnas de cada fila para la generacion de reporte de pedidos
        /// </summary>
        public ReportePedido() { }
    }
}
