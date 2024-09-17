using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Context
{
    public class OpticaDbContext : DbContext
    {
        public DbSet<Mica> Micas { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<LoteMica> LoteMicaIntermedia { get; set; }

        public OpticaDbContext(DbContextOptions<DbContext> options) : base(options) { }
    }
}
