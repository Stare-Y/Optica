namespace Domain.Entities
{
    public class Lote
    {
        public int Id { get; set; }
        public int Referencia { get; set; }
        public DateTime FechaEntrada { get; set; }
        //necesitamos una tabla intermedia, para saber cuantos micas hay en un lote y de que tipos y graduaciones


    }
}
