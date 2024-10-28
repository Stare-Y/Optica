using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLens.Presentacion.Events
{
    public class GraduacionesSelectedEventArgs : EventArgs
    {
        public IEnumerable<LoteMica>? GraduacionesLoteSelected { get; set; }
        public IEnumerable<PedidoMica>? GraduacionesPedidoMicaSelected { get; set; }
    }
}
