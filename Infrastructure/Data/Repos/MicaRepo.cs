using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using System.Runtime.InteropServices;


namespace Infrastructure.Data.Repos
{
    public class MicaRepo : IMicaRepo
    {
        private readonly OpticaDbContext _dbContext;
        private readonly DbSet<Mica> _micas;
        private readonly ILoteMicaRepo _loteMicaRepo;
        private readonly IPedidoMicaRepo _pedidoMicaRepo;
        private readonly IMicaGraduacionRepo _micaGraduacionRepo;
        public MicaRepo(OpticaDbContext dbContext, ILoteMicaRepo loteMicaRepo, IPedidoMicaRepo pedidioMicaRepo, IMicaGraduacionRepo micaGraduacionRepo)
        {
            _dbContext = dbContext;
            _micas = dbContext.Set<Mica>();
            _loteMicaRepo = loteMicaRepo;
            _pedidoMicaRepo = pedidioMicaRepo;
            _micaGraduacionRepo = micaGraduacionRepo;
        }

        public async Task<Mica> GetMica(int idMica)
        {
            var mica = await Task.FromResult(_micas.FirstOrDefault(m => m.Id == idMica));
            if (mica == null)
            {
                throw new NotFoundException("La mica no existe en el repositorio");
            }
            return mica;
        }

        public async Task<IEnumerable<Mica>> GetAllMicas()
        {
            return await Task.FromResult(_micas.AsEnumerable());
        }

        public async Task<Mica> InsertMica(Mica mica)
        {
            if (mica.Id == 0)
            {
                mica.Id = await GetSiguienteId();
            }

            //si ya existe, editarla mejor
            var exists = await MicaDetailsExist(mica);
            if (exists)
            {
                throw new BadRequestException("Los detalles proporcionados de la mica ya existen, no se puede proceder");
            }

            exists = await _micas.AnyAsync(m => m.Id == mica.Id);
            if (exists)
            {
                await UpdateMica(mica);
                return mica;
            }


            await _micas.AddAsync(mica);
            await _dbContext.SaveChangesAsync();
            //no tracking
            var micaChida = await _micas.AsNoTracking().FirstOrDefaultAsync(m => m.Id == mica.Id);
            if (micaChida == null)
            {
                throw new Exception("Error al insertar la mica");
            }

            Console.WriteLine($"Mica insertada id: {micaChida.Id}");

            return micaChida;
        }

        public async Task<Mica> AddMica(Mica mica, IEnumerable<MicaGraduacion>? micaGraduacions)
        {
            try
            {
                ValidarMica(mica);

                var exists = await MicaDetailsExist(mica);
                if (exists)
                {
                    throw new BadRequestException("Los detalles proporcionados de la mica ya existen, no se puede crear");
                }

                if (mica.Id == 0)
                {
                    mica.Id = await GetSiguienteId();
                    if (micaGraduacions != null)
                    {
                        foreach (var micaGraduacion in micaGraduacions)
                        {
                            micaGraduacion.IdMica = mica.Id;
                        }
                    }
                }

                if (micaGraduacions != null)
                {
                    await _micaGraduacionRepo.InsertMicaGraduacion(micaGraduacions);
                }

                await _micas.AddAsync(mica);

                await _dbContext.SaveChangesAsync();

                return mica;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al agregar la mica: ({e}) (Inner: {e.InnerException})");
            }
        }

        private async Task<bool> MicaDetailsExist(Mica mica)
        {
            var exists = await _micas.AsNoTracking().AnyAsync(m => m.Tipo == mica.Tipo && m.Fabricante == mica.Fabricante && m.Material == mica.Material && m.Tratamiento == mica.Tratamiento && m.Proposito == mica.Proposito);
            if (exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task UpdateMica(Mica mica)
        {
            ValidarMica(mica);
            var micaToUpdate = await _micas.FirstOrDefaultAsync(m => m.Id == mica.Id);
            if (micaToUpdate == null)
            {
                throw new NotFoundException("La mica no existe en el repositorio");
            }
            micaToUpdate = mica;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMica(int idMica)
        {
            await _micaGraduacionRepo.EliminarMicaGraduacionByMica(idMica);

            var micaToDelete = await _micas.FindAsync(idMica);
            if (micaToDelete == null)
            {
                throw new NotFoundException("La mica no existe en el repositorio");
            }
            _micas.Remove(micaToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetStock(int idMicaGraduacion)
        {
            return await _loteMicaRepo.GetStock(idMicaGraduacion);
        }

        public void ValidarMica(Mica mica)
        {
            if (string.IsNullOrWhiteSpace(mica.Tipo))
            {
                throw new BadRequestException("El nombre de la mica no puede estar vacio");
            }
            if (string.IsNullOrWhiteSpace(mica.Fabricante))
            {
                throw new BadRequestException("El fabricante de la mica no puede estar vacio");
            }
            if (string.IsNullOrWhiteSpace(mica.Material))
            {
                throw new BadRequestException("El material de la mica no puede estar vacio");
            }
            return;
        }

        public async Task<int> GetSiguienteId()
        {
            try
            {
                if (!await _micas.AnyAsync())
                {
                    return 1;
                }
                return await _micas.MaxAsync(m => m.Id) + 1;
            }
            catch
            (Exception e)
            {
                throw new Exception($"({e.GetType})Error al obtener el siguiente id de mica: ({e}) (Inner: {e.InnerException})");
            }
        }

        public async Task<List<Mica>> GetMicasByIds(IEnumerable<int> idsMicas)
        {
            return await _micas.Where(m => idsMicas.Contains(m.Id)).ToListAsync();
        }

        public async Task<Mica?> GetMicaByTipoTEST(string tipo)
        {
            return await _micas.FirstOrDefaultAsync(m => m.Tipo == tipo);
        }


        public async Task<IEnumerable<String>> GetTiposMicas()
        {
            return await _micas.Select(m => m.Tipo).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<String>> GetFabricanteMicas()
        {
            return await _micas.Select(m => m.Fabricante).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<String>> GetMaterialMicas()
        {
            return await _micas.Select(m => m.Material).Distinct().ToListAsync();
        }
        public async Task<IEnumerable<String>> GetTratamientoMicas()
        {
            return await _micas.Select(m => m.Tratamiento).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<String>> GetPropositoMicas()
        {
            return await _micas.Select(m => m.Proposito).Distinct().ToListAsync();
        }

        public async Task<IEnumerable<Mica>> GetMicasByFiltros(string tipo, string material, string fabricante, string tratamiento, string proposito)
        {
            var micas = _micas.AsQueryable();
            if (!string.IsNullOrWhiteSpace(tipo))
            {
                micas = micas.Where(m => m.Tipo == tipo);
            }
            if (!string.IsNullOrWhiteSpace(material))
            {
                micas = micas.Where(m => m.Material == material);
            }
            if (!string.IsNullOrWhiteSpace(fabricante))
            {
                micas = micas.Where(m => m.Fabricante == fabricante);
            }
            if (!string.IsNullOrWhiteSpace(tratamiento))
            {
                micas = micas.Where(m => m.Tratamiento == tratamiento);
            }
            if (!string.IsNullOrWhiteSpace(proposito))
            {
                micas = micas.Where(m => m.Proposito == proposito);
            }
            return await micas.ToListAsync();
        }
    }
}
