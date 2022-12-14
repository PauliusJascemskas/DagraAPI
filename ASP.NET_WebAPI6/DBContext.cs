using ASP.NET_WebAPI6.Entities;
using DagraAPI.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DagraAPI
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
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<Company> Companies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=dagradb.mariadb.database.azure.com;port=3306;user=dbroot@dagradb;password=Admin123;database=dagradb;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.ToTable("user");

            //    entity.HasIndex(e => e.email)
            //        .HasName("IX_EMAIL");

            //    entity.Property(e => e.id)
            //        .HasColumnName("ID")
            //        .HasColumnType("int(11)");

            //    entity.Property(e => e.email)
            //        .IsRequired()
            //        .HasColumnName("EMAIL")
            //        .HasColumnType("varchar(50)")
            //        .HasDefaultValueSql("'0'");

            //    entity.Property(e => e.password)
            //        .IsRequired()
            //        .HasColumnName("PASSWORD")
            //        .HasColumnType("varchar(128)")
            //        .HasDefaultValueSql("'0'");

            //    entity.Property(e => e.role)
            //        .IsRequired()
            //        .HasColumnName("ROLE")
            //        .HasColumnType("varchar(20)")
            //        .HasDefaultValueSql("'0'");

            //    entity.Property(e => e.salt)
            //        .IsRequired()
            //        .HasColumnName("SALT")
            //        .HasColumnType("varchar(36)")
            //        .HasDefaultValueSql("'0'");
            //});

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

            //    OnModelCreatingPartial(modelBuilder);
            //}

            //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        }
    }
}