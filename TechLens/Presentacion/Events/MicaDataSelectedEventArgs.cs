using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLens.Presentacion.Events
{
    public class MicaDataSelectedEventArgs : EventArgs
    {
        public MicaGraduacion? MicaGraduacionCaptured { get; set; }
        public int Cantidad { get; set; }
    }
}
