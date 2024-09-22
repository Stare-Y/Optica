using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    public class PedidoMicaException: Exception
    {
        public PedidoMica? pedidoMicaObj;
        public PedidoMicaException(string msg, PedidoMica pedidoMica):base(msg) 
        {
            pedidoMicaObj = pedidoMica;
        }

    }
}
