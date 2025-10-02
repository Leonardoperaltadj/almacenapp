using Microsoft.EntityFrameworkCore;

namespace PlantillaAlmacen.Models
{
    public class AlmacenDbContext : DbContext
    {
        public AlmacenDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Asignacion> Asignaciones { get; set; }
        public DbSet<CatalogoArticulo> CatalogoArticulos { get; set; }
        public DbSet<EstadoEntrega> EstadoEntregas { get; set; }
        public DbSet<EstatusArticulo> EstatusArticulos { get; set; }
        public DbSet<MovimientoInventario> MovimientoInventarios { get; set; }
        public DbSet<Permiso> Permisos { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<RolPermiso> RolPermisos { get; set; }
        public DbSet<TipoMovimiento> TipoMovimientos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioRol> UsuarioRoles { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<EstatusPersonal> EstatusPersonales { get; set; }
        public DbSet<Puesto> Puestos { get; set; }
        public DbSet<Frente> Frentes { get; set; }
        public DbSet<AsignacionEvento> AsignacionEventos { get; set; }
        public DbSet<ArticuloEstadoHistorial> ArticuloEstadoHistoriales { get; set; }
        public DbSet<UnidadMedida> UnidadesMedidas { get; set; }
        public DbSet<Almacen> Almacenes { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<EvidenciaAsignacion> EvidenciaAsignaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>().ToTable("Categoria");
            modelBuilder.Entity<Articulo>()
                    .ToTable("Articulos")
                    .HasDiscriminator<string>("Discriminator")
                    .HasValue<ArticuloTecnologia>("Tecnologia")
                    .HasValue<ArticuloVehiculo>("Vehiculo")
                    .HasValue<ArticuloCombustible>("Combustible")
                    .HasValue<ArticuloHerramienta>("Herramienta")
                    .HasValue<ArticuloPapeleria>("Papeleria")
                    .HasValue<ArticuloEnfermeria>("Enfermeria")
                    .HasValue<ArticuloComunicaciones>("Comunicaciones");
            modelBuilder.Entity<Asignacion>().ToTable("Asignacion");
            modelBuilder.Entity<CatalogoArticulo>().ToTable("CatalogoArticulo");
            modelBuilder.Entity<EstadoEntrega>().ToTable("EstadoEntrega");
            modelBuilder.Entity<EstatusArticulo>().ToTable("EstatusArticulo");
            modelBuilder.Entity<MovimientoInventario>().ToTable("MovimientoInventario");
            modelBuilder.Entity<Permiso>().ToTable("Permiso");
            modelBuilder.Entity<Personal>().ToTable("Personal");
            modelBuilder.Entity<Rol>().ToTable("Rol");
            modelBuilder.Entity<RolPermiso>().ToTable("RolPermiso");
            modelBuilder.Entity<TipoMovimiento>().ToTable("TipoMovimiento");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<UsuarioRol>().ToTable("UsuarioRol");
            modelBuilder.Entity<UsuarioRol>().HasKey(c => new { c.IdUsuario, c.IdRol });
            modelBuilder.Entity<RolPermiso>().HasKey(c => new { c.IdRol, c.IdPermiso });
            modelBuilder.Entity<Departamento>().ToTable("Departamento");
            modelBuilder.Entity<EstatusPersonal>().ToTable("EstatusPersonal");
            modelBuilder.Entity<Puesto>().ToTable("Puesto");
            modelBuilder.Entity<Frente>().ToTable("Frente");
            modelBuilder.Entity<UnidadMedida>().ToTable("UnidadMedida");

            modelBuilder.Entity<Asignacion>()
            .ToTable(tb => tb.HasCheckConstraint(
                "CK_Asignacion_DestinoUnico",
                "((CASE WHEN [IdPersonal] IS NULL THEN 0 ELSE 1 END) + " +
                " (CASE WHEN [IdDepartamento] IS NULL THEN 0 ELSE 1 END) + " +
                " (CASE WHEN [IdFrente] IS NULL THEN 0 ELSE 1 END)) = 1"
            ));
            modelBuilder.Entity<Asignacion>()
           .HasIndex(a => new { a.IdArticulo, a.FechaDevolucion })
           .HasFilter("[FechaDevolucion] IS NULL")
           .IsUnique();
            modelBuilder.Entity<Articulo>()
            .HasOne(a => a.Almacen)
            .WithMany(al => al.Articulos)
            .HasForeignKey(a => a.IdAlmacen)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Articulo>()
            .HasOne(a => a.Factura)
            .WithMany(f => f.Articulos)
            .HasForeignKey(a => a.IdFactura)
            .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<EvidenciaAsignacion>()
           .HasOne(e => e.Asignacion)
           .WithMany(a => a.Evidencias)
           .HasForeignKey(e => e.IdAsignacion)
           .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EvidenciaAsignacion>().ToTable("EvidenciaAsignacion");

        }
    }
}
