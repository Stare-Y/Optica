using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLens.Presentacion.Events
{
    public class MicasSelectedEventArgs : EventArgs
    {
        public IEnumerable<Mica> MicasSelected { get; set; } = new List<Mica>();
    }
}
