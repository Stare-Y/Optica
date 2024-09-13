using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repos
{
    public class PedidoRepo
    {
        // Lista de pedidos en memoria
        private List<Pedido> pedidos = new List<Pedido>
        {
            new Pedido
            {
                Id = 1,
                FechaSalida = DateTime.Now.AddDays(-7),
                IdUsuario = 101,
                RazonSocial = "Empresa A"
            },
            new Pedido
            {
                Id = 2,
                FechaSalida = DateTime.Now.AddDays(-30),
                IdUsuario = 102,
                RazonSocial = "Empresa B"
            },
            new Pedido
            {
                Id = 3,
                FechaSalida = DateTime.Now.AddDays(-15),
                IdUsuario = 103,
                RazonSocial = "Empresa C"
            }
        }; 
        public Task<Pedido> GetPedido(int id)
        {
            return Task.FromResult(pedidos.FirstOrDefault(p => p.Id == id));
        }
        public Task<IEnumerable<Pedido>> GetAllPedidos()
        {
            return Task.FromResult(pedidos.AsEnumerable());
        }
        public Task AddPedido(Pedido pedido)
        {
            return Task.Run(() =>
            {
                if (pedidos.Any(p => p.Id == pedido.Id))
                {
                    throw new Exception("El pedido ya existe en el repositorio");
                }
                pedidos.Add(pedido);
            });
        }
        public Task UpdatePedido(Pedido pedido)
        {
            return Task.Run(() =>
            {
                var pedidoToUpdate = pedidos.FirstOrDefault(p => p.Id == pedido.Id);
                if (pedidoToUpdate == null)
                {
                    throw new Exception("El pedido no existe en el repositorio");
                }
                pedidoToUpdate.FechaSalida = pedido.FechaSalida;
                pedidoToUpdate.IdUsuario = pedido.IdUsuario;
                pedidoToUpdate.RazonSocial = pedido.RazonSocial;
            });
        }
        public Task DeletePedido(int id)
        {
            return Task.Run(() =>
            {
                var pedidoToDelete = pedidos.FirstOrDefault(p => p.Id == id);
                if (pedidoToDelete == null)
                {
                    throw new Exception("El pedido no existe en el repositorio");
                }
                pedidos.Remove(pedidoToDelete);
            });
        }
    }
}