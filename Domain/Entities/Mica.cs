namespace Domain.Entities
{
    public class Mica
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Fabricante { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public float GraduacionESF { get; set; }
        public float GraduacionCIL { get; set; }
        public string Tratamiento { get; set; } = string.Empty;
        public float Precio { get; set; }
        public string Proposito { get; set; } = string.Empty;
    }
}
