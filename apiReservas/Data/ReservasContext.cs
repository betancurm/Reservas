using apiReservas.Identity;
using apiReservas.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace apiReservas.Data
{
    public class ReservasContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<DetalleReserva> DetallesReserva { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Egreso> Egresos { get; set; }
        public DbSet<Inmueble> Inmuebles { get; set; }
        public ReservasContext(DbContextOptions<ReservasContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Reserva>(reserva =>
            {
                reserva.ToTable("Reservas");
                reserva.HasKey(r => r.ReservaId);
                reserva.Property(r => r.FechaInicio).IsRequired();
                reserva.Property(r => r.FechaFin).IsRequired();
                reserva.Property(r => r.ValorReservaUSD).IsRequired();
                reserva.Property(r => r.InmuebleId).IsRequired();
                reserva.Property(r => r.FechaCreacion).IsRequired();
                reserva.Property(r => r.FechaModificacion).IsRequired();
                reserva.Property(r => r.ApplicationUserId).IsRequired();
                reserva.HasOne(r => r.Inmueble).WithMany(i => i.Reservas).HasForeignKey(r => r.InmuebleId);
                reserva.HasOne<ApplicationUser>().WithMany(r => r.Reservas).HasForeignKey(r => r.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<DetalleReserva>(detalleReserva =>
            {
                detalleReserva.ToTable("DetallesReserva");
                detalleReserva.HasKey(dr => dr.DetalleReservaId);
                detalleReserva.Property(dr => dr.FechaInicio).IsRequired();
                detalleReserva.Property(dr => dr.FechaFin).IsRequired();
                detalleReserva.Property(dr => dr.NochesOcupadas).IsRequired();
                detalleReserva.Property(dr => dr.FechaCreacion).IsRequired();
                detalleReserva.Property(dr => dr.FechaModificacion).IsRequired();
                detalleReserva.HasOne(dr => dr.Reserva).WithMany(r => r.DetallesReserva).HasForeignKey(dr => dr.ReservaId);
            }); 
            modelBuilder.Entity<Ingreso>(ingreso =>
            {
                ingreso.ToTable("Ingresos");
                ingreso.HasKey(i => i.IngresoId);
                ingreso.Property(i => i.Descripcion).IsRequired().HasMaxLength(100);
                ingreso.Property(i => i.FechaIngreso).IsRequired();
                ingreso.Property(i => i.TipoPlataforma).IsRequired();
                ingreso.Property(i => i.ValorUSD).IsRequired();
                ingreso.Property(i => i.TRM).IsRequired();
                ingreso.Property(i => i.ValorCOP).IsRequired();
                ingreso.Property(i => i.InmuebleId).IsRequired();
                ingreso.Property(i => i.FechaCreacion).IsRequired();
                ingreso.Property(i => i.FechaModificacion).IsRequired();
                ingreso.Property(i => i.ApplicationUserId).IsRequired();
                ingreso.HasOne(i => i.Inmueble).WithMany(i => i.Ingresos).HasForeignKey(i => i.InmuebleId);
                ingreso.HasOne<ApplicationUser>().WithMany(i => i.Ingresos).HasForeignKey(i => i.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Egreso>(egreso =>
            {
                egreso.ToTable("Egresos");
                egreso.HasKey(e => e.EgresoId);
                egreso.Property(e => e.InmuebleId).IsRequired();
                egreso.Property(e => e.Concepto).IsRequired();
                egreso.Property(e => e.Tipo).IsRequired();
                egreso.Property(e => e.Descripcion).IsRequired().HasMaxLength(50);
                egreso.Property(e => e.FechaEgreso).IsRequired();
                egreso.Property(e=> e.Valor).IsRequired();    
                egreso.Property(e => e.FechaCreacion).IsRequired();
                egreso.Property(e => e.FechaModificacion).IsRequired();
                egreso.Property(e => e.ApplicationUserId).IsRequired();
                egreso.HasOne(e => e.Inmueble).WithMany(i => i.Egresos).HasForeignKey(e => e.InmuebleId);
                egreso.HasOne<ApplicationUser>().WithMany(e => e.Egresos).HasForeignKey(e => e.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Inmueble>(inmueble =>
            {
                inmueble.ToTable("Inmuebles");
                inmueble.HasKey(i => i.InmuebleId);
                inmueble.HasIndex(i => i.CodigoInmueble).IsUnique();
                inmueble.Property(i => i.CodigoInmueble).IsRequired().HasMaxLength(50);
                inmueble.Property(i => i.Ciudad).IsRequired().HasMaxLength(50);
                inmueble.Property(i => i.Direccion).IsRequired().HasMaxLength(100);
                inmueble.Property(i => i.NumeroHabitaciones).IsRequired();
                inmueble.Property(i => i.NumeroBanos).IsRequired();
                inmueble.Property(i => i.Parqueadero).IsRequired();
                inmueble.Property(i => i.FechaCreacion).IsRequired();
                inmueble.Property(i => i.FechaModificacion).IsRequired();
                inmueble.Property(i => i.ApplicationUserId).IsRequired();
                inmueble.HasOne<ApplicationUser>().WithMany(i => i.Inmuebles).HasForeignKey(i => i.ApplicationUserId).OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}