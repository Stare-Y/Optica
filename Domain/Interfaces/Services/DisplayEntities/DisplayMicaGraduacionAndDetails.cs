using Domain.Entities;

namespace Domain.Interfaces.Services.DisplayEntities
{
    public class DisplayMicaGraduacionAndDetails
    {
        public MicaGraduacion MicaGraduacion { get; set; } = new();
        public LoteMica? LoteMica { get; set; } = null;
        public PedidoMica? PedidoMica { get; set; } = null;
    }
}
