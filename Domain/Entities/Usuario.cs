namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}
