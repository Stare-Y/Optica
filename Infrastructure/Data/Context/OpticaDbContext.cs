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
        public DbSet<PedidoMica> PedidoMicaIntermedia { get; set; }
        public DbSet<MicaGraduacion> MicaGraduacionIntermedia { get; set; }

        public OpticaDbContext(DbContextOptions<OpticaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Mica

            modelBuilder.Entity<Mica>()
                .ToTable("mica")
                .HasKey(m => m.Id);

            modelBuilder.Entity<Mica>()
                .Property(m => m.Id)
                .HasColumnName("id_mica")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Tipo)
                .HasColumnName("tipo")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Fabricante)
                .HasColumnName("fabricante")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Material)
                .HasColumnName("material")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Tratamiento)
                .HasColumnName("tratamiento")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Proposito)
                .HasColumnName("proposito")
                .IsRequired();

            #endregion

            #region Lote

            modelBuilder.Entity<Lote>()
                .ToTable("lote")
                .HasKey(l => l.Id);

            modelBuilder.Entity<Lote>()
                .Property(l => l.Id)
                .HasColumnName("id_lote")
                .IsRequired()
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Lote>()
                .Property(l => l.Referencia)
                .HasColumnName("referencia")
                .IsRequired();

            modelBuilder.Entity<Lote>()
                .Property(l => l.Proveedor)
                .HasColumnName("proveedor")
                .IsRequired ();

            modelBuilder.Entity<Lote>()
                .Property(l => l.FechaEntrada)
                .HasColumnName("fecha_entrada")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            modelBuilder.Entity<Lote>()
                .Property(l => l.FechaCaducidad)
                .HasColumnName("fecha_caducidad")
                .HasColumnType("timestamp without time zone")
                .IsRequired();
            
            modelBuilder.Entity<Lote>()
                .Property(l => l.IdUsuario)
                .HasColumnName("id_usuario")
                .IsRequired();

            modelBuilder.Entity<Lote>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(l => l.IdUsuario);

            modelBuilder.Entity<Lote>()
                .Property(l => l.Costo)
                .HasColumnName("costo")
                .IsRequired();
                
            #endregion

            #region Pedido

            modelBuilder.Entity<Pedido>()
                .ToTable("pedido")
                .HasKey (p => p.Id);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Id)
                .HasColumnName("id_pedido")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Pedido>()
                .Property(u => u.IdUsuario)
                .HasColumnName("id_usuario")
                .IsRequired();
           
            modelBuilder.Entity<Pedido>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(p => p.IdUsuario);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.FechaSalida)
                .HasColumnName("fecha_salida")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            modelBuilder.Entity<Pedido>()
                .Property(p => p.RazonSocial)
                .HasColumnName("razon_social")
                .IsRequired();

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Extra)
                .HasColumnName("extra")
                .IsRequired(false); 

            #endregion

            #region Usuario
            modelBuilder.Entity<Usuario>()
                .ToTable("usuario")
                .HasKey(u => u.Id);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .HasColumnName("id_usuario")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.NombreDeUsuario)
                .HasColumnName("nombre_usuario")
                .HasMaxLength(16)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasColumnName("rol")
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Password)
                .HasColumnName("password")
                .HasMaxLength(16)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.NombreDeUsuario)
                .IsUnique();

            #endregion

            #region  LoteMica

            modelBuilder.Entity<LoteMica>()
                 .ToTable("lote_mica")
                 .HasKey(lm => new { lm.IdMicaGraduacion, lm.IdLote });
               
            modelBuilder.Entity<LoteMica>()
                .Property(lm => lm.IdMicaGraduacion)
                .HasColumnName("id_mica_graduacion")
                .IsRequired();

            modelBuilder.Entity<LoteMica>()
                .HasOne<MicaGraduacion>()
                .WithMany()
                .HasForeignKey(lm => lm.IdMicaGraduacion);

            modelBuilder.Entity<LoteMica>()
                .Property(lm => lm.IdLote)
                .HasColumnName("id_lote")
                .IsRequired();

            modelBuilder.Entity<LoteMica>()
                .HasOne<Lote>()
                .WithMany()
                .HasForeignKey(lm => lm.IdLote);

            modelBuilder.Entity<LoteMica>()
                .Property(lm => lm.Cantidad)
                .HasColumnName("cantidad")
                .IsRequired();

            #endregion

            #region PedidoMica
            modelBuilder.Entity<PedidoMica>()
                .ToTable("pedido_mica")
                .HasKey(lm => new { lm.IdMicaGraduacion, lm.IdPedido });

            modelBuilder.Entity<PedidoMica>()
                .Property(pm => pm.IdMicaGraduacion)
                .HasColumnName("id_mica_graduacion")
                .IsRequired();

            modelBuilder.Entity<PedidoMica>()
                .HasOne<MicaGraduacion>()
                .WithMany()
                .HasForeignKey(pm => pm.IdMicaGraduacion);

            modelBuilder.Entity<PedidoMica>()
                .Property(pm => pm.IdPedido)
                .HasColumnName("id_pedido")
                .IsRequired();

            modelBuilder.Entity<PedidoMica>()
                .HasOne<Pedido>()
                .WithMany()
                .HasForeignKey(pm => pm.IdPedido);
            
            modelBuilder.Entity<PedidoMica>()
                .Property (pm => pm.Cantidad)
                .HasColumnName("cantidad")
                .IsRequired();

            modelBuilder.Entity<PedidoMica>()
                .Property(pm => pm.Precio)
                .HasColumnName("precio")
                .IsRequired();

            modelBuilder.Entity<PedidoMica>()
                .Property(pm => pm.IdLoteOrigen)
                .HasColumnName("id_lote_origen")
                .IsRequired();

            modelBuilder.Entity<PedidoMica>()
                .HasOne<Lote>()
                .WithMany()
                .HasForeignKey(pm => pm.IdLoteOrigen);

            #endregion

            #region MicaGraduacion

            modelBuilder.Entity<MicaGraduacion>()
            .ToTable("mica_graduacion")
            .HasKey(m => m.Id);

            modelBuilder.Entity<MicaGraduacion>()
                .Property(m => m.Id)
                .HasColumnName("id").ValueGeneratedOnAdd();

            modelBuilder.Entity<MicaGraduacion>()
                .Property(m => m.IdMica)
                .HasColumnName("id_mica")
                .IsRequired();

            modelBuilder.Entity<MicaGraduacion>()
                .HasOne<Mica>()
                .WithMany()
                .HasForeignKey(m => m.IdMica);

            modelBuilder.Entity<MicaGraduacion>()
                .Property(m => m.Graduacionesf)
                .HasColumnName("graduacionesf")
                .HasColumnType("real")
                .IsRequired();

            modelBuilder.Entity<MicaGraduacion>()
                .Property(m => m.Graduacioncil)
                .HasColumnName("graduacioncil")
                .HasColumnType("real")
                .IsRequired();

            modelBuilder.Entity<MicaGraduacion>()
                .HasIndex(m => new { m.Graduacionesf, m.Graduacioncil, m.IdMica})
                .IsUnique();

            #endregion
        }
    }
}
    