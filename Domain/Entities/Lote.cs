﻿namespace Domain.Entities
{
    public class Lote
    {
        public int Id { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public DateTime FechaEntrada { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public DateTime FechaCaducidad { get; set; }
        public int IdUsuario { get; set; }
        public double Costo { get; set; }
    }
}
