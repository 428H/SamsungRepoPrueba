using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sistema.Models;
using System.Text.Json;

namespace SamsungV1.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categoria { get; set; }
    public DbSet<Producto> Producto { get; set; }
    public DbSet<Cliente> Cliente { get; set; }
    public DbSet<TipoMovimientoI> tipoMovimientoI { get; set; }
    public DbSet<InventarioActual> InventarioActual { get; set; }

    public DbSet<MovimientoInventario> MovimientoInventario { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<MetodosPago> MetodosPagos { get; set; }
    public DbSet<HistorialPrecios> HistorialPrecios { get; set; }

    public DbSet<Venta> Ventas { get; set; }
    public DbSet<DetalleVenta> DetallesVenta { get; set; }
    public DbSet<Factura> Facturas { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración para Producto
        modelBuilder.Entity<Producto>(entity =>
        {
            // Configuración del campo JSON
            entity.Property(p => p.especificaciones)
                .HasColumnType("nvarchar(max)"); 

          
        });
       
        modelBuilder.Entity<MovimientoInventario>(entity =>
        {
            
            entity.HasOne(m => m.Producto)
                .WithMany()
                .HasForeignKey(m => m.id_producto)
                .OnDelete(DeleteBehavior.NoAction);

            
            entity.HasOne(m => m.TipoMovimiento)
                .WithMany()
                .HasForeignKey(m => m.id_tipo_movimiento)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(m => m.Usuario)
                .WithMany()
                .HasForeignKey(m => m.id_usuario)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<HistorialPrecios>(entity =>
        {
            entity.HasKey(e => e.Id_historial);

           
            entity.HasOne(h => h.Producto)
                .WithMany()
                .HasForeignKey(h => h.Id_producto)
                .OnDelete(DeleteBehavior.Restrict); 

           
            entity.HasOne(h => h.Usuario)
                .WithMany()
                .HasForeignKey(h => h.Id_Usuario)
                .OnDelete(DeleteBehavior.Restrict); 

            entity.Property(h => h.Precio_Anterior).HasPrecision(18, 2);
            entity.Property(h => h.Precio_Nuevo).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.Id_cliente)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(v => v.MetodoPago)
                .WithMany()
                .HasForeignKey(v => v.Id_metodo_pago)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(v => v.Usuario)
                .WithMany()
                .HasForeignKey(v => v.Id_usuario)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasOne(d => d.Venta)
                .WithMany()
                .HasForeignKey(d => d.Id_venta)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.Id_producto)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasOne(f => f.Venta)
                .WithOne()
                .HasForeignKey<Factura>(f => f.Id_venta)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(f => f.Cliente)
                .WithMany()
                .HasForeignKey(f => f.Id_cliente)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(f => f.Usuario)
                .WithMany()
                .HasForeignKey(f => f.Id_usuario)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }

}




