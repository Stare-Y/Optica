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
    public class LoteRepo : ILoteRepo
    {
        private List<Lote> lotes = new List<Lote>
        {
           new Lote
            {
                Id = 1,
                Referencia = 1001,
                Extra1 = "Lote1-Extra1",
                Extra2 = "Lote1-Extra2",
                FechaEntrada = DateTime.Now.AddMonths(-3),
                Proveedor = "Proveedor A",
                FechaCaducidad = DateTime.Now.AddYears(1)
            },
            new Lote
            {
                Id = 2,
                Referencia = 1002,
                Extra1 = "Lote2-Extra1",
                Extra2 = "Lote2-Extra2",
                FechaEntrada = DateTime.Now.AddMonths(-2),
                Proveedor = "Proveedor B",
                FechaCaducidad = DateTime.Now.AddMonths(6)
            },
            new Lote
            {
                Id = 3,
                Referencia = 1003,
                Extra1 = "Lote3-Extra1",
                Extra2 = "Lote3-Extra2",
                FechaEntrada = DateTime.Now.AddMonths(-1),
                Proveedor = "Proveedor C",
                FechaCaducidad = DateTime.Now.AddMonths(3)
            }
        };
        public Task<Lote> GetLote(int id)
        {
            return Task.FromResult(lotes.FirstOrDefault(l => l.Id == id));
        }
        public Task<IEnumerable<Lote>> GetAllLotes()
        {
            return Task.FromResult(lotes.AsEnumerable());
        }
        public Task AddLote(Lote lote)
        {
            lotes.Add(lote);
            return Task.CompletedTask;
        }
        public Task UpdateLote(Lote lote)
        {
            var index = lotes.FindIndex(l => l.Id == lote.Id);
            lotes[index] = lote;
            return Task.CompletedTask;
        }
        public Task DeleteLote(Lote lote)
        {
            lotes.Remove(lote);
            return Task.CompletedTask;
        }
    }
}

