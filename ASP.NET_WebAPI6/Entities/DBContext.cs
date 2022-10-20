using Microsoft.EntityFrameworkCore;

namespace ASP.NET_WebAPI6.Entities
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=localhost;port=3306;user=root;password=;database=dagra");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<Schedule>(entity =>
            //{
            //    entity.ToTable("schedules");

            //    entity.Property(e => e.id).HasColumnType("int(11)");

            //    entity.Property(e => e.name)
            //        .IsRequired()
            //        .HasMaxLength(45);

            //    entity.Property(e => e.company)
            //        .IsRequired()
            //        .HasMaxLength(45);

            //    entity.Property(e => e.admin).HasColumnType("int(11)");
            //});

            //modelBuilder.Entity<Job>(entity =>
            //{
            //    entity.ToTable("schedules");

            //    entity.Property(e => e.id).HasColumnType("int(11)");

            //    entity.Property(e => e.name)
            //        .IsRequired()
            //        .HasMaxLength(45);

            //    entity.Property(e => e.fk_schedule).HasColumnType("int(11)");

            //});

            //modelBuilder.Entity<Assignment>(entity =>
            //{
            //    entity.ToTable("schedules");

            //    entity.Property(e => e.id).HasColumnType("int(11)");

            //    entity.Property(e => e.name)
            //        .IsRequired()
            //        .HasMaxLength(45);

            //    entity.Property(e => e.company)
            //        .IsRequired()
            //        .HasMaxLength(45);

            //    entity.Property(e => e.admin).HasColumnType("int(11)");
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}