namespace Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaSalida { get; set; }
        public int IdUsuario { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
    }
}
