using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using System.Reflection;

namespace Infrastructure.Data.Repos
{
    public class MicaRepo : IMicaRepo
    {
        //private readonly OpticaDbContext _dbContext;
        //MicaRepo(OpticaDbContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}
        private List<Mica> micas = new List<Mica>
        {
            new Mica
            {
                Id = 1,
                Tipo = "Mono",
                Fabricante = "Essilor",
                Material = "Policarbonato",
                GraduacionESF = -1.5f,
                GraduacionCIL = -0.75f,
                Tratamiento = "Antirreflejante",
                Precio = 120.50f,
                Proposito = "Corrección de miopía"
            },
            new Mica
            {
                Id = 2,
                Tipo = "Bi",
                Fabricante = "Zeiss",
                Material = "Vidrio",
                GraduacionESF = 2.0f,
                GraduacionCIL = 0.0f,
                Tratamiento = "Fotocromático",
                Precio = 250.75f,
                Proposito = "Corrección de presbicia"
            },
            new Mica
            {
                Id = 3,
                Tipo = "Mono",
                Fabricante = "Hoya",
                Material = "Orgánico",
                GraduacionESF = -2.25f,
                GraduacionCIL = -1.0f,
                Tratamiento = "Blue Light",
                Precio = 180.00f,
                Proposito = "Uso para computadoras"
            },
            new Mica
            {
                Id = 4,
                Tipo = "Bi",
                Fabricante = "Rodenstock",
                Material = "Policarbonato",
                GraduacionESF = 1.75f,
                GraduacionCIL = -0.50f,
                Tratamiento = "Antirreflejante",
                Precio = 230.30f,
                Proposito = "Corrección de astigmatismo"
            },
            new Mica
            {
                Id = 5,
                Tipo = "Mono",
                Fabricante = "Essilor",
                Material = "Vidrio",
                GraduacionESF = -3.0f,
                GraduacionCIL = 0.0f,
                Tratamiento = "Anti-UV",
                Precio = 199.99f,
                Proposito = "Protección solar"
            }
        };
        public Task<Mica> GetMica(int id)
        {
            return Task.FromResult(micas.FirstOrDefault(m => m.Id == id));
        }

        public Task<IEnumerable<Mica>> GetAllMicas()
        {
            return Task.FromResult(micas.AsEnumerable());
        }

        public Task AddMica(Mica mica)
        {
            return Task.Run(() =>
            {
                if (micas.Any(m => m.Id == mica.Id))
                {
                    throw new MicaRepoException("La mica ya existe en el repositorio", mica);
                }
                micas.Add(mica);
            });
        }

        public Task UpdateMica(Mica mica)
        {
            return Task.Run(() =>
            {
                var micaToUpdate = micas.FirstOrDefault(m => m.Id == mica.Id);
                if (micaToUpdate == null)
                {
                    throw new MicaRepoException("La mica no existe en el repositorio", mica);
                }
                micaToUpdate = mica;
            });
        }
        public Task DeleteMica(Mica mica)
        {
            return Task.Run(() =>
            {
                var micaToDelete = micas.FirstOrDefault(m => m.Id == mica.Id);
                if (micaToDelete == null)
                {
                    throw new MicaRepoException("La mica no existe en el repositorio", mica);
                }
                micas.Remove(micaToDelete);
            });
        }
    }
}
