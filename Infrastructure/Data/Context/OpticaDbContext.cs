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


        //public DbSet<PedidoMica> PedidoMicaIntermedia { get; set; }

        public OpticaDbContext(DbContextOptions<DbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Mica

            modelBuilder.Entity<Mica>()
                .ToTable("mica")
                .HasKey(m => m.Id);

            modelBuilder.Entity<Mica>()
                .Property(m => m.Id)
                .HasColumnName("id_mica");

            modelBuilder.Entity<Mica>()
                .Property(m => m.Tipo)
                .HasColumnName("tipo")
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Fabricante)
                .HasColumnName("fabricante")
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Material)
                .HasColumnName("material")
                .HasMaxLength(50);

            modelBuilder.Entity<Mica>()
                .Property(m => m.GraduacionESF)
                .HasColumnName("graduacionesf")
                .HasColumnType("real")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.GraduacionCIL)
                .HasColumnName("graduacioncil")
                .HasColumnType("real")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Tratamiento)
                .HasColumnName("tratamiento")
                .HasMaxLength(80);

            modelBuilder.Entity<Mica>()
                .Property(m => m.Precio)
                .HasColumnName("precio")
                .HasColumnType("real")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Proposito)
                .HasColumnName("proposito")
                .HasMaxLength(80);

            #endregion
        }
    }
}
