namespace Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public bool Tipo { get; set; } //alta 1 o baja 0
        public DateTime FechaSalida { get; set; }
    }
}
