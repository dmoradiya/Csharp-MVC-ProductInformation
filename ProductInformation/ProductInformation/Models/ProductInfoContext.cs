using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductInformation.Models
{
    public class ProductInfoContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connection =
                    "server=localhost;" +
                    "port=3306;" +
                    "user=root;" +
                    "database=mvc_4point2;";

                string version = "10.4.14-MariaDB";

                optionsBuilder.UseMySql(connection, x => x.ServerVersion(version));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                               
            });
            modelBuilder.Entity<Product>(entity =>
            {
                string keyName = "FK_" + nameof(Product) +
                    "_" + nameof(Category);

                entity.Property(e => e.Name)
                .HasCharSet("utf8mb4")
                .HasCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.CategoryID)
                .HasName(keyName);

                entity.HasOne(thisEntity => thisEntity.Category)
                .WithMany(parent => parent.Products)
                .HasForeignKey(thisEntity => thisEntity.CategoryID)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName(keyName);

               
            });
        }
    }
}
