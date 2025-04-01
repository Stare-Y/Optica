using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MicaGraduacion
    {
        public int Id { get; set; }
        public int IdMica { get; set; }
        public float Graduacionesf { get; set; }
        public float Graduacioncil { get; set; }
    }
}
