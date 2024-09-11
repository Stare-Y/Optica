namespace Domain.Entities
{
    public class Mica
    {
        public int Id { get; set; }
        public string Tipo { get; set; } //mono o bi
        public string Fabricante { get; set; }
        public string Material { get; set; }
        public float GraduacionESF { get; set; }
        public float GraduacionCIL { get; set; }
        public string Tratamiento { get; set; }
        public float Precio { get; set; }
        public string Proposito { get; set; }
    }
}
