using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repos
{
    public class LoteMicaRepo : ILoteMicaRepo
    {

        private readonly DbSet<Lote> _lotes;
        private readonly DbSet<Mica> _micas;
        private readonly DbSet<LoteMica> _loteMicasIntermedia;
        private readonly DbContext _dbContext;
        
        public LoteMicaRepo(DbContext dbContext)
        {
            _lotes = dbContext.Set<Lote>();
            _micas = dbContext.Set<Mica>();
            _loteMicasIntermedia = dbContext.Set<LoteMica>();
            _dbContext = dbContext;
        }

        public Task AgregarLoteMica(Lote lote, Dictionary<Mica, int> micas)
        {
            return Task.Run(() =>
            {
               using (var transaction = _dbContext.Database.BeginTransaction())
               {
                   try
                   {
                        
                        //validar si existen las micas, si no, crearlas
                        foreach (var mica in micas.Keys)
                        {
                            var micaExistente = _micas.FirstOrDefault(m => m.Id == mica.Id);
                            if (micaExistente == null)
                            {
                                _micas.Add(mica);
                            }
                        }
                        //agregar datos a la tabla intermedia
                        foreach (var mica in micas.Keys)
                        {
                            _loteMicasIntermedia.Add(new LoteMica
                            {
                                Lote = lote.Id,
                                Mica = mica.Id,
                                Stock = micas[mica]
                            });
                        }

                        //crear lote
                        _lotes.Add(lote);

                        //guardar cambios
                        _dbContext.SaveChanges();
                   }
                   catch
                   {
                       transaction.Rollback();
                       throw;
                   }
               }
            });
        }
    }
}
