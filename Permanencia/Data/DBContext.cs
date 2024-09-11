using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Permanencia.Data
{
    public class DBContext : DbContext
    {
        public DbSet<Mica> Micas { get; set; }
        public DbSet<Lote> Lotes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public DBContext(DbContextOptions<DbContext> options) : base(options) { }
    }
}
