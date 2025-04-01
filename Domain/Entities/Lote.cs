namespace Domain.Entities
{
    public class Lote
    {
        public int Id { get; set; }
        public string Referencia { get; set; } = string.Empty;
        public DateTime FechaEntrada { get; set; }
        public string Proveedor { get; set; } = string.Empty;
        public DateTime FechaCaducidad { get; set; }
        public int IdUsuario { get; set; }
        public double Costo { get; set; }
        public int Existencias { get; set; }

        //tostring
        public override string ToString()
        {
            return $"Id: {Id}, Referencia: {Referencia}, FechaEntrada: {FechaEntrada}, Proveedor: {Proveedor}, FechaCaducidad: {FechaCaducidad}, IdUsuario: {IdUsuario}, Costo: {Costo}, Existencias: {Existencias}";
        }

        public void EnsureIsValid()
        {
            string invalidValues = string.Empty;

            if (FechaCaducidad == DateTime.MinValue)
                invalidValues += "La fecha de caducidad especificada no es valida\n";
            if (FechaEntrada == DateTime.MinValue)
                invalidValues += "La fecha de entrada no es valida\n";
            if (IdUsuario == 0)
                invalidValues += "El lote no fue creado por un usuario invalido\n";
            if (Proveedor == string.Empty)
                invalidValues += "El lote no tiene un proveedor";
            if (Existencias <= 0)
                invalidValues += $"Parece que el lote ya deberia estar vacio, Existencias = {Existencias}\n";
            if (Costo <= 0)
                invalidValues += "Por alguna razon el costo del lote fue 0, lo cual no es valido";

            if (!string.IsNullOrEmpty(invalidValues))
                throw new InvalidDataException(invalidValues);
        }
    }
}
