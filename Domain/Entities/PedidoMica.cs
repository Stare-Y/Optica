namespace Domain.Entities
{
    public class PedidoMica
    {
        public int IdPedido { get; set; } 
        public int IdMicaGraduacion { get; set; } 
        public int IdLoteOrigen { get; set; }
        public int Cantidad { get; set; } 
        public double Precio { get; set; } 

        public void EnsureIsValid()
        {
            string invalidAttributes = string.Empty;

            if (this.IdPedido == 0)
                invalidAttributes += "No se ha especificado a que pedido se asigna la mica\n";
            if (this.IdMicaGraduacion == 0)
                invalidAttributes += "No se ha especificado que graduacion se asignara al pedido\n";
            if (this.IdLoteOrigen == 0)
                invalidAttributes += "No se ha verificado el lote de origen del que se toma la mica\n";
            if (this.Cantidad >= 0)
                invalidAttributes += "La  cantidad de micas a tomar, debe ser mayor a cero\n";
            if (this.Precio >= 0)
                invalidAttributes += "El precio debe ser mayor que 0, si ve este mensaje, contacte al desarrollador por una brecha en la logica de ejecucion\n";

            if (!string.IsNullOrEmpty(invalidAttributes))
                throw new InvalidDataException(invalidAttributes);
        }
    }
}


