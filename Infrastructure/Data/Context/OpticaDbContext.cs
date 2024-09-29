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

            //modelBuilder.Entity<Mica>()
            //    .Property(m => m.GraduacionESF)
            //    .HasColumnName("graduacionesf")
            //    .HasColumnType("real")
            //    .IsRequired();
                                                    //se creo la tabla intermedia en vez de manejar esto aki
            //modelBuilder.Entity<Mica>()
            //    .Property(m => m.GraduacionCIL)
            //    .HasColumnName("graduacioncil")
            //    .HasColumnType("real")
            //    .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Tratamiento)
                .HasColumnName("tratamiento")
                .HasMaxLength(80)
                .IsRequired(false);

            modelBuilder.Entity<Mica>()
                .Property(m => m.Precio)
                .HasColumnName("precio")
                .HasColumnType("real")
                .IsRequired();

            modelBuilder.Entity<Mica>()
                .Property(m => m.Proposito)
                .HasColumnName("proposito")
                .HasMaxLength(80)
                .IsRequired(false);

            #endregion

            #region Lote

            modelBuilder.Entity<Lote>()
                .ToTable("lote")
                .HasKey(l => l.Id);

            modelBuilder.Entity<Lote>()
                .Property(l => l.Id)
                .HasColumnName("id_lote");

            modelBuilder.Entity<Lote>()
                .Property(l => l.Referencia)
                .HasColumnName("referencia");

            modelBuilder.Entity<Lote>()
                .Property(l => l.Extra1)
                .HasColumnName("extra1");

            modelBuilder.Entity<Lote>()
                .Property(l => l.Extra2)
                .HasColumnName("extra2");

            modelBuilder.Entity<Lote>()
                .Property(l => l.Proveedor)
                .HasColumnName("proveedor")
                .HasMaxLength (40)
                .IsRequired ();

            modelBuilder.Entity<Lote>()
                .Property(l => l.FechaEntrada)
                .HasColumnName("fechaentrada")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            modelBuilder.Entity<Lote>()
                .Property(l => l.FechaCaducidad)
                .HasColumnName("fechasalida")
                .HasColumnType("timestamp without time zone")
                .IsRequired();
            #endregion

            #region Pedido

            modelBuilder.Entity<Pedido>()
                .ToTable("pedidos")
                .HasKey (p => p.Id);

            modelBuilder.Entity<Pedido>()
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(p => p.IdUsuario);

            modelBuilder.Entity<Pedido>()
                .Property(u => u.IdUsuario)
                .HasColumnName("id_usuario");
           
            modelBuilder.Entity<Pedido>()
                 .Property(p => p.Id)
                 .HasColumnName("id_pedido");

            modelBuilder.Entity<Pedido>()
                .Property(p => p.FechaSalida)
                .HasColumnName("fechasalida")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            modelBuilder.Entity<Pedido>()
                .Property(p => p.RazonSocial)
                .HasColumnName("razonsocial")
                .HasMaxLength(60);

            #endregion

            #region Usuario
            modelBuilder.Entity<Usuario>()
            .ToTable("usuario")
            .HasKey(u => u.Id);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Id)
                .HasColumnName("id_usuario");

            modelBuilder.Entity<Usuario>()
                .Property(u => u.NombreDeUsuario)
                .HasColumnName("nombre_usuario")
                .HasMaxLength(15)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Rol)
                .HasColumnName("rol")
                .HasMaxLength(40)
                .IsRequired();

            modelBuilder.Entity<Usuario>()
                .Property(u => u.Password)
                .HasColumnName("contrasena")
                .HasMaxLength(15)
                .IsRequired();
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
                .Property(lm => lm.IdLote)
                .HasColumnName("id_lote")
                .IsRequired();

            modelBuilder.Entity<LoteMica>()
                .Property(lm => lm.Stock)
                .HasColumnName("stock")
                .IsRequired();

            modelBuilder.Entity<LoteMica>()
                .Property(lm => lm.FechaCaducidad)
                .HasColumnName("fecha_caducidad")
                .HasColumnType("timestamp without time zone")
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
                .Property(pm => pm.IdPedido)
                .HasColumnName("id_pedido")
                .IsRequired();
            modelBuilder.Entity<PedidoMica>()
                .Property (pm => pm.Cantidad)
                .HasColumnName("cantidad")
                .IsRequired();

            modelBuilder.Entity<PedidoMica>()
                .Property(pm => pm.FechaAsignacion)
                .HasColumnName("fecha_asignacion")
                .HasColumnType("timestamp without time zone")
                .IsRequired();


            #endregion

            #region MicaGraduacion
            modelBuilder.Entity<MicaGraduacion>()
            .ToTable("mica_graduacion")
            .HasKey(m => m.Id);

            modelBuilder.Entity<MicaGraduacion>()
                .Property(m => m.Id)
                .HasColumnName("id")
                .IsRequired();

            modelBuilder.Entity<MicaGraduacion>()
                .Property(m => m.IdMica)
                .HasColumnName("id_mica")
                .IsRequired();

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
            #endregion
        }
    }
}
