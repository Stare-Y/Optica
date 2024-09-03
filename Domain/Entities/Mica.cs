using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Mica
    {
        public string Tipo { get; set; } //mono o bi
        public int Id { get; set; }
        public string Fabricante { get; set; }
        public float GraduacionESF { get; set; }
        public float GraduacionCIL { get; set; }
        public string Tratamiento { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public int Stock { get; set; } // debe ser igual a la sumatoria de las micas en los lotes en la tabla intermedia
        public float Precio { get; set; }
        public string Proposito { get; set; }
    }
}
