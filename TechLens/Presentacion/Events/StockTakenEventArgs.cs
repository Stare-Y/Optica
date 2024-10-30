using Domain.Interfaces.Services.DisplayEntities;

namespace TechLens.Presentacion.Events
{
    public class StockTakenEventArgs : EventArgs
    {
        public ShowConsultaStock? showConsultaStock { get; set; }
    }
}
