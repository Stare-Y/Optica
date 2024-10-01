using Domain.Entities;

namespace TechLens.Presentacion.Events
{
    public class MicasSelectedEventArgs : EventArgs
    {
        public IEnumerable<Mica>? MicasSelected { get; set; } = new List<Mica>();
        public Mica? SelectedMica { get; set; }
    }
}
