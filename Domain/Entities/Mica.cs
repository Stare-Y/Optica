﻿namespace Domain.Entities
{
    public class Mica
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string Tratamiento { get; set; } = string.Empty;
        public string Proposito { get; set; } = string.Empty;
    }
}
