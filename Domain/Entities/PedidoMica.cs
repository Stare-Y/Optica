﻿namespace Domain.Entities
{
    public class PedidoMica
    {
        public int Id { get; set; } 
        public int PedidoId { get; set; } 
        public int MicaId { get; set; } 
        public int Cantidad { get; set; } 
        public DateTime FechaAsignacion { get; set; } 
    }
}

