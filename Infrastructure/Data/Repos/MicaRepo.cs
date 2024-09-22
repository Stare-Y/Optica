using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data.Context;
using Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data.Repos
{
    public class MicaRepo : IMicaRepo
    {
        private readonly OpticaDbContext _dbContext;
        private readonly DbSet<Mica> _micas;
        private readonly ILoteMicaRepo _loteMicaRepo;
        private readonly IPedidoMicaRepo _pedidoMicaRepo;
        public MicaRepo(OpticaDbContext dbContext, ILoteMicaRepo loteMicaRepo, IPedidoMicaRepo pedidioMicaRepo)
        {
            _dbContext = dbContext;
            _micas = dbContext.Set<Mica>();
            _loteMicaRepo = loteMicaRepo;
            _pedidoMicaRepo = pedidioMicaRepo;
        }

        public async Task<Mica?> GetMica(int idMica)
        {
            return await Task.FromResult(_micas.FirstOrDefault(m => m.Id == idMica));
        }

        public async Task<IEnumerable<Mica>> GetAllMicas()
        {
            return await Task.FromResult(_micas.AsEnumerable());
        }

        public async Task<Mica> AddMica(Mica mica)
        {
            try
            {
                ValidarMica(mica);

                mica.Id = await GetSiguienteId();

                await _micas.AddAsync(mica);
                await _dbContext.SaveChangesAsync();

                return mica;
            }
            catch (Exception e)
            {
                throw new Exception($"({e.GetType})Error al agregar la mica: ({e}) (Inner: {e.InnerException})");
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
            //validar que la mica no este en un lotemica o en un pedidomica
            if (await _loteMicaRepo.GetStock(idMica) > 0 || await _pedidoMicaRepo.GetMicasVendidas(idMica) > 0)
            {
                throw new BadRequestException("No se puede eliminar la mica porque esta en stock, o esta en pedidos.");
            }


            var micaToDelete = await _micas.FirstOrDefaultAsync(m => m.Id == idMica);
            if (micaToDelete == null)
            {
                throw new NotFoundException("La mica no existe en el repositorio");
            }
            _micas.Remove(micaToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetStock(int idMica)
        {
            return await _loteMicaRepo.GetStock(idMica);
        }

        public async Task<DateTime> GetCaducidad(int idMica)
        {
            return await _loteMicaRepo.GetCaducidad(idMica);
        }

        public void ValidarMica(Mica mica)
        {
            if (string.IsNullOrWhiteSpace(mica.Tipo))
            {
                throw new BadRequestException("El nombre de la mica no puede estar vacio");
            }
            if(string.IsNullOrWhiteSpace(mica.Fabricante))
            {
                throw new BadRequestException("El fabricante de la mica no puede estar vacio");
            }
            if (string.IsNullOrWhiteSpace(mica.Material))
            {
                throw new BadRequestException("El material de la mica no puede estar vacio");
            }
            if (mica.GraduacionESF == 0)
            {
                throw new BadRequestException("La graduacion esferica de la mica no puede ser 0");
            }
            if (mica.GraduacionCIL == 0)
            {
                throw new BadRequestException("La graduacion cilindrica de la mica no puede ser 0");
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
    }
}
