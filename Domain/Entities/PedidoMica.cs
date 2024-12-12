namespace Domain.Entities
{
    public class PedidoMica
    {
        public int IdPedido { get; set; } 
        public int IdMicaGraduacion { get; set; } 
        public int IdLoteOrigen { get; set; }
        public int Cantidad { get; set; } 
        public double Precio { get; set; }
    }
}


