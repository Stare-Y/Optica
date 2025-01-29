namespace Domain.Entities
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
        public int Existencias { get; set; }

        //tostring
        public override string ToString()
        {
            return $"Id: {Id}, Referencia: {Referencia}, FechaEntrada: {FechaEntrada}, Proveedor: {Proveedor}, FechaCaducidad: {FechaCaducidad}, IdUsuario: {IdUsuario}, Costo: {Costo}, Existencias: {Existencias}";
        }
    }
}
