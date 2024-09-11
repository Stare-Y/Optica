using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class MicaRepoException : Exception
    {
        public Mica Mica { get; set; }  
        public MicaRepoException(string message) : base(message) { }
        public MicaRepoException(string message, Mica mica) : base(message) 
        {
            Mica = mica;
        }
    }
}
