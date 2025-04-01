using Domain.Entities;

namespace Domain.Interfaces.Services.DisplayEntities
{
    public class DisplayMicaGraduacionAndDetails
    {
        public MicaGraduacion MicaGraduacion { get; set; } = new();
        public LoteMica? LoteMica { get; set; } = null;
        public PedidoMica? PedidoMica { get; set; } = null;

        public int Cantidad
        {
            get
            {
                return LoteMica?.Cantidad ?? PedidoMica?.Cantidad ?? 0;
            }
        }
    }
}
