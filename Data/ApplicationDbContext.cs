using Microsoft.EntityFrameworkCore;
using RentManagementApp.Models.MasterEntities;
using RentManagementApp.Models.RelationshipEntities;
using RentManagementApp.Models.TransactionalEntities;

namespace RentManagementApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<House> Houses { get; set; }

        public DbSet<Floor> Floors { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<TenantRoom> TenantRooms { get; set; }

        public DbSet<Meter> Meters { get; set; }

        public DbSet<TenantMeter> TenantMeters { get; set; }

        public DbSet<MeterReading> MeterReadings { get; set; }

        public DbSet<Bill> Bills { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<BillDetail> BillDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DECIMAL PRECISION

            modelBuilder.Entity<House>()
                .Property(h => h.ElectricityRate)
                .HasPrecision(18, 2);

            modelBuilder.Entity<House>()
                .Property(h => h.GarbageFee)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Tenant>()
                .Property(t => t.MonthlyRent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                .Property(b => b.RentAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                .Property(b => b.ElectricityAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                .Property(b => b.GarbageAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bill>()
                .Property(b => b.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BillDetail>()
                .Property(bd => bd.PreviousReading)
                .HasPrecision(18, 2);


            modelBuilder.Entity<BillDetail>()
                .Property(bd => bd.CurrentReading)
                .HasPrecision(18, 2);


            modelBuilder.Entity<BillDetail>()
                .Property(bd => bd.UnitsConsumed)
                .HasPrecision(18, 2);


            modelBuilder.Entity<BillDetail>()
                .Property(bd => bd.Rate)
                .HasPrecision(18, 2);


            modelBuilder.Entity<BillDetail>()
                .Property(bd => bd.Amount)
                .HasPrecision(18, 2);

            // RELATIONSHIP CONFIGURATION

            modelBuilder.Entity<Floor>()
                .HasOne(f => f.House)
                .WithMany(h => h.Floors)
                .HasForeignKey(f => f.HouseId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Room>()
                .HasOne(r => r.Floor)
                .WithMany(f => f.Rooms)
                .HasForeignKey(r => r.FloorId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Meter>()
                .HasOne(m => m.House)
                .WithMany(h => h.Meters)
                .HasForeignKey(m => m.HouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Bill)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BillId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BillDetail>()
                .HasOne(bd => bd.Bill)
                .WithMany(b => b.BillDetails)
                .HasForeignKey(bd => bd.BillId)
                .OnDelete(DeleteBehavior.Restrict);

            // UNIQUE CONSTRAINTS

            modelBuilder.Entity<Bill>()
                .HasIndex(b => b.BillNumber)
                .IsUnique();

        }
    }
}