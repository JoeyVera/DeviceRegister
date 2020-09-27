using Microsoft.EntityFrameworkCore;

namespace DeviceRegister.Models
{
    public partial class DevicesContext : DbContext
    {
        public DevicesContext()
        {
        }

        public DevicesContext(DbContextOptions<DevicesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EnergyMeter> EnergyMeter { get; set; }
        public virtual DbSet<Gateway> Gateway { get; set; }
        public virtual DbSet<WaterMeter> WaterMeter { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<EnergyMeter>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Brand)
                    .HasColumnName("brand")
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(50);

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasColumnName("serialNumber")
                    .HasMaxLength(50);
                entity.Ignore(e => e.Ip);
                entity.Ignore(e => e.Port);
            });

            modelBuilder.Entity<Gateway>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Brand)
                    .HasColumnName("brand")
                    .HasMaxLength(50);

                entity.Property(e => e.Ip)
                    .HasColumnName("ip")
                    .HasMaxLength(15);

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(50);

                entity.Property(e => e.Port).HasColumnName("port");

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasColumnName("serialNumber")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<WaterMeter>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Brand)
                    .HasColumnName("brand")
                    .HasMaxLength(50);

                entity.Property(e => e.Model)
                    .HasColumnName("model")
                    .HasMaxLength(50);

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasColumnName("serialNumber")
                    .HasMaxLength(50);
                entity.Ignore(e => e.Ip);
                entity.Ignore(e => e.Port);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=Devices;Trusted_Connection=True;");

        }
    }
}
