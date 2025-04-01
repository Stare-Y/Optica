using Domain.Entities;

namespace TechLens.Presentacion.Events
{
    public class LoteSelectedEventArgs : EventArgs
    {
        public Lote? SelectedLote { get; set; }
    }
}
