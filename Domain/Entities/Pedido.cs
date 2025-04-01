namespace Domain.Entities
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime FechaSalida { get; set; }
        public int IdUsuario { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public string? Extra { get; set; } = null;

        public override string ToString()
        {
            return $"Id: {Id}, FechaSalida: {FechaSalida}, IdUsuario: {IdUsuario}, RazonSocial: {RazonSocial}, Extra: {Extra}";
        }

        public void EnsureIsValid()
        {
            string incompleteAttributes = string.Empty;

            if (this.FechaSalida == DateTime.MinValue)
                incompleteAttributes += "La fecha del pedido no es valida.\n";
            if (this.IdUsuario == 0)
                incompleteAttributes += "No se ha especificado que usuario levanta el pedido\n";
            if (string.IsNullOrEmpty(RazonSocial))
                incompleteAttributes += "No se ha proporcionado la razon social del cliente del pedido\n";

            if (!string.IsNullOrEmpty(incompleteAttributes))
                throw new InvalidDataException(incompleteAttributes);
        }
    }
}
