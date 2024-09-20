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
        MicaRepo(OpticaDbContext dbContext, ILoteMicaRepo loteMicaRepo)
        {
            _dbContext = dbContext;
            _micas = dbContext.Set<Mica>();
            _loteMicaRepo = loteMicaRepo;
        }

        public async Task<Mica?> GetMica(int id)
        {
            return await Task.FromResult(_micas.FirstOrDefault(m => m.Id == id));
        }

        public async Task<IEnumerable<Mica>> GetAllMicas()
        {
            return await Task.FromResult(_micas.AsEnumerable());
        }

        public async Task AddMica(Mica mica)
        {
            ValidarMica(mica);
            if (await _micas.AnyAsync(m => m.Id == mica.Id))
            {
                throw new NotFoundException("La mica ya existe en el repositorio");
            }
            await _micas.AddAsync(mica);
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

        public Task DeleteMica(int idMica)
        {
            return Task.Run(() =>
            {
                var micaToDelete = _micas.FirstOrDefault(m => m.Id == idMica;
                if (micaToDelete == null)
                {
                    throw new NotFoundException("La mica no existe en el repositorio");
                }
                _micas.Remove(micaToDelete);
            });
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
            return;
        }
    }
}
