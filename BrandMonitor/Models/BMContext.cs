using Microsoft.EntityFrameworkCore;

#nullable disable
namespace BrandMonitor.Models
{
    public class BMContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }

        public BMContext()
        {
        }

        public BMContext(DbContextOptions<BMContext> dbContextOptions) 
            : base(dbContextOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseNpgsql("Host=localhost;Port=5432;Database=BrandMonitor;Username=postgres;Password=qwerty");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasPostgresExtension("uuid-ossp");

            builder.Entity<Task>(entity =>
            {
                entity.ToTable("Task")
                    .HasKey(e => e.Guid);

                entity.HasIndex(e => e.Guid)
                    .IsUnique(); 

                entity.Property(e => e.Guid)
                    .HasColumnName("guid")
                    //.HasColumnType("integer");
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .HasMaxLength(16)
                    .HasColumnType("varchar");

                entity.Property(e => e.Timestamp)
                    .HasColumnName("Timestamp");
            });
        }
    }
}
