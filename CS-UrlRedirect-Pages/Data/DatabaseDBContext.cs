using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using CS_UrlRedirect_Pages.Models;

#nullable disable

namespace CS_UrlRedirect_Pages.Data
{
    public partial class DatabaseDBContext : DbContext
    {
        public DatabaseDBContext()
        {
        }

        public DatabaseDBContext(DbContextOptions<DatabaseDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Redirect> Redirects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Redirect>(entity =>
            {
                entity.HasIndex(e => e.ShortCode, "U_SHORTCODE")
                    .IsUnique();

                entity.Property(e => e.NumVisits).HasColumnName("numVisits");

                entity.Property(e => e.ShortCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("shortCode");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("url");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
