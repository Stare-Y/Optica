namespace Domain.Entities
{
    public class Lote
    {
        public int Id { get; set; }
        public int Referencia { get; set; }
        public string Extra1 { get; set; } = string.Empty;
        public string Extra2 { get; set; } = string.Empty;
        public DateTime FechaEntrada { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public DateTime FechaCaducidad { get; set; }
    }
}
