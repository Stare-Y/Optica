namespace Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreDeUsuario { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
    }
}
