﻿namespace Domain.Entities
{
    public class LoteMica
    {
        public int IdLote { get; set; } 
        public int IdMica { get; set; } 
        public int Stock { get; set; }
        public DateTime FechaCaducidad { get; set; }
    }
}

